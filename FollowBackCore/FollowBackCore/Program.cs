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
        static Dictionary<string, string> _Args = new Dictionary<string, string>();
        static string _Action = string.Empty;

        public static string SQLitePath { get; set; }
        public static CommandList _Commands = new CommandList();

        static void SetArgs(string[] args)
        {

            for (int i = 0; i < args.Length; i++)
            {

                var check = (from x in _Commands.Items
                           where x.Key.Equals(args[i].ToLower())
                           select x).FirstOrDefault();

                if (check != null)
                {
                    if (check.CommandType == Command.CommandTypeEnum.Action)
                    {
                        // アクションキーが存在しないことを確認する
                        if (!_Args.ContainsKey("action"))
                        {
                            // キーの登録
                            _Args.Add("action", args.Length > i ? args[i] : string.Empty);
                        }
                    }
                    else if (check.CommandType == Command.CommandTypeEnum.Keys)
                    {
                        switch (args[i].ToLower())
                        {
                            case "-consumer_key":
                            case "-ck":
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.ConsumerKey = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-consumer_secret":
                            case "-cs":
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.ConsumerSecretKey = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-access_token":
                            case "-at":
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.AccessToken = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-access_secret":
                            case "-as":
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.AccessSecret = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                        }
                    }
                    else if (check.CommandType == Command.CommandTypeEnum.Option)
                    {
                        switch (args[i].ToLower())
                        {
                            case "-d":
                                {
                                    i++;
                                    _Args.Add(check.Key.ToLower(), args.Length > i ? args[i] : string.Empty);
                                    break;
                                }
                        }
                    }
                    else if (check.CommandType == Command.CommandTypeEnum.Parameter)
                    {
                        i++;
                        _Args.Add(check.Key.ToLower(), args.Length > i ? args[i] : string.Empty);
                    }
                }
                //else
                //{
                //    switch (args[i].ToLower())
                //    {
                //        case "-msg":
                //            {
                //                _Args.Add("message", args.Length > i ? args[i] : string.Empty);
                //                break;
                //            }
                //        case "-scn":
                //            {
                //                i++;
                //                _Args.Add("screen_name", args.Length > i ? args[i] : string.Empty);
                //                break;
                //            }
                //        case "-kwd":
                //            {
                //                i++;
                //                _Args.Add("search_keyword", args.Length > i ? args[i] : string.Empty);
                //                break;
                //            }
                //        case "-f":
                //            {
                //                i++;
                //                _Args.Add("file_path", args.Length > i ? args[i] : string.Empty);
                //                break;
                //            }
                //        case "-ckey":
                //            {
                //                i++;
                //                TwitterAPI.TwitterKeys.ConsumerKey = args.Length > i ? args[i] : string.Empty;
                //                break;
                //            }
                //        case "-cscr":
                //            {
                //                i++;
                //                TwitterAPI.TwitterKeys.ConsumerSecretKey = args.Length > i ? args[i] : string.Empty;
                //                break;
                //            }
                //        case "-atkn":
                //            {
                //                i++;
                //                TwitterAPI.TwitterKeys.AccessToken = args.Length > i ? args[i] : string.Empty;
                //                break;
                //            }
                //        case "-ascr":
                //            {
                //                i++;
                //                TwitterAPI.TwitterKeys.AccessSecret = args.Length > i ? args[i] : string.Empty;
                //                break;
                //            }

                //    }
                //}
            }
        }

        /// <summary>
        /// JSONファイルの出力
        /// </summary>
        /// <param name="json">JSONデータ(テキスト)</param>
        /// <param name="action">アクション名</param>
        static void OutputJSON(string json, string action)
        {
            var check = (from x in _Args
                       where x.Key.Equals("-d")
                       select x).Any();

            if (check)
            {
                var dir = _Args["-d"].Trim();

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
            SetArgs(args);
            var action = _Args["action"];

            switch (action)
            {
                case "/?":
                case "/h":
                    {
                        Console.WriteLine("使用方法：");
                        Console.WriteLine("\tFollowBackCore.exe [actioncommand] [-keys] [-options]");

                        Console.WriteLine("");
                        Console.WriteLine("actioncommand :");

                        foreach (var tmp in _Commands.Items)
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
                        string q1 = _Args.ContainsKey("-q") ? _Args["-q"] : null;
                        var geocode1 = _Args.ContainsKey("-geocode") ? _Args["-geocode"] : null;
                        var lang1 = _Args.ContainsKey("-lang") ? _Args["-lang"] : null;
                        var locale1 = _Args.ContainsKey("-locale") ? _Args["-locale"] : null;
                        var result_type1 = _Args.ContainsKey("-result_type") ? _Args["-result_type"] : null;
                        var count1 = _Args.ContainsKey("-count") ? _Args["-count"] : null;
                        var until1 = _Args.ContainsKey("-until") ? _Args["-until"] : null;
                        var since_id1 = _Args.ContainsKey("-since_id") ? _Args["-since_id"] : null;
                        var max_id1 = _Args.ContainsKey("-max_id") ? _Args["-max_id"] : null;
                        var include_entities1 = _Args.ContainsKey("-include_entities") ? _Args["-include_entities"] : null;


                        // 検索
                        var result = TwitterAPI.Token.Search.Tweets(
                            q => q1,
                            geocode => geocode1,
                            lang => lang1,
                            locale => locale1,
                            result_type => result_type1,
                            count => count1,
                            until => until1,
                            since_id => since_id1,
                            max_id => max_id1,
                            include_entities => include_entities1
                            );


                        Console.WriteLine(result.Json);
                        Console.WriteLine();
                        Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" +  result.RateLimit.Limit.ToString());


                        OutputJSON(result.Json, action);
                        break;
                    }













                case "/trend.place":
                    {
                        // woeid https://qiita.com/hogeta_/items/8e3224c4960e19b7a33a

                        var tmp = TwitterAPI.Place(TwitterAPI.TwitterKeys, 1117817);
                        foreach (var result in tmp)
                        {
                            foreach (var tr in result.Trends)
                            {
                                Console.WriteLine(tr.Name);
                            }
                        }
                        break;
                    }
                case "/test":
                    {

                        TwitterAPI.Test(TwitterAPI.TwitterKeys, "orta");

                        break;
                    }
                case "/tw":
                    {
                        var message = _Args["message"];
                        TwitterAPI.Tweet(TwitterAPI.TwitterKeys, message.Replace("\\r\\n", "\r\n"));
                        break;
                    }
                case "/cfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.CreateFollow(TwitterAPI.TwitterKeys, screen_name);
                        break;
                    }
                case "/dfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.BreakFollow(TwitterAPI.TwitterKeys, screen_name);
                        break;
                    }
                case "/tws":
                    {
                        string search_key = _Args["search_keyword"];
                        var ret = TwitterAPI.TweetSearch(TwitterAPI.TwitterKeys, search_key);
                        SQLitePath = _Args["file_path"];

                        using (var db = new SQLiteDataContext())
                        {
                            db.Database.EnsureCreated();
                        }


                        DateTime insert_time = DateTime.Now;
                        string guid = Guid.NewGuid().ToString();

                        foreach (var tmp in ret)
                        {
                            TwitterSearchResultsBase data = new TwitterSearchResultsBase();
                            data.CreateDt = insert_time;
                            data.Guid = guid;
                            data.Id = tmp.Id;
                            data.FavoritesCount = tmp.FavoriteCount;
                            if (tmp.User != null)
                            {
                                data.UserId = tmp.User.Id;
                                data.ScreenName = tmp.User.ScreenName;
                                data.FriendsCount = tmp.User.FriendsCount;
                                data.FollowerCount = tmp.User.FollowersCount;
                            }

                            data.Text = tmp.Text;


                            TwitterSearchResultsBase.Insert(data);
                        }

                        //int index = 1;

                        //// Excelファイルを作る
                        //using (var workbook = new XLWorkbook())
                        //{
                        //    var worksheet = workbook.Worksheets.Add("result");
                        //    // セルに値や数式をセット
                        //    worksheet.Cell($"A{index}").Value = "tweet.ID";
                        //    worksheet.Cell($"B{index}").Value = "user.Id";
                        //    worksheet.Cell($"C{index}").Value = "screen_name";
                        //    worksheet.Cell($"D{index}").Value = "text";
                        //    worksheet.Cell($"E{index}").Value = "FavoriteCount";
                        //    worksheet.Cell($"F{index}").Value = "user.FriendsCount";
                        //    worksheet.Cell($"G{index}").Value = "user.FollowerCount";
                        //    worksheet.Cell($"H{index}").Value = "CreateAt";

                        //    foreach (var tw in ret)
                        //    {
                        //        index++;
                        //        worksheet.Cell($"A{index}").Value = tw.Id;
                        //        worksheet.Cell($"B{index}").Value = tw.User.Id;
                        //        worksheet.Cell($"C{index}").Value = tw.User.ScreenName;
                        //        worksheet.Cell($"D{index}").Value = tw.Text;
                        //        worksheet.Cell($"E{index}").Value = tw.FavoriteCount;
                        //        worksheet.Cell($"F{index}").Value = tw.User.FriendsCount;
                        //        worksheet.Cell($"G{index}").Value = tw.User.FollowersCount;
                        //        worksheet.Cell($"H{index}").Value = tw.CreatedAt.LocalDateTime;
                        //    }

                        //    string file_path = _Args["file_path"];
                        //    // ワークブックを保存する
                        //    workbook.SaveAs(file_path);
                        //}

                        break;
                    }
            }
        }

    }
}
