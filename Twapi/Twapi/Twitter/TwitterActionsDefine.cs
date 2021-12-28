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
        /// <param name="action_name"></param>
        /// <param name="action"></param>
        public TwitterAction(string action_name, Action<string> action)
        {
            this.ActionName = action_name;
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
    }
}
