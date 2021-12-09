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
	/// フォローした履歴
	/// TwitterFollowLogテーブルをベースに作成しています
	/// 作成日：2021/12/09 作成者gohya
	/// </summary>
	[Table("TwitterFollowLog")]
	public class TwitterFollowLogBase : INotifyPropertyChanged
	{
		#region パラメータ
		#region 操作した日[CreateDt]プロパティ
		/// <summary>
		/// 操作した日[CreateDt]プロパティ用変数
		/// </summary>
		DateTime _CreateDt = DateTime.MinValue;
		/// <summary>
		/// 操作した日[CreateDt]プロパティ
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

		#region 同一タイミングの操作であることを意味するGuid[Guid]プロパティ
		/// <summary>
		/// 同一タイミングの操作であることを意味するGuid[Guid]プロパティ用変数
		/// </summary>
		String _Guid = string.Empty;
		/// <summary>
		/// 同一タイミングの操作であることを意味するGuid[Guid]プロパティ
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

		#region フォロー対象のユーザーID[UserId]プロパティ
		/// <summary>
		/// フォロー対象のユーザーID[UserId]プロパティ用変数
		/// </summary>
		long _UserId = 0;
		/// <summary>
		/// フォロー対象のユーザーID[UserId]プロパティ
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

		#region 1:フォローした -1:フォロー外した[Action]プロパティ
		/// <summary>
		/// 1:フォローした -1:フォロー外した[Action]プロパティ用変数
		/// </summary>
		Int32 _Action = 0;
		/// <summary>
		/// 1:フォローした -1:フォロー外した[Action]プロパティ
		/// </summary>
		[Column("Action")]
		public Int32 Action
		{
			get
			{
				return _Action;
			}
			set
			{
				if (!_Action.Equals(value))
				{
					_Action = value;
					NotifyPropertyChanged("Action");
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
		public TwitterFollowLogBase()
		{

		}
		#endregion

		#region コピーコンストラクタ
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピー内容</param>
		public TwitterFollowLogBase(TwitterFollowLogBase item)
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
		public void Copy(TwitterFollowLogBase item)
		{
			this.CreateDt = item.CreateDt;

			this.Guid = item.Guid;

			this.UserId = item.UserId;

			this.Action = item.Action;


		}
		#endregion

		#region Insert処理
		/// <summary>
		/// Insert処理
		/// </summary>
		/// <param name="item">Insertする要素</param>
		public static void Insert(TwitterFollowLogBase item)
		{
			using (var db = new SQLiteDataContext())
			{
				// Insert
				db.Add<TwitterFollowLogBase>(item);

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
		public static void Update(TwitterFollowLogBase pk_item, TwitterFollowLogBase update_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_TwitterFollowLog.SingleOrDefault(x => x.CreateDt.Equals(pk_item.CreateDt) && x.UserId.Equals(pk_item.UserId));

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
		public static void Delete(TwitterFollowLogBase pk_item)
		{
			using (var db = new SQLiteDataContext())
			{
				var item = db.DbSet_TwitterFollowLog.SingleOrDefault(x => x.CreateDt.Equals(pk_item.CreateDt) && x.UserId.Equals(pk_item.UserId));
				if (item != null)
				{
					db.DbSet_TwitterFollowLog.Remove(item);
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
		public static List<TwitterFollowLogBase> Select()
		{
			using (var db = new SQLiteDataContext())
			{
				return db.DbSet_TwitterFollowLog.ToList<TwitterFollowLogBase>();
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
