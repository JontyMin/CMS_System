using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dal;
using Model;
namespace CMS_System.Areas.Admin.Controllers
{

    public class DefaultController : Controller
    {
		AdminDal db = new AdminDal();
		db_CMS d1 = new db_CMS();
        // GET: Admin/Default
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult main()
		{
			return View();
		}

		public ActionResult addarticle()
		{
			return View();
		}
		public ActionResult listarticle()
		{
			return View();
		}
		public ActionResult editarticle(int aid)
		{
			if (aid<0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else if(aid.ToString()=="")
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			else
			{
				return View();
			}
		
		}

		public ActionResult GetArtByAid(int aid)
		{
			var ls = d1.V_CMS_Article.Find(aid);
			return Json(ls);
		}
		[ValidateInput(false)]
		public int UpdArtByAid(CMS_Article a)
		{
			var ls=d1.CMS_Article.Find(a.aid);
			ls.title = a.title;
			ls.cid = a.cid;
			ls.hits = a.hits;
			ls.comments = a.comments;
			ls.state = a.state;
			ls.ahtml = a.ahtml;
			return d1.SaveChanges();
			
		}

		[ValidateInput(false)]
		public int AddArt(CMS_Article u)
		{
			if (u.state==1)
			{
				u.ctime = DateTime.Now;
				u.ptime = null;
			}
			else if(u.state==2)
			{
				u.ctime = DateTime.Now;
				u.ptime = DateTime.Now;
			}
			d1.CMS_Article.Add(u);
			if (d1.SaveChanges()>0)
			{
				return 1;
			}
			else
			{
				return 0;
			}
			
		}

		/// <summary>
		/// 栏目
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCategory()
		{
			return Json(d1.CMS_Category.ToList());
		}

		public int DelArt(int aid)
		{
			var ls=d1.CMS_Article.Find(aid);
			d1.CMS_Article.Remove(ls);
			return d1.SaveChanges();
		}
		#region 用户管理


		public ActionResult UserManager()
		{
			return View();
		}

		/// <summary>
		/// 用户表分页
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetPage(bool ? admin, string uname, int page,int rows)
		{
			if (admin == null && uname == "")
			{
				int total = d1.CMS_User.Count();
				var ls = db.GetCMS_Users(page, rows);
				Dictionary<string, object> dc = new Dictionary<string, object>();
				dc.Add("total", total);
				dc.Add("rows", ls);
				return Json(dc);
			}
			else
			{
				var total = d1.CMS_User.Where(c => c.admin == admin && c.uname.Contains(uname)).Count();
				var ls = d1.CMS_User.Where(c => c.admin == admin && c.uname.Contains(uname))
					.OrderBy(c => c.uid)
					.Skip((page - 1) * rows)
					.Take(rows)
					.ToList();
				Dictionary<string, object> dc = new Dictionary<string, object>();
				dc.Add("total", total);
				dc.Add("rows", ls);
				return Json(dc);
			}




		}

		public ActionResult GetArticlePage(int page, int rows)
		{
			var total = d1.V_CMS_Article.Count();
			var ls = d1.V_CMS_Article.OrderBy(c => c.aid)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total",total);
			dc.Add("rows",ls);
			return Json(dc);
		}


		/// <summary>
		/// 用户删除
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public ActionResult delUser(int uid)
		{
			var ls=d1.CMS_User.Find(uid);
			d1.CMS_User.Remove(ls);
			if (d1.SaveChanges()>0)
			{
				return Json(new
				{
					state=true,
					msg="删除成功",
				});
			}
			else
			{
				return Json(new
				{
					state = false,
					msg = "貌似出错了。。。",
				});
			}
		}

		/// <summary>
		/// 修改用户
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public int UpdInfo(CMS_User u)
		{
			

			if (db.UpdInfo(u) > 0)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}


		/// <summary>
		/// 添加用户
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		[Obsolete]
		public int AddUser(CMS_User u)
		{
			u.face = "images/face.jpg";
			u.upwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(u.upwd, "MD5");
			return db.AddUser(u); 
		}
		#endregion

	}
}