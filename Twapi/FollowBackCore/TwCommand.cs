using Twapi.Twitter;
using Twapi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi
{
    public class TwCommand
    {
        static string _Action = string.Empty;

        public static string SQLitePath { get; set; }

        public static TwitterArgs TwitterArgs { get; set; }


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

            foreach (var tmp in TwitterActions.Actions)
            {
                if (action.Equals(tmp.ActionName))
                {
                    tmp.Action(tmp.ActionName);
                }
            }

            //switch (action)
            //{

            //    case "/test":
            //        {
            //            var result = TwitterAPI.Token.Friendships.Lookup(new List<string> { "Zeikomi552", "route20191212", "OtoJiu", "enginer_ni_naru" });
            //            // 結果出力
            //            TwitterActions.ResultOutput(result.Json, action, result.RateLimit);
            //            break;
            //        }

            //    case "account/settings":
            //        {
            //            var result = TwitterAPI.Token.Account.Settings();
            //            Console.WriteLine(result.Json);
            //            OutputJSON(result.Json, action);
            //            break;
            //        }
            //    case "search/tweets":
            //        {
            //            long last_id = -1;

            //            while (last_id != 0)
            //            {
            //                // 検索
            //                var result = TwitterAPI.Token.Search.Tweets(
            //                    q => TwitterArgs.CommandOptions.Q,
            //                    geocode => TwitterArgs.CommandOptions.Geocode,
            //                    lang => TwitterArgs.CommandOptions.Lang,
            //                    locale => TwitterArgs.CommandOptions.Locale,
            //                    result_type => TwitterArgs.CommandOptions.Result_type,
            //                    count => TwitterArgs.CommandOptions.Count,
            //                    until => TwitterArgs.CommandOptions.Until,
            //                    since_id => TwitterArgs.CommandOptions.Since_id,
            //                    max_id => last_id < 0 ? null : last_id,
            //                    include_entities => TwitterArgs.CommandOptions.Include_entities
            //                );

            //                Console.WriteLine(result.Json);
            //                Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

            //                OutputJSON(result.Json, action);
            //                System.Threading.Thread.Sleep(1000);

            //                var last = result.LastOrDefault();

            //                if (last != null)
            //                {
            //                    last_id = last.Id;
            //                }

            //                // 残り回数が0なら待つ抜ける
            //                if (last_id != 0 && result.RateLimit.Remaining == 0)
            //                {
            //                    //System.Threading.Thread.Sleep(15 * 60 * 1000);
            //                    break;
            //                }
            //            }

            //            break;
            //        }
            //    case "account/verify_credentials":
            //        {
            //            // 検索
            //            var result = TwitterAPI.Token.Account.VerifyCredentials();

            //            Console.WriteLine(result.Json);
            //            Console.WriteLine();
            //            Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

            //            OutputJSON(result.Json, action);

            //            break;
            //        }
            //    case "blocks/ids":
            //        {
            //            long next_cursor = -1;

            //            while (next_cursor != 0)
            //            {
            //                // 検索
            //                var result = TwitterAPI.Token.Blocks.Ids(cursor => TwitterArgs.CommandOptions.Cursor);

            //                Console.WriteLine(result.Json);
            //                Console.WriteLine();
            //                Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

            //                OutputJSON(result.Json, action);
            //                next_cursor = result.NextCursor;
            //            }

            //            break;
            //        }
            //    case "blocks/list":
            //        {
            //            long next_cursor = -1;

            //            while (next_cursor != 0)
            //            {
            //                // 検索
            //                var result = TwitterAPI.Token.Blocks.List(
            //                    cursor => next_cursor == -1 ? null : next_cursor,
            //                    include_entities => TwitterArgs.CommandOptions.Include_entities,
            //                    skip_status => TwitterArgs.CommandOptions.Skip_status,
            //                    include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text
            //                );

            //                Console.WriteLine(result.Json);
            //                Console.WriteLine();
            //                Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

            //                OutputJSON(result.Json, action);
            //                System.Threading.Thread.Sleep(1000);

            //                // 残り回数が0なら待つ
            //                if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
            //                {
            //                    System.Threading.Thread.Sleep(15 * 60 * 1000);
            //                }

            //                next_cursor = result.NextCursor;
            //            }

            //            break;
            //        }
            //    case "followers/ids":
            //        {
            //            long next_cursor = -1;

            //            while (next_cursor != 0)
            //            {

            //                // 検索
            //                var result = TwitterAPI.Token.Followers.Ids(
            //                    user_id => TwitterArgs.CommandOptions.User_id,
            //                    screen_name => TwitterArgs.CommandOptions.Screen_name,
            //                    cursor => next_cursor == -1 ? null : next_cursor,
            //                    count => TwitterArgs.CommandOptions.Count
            //                );

            //                Console.WriteLine(result.Json);
            //                Console.WriteLine();
            //                Console.WriteLine(result.RateLimit.Remaining.ToString() + "/" + result.RateLimit.Limit.ToString());

            //                OutputJSON(result.Json, action);

            //                System.Threading.Thread.Sleep(1000);

            //                // 残り回数が0なら待つ
            //                if (result.NextCursor != 0 && result.RateLimit.Remaining == 0)
            //                {
            //                    System.Threading.Thread.Sleep(15 * 60 * 1000);
            //                }

            //                next_cursor = result.NextCursor;
            //            }

            //            break;
            //        }               
            //    case "statuses/update":
            //        {
            //            TwitterAPI.Token.Statuses.Update(status=> TwitterArgs.CommandOptions.Status);
            //            break;
            //        }
            //    case "statuses/home_timeline":
            //        {
            //            var result = TwitterAPI.Token.Statuses.HomeTimeline(
            //                count=> TwitterArgs.CommandOptions.Count,
            //                since_id=> TwitterArgs.CommandOptions.Since_id,
            //                max_id=> TwitterArgs.CommandOptions.Max_id,
            //                trim_user => TwitterArgs.CommandOptions.Trim_User,
            //                exclude_replies=> TwitterArgs.CommandOptions.Exclude_replies,
            //                contributor_details=> TwitterArgs.CommandOptions.Contributor_details,
            //                include_entities=> TwitterArgs.CommandOptions.Include_entities,
            //                include_ext_alt_text => TwitterArgs.CommandOptions.Include_ext_alt_text,
            //                tweet_mode => TwitterArgs.CommandOptions.Tweet_mode
            //            );
            //            OutputJSON(result.Json, action);

            //            break;
            //        }
            //}
        }



    }
}
