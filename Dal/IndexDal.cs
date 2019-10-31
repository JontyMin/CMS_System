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
		public List<CMS_Article> GetCMS_ArticlesByCidTopN(int  cid, int top)
		{

			return db.CMS_Article.OrderByDescending(c => c.hits)//根据点击率倒序查询
				.Where(c => c.state == 2 && c.cid==cid)//状态为发布
				.Take(top)//提取
				.ToList();
		}
		

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="uname"></param>
		/// <param name="upwd"></param>
		/// <returns></returns>
		public CMS_User Login(string uname, string upwd)
		{
			try
			{
				if (db.CMS_User.FirstOrDefault(c => c.uname == uname && c.upwd == upwd)!=null)
				{
					return db.CMS_User.FirstOrDefault(c => c.uname == uname && c.upwd == upwd);
				}
				else
				{
					return null;
				}
				  

				
			}
			catch (Exception)
			{

				throw;
			}
			
		}


		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public int Regiest(CMS_User u)
		{
			try
			{
				db.CMS_User.Add(u);
				return db.SaveChanges();
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		/// <summary>
		/// 文章行数
		/// </summary>
		/// <param name="cid">栏目id</param>
		/// <returns></returns>
		public int GetCount(int ? cid,int ? aid)
		{
			if (cid!=null)
			{
				return db.CMS_Article.Where(c => c.cid == cid).Count();
			}
			else if (aid!=null)
			{
				return db.CMS_Comment.Where(c => c.aid == aid).Count();
			}
			else
			{
				return 0;
			}
			
		}

		

		/// <summary>
		/// 文章分页
		/// </summary>
		/// <param name="cid"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public List<V_CMS_Article> GetCMS_Articles(int? cid, int page, int rows)
		{
			var ls = db.V_CMS_Article.OrderByDescending(c => c.hits)
				.Where(c=>c.cid==cid&& c.state == 2)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			return ls;
		}

		public List<V_CMS_Comment> GetCMS_Comments(int ? aid,int page,int rows)
		{
			var ls = db.V_CMS_Comment.OrderByDescending(c => c.cmtime)
				.Where(c => c.aid == aid)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			return ls;

		}

		/// <summary>
		/// 根据文章id查询详细信息
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public V_CMS_Article GetV_CMS_ArticleByAid(int? aid)
		{
			return db.V_CMS_Article.Find(aid);
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
