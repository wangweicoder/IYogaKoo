using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using zzfIBM.WebControls.Mvc;
using Commons.Helper;

namespace IYogaKoo.Controllers
{
    public class EvaluatesController : Controller
    {
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        method method;
        public EvaluatesController()
        { 
            ViewBag.user = user;
            method = new Commons.Helper.method();
            #region  站内信-信息数量

            int tinstatcount = 0;
            int follcount = 0;
            int zancount = 0;
            int msgcount = 0;

            method.InstationInfo(user.Uid, out   tinstatcount, out   follcount, out   zancount, out   msgcount);

            ViewBag.tinstatcount = tinstatcount;
            ViewBag.follcount = follcount;
            ViewBag.zancount = zancount;
            ViewBag.msgcount = msgcount;
            ViewBag.AllCount = tinstatcount + follcount + zancount + msgcount;
            #endregion
        }
        //
        // GET: /Evaluates/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Evaluates/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
 
     
        //
        // POST: /Evaluates/Delete/5

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
       
        
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        public JsonResult AddEvalInfo()
        {
            try
            {
                // TODO: Add delete logic here

                ViewEvaluates model = new ViewEvaluates();
                string strContent = Request.Form["sContent"].ToString();
                int ToUid = Convert.ToInt32(Request.Form["hidid"]);
                int recomm = Convert.ToInt32(Request.Form["recomm"]);
                int FromUid = user.Uid;

                using (EvaluatesServiceClient client = new EvaluatesServiceClient())
                {
                    model = client.GettEval(ToUid, strContent, FromUid);
                    if (model == null)
                    {
                        model = new ViewEvaluates();
                        model.EContent = FilterSpecial(strContent);
                        model.ToUid = ToUid;
                        model.FromUid = FromUid;
                        model.CreateDate = DateTime.Now;
                        model.ParentID = 0;
                        model.iZan = 0;
                        model.iShow = 0;
                        model.Recommend = recomm;
                        //相册
                        model.Pic = Request.Form["Diploma"];

                        int a= client.Add(model);
                        return Json(new { code = 0 });
                    }
                    else
                    {
                        return Json(new { code = 2 });//重复
                    }
                }
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        
        /// <summary>
        /// 过滤评价
        /// </summary>
        /// <param name="str">过滤字符串</param>
        /// <returns></returns>

        private string FilterSpecial(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                //str = str.Replace(" ", "");
                return str;

            }
        }
       
        /// <summary>
        /// 赞
        /// </summary>
        /// <returns></returns>
        public JsonResult iZan()
        {
            try
            {
                // TODO: Add update logic here
                int Eid = Convert.ToInt32(Request.Form["id"]);
                using (EvaluatesServiceClient client = new EvaluatesServiceClient())
                {
                    ViewEvaluates model = client.GetById(Eid);
                    if (model.iZan == null)
                    {
                        model.iZan = 1;
                    }
                    else
                    {
                        model.iZan = model.iZan + 1;
                    }
                    client.Update(model);
                }

                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 星级打分
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveStare()
        {
            try
            {
                using (CenterStareServiceClient client = new CenterStareServiceClient())
                {
                    int uid = user.Uid;
                    int mid = Convert.ToInt32(Request.Form["mid"]);
                    if (client.IfUidSave(uid, mid) > 0)
                    {
                        return Json(new { code = 2 }); //已经推荐，不需重复推荐
                    }
                    else
                    {
                        ViewCenterStare model = new ViewCenterStare();
                        decimal price = 0;
                        if (Request.Form["price"] != null)
                        {
                            if (Request.Form["price"] != "")
                            {
                                price = Convert.ToDecimal(Request.Form["price"].ToString());
                            }
                        }
                        model.Price=price;
                        model.Centerclass = GetPar("centerclass");
                        model.Service = GetPar("service");
                        model.Env =GetPar("env");
                        model.CreateDate = DateTime.Now;
                        model.Uid = uid;
                        model.Mid = mid;
                        model.Satate = 0;//打分状态 
                        client.Add(model);
                        return Json(new { code = 0 });
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(new { code = ex.Message });
            }
        }
       
        private int GetPar(string par)
        {
            int val = 0;
            if (Request.Form[par] != null)
            {
                val = Convert.ToInt32(Request.Form[par]);
            }
            return val;
        }

        /// <summary>
        /// 回复发表
        /// </summary>
        /// <returns></returns>
        public JsonResult AddFaBiaoInfo()
        {
            try
            {
              
                // TODO: Add delete logic here
                int ToUid = 0;
                if (!string.IsNullOrEmpty(Request.Form["Uid"]))
                {
                    ToUid = Convert.ToInt32(Request.Form["Uid"]);
                }
                string sContent = "";
                if (!string.IsNullOrEmpty(Request.Form["sContent"]))
                {
                    sContent = Request.Form["sContent"].ToString();
                }
                int ParentID = 0;
                if (!string.IsNullOrEmpty(Request.Form["parentID"]))
                {
                    ParentID = Convert.ToInt32(Request.Form["parentID"]);
                }
                using (EvaluatesServiceClient client = new EvaluatesServiceClient())
                {
                    ViewEvaluates model2 = client.GettEval(ToUid, sContent, user.Uid, ParentID);

                    if (model2 != null)
                    {
                        return Json(new { code = 2 });
                    }
                    else
                    {
                        ViewEvaluates model = new ViewEvaluates();
                        model.ToUid = ToUid;
                        model.EContent = sContent;
                        model.iShow = 0;
                        model.iZan = 0;
                        model.ParentID = ParentID;
                        model.FromUid = user.Uid;
                        model.CreateDate = DateTime.Now;
                        client.Add(model);

                        return Json(new { code = 0 });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = ex.Message });
            }
        }
    }
}
