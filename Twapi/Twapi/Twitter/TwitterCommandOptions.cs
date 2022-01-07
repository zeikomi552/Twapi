using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twapi.Utilities;

namespace Twapi.Twitter
{
    public class TwitterCommandOptions : TwapiBase
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
        /// 最終ツイート日からの経過日数を正の整数値で指定する
        /// </summary>
        [EndpointParam("-lastday")]
        public string LastDay { get; set; }

        /// <summary>
        /// エクセルファイルパス
        /// </summary>
        [EndpointParam("-xlsx")]
        public string ExcelPath { get; set; }
        /// <summary>
        /// エクセルファイルパス
        /// </summary>
        [EndpointParam("-tweet")]
        public string Tweet { get; set; }

        /// <summary>
        /// フォローしてからの期間を正の整数値で指定する
        /// </summary>
        [EndpointParam("-followdays")]
        public string FollowDays { get; set; }

        /// <summary>
        /// ユーザー数上限
        /// </summary>
        [EndpointParam("-limit")]
        public string Limit { get; set; }

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

        /// <summary>
        /// キーファイルパス
        /// </summary>
        [EndpointParam("-keysfile")]
        public string KeysFile { get; set; }

    }
}
