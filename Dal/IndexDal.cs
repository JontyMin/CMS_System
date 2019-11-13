using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
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
			return db.CMS_Category.OrderBy(c=>c.navorder).Where(c=>c.nav==true).ToList();
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

		public bool ckuser(string uname)
		{
			return db.CMS_User.Any(c=>c.uname==uname);
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
		/// 根据标题模糊查询文章行数
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public int GetCount(string title)
		{
			return db.CMS_Article.Where(c => c.title.Contains(title)).Count();
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


		/// <summary>
		/// 根据标题模糊查询文章
		/// </summary>
		/// <param name="title"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public List<V_CMS_Article> GetCMS_ArticlesBytitle(string title, int page, int rows)
		{
			var ls = db.V_CMS_Article.OrderByDescending(c => c.hits)
				.Where(c => c.title.Contains(title) && c.state == 2)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			return ls;
		}
		/// <summary>
		/// 评论分页
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <param name="page">页码</param>
		/// <param name="rows">页大小</param>
		/// <returns></returns>
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
		/// 添加点击数
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public int Addhit(int ? aid)
		{
			string sql = "update CMS_Article set hits=(hits+1) where aid=@aid";
			SqlParameter[] sp = {
				new SqlParameter("@aid",aid),
			};
			int count= db.Database.ExecuteSqlCommand(sql,sp);
			return count;
		}
		/// <summary>
		/// 热门关键字
		/// </summary>
		/// <returns></returns>
		public List<CMS_Keyword> GetCMS_Keywords()
		{
			return db.CMS_Keyword.OrderByDescending(c => c.stimes)
				.Where(c=>c.show==true)
				.Take(5)
				.ToList();
		}

		/// <summary>
		/// 判断关键字是否存在
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public bool cktitle(string title)
		{
			return db.CMS_Keyword.Any(c => c.keyword == title);
		}
		/// <summary>
		/// 修改关键字
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public int UpdKeyWord(string title)
		{
			string sql = "update CMS_Keyword set stimes=(stimes+1),ltimes=GETDATE() where keyword=@keyword";
			SqlParameter[] sp = {
				new SqlParameter("@keyword",title),
			};
			int count = db.Database.ExecuteSqlCommand(sql, sp);
			return count;

		}

		/// <summary>
		/// 添加关键字
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		public int AddKeyWord(CMS_Keyword k)
		{
			db.CMS_Keyword.Add(k);
			return db.SaveChanges();
		}
		/// <summary>
		/// 添加评论
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public int AddComment(CMS_Comment c)
		{
			db.CMS_Comment.Add(c);
			return db.SaveChanges();
		}

		public int AddCommentCount(int aid)
		{
			string sql = "update CMS_Article set comments=(comments+1) where aid=@aid";
			SqlParameter[] sp = {
				new SqlParameter("@aid",aid),
			};
			int count = db.Database.ExecuteSqlCommand(sql, sp);
			return count;
		}

		public int UpdInfo(CMS_User u)
		{
			//db.Entry<CMS_User>(u).State = System.Data.Entity.EntityState.Modified;
			CMS_User ls = db.CMS_User.Find(u.uid);
			ls.uname = u.uname;
			ls.nname = u.nname;
			ls.face = u.face;
			ls.mobile = u.mobile;


			return db.SaveChanges();
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
