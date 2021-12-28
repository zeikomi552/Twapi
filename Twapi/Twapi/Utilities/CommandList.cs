using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Utilities
{
    public class CommandList
    {
        public List<Command> Items { get; set; } = new List<Command>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandList()
        {
            #region Action Commands
            this.Items.Add(new Command("/h", "ヘルプ", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("/?", "ヘルプ", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("regist", 
                "コンシューマーキー・コンシューマーシークレット・アクセストークン・アクセスシークレットを保存します。",
                Command.CommandTypeEnum.Action, true));

            this.Items.Add(new Command("/refresh", "自分がフォローしているユーザーとフォローされているユーザーのリストを更新する", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("/create", "フォロバリストを作成する", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("/follow", "フォロバリストを元にフォローする", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("/remove", "フォローを解除する", Command.CommandTypeEnum.Action, true));
            #endregion

        }
    }
}
