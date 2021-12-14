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
    }
}
