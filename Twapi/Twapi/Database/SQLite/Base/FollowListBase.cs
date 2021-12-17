using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Database.SQLite.Base
{
	/// <summary>
	/// フォローリスト
	/// FollowListテーブルをベースに作成しています
	/// 作成日：2021/12/18 作成者gohya
	/// </summary>
	[Table("FollowList")]
	public class FollowListBase : INotifyPropertyChanged
	{
		#region パラメータ
		#region ユーザーID[UserId]プロパティ
		/// <summary>
		/// ユーザーID[UserId]プロパティ用変数
		/// </summary>
		long _UserId = 0;
		/// <summary>
		/// ユーザーID[UserId]プロパティ
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

		#region フォロー日時[FriendAt]プロパティ
		/// <summary>
		/// フォロー日時[FriendAt]プロパティ用変数
		/// </summary>
		DateTime? _FriendAt = null;
		/// <summary>
		/// フォロー日時[FriendAt]プロパティ
		/// </summary>
		[Column("FriendAt")]
		public DateTime? FriendAt
		{
			get
			{
				return _FriendAt;
			}
			set
			{
				if (!_FriendAt.Equals(value))
				{
					_FriendAt = value;
					NotifyPropertyChanged("FriendAt");
				}
			}
		}
		#endregion

		#region フォローしているかどうか[IsFriend]プロパティ
		/// <summary>
		/// フォローしているかどうか[IsFriend]プロパティ用変数
		/// </summary>
		bool _IsFriend = false;
		/// <summary>
		/// フォローしているかどうか[IsFriend]プロパティ
		/// </summary>
		[Column("IsFriend")]
		public bool IsFriend
		{
			get
			{
				return _IsFriend;
			}
			set
			{
				if (!_IsFriend.Equals(value))
				{
					_IsFriend = value;
					NotifyPropertyChanged("IsFriend");
				}
			}
		}
		#endregion

		#region フォロー確認日時[FollowBackAt]プロパティ
		/// <summary>
		/// フォロー確認日時[FollowBackAt]プロパティ用変数
		/// </summary>
		DateTime? _FollowBackAt = null;
		/// <summary>
		/// フォロー確認日時[FollowBackAt]プロパティ
		/// </summary>
		[Column("FollowBackAt")]
		public DateTime? FollowBackAt
		{
			get
			{
				return _FollowBackAt;
			}
			set
			{
				if (!_FollowBackAt.Equals(value))
				{
					_FollowBackAt = value;
					NotifyPropertyChanged("FollowBackAt");
				}
			}
		}
		#endregion

		#region フォローされているかどうか[IsFollowBack]プロパティ
		/// <summary>
		/// フォローされているかどうか[IsFollowBack]プロパティ用変数
		/// </summary>
		bool _IsFollowBack = false;
		/// <summary>
		/// フォローされているかどうか[IsFollowBack]プロパティ
		/// </summary>
		[Column("IsFollowBack")]
		public bool IsFollowBack
		{
			get
			{
				return _IsFollowBack;
			}
			set
			{
				if (!_IsFollowBack.Equals(value))
				{
					_IsFollowBack = value;
					NotifyPropertyChanged("IsFollowBack");
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
		public FollowListBase()
		{

		}
		#endregion

		#region コピーコンストラクタ
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピー内容</param>
		public FollowListBase(FollowListBase item)
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
		public void Copy(FollowListBase item)
		{
			this.UserId = item.UserId;

			this.FriendAt = item.FriendAt;

			this.IsFriend = item.IsFriend;

			this.FollowBackAt = item.FollowBackAt;

			this.IsFollowBack = item.IsFollowBack;


		}
		#endregion

		#region Insert処理
		/// <summary>
		/// Insert処理
		/// </summary>
		/// <param name="item">Insertする要素</param>
		public static void Insert(FollowListBase item)
		{
			using (var db = new SQLiteDataContext())
			{
				// Insert
				db.Add<FollowListBase>(item);

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
		public static void Update(FollowListBase pk_item, FollowListBase update_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_FollowList.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));

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
		public static void Delete(FollowListBase pk_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_FollowList.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));
				if (item != null)
				{
					db.DbSet_FollowList.Remove(item);
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
		public static List<FollowListBase> Select()
		{
			using (var db = new SQLiteDataContext())
			{
				return db.DbSet_FollowList.ToList<FollowListBase>();
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
