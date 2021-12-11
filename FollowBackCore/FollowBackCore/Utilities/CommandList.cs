using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Utilities
{
    public class CommandList
    {
        public List<Command> Items { get; set; } = new List<Command>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandList()
        {
            this.Items.Add(new Command("/h", "ヘルプ", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("/?", "ヘルプ", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("regist", 
                "コンシューマーキー・コンシューマーシークレット・アクセストークン・アクセスシークレットを保存します。",
                Command.CommandTypeEnum.Action, true));

            #region ActionCommand
            #region account
            this.Items.Add(new Command("account/settings", "アカウント設定を取得する", Command.CommandTypeEnum.Action, true));
            this.Items.Add(new Command("account/verify_credentials", "アカウントの有効性を確認する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/remove_profile_banner", "バナー画像を削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/settingsr", "アカウント設定を更新する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/update_profile", "プロフィールを更新する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/update_profile_background_image", "背景画像を更新する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/update_profile_banner", "バナー画像を更新する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("account/update_profile_image", "アイコン画像を更新する", Command.CommandTypeEnum.Action));
            #endregion

            #region application
            this.Items.Add(new Command("application/rate_limit_status", "レートリミットを取得する", Command.CommandTypeEnum.Action));
            #endregion

            #region blocks
            this.Items.Add(new Command("blocks/ids", "ブロックユーザーをIDで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("blocks/list", "ブロックユーザーをオブジェクトで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("blocks/create", "ブロックを実行する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("blocks/destroy", "ブロックを解除する", Command.CommandTypeEnum.Action));
            #endregion

            #region collections
            this.Items.Add(new Command("collections/entries", "コレクションをタイムライン付きで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/list", "ユーザー、またはツイートからコレクションを検索する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/show", "コレクションをIDから取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/create", "コレクションを作成する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/destroy", "コレクションを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/entries/add", "コレクションにツイートを追加する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/entries/curate", "コレクションをまとめて編集する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/entries/move", "コレクションのツイートの位置を移動する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/entries/remove", "コレクションのツイートを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("collections/update", "コレクションの設定を更新する", Command.CommandTypeEnum.Action));
            #endregion

            #region direct_messages
            this.Items.Add(new Command("direct_messages", "受信したDMを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("direct_messages/sent", "送信したDMを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("direct_messages/show", "個別のDMの内容を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("direct_messages/destroy", "ダイレクトメッセージを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("direct_messages/new", "ダイレクトメッセージを送信する", Command.CommandTypeEnum.Action));
            #endregion

            #region favorites
            this.Items.Add(new Command("favorites/list", "お気に入りしたツイートを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("favorites/create", "いいねを付ける", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("favorites/destroy", "いいねを取り消す", Command.CommandTypeEnum.Action));
            #endregion

            #region followers
            this.Items.Add(new Command("followers/ids", "フォロワーをIDの一覧で取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("followers/list", "フォロワーをオブジェクトの一覧で取得する", Command.CommandTypeEnum.Action));
            #endregion

            #region friends
            this.Items.Add(new Command("friends/ids", "フォローしているユーザーをIDの一覧で取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friends/list", "フォローているユーザーをオブジェクトの一覧で取得する", Command.CommandTypeEnum.Action));
            #endregion

            #region friendships
            this.Items.Add(new Command("friendships/incoming", "自分に対するフォローリクエストを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/lookup", "自分と対象ユーザーの関係を調べる", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/no_retweets/ids", "RT非表示中のユーザーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/outgoing", "自分がフォローリクエストを送ったユーザーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/show", "2人のユーザーの関係を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/create", "フォローする", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/destroy", "フォローを解除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("friendships/update", "RT非表示とツイート通知の設定を更新する", Command.CommandTypeEnum.Action));
            #endregion

            #region geo
            this.Items.Add(new Command("geo/id/:place_id", "場所の情報を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("geo/reverse_geocode", "緯度、経度から場所を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("geo/search", "場所を検索する", Command.CommandTypeEnum.Action));
            #endregion

            #region help
            this.Items.Add(new Command("help/configuration", "内部設定を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("help/languages", "言語の一覧を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("help/privacy", "プライバシーポリシーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("help/tos", "サービス利用規約を取得する", Command.CommandTypeEnum.Action));
            #endregion

            #region lists
            this.Items.Add(new Command("lists/list", "リストの一覧を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members", "リストのメンバーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members/show", "リストのメンバーか確認する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/memberships", "メンバーになっているリストを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/ownerships", "作成したリストを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/show", "リストを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/statuses", "リストのタイムラインを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/subscribers", "リストの購読者を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/create", "リストを作成する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/destroy", "リストを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members/create", "リストにメンバーを追加する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members/create_all", "リストに複数のメンバーを追加する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members/destroy", "リストからメンバーを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/members/destroy_all", "リストから複数のメンバーを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/subscribers/create", "リストを保存する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/subscribers/destroy", "リストの購読を取り消す", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("lists/update", "リストの設定を更新する", Command.CommandTypeEnum.Action));
            #endregion

            #region media
            this.Items.Add(new Command("media/upload", "画像をアップロードする", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("media/upload (APPEND)", "動画のアップロードを実行する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("media/upload (FINALIZE)", "動画のアップロードを完了する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("media/upload (INIT)", "動画のアップロードの準備をする", Command.CommandTypeEnum.Action));
            #endregion

            #region mutes
            this.Items.Add(new Command("mutes/users/ids", "ミュートしているユーザーをIDの一覧で取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("mutes/users/list", "ミュートしているユーザーをオブジェクトの一覧で取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("mutes/users/create", "ミュートを実行する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("mutes/users/destroy", "ミュートを解除する", Command.CommandTypeEnum.Action));
            #endregion

            #region saved_searches
            this.Items.Add(new Command("saved_searches/list", "検索メモを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("saved_searches/show/:id", "検索メモを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("saved_searches/create", "検索メモを保存する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("saved_searches/destroy/:id", "検索メモを削除する", Command.CommandTypeEnum.Action));
            #endregion

            #region search
            this.Items.Add(new Command("search/tweets", "ツイートを検索する", Command.CommandTypeEnum.Action, true));
            #endregion

            #region statuses
            this.Items.Add(new Command("statuses/home_timeline", "ホームタイムラインを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/lookup", "複数のツイートを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/mentions_timeline", "メンションタイムラインを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/oembed", "ツイートの埋め込み用HTMLを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/retweeters/ids", "ツイートをRTしたユーザーをIDで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/retweets/:id", "ツイートをRTしたユーザーをオブジェクトで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/retweets_of_me", "リツイートされたツイートを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/show/:id", "ツイートを個別に取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/user_timeline", "ユーザータイムラインを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/destroy/:id", "ツイートを削除する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/retweet/:id", "リツイートを実行する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/unretweet/:id", "リツイートを取り消す", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("statuses/update", "ツイートを投稿する", Command.CommandTypeEnum.Action));
            #endregion

            #region trends
            this.Items.Add(new Command("trends/available", "トレンドの地域を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("trends/closest", "位置座標からWOEIDを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("trends/place", "トレンドを取得する", Command.CommandTypeEnum.Action));
            #endregion

            #region users
            this.Items.Add(new Command("users/lookup", "複数のユーザーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/profile_banner", "バナー画像を取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/search", "ユーザーを検索する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/show", "ユーザーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/suggestions", "サジェストのカテゴリを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/suggestions/:slug", "おすすめユーザーを取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/suggestions/:slug/members", "おすすめユーザーをツイート付きで取得する", Command.CommandTypeEnum.Action));
            this.Items.Add(new Command("users/report_spam", "スパム報告を実行する", Command.CommandTypeEnum.Action));
            #endregion
            #endregion

            #region Keys
            this.Items.Add(new Command("-ck", "TwitterAPIを使用する際に必要となるコンシューマーキーを指定する", Command.CommandTypeEnum.Keys));
            
            this.Items.Add(new Command("-cs", "TwitterAPIを使用する際に必要となるコンシューマーシークレットを指定する", Command.CommandTypeEnum.Keys));

            this.Items.Add(new Command("-at", "TwitterAPIを使用する際に必要となるアクセストークンを指定する", Command.CommandTypeEnum.Keys));

            this.Items.Add(new Command("-as", "TwitterAPIを使用する際に必要となるアクセスシークレットを指定する", Command.CommandTypeEnum.Keys));

            #endregion

            #region Options
            this.Items.Add(new Command("-d", "出力先ディレクトリを指定します", Command.CommandTypeEnum.Option));
            this.Items.Add(new Command("-sql", "SqLiteで出力します", Command.CommandTypeEnum.Option));
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
