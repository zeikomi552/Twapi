using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Twitter
{
    public class TwitterCommandOptions
    {
        /// <summary>
        /// 検索のためのキーワードを指定します
        /// </summary>
        [EndpointParam("-keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// SQLiteのファイルパスを指定します
        /// </summary>
        [EndpointParam("-sql")]
        public string Sql { get; set; }

        /// <summary>
        /// FF比の最小値を指定します 0～1の範囲(0指定の場合は無制限)
        /// </summary>
        [EndpointParam("-ffmin")]
        public string FFmin { get; set; }

        /// <summary>
        /// FF比の最大値を指定します 0～1の範囲(0指定の場合は無制限)
        /// </summary>
        [EndpointParam("-ffmax")]
        public string FFmax { get; set; }

        /// <summary>
        /// 最終ツイート日の指定
        /// </summary>
        [EndpointParam("-lastday")]
        public string LastDay { get; set; }

        /// <summary>
        /// エクセルファイルパス
        /// </summary>
        [EndpointParam("-xlsx")]
        public string ExcelPath { get; set; }

        /// <summary>
        /// フォローしてからの期間
        /// </summary>
        [EndpointParam("-followdays")]
        public string FollowDays { get; set; }

        /// <summary>
        /// コンシューマーキー
        /// </summary>
        [EndpointParam("-ck")]
        public string ConsumerKey { get; set; }

        /// <summary>
        /// コンシューマーシークレット
        /// </summary>
        [EndpointParam("-cs")]
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// アクセストークン
        /// </summary>
        [EndpointParam("-at")]
        public string AccessToken { get; set; }

        /// <summary>
        /// アクセスシークレット
        /// </summary>
        [EndpointParam("-as")]
        public string AccessSecret { get; set; }

    }
}
