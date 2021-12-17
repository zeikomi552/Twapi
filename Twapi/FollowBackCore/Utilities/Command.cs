using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Utilities
{
    public class Command
    {
        public enum CommandTypeEnum
        {
            /// <summary>
            /// アクション：コマンドの動作を決定する
            /// 例：ヘルプ、キーファイルの保存、ツイッター系のAPIエンドポイント
            /// </summary>
            Action,
            /// <summary>
            /// オプション：ツイッターAPI外の指定条件
            /// 例：出力先ディレクトリ指定
            /// </summary>
            Option,
            /// <summary>
            /// パラメータ：ツイッターAPIのエンドポイントに付属するパラメータ
            /// </summary>
            Parameter,
            /// <summary>
            /// キー：アクセスシークレットやトークン等
            /// </summary>
            Keys
        }
    
        /// <summary>
        /// キー
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// コマンドタイプ
        /// </summary>
        public CommandTypeEnum CommandType { get; set; }

        /// <summary>
        /// 有効（実装済）/無効(未実装)
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="description">説明</param>
        /// <param name="command_type">タイプ</param>
        /// <param name="is_enable">true:実装済 false:未実装</param>
        public Command(string key, string description, CommandTypeEnum command_type, bool is_enable = false)
        {
            this.Key = key;
            this.Description = description;
            this.CommandType = command_type;
            this.IsEnable = is_enable;
        }
    }
}
