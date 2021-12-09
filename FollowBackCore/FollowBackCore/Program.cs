using ClosedXML.Excel;
using FollowBackCore.Database.SQLite;
using FollowBackCore.Database.SQLite.Base;
using FollowBackCore.Twitter;
using FollowBackCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FollowBackCore
{
    class Program
    {
        static Dictionary<string, string> _Args = new Dictionary<string, string>();
        static TwitterKeys _Keys = new TwitterKeys();
        static string _Action = string.Empty;

        public static string SQLitePath { get; set; }

        static void SetArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/tw":
                    case "/twd":
                    case "/tws":
                    case "/frc":
                    case "/frd":
                    case "/fvc":
                    case "/fvd":
                    case "/?":
                        {
                            _Args.Add("action", args.Length > i ? args[i] : string.Empty);
                            break;
                        }
                    case "-msg":
                        {
                            _Args.Add("message", args.Length > i ? args[i] : string.Empty);
                            break;
                        }
                    case "-scn":
                        {
                            i++;
                            _Args.Add("screen_name", args.Length > i ? args[i] : string.Empty);
                            break;
                        }
                    case "-kwd":
                        {
                            i++;
                            _Args.Add("search_keyword", args.Length > i ? args[i] : string.Empty);
                            break;
                        }
                    case "-f":
                        {
                            i++;
                            _Args.Add("file_path", args.Length > i ? args[i] : string.Empty);
                            break;
                        }
                    case "-ckey":
                        {
                            i++;
                            _Keys.ConsumerKey = args.Length > i ? args[i] : string.Empty;
                            break;
                        }
                    case "-cscr":
                        {
                            i++;
                            _Keys.ConsumerSecretKey = args.Length > i ? args[i] : string.Empty;
                            break;
                        }
                    case "-atkn":
                        {
                            i++;
                            _Keys.AccessToken = args.Length > i ? args[i] : string.Empty;
                            break;
                        }
                    case "-ascr":
                        {
                            i++;
                            _Keys.AccessSecret = args.Length > i ? args[i] : string.Empty;
                            break;
                        }

                }

            }
        }

        static void Main(string[] args)
        {
            // 引数のセット
            SetArgs(args);
            var action = _Args["action"];

            switch (action)
            {
                case "/?":
                    {
                        Console.WriteLine("使用方法：");
                        Console.WriteLine("\tFollowBackCore.exe [/actioncommand] [-keys] [-options]");

                        Console.WriteLine("");
                        Console.WriteLine("/actioncommand :");
                        Console.WriteLine("\t/?\t...ヘルプを表示します");
                        Console.WriteLine("\t/tw\t...ツイートします");
                        Console.WriteLine("\t/cfr\t...フォローします");
                        Console.WriteLine("\t/dfr\t...フォローを解除します");
                        Console.WriteLine("\t/tws\t...ツイッター検索を行います");
                        Console.WriteLine("");

                        Console.WriteLine("");
                        Console.WriteLine("-keys :");
                        Console.WriteLine("\t-ckey\t...TwitterAPIを使用する際に必要となるConsumer Keyを指定します");
                        Console.WriteLine("\t-cscr\t...TwitterAPIを使用する際に必要となるConsumer Secretを指定します");
                        Console.WriteLine("\t-atkn\t...TwitterAPIを使用する際に必要となるAccess Tokenを指定します");
                        Console.WriteLine("\t-ascr\t...TwitterAPIを使用する際に必要となるAccess Secretを指定します");

                        Console.WriteLine("");
                        Console.WriteLine("-options :");
                        Console.WriteLine("\t-msg\t...ツイートする際に送信するメッセージを指定します");
                        Console.WriteLine("\t-scn\t...フォロー対象のscreen_nameを指定します");
                        Console.WriteLine("\t-kwd\t...ツイッター検索で使用する検索キーワードを指定します");
                        Console.WriteLine("\t-f\t...ツイッター検索で得た結果を保存するファイルパスを指定します(.xlsx)");

                        break;
                    }
                case "/tw":
                    {
                        var message = _Args["message"];
                        TwitterAPI.Tweet(_Keys, message.Replace("\\r\\n", "\r\n"));
                        break;
                    }
                case "/cfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.CreateFollow(_Keys, screen_name);
                        break;
                    }
                case "/dfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.BreakFollow(_Keys, screen_name);
                        break;
                    }
                case "/tws":
                    {
                        string search_key = _Args["search_keyword"];
                        var ret = TwitterAPI.TweetSearch(_Keys, search_key);
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
