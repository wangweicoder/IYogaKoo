using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using zzfIBM.WebControls.Mvc;
using Commons.Helper;
using System.Text.RegularExpressions;
namespace IYogaKoo.Controllers
{
    public class MechanismController : Controller
    {
        YogaUserServiceClient clientUser;
        YogaUserDetailServiceClient clientuserdetail;
        YogisModelsServiceClient clientmodel;
        YogaDicItemServiceClient dicclient;
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        method method;
        public MechanismController()
        {
            ViewBag.user = user;
            clientUser = new YogaUserServiceClient();
            dicclient = new YogaDicItemServiceClient();
            clientmodel = new YogisModelsServiceClient();
            clientuserdetail = new YogaUserDetailServiceClient();
            method = new method();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = clientuserdetail.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = clientmodel.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                    ViewBag.level = vyogism.YogisLevel;

            }
            #endregion
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

        public ViewCenters GetAllMechanis(int id, out int mycount, int page = 1)
        {
            ViewCenters c = new ViewCenters();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                c = client.GetCentersById(id);
            }
            ViewBag.centerid = id;

            #region 评价列表
            int count = 0;
            int pagesize = 10;
            List<ViewEvaluatesGroup> listGroupMsg = new List<ViewEvaluatesGroup>();
            using (EvaluatesServiceClient clientEval = new EvaluatesServiceClient())
            {
                int tjcount = 0;
                clientEval.GetRecommendCount(id, out tjcount);
                ViewBag.Recommend = tjcount;

                List<ViewEvaluates> evalEntity = new List<ViewEvaluates>();
                evalEntity = clientEval.GettEvalUid(id, page, pagesize, out count);
                mycount = count;

                foreach (var item in evalEntity)
                {
                    ViewEvaluatesGroup model = new ViewEvaluatesGroup();
                    model.entity = item;
                    //评论人
                    ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                    if (usermodel != null)
                    {
                        model.FromUser = usermodel.NickName;
                    }
                    if (usermodel.UserType == 0)
                    {
                        ViewYogaUserDetail newmodel = clientuserdetail.GetYogaUserDetailById(item.FromUid.Value);
                        if (newmodel != null)
                        {
                            model.DisplayImg = CommonInfo.GetDisplayImg(newmodel.DisplayImg);
                            model.Url = "/YogaUserDetail/Details/" + item.FromUid.Value;
                        }
                    }
                    else
                    {
                        ViewYogisModels newmodel = clientmodel.GetYogisModelsById(item.FromUid.Value);
                        if (newmodel != null)
                        {
                            model.DisplayImg = CommonInfo.GetDisplayImg(newmodel.DisplayImg);
                            model.FromUser = newmodel.RealName;
                            if (newmodel.YogisLevel == 4)
                            {
                                model.Url = "/Yogaguru/Details/" + item.FromUid.Value;
                            }
                            else
                            {
                                model.Url = "/YogisModels/Details/" + item.FromUid.Value;
                            }
                        }
                    }

                    //回复
                    List<ViewEvaluates> listM = clientEval.GetEvalParentID(item.Evaluateid);
                    List<ViewEvaluatesGroup> entitylist = new List<ViewEvaluatesGroup>();
                    foreach (var it in listM)
                    {
                        ViewEvaluatesGroup entityMsg = new ViewEvaluatesGroup();
                        entityMsg.entity = it;

                        //评论人
                        ViewYogaUser usermodel2 = clientUser.GetYogaUserById(it.FromUid.Value);
                        if (usermodel2 != null)
                            entityMsg.FromUser = usermodel2.NickName;
                        entitylist.Add(entityMsg);
                    }
                    model.msgList = entitylist;
                    listGroupMsg.Add(model);
                }
                ViewBag.EvalInfo = listGroupMsg;

            }
            #endregion
            return c;
        }


        //
        // GET: /Mechanism/

        public ActionResult Index(int page = 1, string id = "0")
        {
            //派别
            int typeid = 0;
            if (!string.IsNullOrEmpty(Request.Form["typeid"]))
            {
                typeid = Convert.ToInt32(Request.Form["typeid"]);
            }
            //瑜伽类别
            if (!string.IsNullOrEmpty(Request.Form["centerclassid"]))
            {
                id = Request.Form["centerclassid"];
            }

            //国家
            int CountryID = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidCountryID"]))
            {
                CountryID = Convert.ToInt32(Request.Form["hidCountryID"]);
            }

            //省份
            int ProvinceID = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidProvinceID"]))
            {
                ProvinceID = Convert.ToInt32(Request.Form["hidProvinceID"]);
            }

            //城市
            int CityID = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidCityID"]))
            {
                CityID = Convert.ToInt32(Request.Form["hidCityID"]);
            }
            //地区
            int DistrictID = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidDistrictID"]))
            {
                DistrictID = Convert.ToInt32(Request.Form["hidDistrictID"]);
            }


            List<ViewCenters> list = new List<ViewCenters>();
            int count = 0;
            int pagesize = 5;
            using (CentersServiceClient client = new CentersServiceClient())
            {
                list = client.GetCentersPageList("", DistrictID, CityID, ProvinceID, CountryID, typeid, id, page, pagesize, out count);
            }
            List<ViewCentersGroup> centergroups = new List<ViewCentersGroup>();
            ViewCentersGroup centergroup = new ViewCentersGroup();
            using (EvaluatesServiceClient eclient = new EvaluatesServiceClient())
            {
                int rcount = 0;
                foreach (ViewCenters c in list)
                {
                    rcount = 0;
                    centergroup = new ViewCentersGroup();

                    centergroup.center = c;
                    eclient.GetRecommendCount(c.CenterId, out rcount);
                    centergroup.recommond = rcount;

                    //派别
                    string typename = string.Empty;
                    if (c.YogaTypeid != null)
                    {
                        string[] arrtypeid = c.YogaTypeid.Replace("|", "").Split(',');

                        for (int i = 0; i < arrtypeid.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(arrtypeid[i]))
                            {
                                typename += dicclient.GetYogaDicItemById(Convert.ToInt32(arrtypeid[i])).ItemName + ",";
                            }
                        }
                        if (!String.IsNullOrEmpty(typename))
                        {
                            typename = typename.Substring(0, typename.Length - 1);
                        }
                    }
                    centergroup.Address = method.GetItemName(c.CountryID.Value) + "·"
                        + method.GetItemName(c.ProvinceID.Value) + "·"
                        + method.GetItemName(c.CityID.Value) + "·"
                        + method.GetItemName(c.DistrictID.Value);
                    centergroup.centertype = typename;

                    centergroups.Add(centergroup);
                }
            }

            Webdiyer.WebControls.Mvc.PagedList<ViewCentersGroup> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewCentersGroup>(centergroups, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexList", pagelist);
            }
            return View(pagelist);
        }

        //
        // GET: /Mechanism/Details/5

        public ActionResult Details(int id, int page = 1)
        {
            int mycount = 0;
            ViewBag.url = Request.Url.AbsolutePath;
            ViewCenters c = GetAllMechanis(id, out mycount, page);
            ViewBag.C = c;
            ViewBag.evalcount = mycount;

            #region 机构相册
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                //机构相册分类5
                List<ViewYogaPicture> pic = clientpic.GetListByType(id, 5);
                if (pic != null)
                {
                    ViewBag.Pic = pic;
                }
            }

            #endregion

            #region 机构星级分数
            using (CenterStareServiceClient client = new CenterStareServiceClient())
            {
                int count = 0;
                decimal price = 0;
                double centerclass = 0;
                double env = 0;
                double service = 0;
                List<ViewCenterStare> starelist = client.GetCentersPageList(id, out count);
                if (starelist != null && starelist.Count != 0)
                {
                    price = (from s in starelist select s.Price).Average();
                    centerclass = (from s in starelist select s.Centerclass).Average();
                    env = (from s in starelist select s.Env).Average();
                    service = (from s in starelist select s.Service).Average();
                }
                ViewBag.price = price;
                ViewBag.centerclass = centerclass;
                ViewBag.env = env;
                ViewBag.service = service;
            }
            #endregion


            #region 机构活动
            ClassServiceClient classclient = new ClassServiceClient();
            List<ViewClass> classlist = classclient.GetClassesByZhuanYe(0, page, c.CenterId, 3);
            ViewBag.classlist = classlist;
            #endregion

            ViewBag.Members = Members(id);
            Webdiyer.WebControls.Mvc.PagedList<ViewEvaluatesGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewEvaluatesGroup>(ViewBag.EvalInfo, page, 10, mycount);
            if (Request.IsAjaxRequest())
                return PartialView("GetAllMechanis", l);
            return View(l);
        }

        [HttpGet, ValidateInput(false)]
        public string GetClass(int centerId, int page = 1)
        {
            ClassServiceClient classclient = new ClassServiceClient();
            List<ViewClass> classlist = classclient.GetClassesByZhuanYe(0, page, centerId, 3);

            string htmlStr = "";
            foreach (var item in classlist)
            {
                if (item.EndTime > DateTime.Now || string.IsNullOrWhiteSpace(item.MessageDes))
                {
                    htmlStr += "<li>";
                    htmlStr += "    <div class='lb_add_new_huodong'>";
                    htmlStr += "        <a href='/class/viewactivity?id=" + item.Id + "'>";
                    htmlStr += "            <img src=" + item.Banner + " />";
                    htmlStr += "        </a>";
                    htmlStr += "        <div class='lb_add_new_huodong_hide'>";
                    htmlStr += "            <p>" + item.Name + "</p>";
                    htmlStr += "        </div>";
                    htmlStr += "    </div>";
                    htmlStr += "</li>";
                }
                else
                {
                    htmlStr += "<li>";
                    htmlStr += "   <div class='lb_add_new_huodong'>";
                    htmlStr += "       <a href='/class/ActivityReportDetailsPage?id=" + item.Id + "&&classReportId=-99'>";
                    htmlStr += "          <img src=" + item.Banner + " />";
                    htmlStr += "     </a>";
                    htmlStr += "     <div class='lb_add_new_huodong_hide'>";
                    htmlStr += "        <p>" + item.Name + "</p>";
                    htmlStr += "    </div>";
                    htmlStr += " </div>";
                    htmlStr += "</li>";
                }
            }
            return htmlStr;
        }

        /// <summary>
        /// 属于机构的导师列表
        /// </summary>
        /// <returns></returns>
        public List<ViewUserModelsGroup> Members(int id)
        {
            #region 属于机构的导师列表
            List<ViewYogisModels> list = new List<ViewYogisModels>();
            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                //返回个数
                int count = 4;
                list = client.GetYogisModelsByCenterId(id, count);
            }
            List<ViewUserModelsGroup> users = new List<ViewUserModelsGroup>();
            FollowServiceClient clientFoll = new FollowServiceClient();
            ViewUserModelsGroup userModels = null;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].DisplayImg = CommonInfo.GetDisplayImg(list[i].DisplayImg);
                if (!string.IsNullOrEmpty(list[i].YogisDepict))
                {
                    string tempdepict = NoHTML(list[i].YogisDepict);

                    list[i].YogisDepict = tempdepict.Length > 320 ? tempdepict.Substring(0, 320) + "..." : tempdepict;
                }
                userModels = new ViewUserModelsGroup();
                if (list[i].CountryID.Value != -1)
                    userModels.CountryName = dicclient.GetById(list[i].CountryID.Value).ItemName;
                else
                    userModels.CountryName = dicclient.GetById(66).ItemName;
                userModels.VmList = list[i];
                //user.FollowCount = clientFoll.GetFollowByCount(list[i].UID);
                userModels.iFollow = method.iGetFollow(user.Uid, list[i].UID);
                users.Add(userModels);
            }

            return users;
            // return Json(users,JsonRequestBehavior.AllowGet);
            // ViewBag.Members = users;
            #endregion
        }

        /// <summary>
        /// 将html文本转化为 文本内容方法NoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("/r/n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }


        public ActionResult SchoolPicList(int id, int page = 1)
        {
            ViewCenters c = new ViewCenters();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                c = client.GetCentersById(id);
            }
            int mycount = 0;
            ViewBag.url = Request.Url.AbsolutePath;
            ViewBag.id = id;
            ViewBag.C = c;
            ViewBag.evalcount = mycount;

            #region 机构相册
            List<ViewYogaPicture> pic = null;
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                //机构相册分类5
                pic = clientpic.GetListByType(id, 5);

            }

            #endregion

            return View(pic);
        }

        /// <summary>
        /// Action：获取图片文件
        /// </summary>
        public FileContentResult GetImg(string url, int width = 100, int height = 100, string mode = "Cut")
        {
            string path = Server.MapPath(url);
            if (System.IO.File.Exists(path))
            {
                byte[] bb = CommonInfo.MakeThumbnail(path, width, height, mode, true);
                FileContentResult r = File(bb, "image/jpg");
                return r;
            }
            else
            {
                return null;
            }
        }


        private ViewCenters GetCenterAllInfo(int id)
        {
            ViewCenters c = new ViewCenters();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                c = client.GetCentersById(id);
            }
            ViewBag.centerid = id;

            #region 评价列表
            using (EvaluatesServiceClient clientEval = new EvaluatesServiceClient())
            {
                int count = 0;
                clientEval.GetRecommendCount(id, out count);
                ViewBag.Recommend = count;

                List<ViewEvaluates> evalEntity = new List<ViewEvaluates>();
                evalEntity = clientEval.GettEvalUid(id);
                List<ViewEvaluatesGroup> listGroupMsg = new List<ViewEvaluatesGroup>();

                foreach (var item in evalEntity)
                {
                    ViewEvaluatesGroup model = new ViewEvaluatesGroup();

                    model.entity = item;

                    //评论人
                    ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                    if (usermodel != null)
                        model.FromUser = usermodel.NickName;

                    //回复
                    List<ViewEvaluates> listM = clientEval.GetEvalParentID(item.Evaluateid);
                    List<ViewEvaluatesGroup> entitylist = new List<ViewEvaluatesGroup>();
                    foreach (var it in listM)
                    {
                        ViewEvaluatesGroup entityMsg = new ViewEvaluatesGroup();
                        entityMsg.entity = it;

                        //评论人
                        ViewYogaUser usermodel2 = clientUser.GetYogaUserById(it.FromUid.Value);
                        if (usermodel2 != null)
                            entityMsg.FromUser = usermodel2.NickName;

                        entitylist.Add(entityMsg);

                    }
                    model.msgList = entitylist;
                    listGroupMsg.Add(model);
                }
                ViewBag.EvalInfo = listGroupMsg;

            }
            #endregion
            return c;
        }

        //
        // GET: /Mechanism/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Mechanism/Create

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
        // POST: /Mechanism/Edit/5

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
        // POST: /Mechanism/Delete/5 
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

        //学院首页
        public ActionResult SchoolIndex(int page = 1)
        {
            List<ViewCenters> list = new List<ViewCenters>();
            int count = 0;
            using (CentersServiceClient client = new CentersServiceClient())
            {
                list = client.GetCentersPageList(page, 10, "1", out count);
            }
            PagedList<ViewCenters> pagelist = new PagedList<ViewCenters>(list, page, 10, count);

            return View("SchoolIndex", pagelist);
        }
        ///学院详细信息
        public ActionResult SchoolDetails(int id, int page = 1)
        {
            int mycount = 0;
            ViewBag.url = Request.Url.AbsolutePath;
            ViewCenters c = GetAllMechanis(id, out mycount, page);
            ViewBag.C = c;
            ViewBag.evalcount = mycount;

            Webdiyer.WebControls.Mvc.PagedList<ViewEvaluatesGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewEvaluatesGroup>(ViewBag.EvalInfo, page, 2, mycount);
            if (Request.IsAjaxRequest())
                return PartialView("GetAllMechanis", l);
            return View(l);

        }

        /// <summary>
        /// 返回学院简介
        /// </summary>
        /// <param name="id">学院编号</param>
        /// <returns></returns>
        public string SchoolDetailsByJson(int id)
        {
            ViewCenters c = new ViewCenters();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                c = client.GetCentersById(id);
            }
            return c.CenterProfile;
        }

        /// <summary>
        /// 获取本学院导师
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetModelBuycenterid(int id)
        {
            List<ViewUserModelsGroup> users = new List<ViewUserModelsGroup>();
            FollowServiceClient clientFoll = new FollowServiceClient();
            ViewUserModelsGroup user = null;

            JsonResult js = new JsonResult();

            List<ViewYogisModels> list = new List<ViewYogisModels>();
            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                //返回个数
                int count = 4;
                list = client.GetYogisModelsByCenterId(id, count);
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].DisplayImg = CommonInfo.GetDisplayImg(list[i].DisplayImg);
                list[i].YogisDepict = list[i].YogisDepict.Length > 320 ? list[i].YogisDepict.Substring(0, 320) + "..." : list[i].YogisDepict;
                user = new ViewUserModelsGroup();
                user.VmList = list[i];
                user.FollowCount = clientFoll.GetFollowByCount(list[i].UID);
                users.Add(user);
            }
            js.Data = users;
            js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return js;
        }
        /// <summary>
        /// 评价晒图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult picCreate()
        {

            // TODO: Add insert logic here
            if (!string.IsNullOrEmpty(Request.Form["Diploma"]))
            {
                //相册
                string[] strPic = Request.Form["Diploma"].ToString().Split(';');
                string[] strpicContent = Request.Form["PictureContent"].ToString().TrimEnd('|').Split('|');

                ViewYogaPicture picModel = new ViewYogaPicture();
                for (int i = 0; i < strPic.Length - 1; i++)
                {
                    #region
                    if (!string.IsNullOrEmpty(strPic[i]))
                    {
                        picModel.PictureOriginal = strPic[i];

                        picModel.Uid = Convert.ToInt32(Request.Form["centerid"]);

                        picModel.PictureType = 6;
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

                        using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                        {
                            clientpic.Add(picModel);
                        }
                    }
                    #endregion
                }
            }
            return Json(new { code = 0 });
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="iType">机构类型</param>
        ///  <param name="urlContent">搜索内容</param>
        /// <returns></returns>
        public ActionResult Search(string iType, string urlContent = "", int page = 1)
        {
            int count = 0;
            string YogaTypeid = string.Empty;

            List<ViewCenters> centers = new List<ViewCenters>();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                centers = client.GetCentersPageList(urlContent, 0, 0, iType, 1, 10, out count);
            }
            if (count != 0)
            {
                ViewBag.listGroup = centers;
                ViewBag.Count = count;
            }
            ViewBag.type = "jigou";
            ViewBag.iType = iType;
            ViewBag.urlContent = urlContent;
            return View();
        }
    }
}
