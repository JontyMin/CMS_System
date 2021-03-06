﻿using System;
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

		/// <summary>
		/// 首页数据
		/// </summary>
		/// <returns></returns>
		// GET: Index
		public ActionResult Index()
		{

			var ls = db.GetCMS_ArticlesByCidTopN(1, 7);//网站公告
			ViewBag.Keyword = db.GetCMS_Keywords();
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


		/// <summary>
		/// 文章页
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Page(int ? aid)
		{
			if (aid==null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			else if (aid<1)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				if (db.Addhit(aid)>0)
				{
					var Article = db.GetV_CMS_ArticleByAid(aid);
					return View(Article);
				}
				else
				{
					return null;
				}
				
			}
			
		}

	
		/// <summary>
		/// 评论分页
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
	
		public ActionResult Page(int ? aid,int page,int rows)
		{
			int total = db.GetCount(null, aid);
			var Comments = db.GetCMS_Comments(aid,page,rows);
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total",total);
			dc.Add("Comments", Comments);
			return Json(dc);
		}


		/// <summary>
		/// 搜索
		/// </summary>
		/// <param name="title">标题内容</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Search(string title)
		{
			if (title==null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			else
			{
				if (db.cktitle(title))
				{
					db.UpdKeyWord(title);
				}
				else
				{
					CMS_Keyword k = new CMS_Keyword()
					{
						keyword = title,
						ltimes = DateTime.Now,
						stimes=1,
						show=true
					};
					db.AddKeyWord(k);
				}

				return View();
			}
		}

		/// <summary>
		/// 模糊查询内容分页
		/// </summary>
		/// <param name="title"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Search(string title,int page,int rows)
		{
			int total = db.GetCount(title);
			var ls = db.GetCMS_ArticlesBytitle(title, page, rows);
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total", total);
			dc.Add("rows", ls);
			return Json(dc);
		}

		/// <summary>
		/// 评论
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <param name="cmhtml">评论内容</param>
		/// <returns></returns>
		[ValidateInput(false)]
		public ActionResult AddComment(int aid, string cmhtml)
		{
			if (Session["user"] == null)
			{
				return Json(new
				{
					state = false,
					message = "请先登录"
				});
			}
			else
			{


				CMS_Comment c = new CMS_Comment()
				{
					aid = aid,
					uid = (Session["user"] as CMS_User).uid,
					cmtime = DateTime.Now,
					cmhtml = cmhtml
				};
				if (db.AddCommentCount(aid)>0)
				{

				}
				if (db.AddComment(c)>0)
				{
					
					return Json(new
					{
						state = true,
						message = "评论成功"
					});
				}
				else
				{
					return Json(new
					{
						state = false,
						message = "网络貌似出问题了..."
					});
				}
				
			}
		
		}

		/// <summary>
		/// 文章列表
		/// </summary>
		/// <param name="cid">栏目id</param>
		/// <returns></returns>
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


		/// <summary>
		/// 文章列表分页
		/// </summary>
		/// <param name="cid"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult List( int ? cid,int page, int rows)
		{
			
				int total = db.GetCount(cid,null);
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
			u.admin = false;//默认非管理
			u.face = "images/face.jpg";//默认头像
			if (db.ckuser(u.uname))//验证用户名是否存在
			{
				return Json(new
				{
					state = false,
					message = "用户名已存在",

				});
			}
			else
			if (db.Regiest(u) > 0)
			{
				Session["user"] = u;
				return Json(new
				{
					state = true,
					message = "注册成功,正在跳转首页",

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



		/// <summary>
		/// 用户信息
		/// </summary>
		/// <returns></returns>

		public ActionResult Info()
		{

			CMS_User u = Session["user"] as CMS_User;
			
			return View(u);
		}

		[HttpPost]
		public JsonResult Upload()
		{
			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase f = Request.Files["file"];//最简单的获取方法
				string name = f.FileName;
				int index = name.LastIndexOf('.');
				string str = name.Substring(index);
				string newName = "face" + new Random().Next().ToString() + str;
				f.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "/images/" + newName);//保存图片

				//这下面是返回json给前端 
				var data1 = new
				{
					src = "images/" + newName,//服务器储存路径
				};
				var Person = new
				{
					code = 0,//0表示成功
					msg = "上传成功",//这个是失败返回的错误
					data = data1
				};
				return Json(Person);//格式化为json
			}
			else
			{
				var Person = new
				{
					code = 1,//0表示成功
					msg = "上传失败",//这个是失败返回的错误
					
				};
				return Json(Person);//格式化为json
				
			}

		}

		public ActionResult UpdInfo(CMS_User u)
		{
			u.uid = (Session["user"] as CMS_User).uid;
			
			if (db.UpdInfo(u)>0)
			{
				Session.Clear();
				Session["user"] = u;
				return Json(new
				{
					state = 1,
					message="修改成功"
				}) ;
			}
			else
			{
				return Json(new
				{
					state = 0,
					message = "修改失败"
				});
			}
		}

		/// <summary>
		/// 资源释放
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}