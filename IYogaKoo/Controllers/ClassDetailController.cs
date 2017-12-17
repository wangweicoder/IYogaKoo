using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Controllers
{
    public class ClassDetailController : Controller
    {
        //
        // GET: /ClassDetail/

        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult AddClassDetail(FormCollection collection)
        {
            ViewClassDetail entity = new ViewClassDetail();
            ClassDetailServiceClient client = new ClassDetailServiceClient();
            try
            {
                entity.Address = collection["classDetailAddress"];
                entity.CenterID = int.Parse(collection["classDetailCenterID"]);
                entity.Duration = int.Parse(collection["classDetailDuration"]);
                entity.EndTime = DateTime.Parse(collection["classDetailStartTime"]).AddMinutes(int.Parse(collection["classDetailDuration"]));
                entity.Level = int.Parse(collection["classDetailLevel"]);
                entity.Name = collection["classDetailName"];
                entity.Price = decimal.Parse(collection["classDetailPrice"]);
                entity.StartTime = DateTime.Parse(collection["classDetailStartTime"]);
                entity.UserID = user.Uid;
                entity.YID = new YogisModelsServiceClient().GetYogisModelsById(user.Uid).YID;
                client.Add(entity);
                return Json(new { code = 0 });
            }
            catch (Exception e)
            {
                return Json(new { code = 1 });
            }
        }
    }
}
