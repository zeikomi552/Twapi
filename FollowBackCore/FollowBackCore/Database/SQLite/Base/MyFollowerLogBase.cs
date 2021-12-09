using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Database.SQLite.Base
{
	/// <summary>
	/// 自分のフォロワーのリストログ
	/// MyFollowerLogテーブルをベースに作成しています
	/// 作成日：2021/12/09 作成者gohya
	/// </summary>
	[Table("MyFollowerLog")]
	public class MyFollowerLogBase : INotifyPropertyChanged
	{
		#region パラメータ
		#region 確認した日[CreateDt]プロパティ
		/// <summary>
		/// 確認した日[CreateDt]プロパティ用変数
		/// </summary>
		DateTime _CreateDt = DateTime.MinValue;
		/// <summary>
		/// 確認した日[CreateDt]プロパティ
		/// </summary>
		[Key]
		[Column("CreateDt")]
		public DateTime CreateDt
		{
			get
			{
				return _CreateDt;
			}
			set
			{
				if (!_CreateDt.Equals(value))
				{
					_CreateDt = value;
					NotifyPropertyChanged("CreateDt");
				}
			}
		}
		#endregion

		#region 同一タイミングの操作であることを示すGuid[Guid]プロパティ
		/// <summary>
		/// 同一タイミングの操作であることを示すGuid[Guid]プロパティ用変数
		/// </summary>
		String _Guid = string.Empty;
		/// <summary>
		/// 同一タイミングの操作であることを示すGuid[Guid]プロパティ
		/// </summary>
		[Column("Guid")]
		public String Guid
		{
			get
			{
				return _Guid;
			}
			set
			{
				if (!_Guid.Equals(value))
				{
					_Guid = value;
					NotifyPropertyChanged("Guid");
				}
			}
		}
		#endregion

		#region フォロワーのユーザーId[UserId]プロパティ
		/// <summary>
		/// フォロワーのユーザーId[UserId]プロパティ用変数
		/// </summary>
		long _UserId = 0;
		/// <summary>
		/// フォロワーのユーザーId[UserId]プロパティ
		/// </summary>
		[Key]
		[Column("UserId")]
		public long UserId
		{
			get
			{
				return _UserId;
			}
			set
			{
				if (!_UserId.Equals(value))
				{
					_UserId = value;
					NotifyPropertyChanged("UserId");
				}
			}
		}
		#endregion

		#region スクリーン名[ScreenName]プロパティ
		/// <summary>
		/// スクリーン名[ScreenName]プロパティ用変数
		/// </summary>
		String _ScreenName = string.Empty;
		/// <summary>
		/// スクリーン名[ScreenName]プロパティ
		/// </summary>
		[Column("ScreenName")]
		public String ScreenName
		{
			get
			{
				return _ScreenName;
			}
			set
			{
				if (!_ScreenName.Equals(value))
				{
					_ScreenName = value;
					NotifyPropertyChanged("ScreenName");
				}
			}
		}
		#endregion

		#region いいね数[FavoritesCount]プロパティ
		/// <summary>
		/// いいね数[FavoritesCount]プロパティ用変数
		/// </summary>
		long _FavoritesCount = 0;
		/// <summary>
		/// いいね数[FavoritesCount]プロパティ
		/// </summary>
		[Column("FavoritesCount")]
		public long FavoritesCount
		{
			get
			{
				return _FavoritesCount;
			}
			set
			{
				if (!_FavoritesCount.Equals(value))
				{
					_FavoritesCount = value;
					NotifyPropertyChanged("FavoritesCount");
				}
			}
		}
		#endregion

		#region フォロー数[FriendsCount]プロパティ
		/// <summary>
		/// フォロー数[FriendsCount]プロパティ用変数
		/// </summary>
		long _FriendsCount = 0;
		/// <summary>
		/// フォロー数[FriendsCount]プロパティ
		/// </summary>
		[Column("FriendsCount")]
		public long FriendsCount
		{
			get
			{
				return _FriendsCount;
			}
			set
			{
				if (!_FriendsCount.Equals(value))
				{
					_FriendsCount = value;
					NotifyPropertyChanged("FriendsCount");
				}
			}
		}
		#endregion

		#region フォロワー数[FollowerCount]プロパティ
		/// <summary>
		/// フォロワー数[FollowerCount]プロパティ用変数
		/// </summary>
		long _FollowerCount = 0;
		/// <summary>
		/// フォロワー数[FollowerCount]プロパティ
		/// </summary>
		[Column("FollowerCount")]
		public long FollowerCount
		{
			get
			{
				return _FollowerCount;
			}
			set
			{
				if (!_FollowerCount.Equals(value))
				{
					_FollowerCount = value;
					NotifyPropertyChanged("FollowerCount");
				}
			}
		}
		#endregion


		#endregion

		#region 関数
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MyFollowerLogBase()
		{

		}
		#endregion

		#region コピーコンストラクタ
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピー内容</param>
		public MyFollowerLogBase(MyFollowerLogBase item)
		{
			// 要素のコピー
			Copy(item);
		}
		#endregion

		#region コピー
		/// <summary>
		/// コピー
		/// </summary>
		/// <param name="item">コピー内容</param>
		public void Copy(MyFollowerLogBase item)
		{
			this.CreateDt = item.CreateDt;

			this.Guid = item.Guid;

			this.UserId = item.UserId;

			this.ScreenName = item.ScreenName;

			this.FavoritesCount = item.FavoritesCount;

			this.FriendsCount = item.FriendsCount;

			this.FollowerCount = item.FollowerCount;


		}
		#endregion

		#region Insert処理
		/// <summary>
		/// Insert処理
		/// </summary>
		/// <param name="item">Insertする要素</param>
		public static void Insert(MyFollowerLogBase item)
		{
			using (var db = new SQLiteDataContext())
			{
				// Insert
				db.Add<MyFollowerLogBase>(item);

				// コミット
				db.SaveChanges();
			}
		}
		#endregion

		#region Update処理
		/// <summary>
		/// Update処理
		/// </summary>
		/// <param name="pk_item">更新する主キー（主キーの値のみ入っていれば良い）</param>
		/// <param name="update_item">テーブル更新後の状態</param>
		public static void Update(MyFollowerLogBase pk_item, MyFollowerLogBase update_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_MyFollowerLog.SingleOrDefault(x => x.CreateDt.Equals(pk_item.CreateDt) && x.UserId.Equals(pk_item.UserId));

				if (item != null)
				{
					item.Copy(update_item);
					db.SaveChanges();
				}
			}
		}
		#endregion

		#region Delete処理
		/// <summary>
		/// Delete処理
		/// </summary>
		/// <param name="pk_item">削除する主キー（主キーの値のみ入っていれば良い）</param>
		public static void Delete(MyFollowerLogBase pk_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_MyFollowerLog.SingleOrDefault(x => x.CreateDt.Equals(pk_item.CreateDt) && x.UserId.Equals(pk_item.UserId));
				if (item != null)
				{
					db.DbSet_MyFollowerLog.Remove(item);
					db.SaveChanges();
				}
			}
		}
		#endregion

		#region Select処理
		/// <summary>
		/// Select処理
		/// </summary>
		/// <returns>全件取得</returns>
		public static List<MyFollowerLogBase> Select()
		{
			using (var db = new SQLiteDataContext())
			{
				return db.DbSet_MyFollowerLog.ToList<MyFollowerLogBase>();
			}
		}
		#endregion
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
