using Commons.Helper;
using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Commons.Helper.WebHelper;
using Newtonsoft.Json;
using System.Configuration;
using System.Text.RegularExpressions;
using zzfIBM.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class ClassController : Controller
    {
        //
        // GET: /Manage/Class/
        #region 活动维护
        public ActionResult Index()
        {
            ClassServiceClient client = new ClassServiceClient();
            ViewData["ShoudCloseActivityCount"] = client.GetShoudCloseActivityCount();
            return View();
        }
        [HttpPost]
        public JsonResult Index(string text, int page, int size)
        {
            ClassServiceClient client = new ClassServiceClient();
            //ViewData["ShoudCloseActivityCount"] = client.GetShoudCloseActivityCount();
            return Json(client.Classes(page, size), JsonRequestBehavior.AllowGet);
        }

        //设置状态
        [HttpPost]
        public JsonResult SetStatus(int classId, int status, string text)
        {
            ClassServiceClient client = new ClassServiceClient();
            if (client.SetClassStatus(classId, status, text) > 0)
                return Json(new Result(0, ((ClassStatus)status).ToString()));
            else
                return Json(new Result(1, "设置失败"));
        }

        //批量设置状态
        [HttpPost]
        public JsonResult SetClassMany()
        {
            ClassServiceClient client = new ClassServiceClient();
            int result = client.SetClassMany();
            return Json(new { code = result });
        }

        // 添加活动报道
        public ActionResult AddReport(int classId, int? id)
        {
            ViewClassReport report = new ViewClassReport();
            if (id != null)
            {
                ClassReportServiceClient client = new ClassReportServiceClient();
                report = client.Get((int)id);
            }
            else
            {
                report.Id = 0;
                report.ClassId = 0;
            }
            return View(report);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult AddReport()
        {
            JavaScriptSerializer seria = new JavaScriptSerializer();
            ViewClassReport report = seria.Deserialize<ViewClassReport>(Request.Form["data"]);
            ClassReportServiceClient client = new ClassReportServiceClient();
            report.UserId = 0;
            client.Add(report);
            //qiqi 2015-11-23
            //start 把Content中图片添加到相册YogaPicture，类型：6
            YogaPictureServiceClient picclient = new YogaPictureServiceClient();
            Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var m = rg.Match(report.Content);
            while (m.Success)
            {
                ViewYogaPicture picModel = new ViewYogaPicture();
                picModel.PictureOriginal = m.Groups[1].Value;//这里就是图片路径     
                picModel.PictureType = 6;
                picModel.CreateTime = DateTime.Now;
                picModel.PictureName = "活动相册";
                picModel.Uid = 100316;//管理员
                picModel.CreateUser = 100316;
                picModel.PictureContent = "活动相册";
                picModel.HitNum = 0;
                picModel.iAudio = 1;

                picModel.PictureSmall = "";
                picModel.AlbumId = 0;
                picModel.EvaluateId = 0;
                picModel.Comid = 0;
                picModel.PictureLarge = "";
                picModel.PictureMiddle = "";
                picModel.PircureSize = "";
                picModel.CommentCount = 0;
                picModel.LikeCount = 0;
                picModel.NotLikeCount = 0;
                picModel.CommentLimite = 0;
                picModel.LastChangeTime = DateTime.Now;

                picclient.Add(picModel);
                m = m.NextMatch();
            }

            //end

            return Json(report);
        }

        public ActionResult ViewClass(int id)
        {
            ClassServiceClient client = new ClassServiceClient();
            ViewClass @class = client.Get(id);
            return View(@class);
        }

        public ActionResult AddActivity(int? id)
        {
            if (id == null)
                ViewBag.Title = "添加活动";
            else
                ViewBag.Title = "编辑活动";
            ViewBag.Id = id;
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            ViewData["YogaTopic"] = (from topic in (dicClient.Dics(d => d.DicId == CommonInfo.Topic))
                                     select new SelectListItem()
                                     {
                                         Text = topic.ItemName,
                                         Value = topic.ID.ToString()
                                     }).ToList();
            ViewBag.HtmlClassTeacher = TempData["HtmlClassTeacher"] == null ? "''" : TempData["HtmlClassTeacher"];
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult AddActivity(ViewClass entity)
        {
            ClassServiceClient client = new ClassServiceClient();
            entity.Discount = 0;
            entity.IsItem = false;
            entity.ItemClassID = 0;
            entity.ClassType = (int)ClassType.activity;
            entity.ClassStatus = (int)ClassStatus.报名中;
            entity.UserId = int.Parse(ConfigurationManager.AppSettings["BACK_POSTER"]);
            entity.UpdateTime = DateTime.Now;
            entity.IsDeleted = false;
            if (entity.DurationUnit == 1)
                entity.EndTime = Convert.ToDateTime(entity.Start).AddHours(entity.Duration);
            else
                entity.EndTime = Convert.ToDateTime(entity.Start).AddDays(entity.Duration);
            entity.Id = client.Add(entity);
            if (!string.IsNullOrEmpty(entity.TeacherIds) && entity.Id > 0)
            {
                ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
                subClient.AddTeachers(entity.Id, entity.TeacherIds);
            }
            return Json(entity);
        }
        #endregion

        /// <summary>
        /// 修改活动
        /// </summary>
        /// <param name="id">活动ID</param>
        /// <returns></returns>
        public ActionResult EditActivity(int id)
        {
            ViewClass model = new ViewClass();
            ClassServiceClient client = new ClassServiceClient();
            model = client.Get(id);

            ViewBag.Title = "编辑活动";
            ViewBag.Id = id;
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            ViewData["YogaTopic"] = (from topic in (dicClient.Dics(d => d.DicId == CommonInfo.Topic))
                                     select new SelectListItem()
                                     {
                                         Text = topic.ItemName,
                                         Value = topic.ID.ToString(),
                                         Selected = model.TopicIds.Split(',').Contains(topic.ID.ToString())
                                     }).ToList();
            //逐层获取地理位置区域
            List<DistrictModel> DistrictModelList = client.GetDistrictModel(model.AreaID);
            ViewBag.DistrictModelList = Newtonsoft.Json.JsonConvert.SerializeObject(DistrictModelList);

            //获取活动关联的老师并拼成HTML展示在页面
            ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
            List<ViewClassTeacher> ClassTeacherModel = subClient.GetClass_Id(id);
            string html = "";
            if (ClassTeacherModel != null)
            {
                foreach (var item in ClassTeacherModel)
                {
                    html += "<li class=teacher id=teacher_" + item.Id + ""
                    + " teacherid=" + item.TeacherId + "><div class=teacher-info><div class=name><img src=" + item.Avatar + "> "
                    + item.Name + "<span>" + item.Gender + "</span> <span>"
                    + item.Country + "</span> <span>" + item.YogaSystem + "</span></div></div><div class=teacher-close>&nbsp;</div></li>";

                }
            }
            ViewData["html"] = new MvcHtmlString(html);
            string centerHtml = "";
            CentersServiceClient centerClient = new CentersServiceClient();
            if (!string.IsNullOrWhiteSpace(model.CenterID))
            {
                var centerList = centerClient.GetCentersListByClassCenterID(model.CenterID);
                foreach (var dataItem in centerList)
                {
                    centerHtml += "<li class=Center id=CenterID_" + dataItem.CenterId + " CenterID=" + dataItem.CenterId
                        + "><div class=Center-info><div class=name><img src=" + dataItem.CenterPortraint + "/> " + dataItem.CenterName
                        + " </div></div><div class=Center-close>&nbsp;</div></li>";
                }
            }
            ViewData["centerHtml"] = new MvcHtmlString(centerHtml);
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EditActivity(ViewClass entity)
        {
            ClassServiceClient client = new ClassServiceClient();
            ViewClass model = client.Get(entity.Id);
            model.Address = entity.Address;
            model.AreaID = entity.AreaID;
            model.Banner = entity.Banner;
            model.ClassType = entity.ClassType;
            model.Content = entity.Content;
            model.Creater = entity.Creater;
            model.CreateTime = DateTime.Now;
            model.Discount = entity.Discount;
            model.Duration = entity.Duration;
            model.DurationUnit = entity.DurationUnit;
            model.InterestCount = entity.InterestCount;
            model.Max = entity.Max;
            model.Name = entity.Name;
            model.Price = entity.Price;
            model.Start = entity.Start;
            model.Summary = entity.Summary;
            model.Tags = entity.Tags;
            model.TopicIds = entity.TopicIds;
            model.CloseTime = entity.CloseTime;
            model.CenterID = entity.CenterID;
            if (entity.DurationUnit == 1)
                entity.EndTime = Convert.ToDateTime(entity.Start).AddHours(entity.Duration);
            else
                entity.EndTime = Convert.ToDateTime(entity.Start).AddDays(entity.Duration);
            ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
            List<ViewClassTeacher> list = subClient.GetClass_Id(entity.Id);
            if (!string.IsNullOrEmpty(entity.TeacherIds) && entity.Id > 0)
            {
                subClient.EditTeachers(entity.Id, entity.TeacherIds);
            }
            client.Edit(model);
            return Json(entity);
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id, int UserId)
        {
            try
            {
                // TODO: Add delete logic here
                //活动导师 
                //ClassTeacherServiceClient teahclient = new ClassTeacherServiceClient();
                //List<ViewClassTeacher> ctlist = teahclient.GetClass_Id(id);
                //if (ctlist.Count() > 0)
                //{
                //    foreach (var i in ctlist)
                //    {
                //        teahclient.Delete(i.Id.ToString());
                //    }
                //}
                ////兴趣
                //InterestServiceClient insertClient = new InterestServiceClient();
                //List<ViewInterestedClass> interlist = insertClient.GetListClassId(id);

                //if (interlist.Count() > 0)
                //{
                //    foreach (var i in interlist)
                //    {
                //        i.Class = null;
                //        insertClient.Delete(i.Id.ToString());
                //    }

                //}
                //活动报道
                ClassReportServiceClient clentRep = new ClassReportServiceClient();
                if (clentRep.GetClassId(id).Count() == 0)
                {
                    //活动
                    ClassServiceClient client = new ClassServiceClient();
                    ViewClass model = client.Get(id);
                    model.IsDeleted = true;
                    client.Edit(model);
                    return Json(new { code = 0 });
                }
                return Json(new { code = 2 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Order(int page = 1)
        {
            string whereStr = "";

            string Phone = "";
            if (!string.IsNullOrEmpty(Request.Form["Phone"]))
            {
                Phone = Request.Form["Phone"].ToString().Trim();
                whereStr += "Phone!" + Phone + ",";
            }

            string CreateTime = "";
            if (!string.IsNullOrEmpty(Request.Form["CreateTime"]))
            {
                CreateTime = Request.Form["CreateTime"].ToString();
                whereStr += "CreateTime!" + CreateTime + ",";
            }
            string EndTime = "";
            if (!string.IsNullOrEmpty(Request.Form["EndTime"]))
            {
                EndTime = Request.Form["EndTime"].ToString();
                whereStr += "EndTime!" + EndTime + ",";
            }

            OrderServiceClient client = new OrderServiceClient();
            int pagesize = 12;
            int count = 0;
            var list = client.GetOrder(whereStr, page, pagesize, out count);

            Webdiyer.WebControls.Mvc.PagedList<ViewOrder> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewOrder>(list, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("OrderList", pagelist);
            }
            return View(pagelist);
        }

    }
}
