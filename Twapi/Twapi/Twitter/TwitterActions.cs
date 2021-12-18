﻿using Twapi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twapi.Database.SQLite.Base;
using Twapi.Database.SQLite;

namespace Twapi.Twitter
{
    public class TwitterActions
    {

        public static List<TwitterAction> Actions = new List<TwitterAction>()
        {
            new TwitterAction("/test", Test),
            new TwitterAction("/?", Help),
            new TwitterAction("/h", Help),
            new TwitterAction("regist", Regist),
            new TwitterAction("application/rate_limit_status", ApplicationRateLimitStatus),
            new TwitterAction("friendships/create", FriendshipsCreate),
            new TwitterAction("friendships/destroy", FriendshipsDestroy),
            new TwitterAction("friends/list", FriendshipsDestroy),
            new TwitterAction("followers/list", FriendshipsDestroy),
            new TwitterAction("followbacklist/create", FollowbacklistCreate)
        };


        #region 出力処理
        #region JSONファイルの出力
        /// <summary>
        /// JSONファイルの出力
        /// </summary>
        /// <param name="json">JSONデータ(テキスト)</param>
        /// <param name="action">アクション名</param>
        public static void OutputJSON(string json, string action)
        {
            var check = (from x in TwitterArgs.Args
                         where x.Key.Equals("-d")
                         select x).Any();

            if (check)
            {
                var dir = TwitterArgs.Args["-d"].Trim();

                if (Directory.Exists(dir))
                {
                    var out_dir = Path.Combine(dir, action.Replace("-", "").Replace("/", ""));

                    // ディレクトリの存在確認
                    if (!Directory.Exists(out_dir))
                    {
                        // ディレクトリの作成
                        Directory.CreateDirectory(out_dir);
                    }

                    string file_name = Path.Combine(out_dir, DateTime.Now.ToString("yyyyMMddHHmmss") + ".json");

                    File.WriteAllText(file_name, json);
                    Console.WriteLine("出力先：" + file_name);
                }
                else
                {
                    Console.WriteLine("指定したディレクトリが存在しません。\r\n" + dir);
                }
            }
        }
        #endregion

        #region 結果の出力処理
        /// <summary>
        /// 結果の出力処理
        /// </summary>
        /// <param name="json">JSONデータ</param>
        /// <param name="action">アクション</param>
        /// <param name="rateLimit"></param>
        public static void ResultOutput(string json, string action, CoreTweet.RateLimit rateLimit)
        {
            // JSONデータのコンソール出力
            Console.WriteLine(json);

            // RateLimitの出力
            Console.WriteLine(rateLimit.Remaining.ToString() + "/" + rateLimit.Limit.ToString());

            // JSONファイルの出力（ファイル指定時のみ）
            OutputJSON(json, action);
        }
        #endregion
        #endregion

        #region アクション
        public static void Test(string action)
        {
            try
            {
                var result = TwitterAPI.Token.Friendships.Lookup(new List<string> { "Zeikomi552", "route20191212", "OtoJiu", "enginer_ni_naru" });
                // 結果出力
                TwitterActions.ResultOutput(result.Json, action, result.RateLimit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

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
                Console.WriteLine("\tTwapi [actioncommand] [-keys] [-options]");

                Console.WriteLine("");
                Console.WriteLine("actioncommand :");

                foreach (var tmp in TwitterArgs.Commands.Items)
                {
                    if (!tmp.IsEnable)
                        continue;

                    Console.WriteLine($"\t{tmp.Key}\t...{tmp.Description}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region RateLimitの取得処理
        /// <summary>
        /// RateLimitの取得処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void ApplicationRateLimitStatus(string action)
        {
            try
            {
                // RateLimitの取得
                var result = TwitterAPI.Token.Application.RateLimitStatus();

                // 結果出力
                ResultOutput(result.Json, action, result.RateLimit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                // Configフォルダの存在確認
                if (!Directory.Exists("Config"))
                {
                    Directory.CreateDirectory("Config");
                }

                XMLUtil.Seialize<TwitterKeys>(@"Config\Keys", TwitterAPI.TwitterKeys);
                Console.WriteLine("各種キー(コンシューマーキー・コンシューマーシークレット・アクセストークン・アクセスシークレット)を保存しました");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region フォロー処理
        /// <summary>
        /// フォロー処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void FriendshipsCreate(string action)
        {
            try
            {
                CoreTweet.UserResponse result = null;
                // スクリーン名が指定されているかの確認
                if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.Screen_name))
                {
                    // フォロー処理の実行
                    result = TwitterAPI.Token.Friendships.Create(TwitterArgs.CommandOptions.Screen_name);
                }
                // ユーザーIDが指定されているかの確認
                else if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.User_id))
                {
                    long id = long.TryParse(TwitterArgs.CommandOptions.User_id, out id) ? id : -1;

                    // IDの整合チェック
                    if (id > 0)
                    {
                        // IDを使用してフォロー処理の実行
                        result = TwitterAPI.Token.Friendships.Create(id);
                    }
                    else
                    {
                        Console.WriteLine("不正なコマンドです。user_idは数値で指定してください");
                    }
                }
                else
                {
                    Console.WriteLine("不正なコマンドです。screen_nameまたはuser_idを指定してください");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region フォローの解除
        /// <summary>
        /// フォローの解除
        /// </summary>
        /// <param name="action">アクション名</param>
        public static void FriendshipsDestroy(string action)
        {
            try
            {
                CoreTweet.UserResponse result = null;
                // スクリーン名が指定されているかの確認
                if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.Screen_name))
                {
                    // フォロー処理の実行
                    result = TwitterAPI.Token.Friendships.Destroy(TwitterArgs.CommandOptions.Screen_name);
                }
                // ユーザーIDが指定されているかの確認
                else if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.User_id))
                {
                    long id = long.TryParse(TwitterArgs.CommandOptions.User_id, out id) ? id : -1;

                    // IDの整合チェック
                    if (id > 0)
                    {
                        // IDを使用してフォロー処理の実行
                        result = TwitterAPI.Token.Friendships.Destroy(id);
                    }
                    else
                    {
                        Console.WriteLine("不正なコマンドです。user_idは数値で指定してください");
                    }
                }
                else
                {
                    Console.WriteLine("不正なコマンドです。screen_nameまたはuser_idを指定してください");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region フォローリストの取得
        /// <summary>
        /// フォローリストの取得
        /// </summary>
        /// <param name="action">アクション名</param>
        public void FriendsList(string action)
        {
            try
            {
                long next_cursor = -1;

                while (next_cursor != 0)
                {

                    // 検索
                    var result = TwitterAPI.Token.Friends.List(
                        user_id => TwitterArgs.CommandOptions.User_id,
                        screen_name => TwitterArgs.CommandOptions.Screen_name,
                        cursor => next_cursor == -1 ? null : next_cursor,
                        count => TwitterArgs.CommandOptions.Count,
                        skip_status => TwitterArgs.CommandOptions.Skip_status,
                        include_user_entities => TwitterArgs.CommandOptions.Include_entities,
                        include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text,
                        tweet_mode => TwitterArgs.CommandOptions.Tweet_mode
                        );


                    Console.WriteLine(result.Json);
                    Console.WriteLine(result.NextCursor);
                    Console.WriteLine();
                    Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                    OutputJSON(result.Json, action);
                    System.Threading.Thread.Sleep(1000);

                    // 残り回数が0なら待つ
                    if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
                    {
                        Console.WriteLine($"Wait {DateTime.Now.AddMinutes(15)}");
                        System.Threading.Thread.Sleep(15 * 60 * 1000);
                    }

                    next_cursor = result.NextCursor;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region フォローリストの取得処理
        /// <summary>
        /// フォローリストの取得処理
        /// </summary>
        /// <param name="action">アクション名</param>
        public void FollowersList(string action)
        {
            try
            {
                long next_cursor = -1;

                while (next_cursor != 0)
                {

                    // 検索
                    var result = TwitterAPI.Token.Followers.List(
                        user_id => TwitterArgs.CommandOptions.User_id,
                        screen_name => TwitterArgs.CommandOptions.Screen_name,
                        cursor => next_cursor == -1 ? null : next_cursor,
                        count => TwitterArgs.CommandOptions.Count,
                        skip_status => TwitterArgs.CommandOptions.Skip_status,
                        include_user_entities => TwitterArgs.CommandOptions.Include_entities,
                        include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text,
                        tweet_mode => TwitterArgs.CommandOptions.Tweet_mode
                        );


                    Console.WriteLine(result.Json);
                    Console.WriteLine(result.NextCursor);
                    Console.WriteLine();
                    Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                    OutputJSON(result.Json, action);
                    System.Threading.Thread.Sleep(1000);

                    // 残り回数が0なら待つ
                    if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
                    {
                        Console.WriteLine($"Wait {DateTime.Now.AddMinutes(15)}");
                        System.Threading.Thread.Sleep(15 * 60 * 1000);
                    }

                    next_cursor = result.NextCursor;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

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
                                update.RemoveAt = DateTime.Now;

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
                        Console.WriteLine(e.Message);
                        db.Database.RollbackTransaction();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                                update.RemoveAt = DateTime.Now;

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
                        Console.WriteLine(e.Message);
                        db.Database.RollbackTransaction();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion


        #region フォロバリストの自分のフォロワーとフォロー情報を更新する
        /// <summary>
        /// フォロバリストの自分のフォロワーとフォロー情報を更新する
        /// </summary>
        private static void UpdateFollowBackList()
        {
            try
            {
                // データベースの存在保証
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // 自分のアカウントの情報取得
                var settings = TwitterAPI.Token.Account.Settings();

                // フォロワーリストの取得
                RegistFollowerList();

                // フォローリストの取得
                RegistFriendList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        public static void FollowbacklistCreate(string action)
        {
            try
            {
                using (var db = new SQLiteDataContext())
                {
                    db.Database.EnsureCreated();
                }

                // フォローバックリストの更新
                UpdateFollowBackList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion
    }
}
