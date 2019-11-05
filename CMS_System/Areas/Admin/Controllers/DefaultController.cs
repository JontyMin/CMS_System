using System;
using System.Collections.Generic;
using System.Linq;
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
		public ActionResult UserManager()
		{
			return View();
		}

		[HttpPost]
		public ActionResult GetPage(int page,int rows)
		{
			int total = d1.CMS_User.Count();
			var ls = db.GetCMS_Users(page,rows);
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total",total); 
			dc.Add("rows", ls);
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


		

	}
}