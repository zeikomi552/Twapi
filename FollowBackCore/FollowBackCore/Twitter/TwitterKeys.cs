using CoreTweet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Twitter
{
    public class TwitterKeys : INotifyPropertyChanged
	{
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TwitterKeys()
		{

		}
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="consumer_key">コンシューマーキー</param>
		/// <param name="consumer_secret_key">シークレットキー</param>
		/// <param name="access_token">アクセストークン</param>
		/// <param name="access_secret">アクセスシークレット</param>
		public TwitterKeys(string consumer_key, string consumer_secret_key,
			string access_token, string access_secret)
		{
			this.ConsumerKey = consumer_key;
			this.ConsumerSecretKey = consumer_secret_key;
			this.AccessToken = access_token;
			this.AccessSecret = access_secret;
		}
		#endregion

		#region コンシューマーキー[ConsumerKey]プロパティ
		/// <summary>
		/// コンシューマーキー[ConsumerKey]プロパティ用変数
		/// </summary>
		string _ConsumerKey = string.Empty;
		/// <summary>
		/// コンシューマーキー[ConsumerKey]プロパティ
		/// </summary>
		public string ConsumerKey
		{
			get
			{
				return _ConsumerKey;
			}
			set
			{
				if (_ConsumerKey == null || !_ConsumerKey.Equals(value))
				{
					_ConsumerKey = value;
					NotifyPropertyChanged("ConsumerKey");
				}
			}
		}
		#endregion

		#region シークレットキー[ConsumerSecretKey]プロパティ
		/// <summary>
		/// シークレットキー[ConsumerSecretKey]プロパティ用変数
		/// </summary>
		string _ConsumerSecretKey = string.Empty;
		/// <summary>
		/// シークレットキー[ConsumerSecretKey]プロパティ
		/// </summary>
		public string ConsumerSecretKey
		{
			get
			{
				return _ConsumerSecretKey;
			}
			set
			{
				if (_ConsumerSecretKey == null || !_ConsumerSecretKey.Equals(value))
				{
					_ConsumerSecretKey = value;
					NotifyPropertyChanged("ConsumerSecretKey");
				}
			}
		}
		#endregion

		#region アクセストークン[AccessToken]プロパティ
		/// <summary>
		/// アクセストークン[AccessToken]プロパティ用変数
		/// </summary>
		string _AccessToken = string.Empty;
		/// <summary>
		/// アクセストークン[AccessToken]プロパティ
		/// </summary>
		public string AccessToken
		{
			get
			{
				return _AccessToken;
			}
			set
			{
				if (_AccessToken == null || !_AccessToken.Equals(value))
				{
					_AccessToken = value;
					NotifyPropertyChanged("AccessToken");
				}
			}
		}
		#endregion

		#region アクセスシークレット[AccessSecret]プロパティ
		/// <summary>
		/// アクセスシークレット[AccessSecret]プロパティ用変数
		/// </summary>
		string _AccessSecret = string.Empty;
		/// <summary>
		/// アクセスシークレット[AccessSecret]プロパティ
		/// </summary>
		public string AccessSecret
		{
			get
			{
				return _AccessSecret;
			}
			set
			{
				if (_AccessSecret == null || !_AccessSecret.Equals(value))
				{
					_AccessSecret = value;
					NotifyPropertyChanged("AccessSecret");
				}
			}
		}
		#endregion

		#region トークンの作成
		/// <summary>
		/// トークンの作成
		/// </summary>
		/// <returns>トークン</returns>
		public Tokens CreateToken()
		{
			// トークンの作成
			return CoreTweet.Tokens.Create(this.ConsumerKey,
					this.ConsumerSecretKey,
					this.AccessToken,
					this.AccessSecret);
		}
		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion
	}
}
