using Twapi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twapi.Database.SQLite.Base;
using Twapi.Database.SQLite;
using ClosedXML.Excel;

namespace Twapi.Twitter
{
    public class TwitterActions : TwapiBase
    {
        #region 乱数発生用
        /// <summary>
        /// 乱数発生用
        /// </summary>
        static Random _Rand = new Random();
        #endregion

        #region アクション一覧
        /// <summary>
        /// アクション一覧
        /// </summary>
        public static List<TwitterAction> Actions = new List<TwitterAction>()
        {
            new TwitterAction("/?", "ヘルプを表示します",
                Help),
            new TwitterAction("/h", "ヘルプを表示します",
                Help),
            new TwitterAction("/regist", "各種キーの保存処理" + "\r\n"
                + "\t\t-ck コンシューマーキー(必須)" + "\r\n"
                + "\t\t-cs コンシューマーキー(必須)" + "\r\n"
                + "\t\t-at アクセストークン(必須)" + "\r\n"
                + "\t\t-as アクセスシークレット(必須)" + "\r\n"
                + "\t\t-keysfile キーファイルの保存先(省略時はデフォルトのパス)" + "\r\n"
                , Regist),
            new TwitterAction("/refresh", "フォローリスト・フォロワーリスト・フォロバ候補リストの更新します" + "\r\n"
                + "\t\t-sql データ保存先ファイルパス(省略時はデフォルトのパス)" + "\r\n"
                + "\t\t-keysfile キーファイルの保存先(省略時はデフォルトのパス)" + "\r\n"
                , TwapiUpdate),
            new TwitterAction("/search", "フォロバ候補を探します" + "\r\n"
                + "\t\t-keywords 自己紹介文に含まれるキーワード(必須)。カンマ区切りでOR指定。ex.プログラマー,プログラミング,ソフトウェア" + "\r\n"
                + "\t\t-sql データ保存先ファイルパス(省略時はデフォルトのパス)" + "\r\n"
                + "\t\t-keysfile キーファイルの保存先(省略時はデフォルトのパス)" + "\r\n"
                + "\t\t-limit フォロバ候補としてストックしておくユーザー数の上限値を指定します(省略時は5000)" + "\r\n"
                , TwapiSearch),
            new TwitterAction("/follow", "フォローを実行します" + "\r\n"
                + "\t\t-lastday 最終ツイート日からの日数を正の整数値で指定する(省略時は0)" + "\r\n"
                + "\t\t-ffmin フォロー数/フォロワー数最小値(0～1を指定：0指定時は無制限)" + "\r\n"
                + "\t\t-ffmax フォロー数/フォロワー数最大値(0～1を指定：0指定時は無制限)" + "\r\n"
                + "\t\t-sql データ保存先ファイルパス(省略時はデフォルトのパス)" + "\r\n"
                + "\t\t-keysfile キーファイルの保存先(省略時はデフォルトのパス)" + "\r\n"
                , TwapiFollow),
            new TwitterAction("/remove", "フォローを解除します" + "\r\n"
                + "\t\t-lastday 最終ツイート日からの日数を正の整数値で指定する(省略時は0)" + "\r\n"
                + "\t\t-followdays フォローしてからの期間を正の整数値で指定する(省略時は0)" + "\r\n"
                + "\t\t-sql データ保存先ファイルパス(省略時はデフォルトのパス)" + "\r\n"
                + "\t\t-keysfile キーファイルの保存先(省略時はデフォルトのパス)" + "\r\n"
                , TwapiRemove),
            new TwitterAction("/autotweet", "Excelの内容をランダムでツイートします" + "\r\n"
                + "\t\t-xlsx エクセルファイルパスを指定(必須)"
                + "\t\t-keysfile キーファイルの保存先(省略可)" + "\r\n"
                , AutoTweet),
            //new TwitterAction("/init", "各種キーの初期化設定を行います"
            //    , Init),
        };
        #endregion

        #region アクション
        #region Private Method
        #region フォローリストの取得処理
        /// <summary>
        /// フォローリストの取得処理
        /// </summary>
        /// <param name="screen_name">スクリーン名</param>
        /// <returns>フォローリスト</returns>
        private static List<long> GetFriendList(string screen_name)
        {
            long cursor = -1;
            List<long> ret = new List<long>();
            while (cursor != 0)
            {
                // フレンドの取得
                var result = TwitterAPI.Token.Friends.Ids(screen_name, cursor < 0 ? null : cursor, 5000);

                // 登録処理
                ret.AddRange(result);

                // 次のカーソル取得
                cursor = result.NextCursor;

                // RateLimit確認
                if (result.RateLimit.Remaining <= 0)
                {
                    System.Threading.Thread.Sleep(15 * 60 * 1000);
                }

            }

            return ret;
        }
        #endregion

        #region フォローリストのSQLite登録
        /// <summary>
        /// フォローリストのSQLite登録
        /// </summary>
        private static void RegistFriendList()
        {
            try
            {
                // 自分のアカウントの情報取得
                var settings = TwitterAPI.Token.Account.Settings();

                var rate_limit = TwitterAPI.Token.Application.RateLimitStatus();

                var tmp = rate_limit.Values.ElementAt(33).Values.ElementAt(0);
                Console.WriteLine(rate_limit.Values.ElementAt(33).Values.ElementAt(0).Reset.LocalDateTime);

                // データベース上に登録されているかを確認
                var db_friends = FrinedsLogBase.Select();

                using (var db = new SQLiteDataContext())
                {
                    try
                    {
                        // データベースの存在確認
                        db.Database.EnsureCreated();

                        // トランザクションをかける
                        db.Database.BeginTransaction();

                        // フォローリストの取得
                        var api_frineds = GetFriendList(settings.ScreenName);

                        foreach (var db_friend in db_friends)
                        {
                            // DBとAPIで取得したデータの相違
                            if (!(from x in api_frineds where x.Equals(db_friend.UserId) select x).Any())
                            {
                                var update = new FrinedsLogBase();
                                // コピー処理
                                update.Copy(db_friend);

                                // 解除したことを確認した時刻をセット
                                update.RemoveAt = update.RemoveAt.HasValue ? update.RemoveAt : DateTime.Now;

                                // データの更新
                                FrinedsLogBase.Update(db, update, update);
                            }
                        }

                        // フレンドを確認
                        foreach (var api_frined in api_frineds)
                        {
                            // データベース上に登録されているかを確認
                            var db_friend = db_friends.Where(x => x.UserId.Equals(api_frined));

                            // 存在しないならデータの作成
                            if (!db_friend.Any())
                            {
                                FrinedsLogBase.Insert(db, new FrinedsLogBase()
                                {
                                    UserId = api_frined,
                                    FollowAt = DateTime.Now,
                                    RemoveAt = null
                                });
                            }
                            // 存在するならアップデート
                            else
                            {
                                var update = new FrinedsLogBase();
                                update.Copy(db_friend.First());
                                FrinedsLogBase.Update(db, update, update);
                            }
                        }
                        db.SaveChanges();
                        db.Database.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        db.Database.RollbackTransaction();
                        Console.WriteLine(e.Message);
                        Logger.Error(e.Message);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロワーリストの取得処理
        /// <summary>
        /// フォロワーリストの取得処理
        /// </summary>
        /// <param name="screen_name">スクリーン名</param>
        /// <returns>フォロワーリスト</returns>
        private static List<long> GetFollowerList(string screen_name)
        {
            long cursor = -1;
            List<long> ret = new List<long>();
            while (cursor != 0)
            {
                // フォロワーリストの取得
                var result = TwitterAPI.Token.Followers.Ids(screen_name, cursor < 0 ? null : cursor, 5000);
                ret.AddRange(result);
                var cusor = result.NextCursor;

                // RateLimitの角煮
                if (result.RateLimit.Remaining <= 0)
                {
                    System.Threading.Thread.Sleep(15 * 60 * 1000);
                }
                cursor = result.NextCursor;
            }

            return ret;
        }
        #endregion

        #region フォロワーリストのSQLite登録
        /// <summary>
        /// フォロワーリストのSQLite登録
        /// </summary>
        private static void RegistFollowerList()
        {
            try
            {
                // 自分のアカウントの情報取得
                var settings = TwitterAPI.Token.Account.Settings();

                // データベース上に登録されているかを確認
                var db_friends = FollowersLogBase.Select();

                using (var db = new SQLiteDataContext())
                {
                    try
                    {
                        // データベースの存在確認
                        db.Database.EnsureCreated();

                        // トランザクションをかける
                        db.Database.BeginTransaction();

                        // フォローリストの取得
                        var api_frineds = GetFollowerList(settings.ScreenName);

                        foreach (var db_friend in db_friends)
                        {
                            // DBとAPIで取得したデータの相違
                            if (!(from x in api_frineds where x.Equals(db_friend.UserId) select x).Any())
                            {
                                var update = new FollowersLogBase();
                                // コピー処理
                                update.Copy(db_friend);

                                // 解除したことを確認した時刻をセット
                                update.RemoveAt = update.RemoveAt.HasValue ? update.RemoveAt : DateTime.Now;

                                // データの更新
                                FollowersLogBase.Update(db, update, update);
                            }
                        }

                        // フレンドを確認
                        foreach (var api_frined in api_frineds)
                        {
                            // データベース上に登録されているかを確認
                            var db_friend = db_friends.Where(x => x.UserId.Equals(api_frined));

                            // 存在しないならデータの作成
                            if (!db_friend.Any())
                            {
                                FollowersLogBase.Insert(db, new FollowersLogBase()
                                {
                                    UserId = api_frined,
                                    FollowerAt = DateTime.Now,
                                    RemoveAt = null
                                });
                            }
                            // 存在するならアップデート
                            else
                            {
                                var update = new FollowersLogBase();
                                update.Copy(db_friend.First());
                                FollowersLogBase.Update(db, update, update);
                            }
                        }
                        db.SaveChanges();
                        db.Database.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        db.Database.RollbackTransaction();
                        Console.WriteLine(e.Message);
                        Logger.Error(e.Message);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region ユーザーリストの更新
        /// <summary>
        /// ユーザーリストの更新
        /// </summary>
        private static void RefreshUserList()
        {
            try
            {
                // フォローリストの取得
                var db_tmp = FrinedsLogBase.Select().Select(x => x.UserId).ToList<long>();

                // フォロワーリストの取得
                db_tmp.AddRange(FollowersLogBase.Select().Select(x => x.UserId).ToList<long>());

                // フォロバリストの取得
                //db_tmp.AddRange(UserListBase.Select().Select(x => x.UserId).ToList<long>());

                // 重複を除外
                db_tmp = db_tmp.Distinct().ToList<long>();

                while (db_tmp.Count > 0)
                {
                    List<long> db_tmp2;
                    int length = 100, max_length = 100;

                    if (db_tmp.Count < max_length)
                        length = db_tmp.Count;

                    db_tmp2 = db_tmp.GetRange(0, length);

                    var user_list = TwitterAPI.Token.Users.Lookup(user_id => db_tmp2);

                    using (var db = new SQLiteDataContext())
                    {
                        try
                        {
                            // データベースの存在確認
                            db.Database.EnsureCreated();

                            // トランザクションをかける
                            db.Database.BeginTransaction();

                            //// データベース上に登録されているかを確認
                            var db_friends = UserListBase.Select();

                            foreach (var api_user in user_list)
                            {
                                // データベース上に登録されているかを確認
                                var db_friend = db_friends.Where(x => x.UserId.Equals(api_user.Id));

                                // 存在しないならデータの作成
                                if (!db_friend.Any())
                                {
                                    UserListBase.Insert(db, new UserListBase()
                                    {
                                        UserId = api_user.Id.Value,
                                        CreateAt = api_user.CreatedAt.LocalDateTime,
                                        Description = api_user.Description,
                                        FavouritesCount = api_user.FavouritesCount,
                                        FollowersCount = api_user.FollowersCount,
                                        FriendsCount = api_user.FriendsCount,
                                        IsExclude = false,
                                        LastTweet = api_user.Status == null ? string.Empty : api_user.Status.Text,
                                        LastTweetAt = api_user.Status == null ? null : api_user.Status.CreatedAt.LocalDateTime,
                                        ScreenName = api_user.ScreenName,
                                        Reason = 0,
                                        TweetCount = api_user.StatusesCount,
                                        IsProtected = api_user.IsProtected,
                                        IsSuspended = api_user.IsSuspended
                                    });
                                }
                                // 存在するならアップデート
                                else
                                {
                                    var update = new UserListBase();
                                    update.Copy(db_friend.First());
                                    update.Description = api_user.Description;
                                    update.FavouritesCount = api_user.FavouritesCount;
                                    update.FollowersCount = api_user.FollowersCount;
                                    update.FriendsCount = api_user.FriendsCount;
                                    update.LastTweet = api_user.Status == null ? string.Empty : api_user.Status.Text;
                                    update.LastTweetAt = api_user.Status == null ? null : api_user.Status.CreatedAt.LocalDateTime;
                                    update.ScreenName = api_user.ScreenName;
                                    update.TweetCount = api_user.StatusesCount;
                                    update.IsSuspended = api_user.IsSuspended;
                                    update.IsProtected = api_user.IsProtected;
                                    UserListBase.Update(db, update, update);
                                }
                            }

                            db.SaveChanges();
                            db.Database.CommitTransaction();
                        }
                        catch (Exception e)
                        {
                            db.Database.RollbackTransaction();
                            Console.WriteLine(e.Message);
                            Logger.Error(e.Message);
                            throw;
                        }
                    }


                    db_tmp.RemoveRange(0, length);

                    if (user_list.RateLimit.Remaining <= 0)
                    {
                        // 15分待機
                        System.Threading.Thread.Sleep(15 * 60 * 1000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロバリストの更新
        /// <summary>
        /// フォロバリストの更新
        /// </summary>
        private static void RefreshFollowBackList()
        {
            try
            {
                // フォローリストの取得
                var db_tmp = FollowBackListBase.Select().Select(x => x.UserId).ToList<long>();

                // 重複を除外
                db_tmp = db_tmp.Distinct().ToList<long>();

                while (db_tmp.Count > 0)
                {
                    List<long> db_tmp2;
                    int length = 100, max_length = 100;

                    if (db_tmp.Count < max_length)
                        length = db_tmp.Count;

                    db_tmp2 = db_tmp.GetRange(0, length);

                    var user_list = TwitterAPI.Token.Users.Lookup(user_id => db_tmp2);

                    using (var db = new SQLiteDataContext())
                    {
                        try
                        {
                            // データベースの存在確認
                            db.Database.EnsureCreated();

                            // トランザクションをかける
                            db.Database.BeginTransaction();

                            // データベース上に登録されているかを確認
                            var db_friends = FollowBackListBase.Select();

                            foreach (var api_user in user_list)
                            {
                                // データベース上に登録されているかを確認
                                var db_friend = db_friends.Where(x => x.UserId.Equals(api_user.Id));

                                // 存在しないならデータの作成
                                if (!db_friend.Any())
                                {
                                    FollowBackListBase.Insert(db, new FollowBackListBase()
                                    {
                                        UserId = api_user.Id.Value,
                                        InsertAt = DateTime.Now,
                                        UpdateAt = DateTime.Now,
                                        CreateAt = api_user.CreatedAt.LocalDateTime,
                                        Description = api_user.Description,
                                        FavouritesCount = api_user.FavouritesCount,
                                        FollowersCount = api_user.FollowersCount,
                                        FriendsCount = api_user.FriendsCount,
                                        LastTweet = api_user.Status == null ? string.Empty : api_user.Status.Text,
                                        LastTweetAt = api_user.Status == null ? null : api_user.Status.CreatedAt.LocalDateTime,
                                        ScreenName = api_user.ScreenName,
                                        TweetCount = api_user.StatusesCount,
                                        IsProtected = api_user.IsProtected,
                                        IsSuspended = api_user.IsSuspended
                                    });
                                }
                                // 存在するならアップデート
                                else
                                {
                                    var update = new FollowBackListBase();
                                    update.Copy(db_friend.First());
                                    update.UpdateAt = DateTime.Now;
                                    update.Description = api_user.Description;
                                    update.FavouritesCount = api_user.FavouritesCount;
                                    update.FollowersCount = api_user.FollowersCount;
                                    update.FriendsCount = api_user.FriendsCount;
                                    update.LastTweet = api_user.Status == null ? string.Empty : api_user.Status.Text;
                                    update.LastTweetAt = api_user.Status == null ? null : api_user.Status.CreatedAt.LocalDateTime;
                                    update.ScreenName = api_user.ScreenName;
                                    update.TweetCount = api_user.StatusesCount;
                                    update.IsSuspended = api_user.IsSuspended;
                                    update.IsProtected = api_user.IsProtected;
                                    FollowBackListBase.Update(db, update, update);
                                }
                            }

                            db.SaveChanges();
                            db.Database.CommitTransaction();
                        }
                        catch (Exception e)
                        {
                            db.Database.RollbackTransaction();
                            Console.WriteLine(e.Message);
                            Logger.Error(e.Message);
                            throw;
                        }
                    }


                    db_tmp.RemoveRange(0, length);

                    if (user_list.RateLimit.Remaining <= 0)
                    {
                        // 15分待機
                        System.Threading.Thread.Sleep(15 * 60 * 1000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region 自分のフォロワーとフォロー情報を更新する
        /// <summary>
        /// フォロバリストの自分のフォロワーとフォロー情報を更新する
        /// </summary>
        private static void TwapiUpdate()
        {
            try
            {
                // フォロワーリストの取得
                RegistFollowerList();

                // フォローリストの取得
                RegistFriendList();

                // ユーザーリストの更新
                RefreshUserList();

                // フォロバリストの更新
                RefreshFollowBackList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロバリストのリスト数をlimitで指定されたサイズに調整する
        /// <summary>
        /// フォロバリストのリスト数をlimitで指定されたサイズに調整する
        /// </summary>
        private static void AdjustFollowbackList()
        {
            try
            {
                List<long> userid_list = new List<long>();

                // フォロー対象リストの取得
                var db_follow_list = (from x in FollowBackListBase.Select()
                                      orderby x.InsertAt
                                      select x).ToList<FollowBackListBase>();

                int limit = 5000;

                // ユーザー数上限（この値を超えてフォロバリストはつくれない）
                if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.Limit))
                {
                    int.TryParse(TwitterArgs.CommandOptions.Limit, out limit);
                }

                // 上限数を超えた数を取り出す
                int range = db_follow_list.Count - limit;

                // 上限数を超えた場合
                if (range > 0)
                {
                    using (var db = new SQLiteDataContext())
                    {
                        try
                        {
                            // データベースの存在確認
                            db.Database.EnsureCreated();

                            // トランザクションをかける
                            db.Database.BeginTransaction();

                            // limitオーバーしたサイズ分取得する
                            var del_list = db_follow_list.GetRange(0, range);

                            // 古いものから削除していく
                            foreach (var user in del_list)
                            {
                                FollowBackListBase.Delete(db, new FollowBackListBase()
                                { UserId = user.UserId });
                            }

                            db.SaveChanges();
                            db.Database.CommitTransaction();
                        }
                        catch (Exception e)
                        {
                            db.Database.RollbackTransaction();
                            Console.WriteLine(e.Message);
                            Logger.Error(e.Message);
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロバ対象ユーザーの検索
        /// <summary>
        /// フォロバ対象ユーザーの検索
        /// </summary>
        private static void TwapiSearch()
        {
            try
            {
                string[] keywords = null;

                // キーワードの入力確認
                if (TwitterArgs.CommandOptions.Keywords != null)
                {
                    keywords = TwitterArgs.CommandOptions.Keywords.Split(',');
                    if (keywords.Length <= 0)
                    {
                        Console.WriteLine("-keywordsが指定されていません。ex: -keywords ソフトウェア,プログラマー,プログラミング");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("-keywordsが指定されていません。ex: -keywords ソフトウェア,プログラマー,プログラミング");
                    return;
                }


                List<long> userid_list = new List<long>();

                // フォロー対象リストの取得
                var db_follow_list = (from x in FollowBackListBase.Select()
                                      select x.UserId).ToList<long>();

                // フォロバリストからuser_idのリスト作成
                userid_list.AddRange(db_follow_list);

                // フォロー対象リストの取得
                var friends_list = (from x in FrinedsLogBase.Select()
                                      select x.UserId).ToList<long>();

                // 現在フォローしているユーザーからuser_idのリストを作成
                userid_list.AddRange(friends_list);

                // 冗長を削除
                friends_list = friends_list.Distinct().ToList<long>();

                // フォローリストにもフォロバリストにもユーザーが登録されていない
                if (userid_list.Count <= 0)
                {
                    Console.Write("数名ツイッターでフォローしてから実行してください");
                    return;
                }

                // インデックスの取得
                var index = _Rand.Next(0, friends_list.Count);

                // ユーザーの取得
                var target_id = friends_list.ElementAt(index);

                // 対象ユーザーのフレンド（フォローしている人）を取得する
                var api_user_list = TwitterAPI.Token.Friends.List(
                    user_id => target_id, count => 100);

                Console.WriteLine(api_user_list.RateLimit.Remaining.ToString() + "/" + api_user_list.RateLimit.Limit.ToString());
                Console.WriteLine(api_user_list.RateLimit.Reset.LocalDateTime);

                // データベース操作
                using (var db = new SQLiteDataContext())
                {
                    try
                    {
                        // トランザクションをかける
                        db.Database.BeginTransaction();

                        foreach (var api_user in api_user_list)
                        {
                            foreach (var key in keywords)
                            {
                                if (api_user.Description.Contains(key))
                                {
                                    // データベース上に登録されているかを確認
                                    var db_friend = friends_list.Where(x => x.Equals(api_user.Id));

                                    // 存在しないならデータの作成
                                    if (!db_friend.Any())
                                    {
                                        FollowBackListBase.Insert(db, new FollowBackListBase()
                                        {
                                            UserId = api_user.Id.Value,
                                            InsertAt = DateTime.Now,
                                            UpdateAt = DateTime.Now,
                                            CreateAt = api_user.CreatedAt.LocalDateTime,
                                            Description = api_user.Description,
                                            FavouritesCount = api_user.FavouritesCount,
                                            FollowersCount = api_user.FollowersCount,
                                            FriendsCount = api_user.FriendsCount,
                                            LastTweet = api_user.Status == null ? string.Empty : api_user.Status.Text,
                                            LastTweetAt = api_user.Status == null ? null : api_user.Status.CreatedAt.LocalDateTime,
                                            ScreenName = api_user.ScreenName,
                                            TweetCount = api_user.StatusesCount,
                                            IsProtected = api_user.IsProtected,
                                            IsSuspended = api_user.IsSuspended
                                        });

                                        break;
                                    }
                                }
                            }
                        }
                        db.SaveChanges();
                        db.Database.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        db.Database.RollbackTransaction();
                        Console.WriteLine(e.Message);
                        Logger.Error(e.Message);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロー処理
        /// <summary>
        /// フォロー処理
        /// </summary>
        private static void TwapiFollow()
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();

                    double ff_min = 0.0, ff_max = 0.0;
                    int last_day = 0;
                    // 範囲指定(min)
                    if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.FFmin))
                    {
                        double.TryParse(TwitterArgs.CommandOptions.FFmin, out ff_min);
                    }

                    // 範囲指定(max)
                    if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.FFmax))
                    {
                        double.TryParse(TwitterArgs.CommandOptions.FFmax, out ff_max);
                    }

                    // 最終ツイート日指定
                    if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.LastDay))
                    {
                        int.TryParse(TwitterArgs.CommandOptions.LastDay, out last_day);
                    }


                    // フォローしていない人を抽出
                    var tmp = (from follow_list in db.DbSet_FollowBackList
                               join my_friends in db.DbSet_FrinedsLog
                               on follow_list.UserId equals my_friends.UserId into groupping
                               from my_friends in groupping.DefaultIfEmpty()
                               where my_friends == null      // 未だフォローしていない
                               && !follow_list.IsProtected   // プライベートアカウントでない
                               && !(follow_list.IsSuspended.HasValue && follow_list.IsSuspended.Value)   // ロックされていない
                               select new
                               {
                                   follow_list.UserId,
                                   follow_list.ScreenName,
                                   follow_list.LastTweetAt,
                                   ff_ratio = follow_list.FollowersCount == 0 ? 0 : follow_list.FriendsCount / (double)follow_list.FollowersCount
                               }).Where(x => (ff_min == 0.0 || x.ff_ratio >= ff_min) && (ff_max == 0.0 || x.ff_ratio <= ff_max)
                               && (last_day == 0 || (x.LastTweetAt.HasValue && DateTime.Now.AddDays(-last_day).CompareTo(x.LastTweetAt.Value) <= 0))).ToList();

                    // フォロー対象が見つかった
                    if (tmp.Any())
                    {
                        int index = _Rand.Next(0, tmp.Count());
                        var user = tmp.ToList().ElementAt(index);

                        // フォロバリストから削除
                        FollowBackListBase.Delete(db, new FollowBackListBase()
                        {
                            UserId = user.UserId
                        });

                        // フォローログに追加
                        FrinedsLogBase.Insert(db, new FrinedsLogBase()
                        {
                            UserId = user.UserId,
                            FollowAt = DateTime.Now,
                            RemoveAt = null
                        }
                        );

                        // フォロー処理
                        TwitterAPI.Token.Friendships.Create(user.UserId);

                        // データベースの保存処理
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロー解除処理
        /// <summary>
        /// フォロー解除処理
        /// </summary>
        public static void TwapiRemove()
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();

                    int last_day = 0;
                    int followdays = 0;

                    // 最終ツイート日指定
                    if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.LastDay))
                    {
                        int.TryParse(TwitterArgs.CommandOptions.LastDay, out last_day);
                    }
                    // 最終フォローした日からの期間
                    if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.FollowDays))
                    {
                        int.TryParse(TwitterArgs.CommandOptions.FollowDays, out followdays);
                    }

                    // フォローしていない人を抽出
                    var tmp = (from my_friends in db.DbSet_FrinedsLog
                               join my_follower in db.DbSet_FollowersLog
                               on my_friends.UserId equals my_follower.UserId into groupping
                               from my_followers in groupping.DefaultIfEmpty()
                               where (my_followers == null || my_followers.RemoveAt.HasValue)       // フォローされていないまたはフォロー解除された
                               && !my_friends.RemoveAt.HasValue     // フォローの解除をしていない
                               && (followdays == 0 || DateTime.Now.AddDays(-followdays).CompareTo(my_friends.FollowAt) >= 0)    // フォロー期間を過ぎた
                               select new
                               {
                                   my_friends.UserId,
                                   my_friends.FollowAt
                               }).ToList();

                    var tmp2 = (from x in tmp
                                join my_followerlist in db.DbSet_UserList
                                on x.UserId equals my_followerlist.UserId into grouping
                                from myfollowerlist in grouping.DefaultIfEmpty()
                                where (!myfollowerlist.LastTweetAt.HasValue || DateTime.Now.AddDays(-last_day).CompareTo(myfollowerlist.LastTweetAt.Value) >= 0) // 最終ツイート日からの経過時間を過ぎた
                                && !myfollowerlist.IsExclude
                                select new
                                {
                                    x.UserId,
                                    x.FollowAt
                                }).ToList();

                    // フォロー解除対象が見つかった
                    if (tmp2.Any())
                    {
                        int index = _Rand.Next(0, tmp2.Count());
                        var user = tmp2.ToList().ElementAt(index);

                        FrinedsLogBase.Update(db, new FrinedsLogBase()
                        {
                            UserId = user.UserId,
                            FollowAt = user.FollowAt,
                            RemoveAt = null
                        }
                        , new FrinedsLogBase()
                        {
                            UserId = user.UserId,
                            FollowAt = user.FollowAt,
                            RemoveAt = DateTime.Now
                        }
                        );

                        TwitterAPI.Token.Friendships.Destroy(user.UserId);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion
        
        #region 自動ツイート
        /// <summary>
        /// 自動ツイート
        /// </summary>
        private static void AutoTweet()
        {
            try
            {
                string path = TwitterArgs.CommandOptions.ExcelPath;

                // ファイルパスが指定されていない場合
                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine("ファイルパスが指定されていません。");
                    return;
                }

                // 指定されたファイルパスが存在しない場合
                if (!File.Exists(path))
                {
                    Console.WriteLine("ファイルが存在しません。");
                    return;
                }

                // 読み取り専用で開く
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // Bookの操作
                    using (XLWorkbook book = new XLWorkbook(fs, XLEventTracking.Disabled))
                    {
                        // シートの一番目を取得
                        var sheet = book.Worksheets.ElementAt(0);
                        List<KeyValuePair<string, string>> msg_list = new List<KeyValuePair<string, string>>();
                        int row = 2;    // 先頭行はヘッダとして扱う

                        // ループ
                        while (true)
                        {
                            
                            string msg = sheet.Cell(row, 1).Value != null ? sheet.Cell(row, 1).Value.ToString() : string.Empty; // メッセージの取得
                            string url = sheet.Cell(row, 2).Value != null ? sheet.Cell(row, 2).Value.ToString() : string.Empty; // URLの取得

                            // メッセージ部が空ならば抜ける
                            if (string.IsNullOrEmpty(msg))
                            {
                                break;
                            }
                            else
                            {
                                // リストに追加（メッセージとURLをセットで保持）
                                msg_list.Add(new KeyValuePair<string, string>(msg, url));
                            }
                            row++;  // 行をインクリメント
                        }

                        int max = 280;  // ツイート数最大値
                        if (msg_list.Count > 0)
                        {
                            // ツイート内容の取得（ランダム）
                            int index = _Rand.Next(0, msg_list.Count);
                            var lst = msg_list.ElementAt(index);
                            var msg = lst.Key;      // メッセージの取り出し
                            var url = lst.Value;    // URLの取り出し

                            if (string.IsNullOrEmpty(url))
                            {
                                // バイト数で切り取る
                                msg = StrUtil.SubstringByte(lst.Key, max);
                            }
                            else
                            {
                                // URLは23文字扱いのため-23 改行を含むので24文字
                                msg = StrUtil.SubstringByte(lst.Key, max - 24);
                                msg = msg + "\r\n" + url;
                            }

                            // ツイート
                            TwitterAPI.Token.Statuses.Update(msg);
                            Console.WriteLine(msg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        private static string YesNoQuestion(string msg)
        {
            string ans = "-";

            // 入力内容を確認 y/yes or n/no出ない場合は再度確認
            while (!ans.Equals("y") && !ans.Equals("yes") && !ans.Equals("n") && !ans.Equals("no"))
            {
                Console.WriteLine(msg);
                ans = Console.ReadLine();   // 入力待ち
                ans = string.IsNullOrWhiteSpace(ans) ? string.Empty : ans.Trim().ToLower(); // nullチェックして小文字に変換
            }
            return ans;
        }


        #endregion

        #region Public Method
        #region ヘルプ
        /// <summary>
        /// ヘルプ
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void Help(string action)
        {
            try
            {
                Console.WriteLine("使用方法：");
                Console.WriteLine("\ttwapi /actioncommand [-options]");

                Console.WriteLine("");
                Console.WriteLine("actioncommand :");

                foreach (var tmp in TwitterActions.Actions)
                {
                    Console.WriteLine($"\t{tmp.ActionName}\t...{tmp.Help}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region キーの登録処理
        /// <summary>
        /// キーの登録処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void Regist(string action)
        {
            try
            {
                XMLUtil.Seialize<TwitterKeys>(ConfigManager.KeysFile, TwitterAPI.TwitterKeys);
                Console.WriteLine("各種キー(コンシューマーキー・コンシューマーシークレット・アクセストークン・アクセスシークレット)を保存しました");
                Console.WriteLine("==>" + ConfigManager.KeysFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region 自分がフォローしているユーザーとフォローされているユーザーを更新する
        /// <summary>
        /// 自分がフォローしているユーザーとフォローされているユーザーを更新する
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void TwapiUpdate(string action)
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // フォローバックリストの更新
                TwapiUpdate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロバさん検索
        /// <summary>
        /// フォロバさん検索
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void TwapiSearch(string action)
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // フォローバックリストの更新
                TwapiSearch();

                // フォロバリストの調整（上限値でフォロバリストのユーザー数を制限する）
                AdjustFollowbackList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロー処理
        /// <summary>
        /// フォロー処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void TwapiFollow(string action)
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // フォローバックリストの更新
                TwapiFollow();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region フォロー解除処理
        /// <summary>
        /// フォロー解除処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void TwapiRemove(string action)
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // フォロー解除処理の実行
                TwapiRemove();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #region 自動ツイート
        /// <summary>
        /// 自動ツイート
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void AutoTweet(string action)
        {
            try
            {
                // フォロー解除処理の実行
                AutoTweet();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Logger.Error(e.Message);
                throw;
            }
        }
        #endregion

        #endregion
        #endregion
    }
}
