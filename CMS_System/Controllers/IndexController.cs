using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Dal;
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

		public ActionResult rest()
		{

			Session.Clear();
			return Redirect("/Index/index");
		}

		public ActionResult Page(int aid)
		{
			return View();
		}

		public ActionResult List(int? cid)
		{

			return View();
		}

		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Login(string uname,string upwd)
		{
			if (Session["uname"]!=null)
			{
				Session.Clear();
			}
			if (db.Login(uname,upwd))
			{
				Session["uname"] = uname;
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

		public ActionResult Regiest()
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