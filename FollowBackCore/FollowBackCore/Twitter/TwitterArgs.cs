using FollowBackCore.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Twitter
{
    [AttributeUsage(AttributeTargets.Property)]
    class EndpointParamAttribute : Attribute
    {
        public EndpointParamAttribute(string key)
        {
            this.Key = key;
        }
        public string Key { get; set; }
    }
    public class TwitterArgs
    {
        /// <summary>
        /// 引数リスト
        /// </summary>
        public static Dictionary<string, string> Args { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// コマンドリスト
        /// </summary>
        public static CommandList Commands { get; set; } = new CommandList();


        public static TwitterCommandOptions CommandOptions { get; set; } = new TwitterCommandOptions();

        /// <summary>
        /// コマンドのパラメータを設定する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCommandParameter(string key, string value)
        {
            // Get instance of the attribute.
            EndpointParamAttribute myAttribute =
                (EndpointParamAttribute)Attribute.GetCustomAttribute(typeof(TwitterCommandOptions), typeof(EndpointParamAttribute));

            //プロパティ一覧を取得
            var properties = CommandOptions.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // プロパティに付いている属性を取得する
                var endpoint = (Attribute.GetCustomAttributes(
                            typeof(TwitterCommandOptions).GetProperty(prop.Name),
                            typeof(EndpointParamAttribute)) as EndpointParamAttribute[]).FirstOrDefault();

                // 取得内容の確認
                if (endpoint != null)
                {
                    if (endpoint.Key.Equals(key))
                    {
                        prop.SetValue(CommandOptions, (string)value);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// コマンドを分解してセットする
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        public static void SetCommand(string[] args)
        {
            // 引数分ループする
            for (int i = 0; i < args.Length; i++)
            {
                var check = (from x in Commands.Items
                             where x.Key.Equals(args[i].ToLower())
                             select x).FirstOrDefault();

                // nullチェック
                if (check != null)
                {
                    // Action系のコマンドかどうかを確認する
                    if (check.CommandType == Command.CommandTypeEnum.Action)
                    {
                        // アクションキーが存在しないことを確認する
                        if (!Args.ContainsKey("action"))
                        {
                            // キーの登録
                            Args.Add("action", args.Length > i ? args[i] : string.Empty);
                        }
                    }
                    // コンシューマーキーなどのキー系かどうかを確認する
                    else if (check.CommandType == Command.CommandTypeEnum.Keys)
                    {
                        switch (args[i].ToLower())
                        {
                            case "-ck": // コンシューマーキー
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.ConsumerKey = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-cs": // コンシューマーシークレット
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.ConsumerSecretKey = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-at": // アクセストークン
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.AccessToken = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                            case "-as": // アクセスシークレット
                                {
                                    i++;
                                    TwitterAPI.TwitterKeys.AccessSecret = args.Length > i ? args[i] : string.Empty;
                                    break;
                                }
                        }
                    }
                    // オプション系
                    else if (check.CommandType == Command.CommandTypeEnum.Option)
                    {
                        switch (args[i].ToLower())
                        {
                            case "-d":  // ディレクトリ
                                {
                                    i++;
                                    Args.Add(check.Key.ToLower(), args.Length > i ? args[i] : string.Empty);
                                    break;
                                }
                        }
                    }
                    // アクションに対するパラメータ系
                    else if (check.CommandType == Command.CommandTypeEnum.Parameter)
                    {
                        i++;
                        Args.Add(check.Key.ToLower(), args.Length > i ? args[i] : string.Empty);
                    }
                }
            }

            foreach (var param in TwitterArgs.Args)
            {
                SetCommandParameter(param.Key, param.Value);

            }

        }

    }
}
