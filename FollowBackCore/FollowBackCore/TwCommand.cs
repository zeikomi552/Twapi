using FollowBackCore.Twitter;
using FollowBackCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore
{
    public class TwCommand
    {
        static string _Action = string.Empty;

        public static string SQLitePath { get; set; }

        public static TwitterArgs TwitterArgs { get; set; }

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


        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="args">パラメータ</param>
        public static void ExecuteCommand(string[] args)
        {
            // キーファイルの存在確認
            if (File.Exists(@"Config\Keys"))
            {
                // キーの読み込み
                TwitterAPI.TwitterKeys = XMLUtil.Deserialize<TwitterKeys>(@"Config\Keys");
            }

            // 引数のセット
            TwitterArgs.SetCommand(args);

            string action = string.Empty;

            if (TwitterArgs.Args.ContainsKey("action"))
            {
                action = TwitterArgs.Args["action"];
            }
            else
            {
                Console.WriteLine("不正なコマンドです。\r\n");
                action = "/?";
            }


            switch (action)
            {
                case "/?":
                case "/h":
                    {
                        Console.WriteLine("使用方法：");
                        Console.WriteLine("\tFollowBackCore [actioncommand] [-keys] [-options]");

                        Console.WriteLine("");
                        Console.WriteLine("actioncommand :");

                        foreach (var tmp in TwitterArgs.Commands.Items)
                        {
                            if (!tmp.IsEnable)
                                continue;

                            Console.WriteLine($"\t{tmp.Key}\t...{tmp.Description}");
                        }

                        break;
                    }
                case "regist":
                    {
                        // Configフォルダの存在確認
                        if (!Directory.Exists("Config"))
                        {
                            Directory.CreateDirectory("Config");
                        }

                        XMLUtil.Seialize<TwitterKeys>(@"Config\Keys", TwitterAPI.TwitterKeys);
                        Console.WriteLine("各種キー(コンシューマーキー・コンシューマーシークレット・アクセストークン・アクセスシークレット)を保存しました");
                        break;
                    }
                case "account/settings":
                    {
                        var result = TwitterAPI.Token.Account.Settings();
                        Console.WriteLine(result.Json);
                        OutputJSON(result.Json, action);
                        break;
                    }
                case "search/tweets":
                    {
                        long last_id = -1;

                        while (last_id != 0)
                        {
                            // 検索
                            var result = TwitterAPI.Token.Search.Tweets(
                                q => TwitterArgs.CommandOptions.Q,
                                geocode => TwitterArgs.CommandOptions.Geocode,
                                lang => TwitterArgs.CommandOptions.Lang,
                                locale => TwitterArgs.CommandOptions.Locale,
                                result_type => TwitterArgs.CommandOptions.Result_type,
                                count => TwitterArgs.CommandOptions.Count,
                                until => TwitterArgs.CommandOptions.Until,
                                since_id => TwitterArgs.CommandOptions.Since_id,
                                max_id => last_id < 0 ? null : last_id,
                                include_entities => TwitterArgs.CommandOptions.Include_entities
                            );

                            Console.WriteLine(result.Json);
                            Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                            OutputJSON(result.Json, action);
                            System.Threading.Thread.Sleep(1000);

                            var last = result.LastOrDefault();

                            if (last != null)
                            {
                                last_id = last.Id;
                            }

                            // 残り回数が0なら待つ抜ける
                            if (last_id != 0 && result.RateLimit.Remaining == 0)
                            {
                                //System.Threading.Thread.Sleep(15 * 60 * 1000);
                                break;
                            }
                        }

                        break;
                    }
                case "application/rate_limit_status":
                    {
                        // 検索
                        var result = TwitterAPI.Token.Application.RateLimitStatus();

                        Console.WriteLine(result.Json);
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);

                        break;
                    }
                case "account/verify_credentials":
                    {
                        // 検索
                        var result = TwitterAPI.Token.Account.VerifyCredentials();

                        Console.WriteLine(result.Json);
                        Console.WriteLine();
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);

                        break;
                    }
                case "blocks/ids":
                    {
                        long next_cursor = -1;

                        while (next_cursor != 0)
                        {
                            // 検索
                            var result = TwitterAPI.Token.Blocks.Ids(cursor => TwitterArgs.CommandOptions.Cursor);

                            Console.WriteLine(result.Json);
                            Console.WriteLine();
                            Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                            OutputJSON(result.Json, action);
                            next_cursor = result.NextCursor;
                        }

                        break;
                    }
                case "blocks/list":
                    {
                        long next_cursor = -1;

                        while (next_cursor != 0)
                        {
                            // 検索
                            var result = TwitterAPI.Token.Blocks.List(
                                cursor => next_cursor == -1 ? null : next_cursor,
                                include_entities => TwitterArgs.CommandOptions.Include_entities,
                                skip_status => TwitterArgs.CommandOptions.Skip_status,
                                include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text
                            );

                            Console.WriteLine(result.Json);
                            Console.WriteLine();
                            Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                            OutputJSON(result.Json, action);
                            System.Threading.Thread.Sleep(1000);

                            // 残り回数が0なら待つ
                            if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
                            {
                                System.Threading.Thread.Sleep(15 * 60 * 1000);
                            }

                            next_cursor = result.NextCursor;
                        }

                        break;
                    }
                case "followers/ids":
                    {
                        long next_cursor = -1;

                        while (next_cursor != 0)
                        {

                            // 検索
                            var result = TwitterAPI.Token.Followers.Ids(
                                user_id => TwitterArgs.CommandOptions.User_id,
                                screen_name => TwitterArgs.CommandOptions.Screen_name,
                                cursor => next_cursor == -1 ? null : next_cursor,
                                count => TwitterArgs.CommandOptions.Count
                            );

                            Console.WriteLine(result.Json);
                            Console.WriteLine();
                            Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                            OutputJSON(result.Json, action);

                            System.Threading.Thread.Sleep(1000);

                            // 残り回数が0なら待つ
                            if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
                            {
                                System.Threading.Thread.Sleep(15 * 60 * 1000);
                            }

                            next_cursor = result.NextCursor;
                        }

                        break;
                    }
                case "followers/list":
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

                        break;
                    }
                case "friends/list":
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

                        break;
                    }
                case "friendships/create":
                case "friendships/destroy":
                    {
                        if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.Screen_name))
                        {
                            if (action == "friendships/create")
                            {
                                TwitterAPI.Token.Friendships.Create(TwitterArgs.CommandOptions.Screen_name);
                            }
                            else
                            {
                                TwitterAPI.Token.Friendships.Destroy(TwitterArgs.CommandOptions.Screen_name);
                            }
                        }
                        else if (!string.IsNullOrEmpty(TwitterArgs.CommandOptions.User_id))
                        {
                            long id = long.TryParse(TwitterArgs.CommandOptions.User_id, out id) ? id : -1;
                            if (id > 0)
                            {
                                if (action == "friendships/create")
                                {
                                    TwitterAPI.Token.Friendships.Create(id);
                                }
                                else
                                {
                                    TwitterAPI.Token.Friendships.Destroy(id);
                                }
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
                        break;
                    }
                case "statuses/update":
                    {
                        TwitterAPI.Token.Statuses.Update(status=> TwitterArgs.CommandOptions.Status);
                        break;
                    }
                case "statuses/home_timeline":
                    {
                        var result = TwitterAPI.Token.Statuses.HomeTimeline(
                            count=> TwitterArgs.CommandOptions.Count,
                            since_id=> TwitterArgs.CommandOptions.Since_id,
                            max_id=> TwitterArgs.CommandOptions.Max_id,
                            trim_user => TwitterArgs.CommandOptions.Trim_User,
                            exclude_replies=> TwitterArgs.CommandOptions.Exclude_replies,
                            contributor_details=> TwitterArgs.CommandOptions.Contributor_details,
                            include_entities=> TwitterArgs.CommandOptions.Include_entities,
                            include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text,
                            tweet_mode => TwitterArgs.CommandOptions.Tweet_mode
                        );
                        OutputJSON(result.Json, action);

                        break;
                    }
            }
        }



    }
}
