using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Dal;
using System.Threading;
using System.Net;

namespace CMS_System.Controllers
{
	[Filter]
	public class IndexController : Controller, IDisposable
	{
		IndexDal db = new IndexDal();
		// GET: Index
		public ActionResult Index()
		{
			var ls = db.GetCMS_ArticlesByCidTopN(1, 7);//网站公告
			ViewBag.Center = db.GetCMS_ArticlesByCidTopN(2, 6);//产品中心
			ViewBag.service = db.GetCMS_ArticlesByCidTopN(3, 6);//定制服务
			ViewBag.Case = db.GetCMS_ArticlesByCidTopN(4, 6);//成功案例
			return View(ls);
		}

		/// <summary>
		/// 注销，清空session
		/// </summary>
		/// <returns></returns>
		public ActionResult rest()
		{
			Session.Clear();
			return Redirect("/Index/index");
		}
		[HttpGet]
		public ActionResult Page()
		{
			return View();
		}
		[HttpGet]
		public ActionResult Page(int ? aid)
		{
			return View();
		}

		[HttpGet]
		public ActionResult List(int ? cid)
		{
			if (cid == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			else
			{
				return View();
			}
			
		}

		[HttpPost]
		public ActionResult List( int ? cid,int page, int rows)
		{
			
				int total = db.GetCount(cid);
				var ls = db.GetCMS_Articles(cid, page, rows);
				Dictionary<string, object> dc = new Dictionary<string, object>();
				dc.Add("total", total);
				dc.Add("rows", ls);
				return Json(dc);
			
		}
		/// <summary>
		/// 请求登录界面
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}


		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="uname"></param>
		/// <param name="upwd"></param>
		/// <returns></returns>
		[HttpPost]
		[Obsolete]
		public ActionResult Login(string uname,string upwd)
		{
			upwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(upwd, "MD5");
			if (db.Login(uname,upwd)!=null)
			{

				var u = db.Login(uname,upwd);

				Session["user"] = u;
				return Json(new
				{
					message = "登录成功",
					state = true

				}) ;


				
			}
			else
			{
				return Json(new
				{
					message="账号或密码错误",
					state=false
				});
			}
			
		}

		/// <summary>
		/// 注册
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Regiest()
		{
			return View();
		}
		[HttpPost]
		[Obsolete]
		public ActionResult Regiest(CMS_User u)
		{
			//Thread.Sleep(3000);
			u.upwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(u.upwd, "MD5");//密码MD5加密
			u.admin = false;
			u.face = "images/face.jpg";
			if (db.Regiest(u)>0)
			{
				Session["user"] = u;
				return Json(new
				{
					state=true,
					message="注册成功,正在跳转首页",

				});
			}
			else
			{
				return Json(new
				{
					state = false,
					message = "服务器开小差了..."
				});
			}
			
		}
		public ActionResult UserInfo()
		{
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}