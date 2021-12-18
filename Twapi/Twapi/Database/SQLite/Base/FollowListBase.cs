﻿using System;
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

		#region 作成日時[CreateAt]プロパティ
		/// <summary>
		/// 作成日時[CreateAt]プロパティ用変数
		/// </summary>
		DateTime _CreateAt = DateTime.MinValue;
		/// <summary>
		/// 作成日時[CreateAt]プロパティ
		/// </summary>
		[Column("CreateAt")]
		public DateTime CreateAt
		{
			get
			{
				return _CreateAt;
			}
			set
			{
				if (!_CreateAt.Equals(value))
				{
					_CreateAt = value;
					NotifyPropertyChanged("CreateAt");
				}
			}
		}
		#endregion

		#region 除外フラグ(フォロー済など)[IsExclude]プロパティ
		/// <summary>
		/// 除外フラグ(フォロー済など)[IsExclude]プロパティ用変数
		/// </summary>
		bool _IsExclude = false;
		/// <summary>
		/// 除外フラグ(フォロー済など)[IsExclude]プロパティ
		/// </summary>
		[Column("IsExclude")]
		public bool IsExclude
		{
			get
			{
				return _IsExclude;
			}
			set
			{
				if (!_IsExclude.Equals(value))
				{
					_IsExclude = value;
					NotifyPropertyChanged("IsExclude");
				}
			}
		}
		#endregion

		#region 除外理由(0:- 1:フォロー済み 2:プロテクト 3:ロック)[Reason]プロパティ
		/// <summary>
		/// 除外理由(0:- 1:フォロー済み 2:プロテクト 3:ロック)[Reason]プロパティ用変数
		/// </summary>
		Int32 _Reason = 0;
		/// <summary>
		/// 除外理由(0:- 1:フォロー済み 2:プロテクト 3:ロック)[Reason]プロパティ
		/// </summary>
		[Column("Reason")]
		public Int32 Reason
		{
			get
			{
				return _Reason;
			}
			set
			{
				if (!_Reason.Equals(value))
				{
					_Reason = value;
					NotifyPropertyChanged("Reason");
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

			this.CreateAt = item.CreateAt;

			this.IsExclude = item.IsExclude;

			this.Reason = item.Reason;


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
		public static void Insert(SQLiteDataContext db, FollowListBase item)
		{
			// Insert
			db.Add<FollowListBase>(item);
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
		public static void Update(SQLiteDataContext db, FollowListBase pk_item, FollowListBase update_item)
		{
			var item = db.DbSet_FollowList.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));

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
		public static void Delete(FollowListBase pk_item)
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
		public static void Delete(SQLiteDataContext db, FollowListBase pk_item)
		{
			var item = db.DbSet_FollowList.SingleOrDefault(x => x.UserId.Equals(pk_item.UserId));
			if (item != null)
			{
				db.DbSet_FollowList.Remove(item);
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
		public static List<FollowListBase> Select(SQLiteDataContext db)
		{
			return db.DbSet_FollowList.ToList<FollowListBase>();
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
