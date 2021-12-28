using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Twitter
{
    public class TwitterAction
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="action_name">アクション名</param>
        /// <param name="help">ヘルプ</param>
        /// <param name="action">アクション</param>
        public TwitterAction(string action_name, string help, Action<string> action)
        {
            this.ActionName = action_name;
            this.Help = help;
            this.Action = action;
        }
        #endregion

        #region アクション名
        /// <summary>
        /// アクション名
        /// </summary>
        public string ActionName { get; set; }
        #endregion

        #region 関数登録
        /// <summary>
        /// 関数登録
        /// </summary>
        public Action<string> Action { get; set; }
        #endregion

        #region Help表示用文字列
        /// <summary>
        /// Help表示用文字列
        /// </summary>
        public string Help { get; set; }
        #endregion

    }
}
