using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace Dal
{
	
	public  class IndexDal:IDisposable
	{
		db_CMS db = new db_CMS();

		/// <summary>
		/// 列表查询
		/// </summary>
		/// <returns></returns>
		public List<Model.CMS_Category> GetCMS_Categories()
		{
			return db.CMS_Category.OrderBy(c=>c.navorder).ToList();
		}


		/// <summary>
		/// 根据文章栏目提取热度最高文章
		/// </summary>
		/// <param name="cid">栏目id</param>
		/// <param name="top">条数</param>
		/// <returns></returns>
		public List<CMS_Article> GetCMS_ArticlesByCidTopN(int cid, int top)
		{

			return db.CMS_Article.OrderByDescending(c => c.hits)//根据点击率倒序查询
				.Where(c => c.state == 2 && c.cid==cid)//状态为发布
				.Take(top)//提取
				.ToList();
		}

		public bool Login(string uname, string upwd)
		{
			try
			{
				return db.CMS_User.Any(c => c.uname == uname && c.upwd == upwd);
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		/// <summary>
		/// 添加错误日志
		/// </summary>
		/// <param name="log"></param>
		/// <returns></returns>
		public int ErrorLog(LogFile log)
		{
			db.LogFile.Add(log);
			return db.SaveChanges();
		}
		/// <summary>
		/// 资源释放
		/// </summary>
		public void Dispose()
		{
			db.Dispose();

		}
	}
}
