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
	/// フォローログ
	/// FrinedsLogテーブルをベースに作成しています
	/// 作成日：2021/12/18 作成者gohya
	/// </summary>
	[Table("FrinedsLog")]
	public class FrinedsLogBase : INotifyPropertyChanged
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

		#region フォローした日[FollowAt]プロパティ
		/// <summary>
		/// フォローした日[FollowAt]プロパティ用変数
		/// </summary>
		DateTime _FollowAt = DateTime.MinValue;
		/// <summary>
		/// フォローした日[FollowAt]プロパティ
		/// </summary>
		[Column("FollowAt")]
		public DateTime FollowAt
		{
			get
			{
				return _FollowAt;
			}
			set
			{
				if (!_FollowAt.Equals(value))
				{
					_FollowAt = value;
					NotifyPropertyChanged("FollowAt");
				}
			}
		}
		#endregion

		#region リムーブ（解除）した日[RemoveAt]プロパティ
		/// <summary>
		/// リムーブ（解除）した日[RemoveAt]プロパティ用変数
		/// </summary>
		DateTime? _RemoveAt = null;
		/// <summary>
		/// リムーブ（解除）した日[RemoveAt]プロパティ
		/// </summary>
		[Column("RemoveAt")]
		public DateTime? RemoveAt
		{
			get
			{
				return _RemoveAt;
			}
			set
			{
				if (!_RemoveAt.Equals(value))
				{
					_RemoveAt = value;
					NotifyPropertyChanged("RemoveAt");
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
		public FrinedsLogBase()
		{

		}
		#endregion

		#region コピーコンストラクタ
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピー内容</param>
		public FrinedsLogBase(FrinedsLogBase item)
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
		public void Copy(FrinedsLogBase item)
		{
			this.UserId = item.UserId;

			this.FollowAt = item.FollowAt;

			this.RemoveAt = item.RemoveAt;


		}
		#endregion

		#region Insert処理
		/// <summary>
		/// Insert処理
		/// </summary>
		/// <param name="item">Insertする要素</param>
		public static void Insert(FrinedsLogBase item)
		{
			using (var db = new SQLiteDataContext())
			{
				Insert(db, item);
				db.SaveChanges();
			}
		}
		#endregion

		#region Insert処理
		/// <summary>
		/// Insert処理
		/// </summary>
		/// <param name="db">SQLiteDataContext</param>
		/// <param name="item">Insertする要素</param>
		public static void Insert(SQLiteDataContext db, FrinedsLogBase item)
		{
			// Insert
			db.Add<FrinedsLogBase>(item);
		}
		#endregion

		#region Update処理
		/// <summary>
		/// Update処理
		/// </summary>
		/// <param name="pk_item">更新する主キー（主キーの値のみ入っていれば良い）</param>
		/// <param name="update_item">テーブル更新後の状態</param>
		public static void Update(FrinedsLogBase pk_item, FrinedsLogBase update_item)
		{
			using (var db = new SQLiteDataContext())
			{
				Update(db, pk_item, update_item);
				db.SaveChanges();
			}
		}
		#endregion

		#region Update処理
		/// <summary>
		/// Update処理
		/// </summary>
		/// <param name="db">SQLiteDataContext</param>
		/// <param name="pk_item">更新する主キー（主キーの値のみ入っていれば良い）</param>
		/// <param name="update_item">テーブル更新後の状態</param>
		public static void Update(SQLiteDataContext db, FrinedsLogBase pk_item, FrinedsLogBase update_item)
		{
			var item = db.DbSet_FrinedsLog.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));

			if (item != null)
			{
				item.Copy(update_item);
			}
		}
		#endregion

		#region Delete処理
		/// <summary>
		/// Delete処理
		/// </summary>
		/// <param name="pk_item">削除する主キー（主キーの値のみ入っていれば良い）</param>
		public static void Delete(FrinedsLogBase pk_item)
		{
			using (var db = new SQLiteDataContext())
			{
				Delete(db, pk_item);
				db.SaveChanges();
			}
		}
		#endregion

		#region Delete処理
		/// <summary>
		/// Delete処理
		/// </summary>
		/// <param name="db">SQLiteDataContext</param>
		/// <param name="pk_item">削除する主キー（主キーの値のみ入っていれば良い）</param>
		public static void Delete(SQLiteDataContext db, FrinedsLogBase pk_item)
		{
			var item = db.DbSet_FrinedsLog.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));
			if (item != null)
			{
				db.DbSet_FrinedsLog.Remove(item);
			}
		}
		#endregion

		#region Select処理
		/// <summary>
		/// Select処理
		/// </summary>
		/// <returns>全件取得</returns>
		public static List<FrinedsLogBase> Select()
		{
			using (var db = new SQLiteDataContext())
			{
				return Select(db);
			}
		}
		#endregion

		#region Select処理
		/// <summary>
		/// Select処理
		/// </summary>
		/// <param name="db">SQLiteDataContext</param>
		/// <returns>全件取得</returns>
		public static List<FrinedsLogBase> Select(SQLiteDataContext db)
		{
			return db.DbSet_FrinedsLog.ToList<FrinedsLogBase>();
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
