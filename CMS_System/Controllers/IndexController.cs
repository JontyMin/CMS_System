using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Dal;
namespace CMS_System.Controllers
{
    public class IndexController : Controller ,IDisposable
    {
		IndexDal db = new IndexDal();
        // GET: Index
        public ActionResult Index()
        {
			var ls = db.GetCMS_ArticlesByCidTopN(1,7);//网站公告
			ViewBag.Center = db.GetCMS_ArticlesByCidTopN(2, 6);//产品中心
			ViewBag.service= db.GetCMS_ArticlesByCidTopN(3, 6);//定制服务
			ViewBag.Case = db.GetCMS_ArticlesByCidTopN(4, 6);//成功案例
			return View(ls);
        }

		public ActionResult Page(int aid)
		{
			return View();
		}

		public ActionResult List(int cid)
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