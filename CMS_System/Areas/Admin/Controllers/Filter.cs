using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal;
using Model;
namespace CMS_System.Areas.Admin.Controllers
{
	public class Filter:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var path = filterContext.HttpContext.Request.Path;
			if (path.IndexOf("Login")>0)
			{
				return;
			}
			if (filterContext.HttpContext.Session["admin"] == null)
			{
				filterContext.Result = new System.Web.Mvc.RedirectResult("Login");
			}

		}
	}
}