using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zzfIBM.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    
    public class CentersAddController : Controller
    {
        //会馆分类（1学院、2会馆、3工作室）

        //
        // GET: /Manage/Centers/
        CentersServiceClient client;
        YogaUserServiceClient clientUser;
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        method method;
        public CentersAddController()
        { 
            ViewBag.user = user;
            client = new CentersServiceClient();
            clientUser = new YogaUserServiceClient();
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
        // GET: /Manage/Centers/Create

        public ActionResult Create()
        { 
            return View();
        }

        //
        // POST: /Manage/Centers/Create

        [HttpPost, ValidateInput(false)]
        public JsonResult Create(ViewCenters Model)
        {
            try
            {
                using (CentersServiceClient client = new CentersServiceClient())
                {
                    Model.UpgradeDate = Model.CreateDate = DateTime.Now;
                    Model.CenterName = Request.Form["CenterName"].ToString();
                    Model.CenterAddress = "";
                    Model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"]);
                    Model.CityID = Convert.ToInt32(Request.Form["ddlCityID"]);
                    Model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"]);
                    Model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"]);
                    Model.CenterType = "1";
                    Model.CenterBanner = "";
                    Model.CenterIntrodition = "";
                    Model.CenterPortraint = "";
                    Model.Uid = user.Uid.ToString();

                    Model.OpenTime = DateTime.Now.ToString("yyyy-MM-dd ");
                    Model.CloseTime = DateTime.Now.ToString("yyyy-MM-dd ");
                    client.Add(Model);
                }
                return Json(new { code = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { code = ex.Message });
            }
        }
 


    }
}
