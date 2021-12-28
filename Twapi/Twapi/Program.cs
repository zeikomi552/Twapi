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

namespace Twapi
{
    class Program
    {
        #region TwitterAPI メイン関数
        /// <summary>
        /// TwitterAPI
        /// 参考：https://syncer.jp/Web/API/Twitter/REST_API/
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                TwCommand.ExecuteCommand(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        #endregion
    }
}
