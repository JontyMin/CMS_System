using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal;
using Model;
namespace CMS_System.Controllers
{
	public class Filter : ActionFilterAttribute, IExceptionFilter
	{
		IndexDal db = new IndexDal();
		public void OnException(ExceptionContext filterContext)
		{
			LogFile l = new LogFile()
			{
				Logtext = filterContext.Exception.Message,
				Errtime = DateTime.Now,
				Errip= filterContext.HttpContext.Request.UserHostAddress,
				Logsite= filterContext.HttpContext.Request.Path
			};
			//filterContext.Controller.ViewData["error"] = l.Logtext;
			//filterContext.Result = new ViewResult()
			//{
			//	ViewData = filterContext.Controller.ViewData,
			//	ViewName = "error"

			//};
			db.ErrorLog(l);
			//throw new NotImplementedException();
			filterContext.ExceptionHandled = true;
		}


		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var path = filterContext.HttpContext.Request.Path;
			if (path.IndexOf("Login") > 0 || path.IndexOf("login") > 0)
			{
				if (filterContext.HttpContext.Session["user"] != null)
				{
					filterContext.Result = new System.Web.Mvc.RedirectResult("Index");
				}
			}
		}
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			//if (filterContext.HttpContext.Session["user"] != null)
			//{
			//	filterContext.HttpContext.Response.Write("<script>xtip.msg('请勿重复登录')</script>");
			//}
		}
	}
}