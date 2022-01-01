# Twapi
![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/zeikomi552/twapi)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/zeikomi552/twapi)
![GitHub](https://img.shields.io/github/license/zeikomi552/twapi)

[![Twapi](https://github-readme-stats.vercel.app/api?username=zeikomi552)](https://github.com/zeikomi552/twapi)

フォロバさんを探せで作ってきたソフトがそれなりに形になってきたので公開します。
できることは以下の通りです。

- 自動フォロー
- 自動フォロー解除
- フォロー対象の自動検索
- 自動ツイート

<!--more-->


## 概要

名称：Twapi

ツイッターのツイートやフォローを自動化することを目的としています。
作者や本アプリケーションはTwitter社と一切関係ありません。

|内容|詳細|
|----|----|
|ライセンス|MIT|
|OS|Windows 10, 11|
|フレームワーク|.NET 5.0|
|出力の種類|コンソールアプリケーション|
|リポジトリ|https://github.com/zeikomi552/Twapi|
|不具合連絡|https://github.com/zeikomi552/Twapi/issues|
|進捗|https://github.com/zeikomi552/Twapi/projects/1|
|インストーラー保存場所|https://github.com/zeikomi552/Twapi/releases|

## インストール方法

上記インストーラー保存場所からインストーラを取得しインストーラーの指示に従ってインストールします。
.NET 5.0が必要ですので、必要に応じて[.NET 5.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet)をインストールしてください。

## 使用方法

本アプリケーションはコンソールアプリです。
コマンドプロンプト等から使用することができます。

本アプリケーションにはTwitterAPIを使用するための各種キーが必要となるため[Twitter Developer Platform](https://developer.twitter.com/en)からDeveloper IDの取得が必要です。

### ヘルプ

本コマンドを使用することで、Twapiのヘルプを確認することができます。

```
twapi /?
または
twapi /h
```

### Twitter APIキーの保存

本コマンドを使用することで、Twitter APIに使用する各種APIキーを保存します。

```
twapi /regist -ck XXXXXXXX -ck XXXXXXXX -at XXXXXXXX -as XXXXXXXX -keysfile "C:\xxxxx\xxxxx.keys"
```

- -ck : コンシューマーキー(API Key)
- -cs : コンシューシークレット(API Key Secret)
- -at : アクセストークン(Access Token)
- -as : アクセストークンシークレット(Access Token Secret)
- -keysfile : 省略可、キーを保存するファイルパスを指定します。指定無しの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.keysに保存されます。

### フォロー・フォロワー状況の更新

本コマンドを使用することで、現在のフォロー・フォロワー状況を取得し
データベースに保存します。

```
twapi /refresh -sql "C:\xxxxx\xxxxx.db" -keysfile "C:\xxxxx\xxxxx.keys"
```

- -sql : 省略可、データの保存先を指定します。SQLite形式のファイルが作成されます。指定なしの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.dbに保存されます。
- -keysfile : 省略可、キーが保存されているファイルパスを指定します。指定無しの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.keysを使用します。


### フォロバリストの作成

フォロー対象のユーザーを探しフォロバリストに登録します。

```
twapi /search  -keywords プログラマー,プログラミング,ソフトウェア -limit 10000 -sql "C:\xxxxx\xxxxx.db" -keysfile "C:\xxxxx\xxxxx.keys"
```
- -keywords : 自己紹介文に含まれている文字列を指定します。上記例の場合はプログラマー,プログラミング,ソフトウェアのいずれかが含まれているユーザーをフォロバリストに追加します
- limit : 省略可、フォロバリストの上限値を指定します。処理速度向上のためフォロバリストに登録するユーザー数上限を指定します。上限に達した場合、条件から外れフォローされずに放置されている期間が長いユーザーをリストから削除していきます。省略時は5000。
- -sql : 省略可、使用するデータの保存先を指定します。指定なしの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.dbを使用します。
- -keysfile : 省略可、キーが保存されているファイルパスを指定します。指定無しの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.keysを使用します。


### フォローの実行

本コマンドを使用することで、フォロー処理を自動で実行します。
フォロバリストに登録されているユーザーをランダムにフォローします。

```
twapi /follow  -lastday 32 -ffmin 0.9 -ffmax 1.1 -sql "C:\xxxxx\xxxxx.db" -keysfile "C:\xxxxx\xxxxx.keys"
```

- -lastday : 省略可、最終ツイート日からの経過日数を指定します。32を指定した場合は最終ツイート日から32日以内にツイートした人をフォローします。
- -ffmin : 省略可、フォロー数/フォロワー数最小値(0～を小数で指定：0指定時または省略は無制限)。0.9を指定した場合、フォロー数/フォロワー数が0.9以上の人をフォローします。
- -ffmax : 省略可、フォロー数/フォロワー数(0～を小数で指定：0指定時または省略は無制限)。1.1を指定した場合、フォロー数/フォロワー数が1.1以下の人をフォローします。
- -sql : 省略可、使用するデータの保存先を指定します。指定なしの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.dbを使用します。
- -keysfile : 省略可、キーが保存されているファイルパスを指定します。指定無しの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.keysを使用します。

### フォローの解除

本コマンドを使用することで、フォローを解除します。
フォローしている人でフォローされていない人を解除します。

```
twapi /remove -lastday 32 -followdays 10 -sql "C:\xxxxx\xxxxx.db" -keysfile "C:\xxxxx\xxxxx.keys"
```

- -lastday : 省略可、最終ツイート日からの経過日数を指定します。32を指定した場合は最終ツイート日から32日以内にツイートしていない人を解除します。
- -followdays : フォローした日からの経過日数を指定します。10を指定した場合は、フォローしてから10日以上フォローが返されていない人をフォロー解除します。
- -sql : 省略可、使用するデータの保存先を指定します。指定なしの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.dbを使用します。
- -keysfile : 省略可、キーが保存されているファイルパスを指定します。指定無しの場合はAppData\Roaming\Zeikomi\twapi\Config\twapi.keysを使用します。

## 自動ツイート

本コマンドを使用することで、エクセルに記載された内容をランダムでツイートします。
エクセルファイルのA列にはツイートするテキストを、B列にはリンク先を指定したい場合URLを指定します。


```
twapi /autotweet -xlsx "C:\xxxx\xxxx.xlsx"
```

- xlsx : エクセルファイルパス


## 各コマンドで使用しているTwitter API


|twapiコマンド|カテゴリ|エンドポイント|使用回数制限(15分)|
|----|----|----|----|
|/refresh|account|/account/settings|15|
|/refresh|account|/account/settings|15|
|/refresh|followers|/followers/ids|15|
|/refresh|application|/application/rate_limit_status|180|
|/refresh|friends|/friends/ids|15|
|/refresh|users|/users/lookup|900|
|/search|friends|/friends/list|15|
|/follow|Friendships|Create|?|
|/remove|Friendships|Destroy|?|
|/autotweet|statuses|/statuses/update|?|

## その他

Twitter APIにはそれぞれAPIの使用回数制限があります。
使用回数を節約するため内部にデータベースを保持し節約することを狙います。
そのため、定期的にツイッターとデータベースにズレが生じますので、
以下のコマンドを定期的に実行することをおススメします。

- /refresh

## 注意事項

Twitter APIの使用制限は厳しめです。
アカウントロックがかかってしまうことがありますので
自己責任でのご使用をお願いいたします。
あまり欲張らない方が良いかと思います。

参考までに作者の使用状況は以下の通りです。

|twapiコマンド|頻度|
|----|----|
|/refresh|1時間/回|
|/search|10分/回|
|/follow|15分/回|
|/remove|15分/回|
|/remove|1日/回|

タスクスケジューラで動作させています。

以上