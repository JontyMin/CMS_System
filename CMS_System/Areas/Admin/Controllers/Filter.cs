using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Dal;
using Model;
using Newtonsoft.Json;

namespace CMS_System.Areas.Admin.Controllers
{
	public class Filter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var path = filterContext.HttpContext.Request.Path;
			if (path.IndexOf("Login") > 0)
			{
				return;
			}
			if (filterContext.HttpContext.Session["admin"] == null)
			{
				filterContext.Result = new System.Web.Mvc.RedirectResult("Login");
			}
			else
			{
				//获取action名称
				string actionName = filterContext.ActionDescriptor.ActionName;
				//获取Controller名称
				string contorllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
				//获取触发当前方法的Action方法的所有参数 
				var paramss = filterContext.ActionParameters;
				string Content = Newtonsoft.Json.JsonConvert.SerializeObject(paramss);
				//获取操作人
				string admin = (filterContext.HttpContext.Session["admin"] as CMS_User).uname;
				//ip地址
				string Ip = filterContext.HttpContext.Request.UserHostAddress;
				//路径
				string Adr = filterContext.HttpContext.Request.Path;
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("-----------------------操作日志------------------------");
				sb.AppendLine("操作时间:" + DateTime.Now.ToString());
				sb.AppendLine("管 理 员:" + admin);
				sb.AppendLine("OnActionExecuting  控制器: " + contorllerName + "  动作:" + actionName + "  参数:" + Content);
				sb.AppendLine("ServerHostName:" + Adr);
				sb.AppendLine("ServerHostIP:" + Ip);

				RecordLog(sb.ToString());
			}
		}

		public void RecordLog(string content)
		{
			string logSite = AppDomain.CurrentDomain.BaseDirectory + "log.txt";//本地文件
			if (!File.Exists(logSite)) File.Create(logSite);

			//历史记录读取
			List<string> historyRecord = new List<string>();
			StreamReader sr = new StreamReader(logSite, false);
			string readLine;
			while (!string.IsNullOrEmpty(readLine = sr.ReadLine()))
			{
				historyRecord.Add(readLine);
			}
			sr.Close();
			sr.Dispose();
			historyRecord.Add(content);

			//新纪录存储
			StreamWriter sw = new StreamWriter(logSite, false);
			for (int i = 0; i < historyRecord.Count; i++)
			{
				sw.WriteLine(historyRecord[i]);
			}
			sw.Close();
			sw.Dispose();





		}
	}
}