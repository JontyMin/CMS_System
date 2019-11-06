using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Web;
namespace Dal
{
	public class AdminDal : IDisposable
	{
		db_CMS db = new db_CMS();


		/// <summary>
		/// 用户表数据
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public List<CMS_User> GetCMS_Users( int page, int rows)
		{
			

			return db.CMS_User.OrderBy(c => c.uid)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
		}


		/// <summary>
		/// 修改用户
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public int UpdInfo(CMS_User u)
		{
			//db.Entry<CMS_User>(u).State = System.Data.Entity.EntityState.Modified;
			CMS_User ls = db.CMS_User.Find(u.uid);
			ls.admin = u.admin;
			return db.SaveChanges();
		}

		/// <summary>
		/// 添加用户
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public int AddUser(CMS_User u)
		{
			
			if (db.CMS_User.Any(c => c.uname == u.uname))
			{
				return -1;
			}
			else
			{
				db.CMS_User.Add(u);
				return db.SaveChanges();
			}
		}
		public void Dispose()
		{
			db.Dispose();
			
		}
	}
}
