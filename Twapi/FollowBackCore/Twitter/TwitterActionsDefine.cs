using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Twitter
{
    public class TwitterAction
    {
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

        /// <summary>
        /// アクション名
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 関数登録
        /// </summary>
        public Action<string> Action { get; set; }

    }
}
