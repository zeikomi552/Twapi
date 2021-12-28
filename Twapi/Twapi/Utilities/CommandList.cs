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
            this.Items.Add(new Command("-d", "出力先ディレクトリを指定します", Command.CommandTypeEnum.Option));
            this.Items.Add(new Command("-sql", "保存先のSqLiteファイルを指定します(-dとは併用できません)", Command.CommandTypeEnum.Option));
            this.Items.Add(new Command("-json", "JSONファイルで出力します(デフォルト)", Command.CommandTypeEnum.Option));
            #endregion

            #region Parameters
            this.Items.Add(new Command("-q",
                "指定例: 黒柴さん" + "\r\n\t\t" +
                "検索キーワード。検索演算子も利用可。", 
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-geocode",
                "指定例: 35.794507,139.790788,1km" + "\r\n\t\t" +
                "位置の範囲。カンマ(,)で区切って緯度、経度、半径を指定する。半径の単位は、km(キロメートル)、またはmi(マイル)で指定する。", Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-lang",
                "指定例: ja" + "\r\n\t\t" +
                "検索クエリの言語コード。検索の際に、内部処理で必要となる。日本人ならjaを指定しておくか、省略しておくのが無難。",
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-locale",
                "指定例: 10" + "\r\n\t\t" +
                "結果の数。1〜100の間で指定する。",
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-result_type",
                "指定例: popular" + "\r\n\t\t" +
                "取得するツイートの種類。popular:人気のツイート, recent:最近のツイート mixed:全てのツイート",
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-count",
                "指定例: 10" + "\r\n\t\t" +
                "結果の数。1〜100の間で指定する。",
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-until",
                "指定例: 2017-01-17" + "\r\n\t\t" +
                "YYYY-MM-DDの形式で日時を指定すると、それより過去のツイートを検索できる。", 
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-since_id",
                "指定例: 643299864344788992" + "\r\n\t\t" +
                "ページングに利用する。ツイートのIDを指定すると、これを含まず、これより未来のツイートを取得できる。", 
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-max_id",
                "指定例: 643299864344788992" + "\r\n\t\t" +
                "ページングに利用する。ツイートのIDを指定すると、これを含まず、これより過去のツイートを取得できる。", 
                Command.CommandTypeEnum.Parameter));
            this.Items.Add(new Command("-include_entities",
                "指定例: true" + "\r\n\t\t" +
                "ツイートオブジェクト内のentitiesプロパティを含めるか否か。", 
                Command.CommandTypeEnum.Parameter));

            #endregion
        }
    }
}
