@echo off

set DIR_PATH=
set /P DIR_PATH="twapiのインストール先のフォルダパスを入力してください:"

set CONSUMER_KEY=
set /P CONSUMER_KEY="TwitterAPIのコンシューマーキーを入力してください:"

set CONSUMER_SECRET=
set /P CONSUMER_SECRET="TwitterAPIのコンシューシークレットを入力してください:"

set ACCESS_TOKEN=
set /P ACCESS_TOKEN="TwitterAPIのアクセストークンを入力してください:"

set ACCESS_SECRET=
set /P ACCESS_SECRET="TwitterAPIのアクセスシークレットを入力してください:"

set SQLITE_PATH=
set /P SQLITE_PATH="データの保存先ファイルパスを指定してください。ファイル名を含む。:"

cd %DIR_PATH%
twapi regist -ck %CONSUMER_KEY% -cs %CONSUMER_SECRET% -at %ACCESS_TOKEN% -as %ACCESS_SECRET%

twapi refresh -sql %SQLITE_PATH%

PAUSE
EXIT