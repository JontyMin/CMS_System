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
			filterContext.Controller.ViewData["error"] = l.Logtext;
			filterContext.Result = new ViewResult()
			{
				ViewData = filterContext.Controller.ViewData,
				ViewName = "error"

			};
			db.ErrorLog(l);
			//throw new NotImplementedException();
			filterContext.ExceptionHandled = true;
		}
	}
}