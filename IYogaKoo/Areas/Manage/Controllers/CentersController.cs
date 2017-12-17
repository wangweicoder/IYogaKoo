using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using Webdiyer.WebControls.Mvc;
namespace IYogaKoo.Areas.Manage.Controllers
{ 
    public class CentersController : BaseController
    {
        //会馆分类（1会馆、2学院、3工作室）

        //
        // GET: /Manage/Centers/
        CentersServiceClient client;
        YogaUserServiceClient clientUser;
        public CentersController()
        {
            client = new CentersServiceClient();
            clientUser = new YogaUserServiceClient();
        }

        /// <summary>
        /// 机构
        /// </summary>
        /// <param name="page"></param>
        /// <param name="centertype"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string centertype = "0")
        {
            ViewBag.centertype = centertype;
            List<ViewCenters> list = new List<ViewCenters>();
            int count = 0;

            list = client.GetCentersPageList(page, 15, centertype, out count);

            PagedList<ViewCenters> pagelist = new PagedList<ViewCenters>(list, page, 15, count);
            return View(pagelist);

        }

        #region 点评
        /// <summary>
        /// 点评
        /// </summary>
        /// <param name="page"></param>
        /// <param name="centertype"></param>
        /// <returns></returns>
        public ActionResult IndexEval(int page = 1, string centertype = "0")
        {
            ViewBag.centertype = centertype;
            List<ViewEvaluatesGroup> listGroupMsg = new List<ViewEvaluatesGroup>();
            List<ViewEvaluates> list = new List<ViewEvaluates>();
            int count = 0;
            using (EvaluatesServiceClient evalclient = new EvaluatesServiceClient())
            {
                list = evalclient.GetEvaluatesPageList(page, 15, out count);
            }
            foreach (var item in list)
            {
                ViewEvaluatesGroup model = new ViewEvaluatesGroup();
                model.entity = item;
                ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                if (usermodel != null)
                    model.FromUser = usermodel.NickName;

                ViewCenters center= client.GetById(item.ToUid.Value);
                if (center != null)
                {
                    model.CetnerName = center.CenterName;
                }
                listGroupMsg.Add(model);
            }

            PagedList<ViewEvaluatesGroup> pagelist = new PagedList<ViewEvaluatesGroup>(listGroupMsg, page, 15, count);
            return View(pagelist);
        } 

        public ActionResult DeleteEval(int id)
        {
            using (EvaluatesServiceClient c = new EvaluatesServiceClient())
            {
                c.Delete(id.ToString());
                return RedirectToAction("IndexEval");
            }
        }

        public ActionResult DetailEval(int id)
        {
            ViewEvaluatesGroup group = new ViewEvaluatesGroup();
            ViewEvaluates eval = new ViewEvaluates();
            using (EvaluatesServiceClient c = new EvaluatesServiceClient())
            {
                eval = c.GetEvaluatesById(id);
                group.entity = eval;
            }
            if (eval != null)
            {
                ViewYogaUser user = clientUser.GetById(eval.Evaluateid);
                if (user != null)
                {
                    group.FromUser = user.NickName;
                }
                ViewCenters center = client.GetById(eval.ToUid.Value);
                if (center != null)
                {
                    group.CetnerName = center.CenterName;
                }
            }
            return View(group);
        }
        public ActionResult UpdateEval(int id)
        {
            ViewEvaluates eval = new ViewEvaluates();

            int isshow = 0;
            using (EvaluatesServiceClient c = new EvaluatesServiceClient())
            {
                eval = c.GetEvaluatesById(id);
                if (isshow == eval.iShow)
                {
                    isshow = 1;
                }
                eval.iShow = isshow;
                c.Update(eval);
            }
            return RedirectToAction("IndexEval");
        }
       
        public ActionResult UpdateEvalComm2(int id)
        {
            ViewEvaluates eval = new ViewEvaluates();
            int isshow = 0;
            using (EvaluatesServiceClient c = new EvaluatesServiceClient())
            {
                eval = c.GetEvaluatesById(id);
                if (isshow == eval.iShow)
                {
                    isshow = 1;
                }
                eval.iShow = isshow;
                c.Update(eval);
            }
            return RedirectToAction("DetailEval", new { id = id });
        }
       
        #endregion




        /// <summary>
        /// 相册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexPic(int id)
        {
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                List<ViewYogaPicture> pic = clientpic.GetBackUidList(id);
                if (pic != null)
                {
                    ViewBag.Pic = pic;
                }
            }
            ViewBag.centerid = id;
            return View();
        }


        [HttpPost]
        public ActionResult IndexPic()
        {
            // TODO: Add insert logic here
            if (!string.IsNullOrEmpty(Request.Form["Diploma"]))
            {
                //相册
                string[] strPic = Request.Form["Diploma"].ToString().Split(';');
                string[] strpicContent = Request.Form["PictureContent"].ToString().TrimEnd('|').Split('|');

                ViewYogaPicture picModel = new ViewYogaPicture();
                using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                {
                    for (int i = 0; i < strPic.Length - 1; i++)
                    {
                        #region
                        if (!string.IsNullOrEmpty(strPic[i]))
                        {
                            picModel.PictureOriginal = strPic[i];

                            picModel.Uid = Convert.ToInt32(Request.Form["centerid"]);

                            picModel.PictureType = 5;//机构
                            try
                            {
                                picModel.PictureContent = strpicContent[i];
                            }
                            catch
                            {
                                picModel.PictureContent = "";
                            }
                            picModel.CreateTime = DateTime.Now;
                            picModel.CreateUser = 0;//登录用户ID
                            picModel.PictureName = "";
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
                            picModel.HitNum = 0;


                            List<ViewYogaPicture> list = clientpic.GetBackUidList(Convert.ToInt32(Request.Form["centerid"]));
                            if (list.Count() == strPic.Length - 1 && list.Count() != 0)
                            {
                                //edit
                                picModel.Pid = list[i].Pid;
                                clientpic.Update(picModel);
                            }
                            else if (list.Count() == 0)
                            {
                                clientpic.Add(picModel);
                            }
                            else
                            {
                                #region del add
                                if (i == 0)
                                {
                                    for (int k = 0; k < list.Count(); k++)
                                    {
                                        clientpic.Delete(list[k].Pid.ToString());
                                    }
                                }
                                clientpic.Add(picModel);

                                #endregion
                            }

                        }
                    }
                        #endregion
                }
            }
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                List<ViewYogaPicture> pic = clientpic.GetBackUidList(Convert.ToInt32(Request.Form["centerid"]));
                if (pic != null)
                {
                    ViewBag.Pic = pic;
                }
            }
            ViewBag.centerid = Request.Form["centerid"];
            return View();
        }


        //
        // GET: /Manage/Centers/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manage/Centers/Create

        public ActionResult Create(string centertype)
        {
            try
            {
                ViewBag.centertype = centertype;
            }
            catch (Exception e)
            {
                Tools.WriteTextLog("添加会馆", e.Message);
            }
            return View();
        }

        //
        // POST: /Manage/Centers/Create

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ViewCenters Model)
        {
            try
            {
                using (CentersServiceClient client = new CentersServiceClient())
                {
                    Model.CenterName = Request.Form["CenterName"].ToString();
                    Model.CenterAddress = Request.Form["CenterAddress"].ToString();
                    Model.UpgradeDate = Model.CreateDate = DateTime.Now;

                    Model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"]);
                    Model.CityID = Convert.ToInt32(Request.Form["ddlCityID"]);
                    Model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"]);
                    Model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"]);
                    Model.CenterType = Request.Form["CenterType"];
                    Model.CenterBanner = Request.Form["CenterBanner"];
                    Model.CenterIntrodition = Request.Form["CenterIntrodition"];
                    Model.CenterPortraint = Request.Form["CenterPortraint"];
                    string temptypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                    string[] arrtypeid = temptypeid.Split(',');
                    string newtypeid = string.Empty;
                    for (int i = 0; i < arrtypeid.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(arrtypeid[i]))
                        {
                            arrtypeid[i] = "|" + arrtypeid[i] + "|";
                            newtypeid += arrtypeid[i] + ",";
                        }
                    }
                    Model.YogaTypeid = newtypeid;
                    string opentime = Request.Form["OpenTime"];
                    string closetime = Request.Form["CloseTime"];
                    Model.OpenTime = DateTime.Now.ToString("yyyy-MM-dd ") + opentime;
                    Model.CloseTime = DateTime.Now.ToString("yyyy-MM-dd ") + closetime;
                    client.Add(Model);
                }
                return RedirectToAction("Index", new { centertype = Request.Form["CenterType"] });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // GET: /Manage/Centers/Edit/5

        public ActionResult Edit(int id)
        {
            ViewCenters center = new ViewCenters();
            center = client.GetCentersById(id);

            #region 流派
            if (!string.IsNullOrEmpty(center.YogaTypeid))
            {
                string[] YogaTypeidlist = center.YogaTypeid.Replace("|","").Split(',');

                List<ViewYogaDicItem> listcenter2 = new List<ViewYogaDicItem>();
                using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                {
                    listcenter2 = YogaDicItemServiceClient.GetYogaDicItemList();
                    string strYogaTypeidValue = "";
                    foreach (var j in YogaTypeidlist)
                    {
                        if (!string.IsNullOrEmpty(j))
                        {
                            foreach (var itemDic in listcenter2)
                            {
                                if (j.ToString() == itemDic.ID.ToString())
                                {
                                    strYogaTypeidValue += itemDic.ItemName + ',';
                                }
                            }
                        }
                    }
                    ViewBag.YogaTypeidValue = strYogaTypeidValue;
                }
            }

            #endregion

            return View(center);
        }


        public ActionResult Detail(int id)
        {
            ViewCenters center = new ViewCenters();
            center = client.GetCentersById(id);
            string country = string.Empty;
            string province = string.Empty;
            string city = string.Empty;
            string dis = string.Empty;
            using (YogaDicItemServiceClient itemclient = new YogaDicItemServiceClient())
            {
                if (center.CountryID != 0)
                {
                    country = itemclient.GetYogaDicItemById(Convert.ToInt32(center.CountryID)).ItemName;
                }
                if (center.ProvinceID != 0)
                {
                    province = itemclient.GetYogaDicItemById(Convert.ToInt32(center.ProvinceID)).ItemName;
                }
                if (center.CityID != 0)
                {
                    city = "--" + itemclient.GetYogaDicItemById(Convert.ToInt32(center.CityID)).ItemName;
                }
                if (center.DistrictID != 0)
                {
                    dis = "--" + itemclient.GetYogaDicItemById(Convert.ToInt32(center.DistrictID)).ItemName;
                }
                ViewBag.country = country;
                ViewBag.addr = province + city + dis;

            }

            return View(center);
        }
        //
        // POST: /Manage/Centers/Edit/5

        [HttpPost, ValidateInput(false)]
        public JsonResult Edit(ViewCenters modeltemp)
        {
            try
            {
                int centerid = Convert.ToInt32(Request.Form["CenterId"]);
                ViewCenters model = client.GetById(centerid);
                model.CenterName = Request.Form["CenterName"].ToString();
                model.CenterAddress = Request.Form["CenterAddress"].ToString();
                model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"]);
                model.CityID = Convert.ToInt32(Request.Form["ddlCityID"]);
                model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"]);
                model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"]);
                model.CenterType = Request.Form["CenterType"];
                if (Request.Form["CenterBanner"] != "")
                {
                    model.CenterBanner = Request.Form["CenterBanner"];
                }
                if (Request.Form["CenterPortraint"] != "")
                {
                    model.CenterPortraint = Request.Form["CenterPortraint"];
                }
                if (Request.Form["hYogaTypeid"] != "")
                {
                    string temptypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                    string[] arrtypeid = temptypeid.Split(',');
                    string newtypeid = string.Empty;
                    for (int i = 0; i < arrtypeid.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(arrtypeid[i]))
                        {
                            arrtypeid[i] = "|" + arrtypeid[i] + "|";
                            newtypeid += arrtypeid[i] + ",";
                        }
                    }
                    model.YogaTypeid = newtypeid;
                }
               
                model.CenterIntrodition = Request.Form["CenterIntrodition"];
                model.UpgradeDate = DateTime.Now;
                model.CenterProfile = Request.Form["CenterProfile"].ToString();
                string opentime = DateTime.Now.ToString("yyyy-MM-dd ") + Request.Form["OpenTime"];
                string closetime = DateTime.Now.ToString("yyyy-MM-dd ") + Request.Form["CloseTime"];
                model.OpenTime = opentime;// Convert.ToDateTime(opentime);
                model.CloseTime = closetime;// Convert.ToDateTime(closetime); 
                client.Update(model);

                return Json(new { code = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { code = ex.ToString() });
            }
        }

        //
        // GET: /Manage/Centers/Delete/5

        public ActionResult Delete(int CenterId, int type)
        {
            using (CentersServiceClient c = new CentersServiceClient())
            {
                c.Delete(CenterId.ToString());
                return RedirectToAction("Index", new { centertype = type });
            }
        }

        public string DeletePicbyid(string id)
        {
            ViewYogaPicture model = new ViewYogaPicture();
            int delid = 0;
            using (YogaPictureServiceClient clipic = new YogaPictureServiceClient())
            {
                delid = clipic.Delete(id);
            }
            return delid.ToString();
        }
 

        //
        // POST: /Manage/Centers/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
