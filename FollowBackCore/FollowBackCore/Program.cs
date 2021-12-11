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











                    //case "/trend.place":
                    //    {
                    //        // woeid https://qiita.com/hogeta_/items/8e3224c4960e19b7a33a

                    //        var tmp = TwitterAPI.Place(TwitterAPI.TwitterKeys, 1117817);
                    //        foreach (var result in tmp)
                    //        {
                    //            foreach (var tr in result.Trends)
                    //            {
                    //                Console.WriteLine(tr.Name);
                    //            }
                    //        }
                    //        break;
                    //    }
                    //case "/test":
                    //    {

                    //        TwitterAPI.Test(TwitterAPI.TwitterKeys, "orta");

                    //        break;
                    //    }
                    //case "/tw":
                    //    {
                    //        var message = _Args["message"];
                    //        TwitterAPI.Tweet(TwitterAPI.TwitterKeys, message.Replace("\\r\\n", "\r\n"));
                    //        break;
                    //    }
                    //case "/cfr":
                    //    {
                    //        string screen_name = _Args["screen_name"];
                    //        TwitterAPI.CreateFollow(TwitterAPI.TwitterKeys, screen_name);
                    //        break;
                    //    }
                    //case "/dfr":
                    //    {
                    //        string screen_name = _Args["screen_name"];
                    //        TwitterAPI.BreakFollow(TwitterAPI.TwitterKeys, screen_name);
                    //        break;
                    //    }
                    //case "/tws":
                    //    {
                    //        string search_key = _Args["search_keyword"];
                    //        var ret = TwitterAPI.TweetSearch(TwitterAPI.TwitterKeys, search_key);
                    //        SQLitePath = _Args["file_path"];

                    //        using (var db = new SQLiteDataContext())
                    //        {
                    //            db.Database.EnsureCreated();
                    //        }


                    //        DateTime insert_time = DateTime.Now;
                    //        string guid = Guid.NewGuid().ToString();

                    //        foreach (var tmp in ret)
                    //        {
                    //            TwitterSearchResultsBase data = new TwitterSearchResultsBase();
                    //            data.CreateDt = insert_time;
                    //            data.Guid = guid;
                    //            data.Id = tmp.Id;
                    //            data.FavoritesCount = tmp.FavoriteCount;
                    //            if (tmp.User != null)
                    //            {
                    //                data.UserId = tmp.User.Id;
                    //                data.ScreenName = tmp.User.ScreenName;
                    //                data.FriendsCount = tmp.User.FriendsCount;
                    //                data.FollowerCount = tmp.User.FollowersCount;
                    //            }

                    //            data.Text = tmp.Text;


                    //            TwitterSearchResultsBase.Insert(data);
                    //        }

                    //        //int index = 1;

                    //        //// Excelファイルを作る
                    //        //using (var workbook = new XLWorkbook())
                    //        //{
                    //        //    var worksheet = workbook.Worksheets.Add("result");
                    //        //    // セルに値や数式をセット
                    //        //    worksheet.Cell($"A{index}").Value = "tweet.ID";
                    //        //    worksheet.Cell($"B{index}").Value = "user.Id";
                    //        //    worksheet.Cell($"C{index}").Value = "screen_name";
                    //        //    worksheet.Cell($"D{index}").Value = "text";
                    //        //    worksheet.Cell($"E{index}").Value = "FavoriteCount";
                    //        //    worksheet.Cell($"F{index}").Value = "user.FriendsCount";
                    //        //    worksheet.Cell($"G{index}").Value = "user.FollowerCount";
                    //        //    worksheet.Cell($"H{index}").Value = "CreateAt";

                    //        //    foreach (var tw in ret)
                    //        //    {
                    //        //        index++;
                    //        //        worksheet.Cell($"A{index}").Value = tw.Id;
                    //        //        worksheet.Cell($"B{index}").Value = tw.User.Id;
                    //        //        worksheet.Cell($"C{index}").Value = tw.User.ScreenName;
                    //        //        worksheet.Cell($"D{index}").Value = tw.Text;
                    //        //        worksheet.Cell($"E{index}").Value = tw.FavoriteCount;
                    //        //        worksheet.Cell($"F{index}").Value = tw.User.FriendsCount;
                    //        //        worksheet.Cell($"G{index}").Value = tw.User.FollowersCount;
                    //        //        worksheet.Cell($"H{index}").Value = tw.CreatedAt.LocalDateTime;
                    //        //    }

                    //        //    string file_path = _Args["file_path"];
                    //        //    // ワークブックを保存する
                    //        //    workbook.SaveAs(file_path);
                    //        //}

                    //        break;
                    //    }
            }
        }

    }
}
