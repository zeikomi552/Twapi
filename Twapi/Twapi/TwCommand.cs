using Twapi.Twitter;
using Twapi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twapi.Database.SQLite;

namespace Twapi
{
    public class TwCommand
    {
        #region Action
        /// <summary>
        /// Action
        /// </summary>
        static string _Action = string.Empty;
        #endregion

        #region SQLiteのファイルパス
        /// <summary>
        /// SQLiteのファイルパス
        /// </summary>
        public static string SQLitePath { get; set; }
        #endregion

        #region コマンドライン引数
        /// <summary>
        /// コマンドライン引数
        /// </summary>
        public static TwitterArgs TwitterArgs { get; set; }
        #endregion

        #region コマンドの実行
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="args">パラメータ</param>
        public static void ExecuteCommand(string[] args)
        {
            // キーファイルの存在確認
            if (File.Exists(ConfigManager.Keys))
            {
                // キーの読み込み
                TwitterAPI.TwitterKeys = XMLUtil.Deserialize<TwitterKeys>(ConfigManager.Keys);
            }

            // SQLiteのデフォルトパス設定
            string path = Path.Combine(ConfigManager.ConfigDir, "twapi.db");
            SQLiteDataContext.db_file_path = path;


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

            // アクションの実行
            foreach (var tmp in TwitterActions.Actions)
            {
                if (action.Equals(tmp.ActionName))
                {
                    tmp.Action(tmp.ActionName);
                    break;
                }
            }
        }
        #endregion
    }
}
