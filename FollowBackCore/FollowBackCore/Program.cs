using ClosedXML.Excel;
using FollowBackCore.Database.SQLite;
using FollowBackCore.Database.SQLite.Base;
using FollowBackCore.Twitter;
using FollowBackCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace FollowBackCore
{
    class Program
    {
        static string _Action = string.Empty;

        public static string SQLitePath { get; set; }

        public static TwitterArgs TwitterArgs { get; set; }

        /// <summary>
        /// JSONファイルの出力
        /// </summary>
        /// <param name="json">JSONデータ(テキスト)</param>
        /// <param name="action">アクション名</param>
        static void OutputJSON(string json, string action)
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
        /// TwitterAPI
        /// 参考：https://syncer.jp/Web/API/Twitter/REST_API/
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // キーファイルの存在確認
            if (File.Exists(@"Config\Keys"))
            {
                // キーの読み込み
                TwitterAPI.TwitterKeys = XMLUtil.Deserialize<TwitterKeys>(@"Config\Keys");
            }

            // 引数のセット
            TwitterArgs.SetCommand(args);

            var action = TwitterArgs.Args["action"];

            switch (action)
            {
                case "/?":
                case "/h":
                    {
                        Console.WriteLine("使用方法：");
                        Console.WriteLine("\tFollowBackCore.exe [actioncommand] [-keys] [-options]");

                        Console.WriteLine("");
                        Console.WriteLine("actioncommand :");

                        foreach (var tmp in TwitterArgs.Commands.Items)
                        {
                            string imp = tmp.IsEnable ? "" : "(未実装)";
                            Console.WriteLine($"\t{imp}{tmp.Key}\t...{tmp.Description}");
                        }

                        break;
                    }
                case "regist":
                    {
                        //string  = _Args.ContainsKey("-ck") ? _Args["-ck"] : null;

                        if (!Directory.Exists("Config"))
                        {
                            Directory.CreateDirectory("Config");
                        }

                        XMLUtil.Seialize<TwitterKeys>(@"Config\Keys", TwitterAPI.TwitterKeys);


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
                        // 検索
                        var result = TwitterAPI.Token.Search.Tweets(
                            q => TwitterArgs.CommandOptions.Q,
                            geocode => TwitterArgs.CommandOptions.Geocode,
                            lang => TwitterArgs.CommandOptions.Lang,
                            locale => TwitterArgs.CommandOptions.Lang,
                            result_type => TwitterArgs.CommandOptions.Result_type,
                            count => TwitterArgs.CommandOptions.Count,
                            until => TwitterArgs.CommandOptions.Until,
                            since_id => TwitterArgs.CommandOptions.Since_id,
                            max_id => TwitterArgs.CommandOptions.Max_id,
                            include_entities => TwitterArgs.CommandOptions.Include_entities
                            );

                        Console.WriteLine(result.Json);
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);
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
                        // 検索
                        var result = TwitterAPI.Token.Blocks.Ids(cursor=>TwitterArgs.CommandOptions.Cursor);

                        Console.WriteLine(result.Json);
                        Console.WriteLine();
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);

                        break;
                    }
                case "blocks/list":
                    {
                        // 検索
                        var result = TwitterAPI.Token.Blocks.List(
                            cursor => TwitterArgs.CommandOptions.Cursor,
                            include_entities => TwitterArgs.CommandOptions.Include_entities,
                            skip_status => TwitterArgs.CommandOptions.Skip_status,
                            include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text
                            );

                        Console.WriteLine(result.Json);
                        Console.WriteLine();
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);

                        break;
                    }
                case "followers/ids":
                    {
                        // 検索
                        var result = TwitterAPI.Token.Followers.Ids(
                            user_id => TwitterArgs.CommandOptions.User_id,
                            screen_name => TwitterArgs.CommandOptions.Screen_name,
                            cursor=> TwitterArgs.CommandOptions.Cursor,
                            count=> TwitterArgs.CommandOptions.Count
                            );

                        Console.WriteLine(result.Json);
                        Console.WriteLine();
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

                        OutputJSON(result.Json, action);

                        break;
                    }

            }
        }

    }
}
