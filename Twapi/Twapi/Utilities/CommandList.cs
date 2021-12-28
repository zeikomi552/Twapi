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

            #region Keys
            this.Items.Add(new Command("-ck", "TwitterAPIを使用する際に必要となるコンシューマーキーを指定する", Command.CommandTypeEnum.Keys));
            this.Items.Add(new Command("-cs", "TwitterAPIを使用する際に必要となるコンシューマーシークレットを指定する", Command.CommandTypeEnum.Keys));
            this.Items.Add(new Command("-at", "TwitterAPIを使用する際に必要となるアクセストークンを指定する", Command.CommandTypeEnum.Keys));
            this.Items.Add(new Command("-as", "TwitterAPIを使用する際に必要となるアクセスシークレットを指定する", Command.CommandTypeEnum.Keys));

            #endregion

            #region Options
            this.Items.Add(new Command("-sql", "保存先のSqLiteファイルを指定します(-dとは併用できません)", Command.CommandTypeEnum.Option));
            #endregion

        }
    }
}
