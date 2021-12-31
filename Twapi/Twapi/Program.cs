using ClosedXML.Excel;
using Twapi.Database.SQLite;
using Twapi.Database.SQLite.Base;
using Twapi.Twitter;
using Twapi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using log4net.Config;

namespace Twapi
{
    class Program : TwapiBase
    {
        #region TwitterAPI メイン関数
        /// <summary>
        /// TwitterAPI
        /// 参考：
        /// https://syncer.jp/Web/API/Twitter/REST_API/
        /// http://westplain.sakuraweb.com/translate/twitter/Documentation/REST-APIs/Public-API/REST-APIs.cgi
        /// </summary>
        /// <param name="args"></param>
        static int Main(string[] args)
        {
            // log4netの設定ファイル読み込み処理
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

            try
            {
                TwCommand.ExecuteCommand(args);
                return 0;
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                foreach (var arg in args) msg.Append(arg + " ");

                Logger.Error("twapi " + e.Message);
                Logger.Error(e.Message);

                Console.WriteLine("twapi " + e.Message);
                Console.WriteLine(e.Message);
                return -1;
            }

        }
        #endregion
    }
}
