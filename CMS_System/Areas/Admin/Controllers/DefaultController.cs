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

	[Filter]
	public class DefaultController : Controller
	{
		AdminDal db = new AdminDal();
		db_CMS d1 = new db_CMS();
		// GET: Admin/Default
		public ActionResult Index()
		{

			return View();

		}

		#region 后台登录


		/// <summary>
		/// 登出
		/// </summary>
		/// <returns></returns>
		public ActionResult LoginOut()
		{
			Session.Clear();
			return Redirect("Login");
		}

		/// <summary>
		/// 后台登录
		/// </summary>
		/// <returns></returns>
		public ActionResult Login()
		{
			return View();
		}

		/// <summary>
		/// 登录验证
		/// </summary>
		/// <param name="uname"></param>
		/// <param name="upwd"></param>
		/// <returns></returns>
		[Obsolete]
		public ActionResult AdminLogin(string uname, string upwd)
		{
			upwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(upwd, "MD5");
			CMS_User ls = d1.CMS_User.FirstOrDefault(c => c.uname == uname && c.upwd == upwd);
			if (ls.admin == true)
			{
				Session["admin"] = ls;
				return Json(new
				{
					msg = "管理员" + ls.uname + "登录成功",
					state = true

				});
			}
			else
			{
				return Json(new
				{
					msg = "管理员或密码错误",
					state = false

				});
			}

		}
		#endregion


		#region 文章管理

		/// <summary>
		/// 添加文章视图
		/// </summary>
		/// <returns></returns>
		public ActionResult addarticle()
		{
			var ls = d1.CMS_Category.ToList();
			return View(ls);
		}

		/// <summary>
		/// 文章列表
		/// </summary>
		/// <param name="stime"></param>
		/// <param name="etime"></param>
		/// <param name="state"></param>
		/// <param name="title"></param>
		/// <param name="cid"></param>
		/// <returns></returns>
		public ActionResult GetArticlePage(int page, int rows, DateTime? stime, DateTime? etime, int? state, string title, int? cid)
		{

			//int total = d1.V_CMS_Article.Count();
			var ls = d1.V_CMS_Article.OrderBy(c => c.aid) as IEnumerable<V_CMS_Article>;
			//if (cid!=null)
			//{
			//	ls = ls.Where(c => c.cid == cid).Skip((page - 1) * rows).Take(rows);
			//	total = d1.V_CMS_Article.Where(c => c.cid == cid).Count();
			//}
			//else
			//{
			//	ls = ls.Skip((page - 1) * rows).Take(rows);
				
			//}


			//var ls = d1.V_CMS_Article.OrderBy(c => c.aid) as IEnumerable<Model.V_CMS_Article>;
			//.ToList();
			if (stime != null )
			{
				ls = ls.Where(x => x.ptime >= stime);
			}
			if (etime != null )
			{
				ls = ls.Where(x => x.ptime <= etime);
			} 
			if (state != null && state > 0)
			{
				ls = ls.Where(x => x.state == state);
			}
			if (title != null && title != "")
			{
				ls = ls.Where(x => x.title.Contains(title));
			}
			if (cid != null && cid > 0)
			{
				ls = ls.Where(c => c.cid == cid);

			}
			int total = ls.Count();
			ls = ls.Skip((page - 1) * rows).Take(rows);

			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total",total);
			dc.Add("rows",ls.ToList());
			return Json(dc);
			//return new JsonResult()
			//{
			//	Data = ls.ToList(),
			//	JsonRequestBehavior = JsonRequestBehavior.AllowGet,
			//	MaxJsonLength = 10240000
			//};
		}

		/// <summary>
		///文章管理视图
		/// </summary>
		/// <returns></returns>
		public ActionResult listarticle()
		{
			return View();
		}

		/// <summary>
		/// 根据id查找文章
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public ActionResult GetArtByAid(int aid)
		{
			var ls = d1.V_CMS_Article.Find(aid);
			return Json(ls);
		}

		/// <summary>
		/// 修改文章
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		[ValidateInput(false)]
		public int UpdArtByAid(CMS_Article a)
		{
			var ls = d1.CMS_Article.Find(a.aid);
			ls.title = a.title;
			ls.cid = a.cid;
			ls.hits = a.hits;
			ls.comments = a.comments;
			ls.state = a.state;
			ls.ahtml = a.ahtml;
			return d1.SaveChanges();

		}

		/// <summary>
		/// 添加文章
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		[ValidateInput(false)]
		public int AddArt(CMS_Article u)
		{
			if (u.state == 1)
			{
				u.ctime = DateTime.Now;
				u.ptime = null;
			}
			else if (u.state == 2)
			{
				u.ctime = DateTime.Now;
				u.ptime = DateTime.Now;
			}
			d1.CMS_Article.Add(u);
			if (d1.SaveChanges() > 0)
			{
				return 1;
			}
			else
			{
				return 0;
			}

		}

		/// <summary>
		/// 修改文章视图
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public ActionResult editarticle(int aid)
		{
			if (aid < 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else if (aid.ToString() == "")
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			else
			{
				var ls = d1.CMS_Category.ToList();
				return View(ls);
			}

		}

		/// <summary>
		/// 根据文章id修改栏目
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <param name="cid">栏目id</param>
		/// <returns></returns>
		public int UpdArtCidByAid(int aid, int cid)
		{
			var ls = d1.CMS_Article.Find(aid);
			ls.cid = cid;
			return d1.SaveChanges();
		}

		/// <summary>
		/// 根据id修改置顶
		/// </summary>
		/// <param name="aid"></param>
		/// <param name="istop"></param>
		/// <returns></returns>
		public int IstopByAid(int aid, int istop)
		{
			var ls = d1.CMS_Article.Find(aid);
			ls.istop = Convert.ToBoolean(istop);
			return d1.SaveChanges();
		}

		/// <summary>
		/// tree栏目
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCategory()
		{

			var ls = d1.CMS_Category.ToList()
				.Select(c => new { id = c.cid, text = c.ctitle });
			return Json(ls);
		}

		/// <summary>
		/// 删除文章
		/// </summary>
		/// <param name="aid">文章id</param>
		/// <returns></returns>
		public int DelArt(int aid)
		{
			var ls = d1.CMS_Article.Find(aid);
			d1.CMS_Article.Remove(ls);
			return d1.SaveChanges();
		}
		#endregion
		

		#region main


		public ActionResult main()
		{

			return View();

		}

		/// <summary>
		/// 最新评论
		/// </summary>
		/// <returns></returns>
		public ActionResult GetNewCom()
		{
			var ls = d1.V_CMS_Comment.Join(d1.V_CMS_Article,
				c => c.cmid,
				a => a.aid,
				(c, a) => new V_ArtCom
				{
					cmid = c.cmid,
					aid = c.aid,
					uid = c.uid,
					nname = a.nname,
					cmtime = c.cmtime,
					title = a.title,
					cmhtml = c.cmhtml
				}
				).OrderByDescending(c => c.cmid).Take(5).ToList();

			//var ls = d1.CMS_Comment.OrderByDescending(c => c.cmid).Take(10).ToList();
			return Json(ls);
		}

		/// <summary>
		/// 最新注册用户
		/// </summary>
		/// <returns></returns>
		public ActionResult NewUser()
		{
			var ls = d1.CMS_User.OrderByDescending(c => c.uid).Take(5).ToList();
			return Json(ls);
		}

		/// <summary>
		/// 最新发布文章
		/// </summary>
		/// <returns></returns>
		public ActionResult NewArt()
		{
			var ls = d1.V_CMS_Article.OrderByDescending(c => c.aid).Take(5).ToList();
			return Json(ls);
		}

		#endregion


		#region 栏目管理

		
		public ActionResult ListColumn()
		{
			return View();
		}

		/// <summary>
		/// 栏目
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCategory1()
		{

			var ls = d1.CMS_Category.ToList();
			return Json(ls);
		}

		/// <summary>
		/// 栏目表分页
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public ActionResult GetPageCol(int page, int rows)
		{
			int total = d1.CMS_Category.Count();
			var ls = d1.CMS_Category.OrderBy(c => c.cid)
				.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total", total);
			dc.Add("rows", ls);
			return Json(dc);
		}

		/// <summary>
		/// 删除栏目
		/// </summary>
		/// <param name="cid"></param>
		/// <returns></returns>
		public ActionResult delCol(int cid)
		{
			var ls = d1.CMS_Category.Find(cid);
			d1.CMS_Category.Remove(ls);
			if (d1.SaveChanges() > 0)
			{
				return Json(new
				{
					state = true,
					msg = "删除成功",
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
		/// 修改栏目信息
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public int UpdInfoByCid(CMS_Category u)
		{

			var ls = d1.CMS_Category.Find(u.cid);
			ls.ctitle = u.ctitle;
			ls.cname = u.cname;
			ls.nav = u.nav;
			ls.search = u.search;
			return d1.SaveChanges();

		}

		/// <summary>
		/// 添加栏目
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public int AddCol(CMS_Category c)
		{
			var ls = d1.CMS_Category.Max(a => a.navorder);
			c.navorder = ls + 1;
			d1.CMS_Category.Add(c);
			return d1.SaveChanges();
		}

		#endregion


		#region 用户管理


		/// <summary>
		/// 用户管理
		/// </summary>
		/// <returns></returns>
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
		public ActionResult GetPage(bool? admin, string uname, int page, int rows)
		{
			var ls = d1.CMS_User.OrderBy(c=>c.uid) as IEnumerable<CMS_User>;
			if (admin !=null)
			{
				ls = ls.Where(c=>c.admin ==admin);
			}
			if (uname!=null && uname!="")
			{
				ls = ls.Where(c => c.uname.Contains(uname));
			}
			int total = ls.Count();
			ls=ls.Skip((page - 1) * rows)
					.Take(rows)
					.ToList();
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total", total);
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
			var ls = d1.CMS_User.Find(uid);
			d1.CMS_User.Remove(ls);
			int Sessionuid = (Session["admin"] as CMS_User).uid;
			if (uid==Sessionuid)
			{
				return Json(new
				{
					state = false,
					msg = "无法进行此操作",
				});
			} else
			if (d1.SaveChanges() > 0)
			{
				return Json(new
				{
					state = true,
					msg = "删除成功",
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

		#region 评论管理

		
		public ActionResult Commentlist()
		{
			return View();
		}

		/// <summary>
		/// 文章评论列表
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public ActionResult ArtComList(int? aid)
		{
			Session["aid"] = aid;
			return View();
		}


		public ActionResult CommentTree()
		{
			return View();
		}

		/// <summary>
		/// 文章评论
		/// </summary>
		/// <returns></returns>
		public JsonResult GetComPage()
		{

			var aid = Convert.ToInt32(Session["aid"]);
			var ls = d1.V_CMS_Comment.Where(c => c.aid == aid).ToList();
			return new JsonResult()
			{
				Data = ls.ToList(),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = 10240000
			};
		}

		public ActionResult KeyWordList()
		{
			return View();
		}

		/// <summary>
		/// 关键字分页
		/// </summary>
		/// <param name="show"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public ActionResult GetKeyWord(bool? show, string keyword,int page,int rows)
		{

			var ls = d1.CMS_Keyword.OrderByDescending(c => c.stimes) as IEnumerable<CMS_Keyword>;
			if (show!=null)
			{
				ls = ls.Where(c => c.show == show);
			}
			if (keyword!=null&&keyword!="")
			{
				ls = ls.Where(c => c.keyword.Contains(keyword));
			}
			int total = ls.Count();
			ls = ls.Skip((page - 1) * rows)
				.Take(rows)
				.ToList();
			Dictionary<string, object> dc = new Dictionary<string, object>();
			dc.Add("total", total);
			dc.Add("rows", ls);
			return Json(dc);
		}


		/// <summary>
		/// 修改关键字状态
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		public int UpdKeyInfo(CMS_Keyword k)
		{
			var ls=d1.CMS_Keyword.Find(k.kid);
			ls.show = k.show;
			return d1.SaveChanges();
		}


		/// <summary>
		/// 根据id删除关键字
		/// </summary>
		/// <param name="kid"></param>
		/// <returns></returns>
		public JsonResult delKey(int kid)
		{
			var ls = d1.CMS_Keyword.Find(kid);
			d1.CMS_Keyword.Remove(ls);
			if (d1.SaveChanges()>0)
			{
				return Json(new
				{
					state = true,
					msg="删除成功",
				}) ;
			}
			else
			{
				return Json(new
				{
					state = false,
					msg = "删除失败，请稍后重试",
				});
			}
		}
		#endregion

		/// <summary>
		/// 释放
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			d1.Dispose();
			db.Dispose();
		}
	}
}