using ClosedXML.Excel;
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
        static void SetArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-tw":
                    case "-twd":
                    case "-tws":
                    case "-frc":
                    case "-frd":
                    case "-fvc":
                    case "-fvd":
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
                case "-h":
                    {
                        Console.WriteLine("\tFollowBackCore.exe -h   ...ヘルプ");
                        Console.WriteLine("\tFollowBackCore.exe -tw    ...ヘルプ");
                        break;
                    }
                case "-tw":
                    {
                        var message = _Args["message"];
                        TwitterAPI.Tweet(_Keys, message.Replace("\\r\\n", "\r\n"));
                        break;
                    }
                case "-cfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.CreateFollow(_Keys, screen_name);
                        break;
                    }
                case "-dfr":
                    {
                        string screen_name = _Args["screen_name"];
                        TwitterAPI.BreakFollow(_Keys, screen_name);
                        break;
                    }
                case "-tws":
                    {
                        string search_key = _Args["search_keyword"];
                        var ret = TwitterAPI.TweetSearch(_Keys, search_key);
                        int index = 1;

                        // Excelファイルを作る
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("result");
                            // セルに値や数式をセット
                            worksheet.Cell($"A{index}").Value = "tweet.ID";
                            worksheet.Cell($"B{index}").Value = "user.Id";
                            worksheet.Cell($"C{index}").Value = "screen_name";
                            worksheet.Cell($"D{index}").Value = "text";
                            worksheet.Cell($"E{index}").Value = "FavoriteCount";
                            worksheet.Cell($"F{index}").Value = "user.FriendsCount";
                            worksheet.Cell($"G{index}").Value = "user.FollowerCount";
                            worksheet.Cell($"H{index}").Value = "CreateAt";

                            foreach (var tw in ret)
                            {
                                index++;
                                worksheet.Cell($"A{index}").Value = tw.Id;
                                worksheet.Cell($"B{index}").Value = tw.User.Id;
                                worksheet.Cell($"C{index}").Value = tw.User.ScreenName;
                                worksheet.Cell($"D{index}").Value = tw.Text;
                                worksheet.Cell($"E{index}").Value = tw.FavoriteCount;
                                worksheet.Cell($"F{index}").Value = tw.User.FriendsCount;
                                worksheet.Cell($"G{index}").Value = tw.User.FollowersCount;
                                worksheet.Cell($"H{index}").Value = tw.CreatedAt.LocalDateTime;
                            }

                            string file_path = _Args["file_path"];
                            // ワークブックを保存する
                            workbook.SaveAs(file_path);
                        }
                        //}

                        break;
                    }
            }
        }

    }
}
