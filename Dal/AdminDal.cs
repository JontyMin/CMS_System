using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
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
		public List<CMS_User> GetCMS_Users(int page, int rows)
		{
			return db.CMS_User.OrderBy(c => c.uid)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
		}

		public void Dispose()
		{
			db.Dispose();
			
		}
	}
}
