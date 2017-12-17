using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    /// <summary>
    /// 后台管理--关键字搜索
    /// </summary>
    public class tKeyWordController : Controller
    {
        //
        // GET: /Manage/tKeyWord/
        tKeyWordServiceClient client;
        YogaUserServiceClient userclient;
        YogisModelsServiceClient modelsclient;
        public tKeyWordController()
        {
            client = new tKeyWordServiceClient();
            userclient = new YogaUserServiceClient();
            modelsclient = new YogisModelsServiceClient();
        }
        public ActionResult IndexSearch(int page = 1)
        {
            #region
             
            int iType = 3;//0 游客，1 习练者，2 导师 
            int count = 0;
            int pagesize = 10;
            string strwhere = "";
            string keyWord = string.Empty;
            string NextkeyWord = string.Empty;
            DateTime? FromTime = null;
            DateTime? ToTime = null;

            if (!string.IsNullOrEmpty(Request.QueryString["keyWord"]))
            {
                keyWord = Request.QueryString["keyWord"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FromTime"]))
            {
                FromTime = Convert.ToDateTime(Request.QueryString["FromTime"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ToTime"]))
            {
                ToTime = Convert.ToDateTime(Request.QueryString["ToTime"].ToString());
            }
            if (keyWord == "2646")
            {
                //全部 
                if (FromTime != null)
                {
                    strwhere += " and CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and CreateTime<='" + ToTime + "'";
                }
            }
            else if (keyWord == "2647")
            {
                //游客
                iType = 0;
                strwhere = " and uid=0 ";
                if (FromTime != null)
                {
                    strwhere += " and CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and CreateTime<='" + ToTime + "'";
                }

            }
            else if (keyWord == "2648")
            {
                iType = 1;

                //习练者
                if (!string.IsNullOrEmpty(Request.QueryString["NextkeyWord"]))
                {
                    NextkeyWord = Request.QueryString["NextkeyWord"].ToString();
                    strwhere += " and c.[Ulevel]=" + NextkeyWord;
                }
                if (FromTime != null)
                {
                    strwhere += " and a.CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and a.CreateTime<='" + ToTime + "'";
                }
            }
            else if (keyWord == "2649")
            {
                iType = 2;
                //导师
                if (!string.IsNullOrEmpty(Request.QueryString["NextkeyWord"]))
                {
                    NextkeyWord = Request.QueryString["NextkeyWord"].ToString();
                    strwhere += " and c.[YogisLevel]=" + NextkeyWord;
                }
                if (FromTime != null)
                {
                    strwhere += " and a.CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and a.CreateTime<='" + ToTime + "'";
                }
            }
            #endregion
            DataTable dt = client.GetPageListdt(iType, strwhere, page, pagesize, out count);

            List<tKeyWordGroup> list = DataTableHelper.TableToEntity<tKeyWordGroup>(dt);
            PagedList<tKeyWordGroup> pagelist = new PagedList<tKeyWordGroup>(list, page, pagesize, count);

            return View(pagelist);
        }
        public PartialViewResult Index(int page = 1)
        {
            #region

            int iType = 3;//0 游客，1 习练者，2 导师 
            int count = 0;
            int pagesize = 10;
            string strwhere = "";
            string keyWord = string.Empty;
            string NextkeyWord = string.Empty;
            DateTime? FromTime = null;
            DateTime? ToTime = null;

            if (!string.IsNullOrEmpty(Request.QueryString["keyWord"]))
            {
                keyWord = Request.QueryString["keyWord"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FromTime"]))
            {
                FromTime = Convert.ToDateTime(Request.QueryString["FromTime"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ToTime"]))
            {
                ToTime = Convert.ToDateTime(Request.QueryString["ToTime"].ToString());
            }
            if (keyWord == "2646")
            {
                //全部 
                if (FromTime != null)
                {
                    strwhere += " and CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and CreateTime<='" + ToTime + "'";
                }
            }
            else if (keyWord == "2647")
            {
                //游客
                iType = 0;
                strwhere = " and uid=0 ";
                if (FromTime != null)
                {
                    strwhere += " and CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and CreateTime<='" + ToTime + "'";
                }

            }
            else if (keyWord == "2648")
            {
                iType = 1;

                //习练者
                if (!string.IsNullOrEmpty(Request.QueryString["NextkeyWord"]))
                {
                    NextkeyWord = Request.QueryString["NextkeyWord"].ToString();
                    strwhere += " and c.[Ulevel]=" + NextkeyWord;
                }
                if (FromTime != null)
                {
                    strwhere += " and a.CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and a.CreateTime<='" + ToTime + "'";
                }
            }
            else if (keyWord == "2649")
            {
                iType = 2;
                //导师
                if (!string.IsNullOrEmpty(Request.QueryString["NextkeyWord"]))
                {
                    NextkeyWord = Request.QueryString["NextkeyWord"].ToString();
                    strwhere += " and c.[YogisLevel]=" + NextkeyWord;
                }
                if (FromTime != null)
                {
                    strwhere += " and a.CreateTime>='" + FromTime + "'";
                }
                if (ToTime != null)
                {
                    strwhere += " and a.CreateTime<='" + ToTime + "'";
                }
            }
            #endregion
            DataTable dt = client.GetPageListdt(iType, strwhere, page, pagesize, out count);

            List<tKeyWordGroup> list = DataTableHelper.TableToEntity<tKeyWordGroup>(dt);
            PagedList<tKeyWordGroup> pagelist = new PagedList<tKeyWordGroup>(list, page, pagesize, count);

            return PartialView("Index", pagelist);
        }

        /// <summary>
        /// 首次加载关键字列表（根据父节点取子节点）：2645
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetKeyWord(int id)
        {
            List<ViewYogaDicItem> dic = new List<ViewYogaDicItem>();
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                dic = YogaDicItemServiceClient.GetDicId(id).OrderBy(a=>a.SortId).ToList();
            }

            return Json(dic);
        }
        //
        // GET: /Manage/tKeyWord/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manage/tKeyWord/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/tKeyWord/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tKeyWord/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manage/tKeyWord/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tKeyWord/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manage/tKeyWord/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
