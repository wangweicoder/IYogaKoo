using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.Controllers;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zzfIBM.WebControls.Mvc;
namespace IYogaKoo.Controllers
{
    public class YogaGuruController : Controller
    {

        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser;
        tMessageServiceClient clientMsg;
        FollowServiceClient clientFoll;
        YogaUserDetailServiceClient clientDetail;
        tWriteLogServiceClient logClient;
        YogaPictureServiceClient clientPic;
        YogaDicItemServiceClient dicclient;
        tZanModelsServiceClient zanclient;
        method method;
        public YogaGuruController()
        {
            ViewBag.user = user;
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();
            clientMsg = new tMessageServiceClient();
            clientFoll = new FollowServiceClient();
            clientDetail = new YogaUserDetailServiceClient();

            logClient = new tWriteLogServiceClient();
            clientPic = new YogaPictureServiceClient();
            dicclient = new YogaDicItemServiceClient();
            zanclient = new tZanModelsServiceClient();
            method = new method();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = clientDetail.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = client.GetYogisModelsById(user.Uid);
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
        //
        // GET: /YogaGuru/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  瑜伽达人/导师 主页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id, int page = 1)
        {
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
            ///Follow iType
            ViewBag.UserType = 1;

            ViewBag.id = id;

            int strUid = user.Uid;
            int iLoginID = user.Uid;//登录用户ID
            ViewBag.iLoginID = user.Uid;

            #region 导师专页 基本信息

            ViewYogisModels temp = new ViewYogisModels();

            temp = client.GetYogisModelsById(id);
            if (!temp.IsNullOrEmpty())
            {
                ///昵称
                ViewBag.NickName = clientUser.GetYogaUserById(temp.UID).NickName;

                strUid = temp.UID;
                ViewBag.strUid = temp.UID;
                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (temp.CountryID != null && temp.CountryID != 0)
                {
                    strCountryID = GetItemName(temp.CountryID.Value) + "· ";
                }
                if (temp.ProvinceID != null && temp.ProvinceID != 0)
                {
                    strProvinceID = GetItemName(temp.ProvinceID.Value) + " · ";
                }
                if (temp.CityID != null && temp.CityID != 0)
                {
                    strCityID = GetItemName(temp.CityID.Value) + " ·";
                }
                if (temp.DistrictID != null && temp.DistrictID != 0)
                {
                    strDistrictID = GetItemName(temp.DistrictID.Value);
                }

                ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
                ///流派
                if (!string.IsNullOrEmpty(temp.YogaTypeid))
                {
                    string[] ids = temp.YogaTypeid.Split(',');
                    foreach (var i in ids)
                    {
                        ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                    }
                }
                else ViewBag.YogaTypeid = "";
                ViewBag.listGroup = temp;
            }
            #endregion

            //关注 粉丝 人气
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }

            #region 瑜伽圈

            List<ViewFollow> listFollow = new List<ViewFollow>();

            listFollow = clientFoll.GetFollowUidQuiltList(id);

            ViewFollow vm = clientFoll.GetFollowById(iLoginID, strUid);
            ViewBag.isfollow = vm == null ? false : vm.isfollow;

            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();

            #region

            string idsUid = string.Join(",", listFollow.Where(a => a.Uid != id).Select(a => a.Uid));
            string idsQuiltUid = string.Join(",", listFollow.Where(a => a.QuiltUid != id).Select(a => a.QuiltUid));

            string idsval = idsUid + "," + idsQuiltUid;
            //数组
            var idslist = idsval.Split(',').Distinct();

            foreach (var item in idslist)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    ViewFollowUserDetail model = new ViewFollowUserDetail();
                    ViewYogaUser modelyoga = clientUser.GetYogaUserById(Convert.ToInt32(item));

                    if (modelyoga.UserType == 0)
                    {
                        //专页id和被关注id是同一个 
                        //习练者
                        ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(Convert.ToInt32(item));
                        model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                        model.userurl = "/YogaUserDetail/Details/";
                        model.uid = udmodel.UID;
                        //登录表                   
                        model.nickname = modelyoga.NickName;

                        list2Group.Add(model);
                    }
                    else if (modelyoga.UserType == 1)
                    {
                        #region 导师
                        ViewYogisModels mmodel = client.GetYogisModelsById(Convert.ToInt32(item));
                        model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                        //登录表                   
                        model.nickname = modelyoga.NickName;
                        if (mmodel.YogisLevel != null)
                        {
                            if (mmodel.YogisLevel == 4)
                            {
                                model.userurl = "/YogaGuru/Details/";
                                model.nickname = mmodel.RealName;
                            }
                            else
                            {
                                model.userurl = "/YogisModels/Details/";
                            }
                        }
                        else
                        {
                            model.userurl = "/YogisModels/Details/";
                        }
                        model.uid = mmodel.UID;
                        #endregion
                        list2Group.Add(model);
                    }
                }
            }

            #endregion

            ViewBag.list2Group = list2Group;

            #endregion

            #region 相册

            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();
            int piccount = 0;
            listPic = clientPic.GetYogaPicturePageList(id, 1, 7, out piccount);
            if (listPic.Count() > 0)
            {
                ViewBag.listPic = listPic;
            }
            #endregion
            int rcount = 0;
            int mcount = 0;
            int pagesize = 10;
            ViewBag.msginfo = method.listMessage(id, 0, page, out mcount);//留言 评论            
            ViewBag.mcount = mcount;
            ViewBag.rcount = clientMsg.GettMessageUid(id, 1).Count();
            #region 我的日志列表
            int count = 0;
            List<ViewtWriteLog> listwriteLog = logClient.GettWriteLogPageList(id, 1, 6, out count);

            List<ViewtWriteLogGroup> listLog = new List<ViewtWriteLogGroup>();
            foreach (var item in listwriteLog)
            {
                ViewtWriteLogGroup model = new ViewtWriteLogGroup();
                model.entity = item;

                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.Uid.Value);
                if (userEntity != null)
                {
                    model.UserName = userEntity.NickName;
                }
                else
                {
                    model.UserName = "";
                }

                listLog.Add(model);
            }
            ViewBag.listLog = listLog;
            #endregion

            ViewBag.url = Request.Url.AbsolutePath;
            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.msginfo, page, pagesize, mcount);

            string strRec = "";
            if (!string.IsNullOrEmpty(Request.Form["levelid"]))
            {
                strRec = Request.Form["levelid"];
            }
            if (Session["levelid"] != null)
            {
                if (string.IsNullOrEmpty(strRec))
                {
                    strRec = Session["levelid"].ToString();
                }

            }
            if (!string.IsNullOrEmpty(strRec))
            {
                if (strRec == "1")
                {
                    #region 推荐数据
                    List<ViewtMessage> recommendEntity = new List<ViewtMessage>();
                    recommendEntity = clientMsg.GettMessageUidList(id, 1, page, pagesize, out rcount);
                    List<ViewtMessageGroup> listGroupRec = new List<ViewtMessageGroup>();

                    foreach (var item in recommendEntity)
                    {
                        ViewtMessageGroup model = new ViewtMessageGroup();

                        model.entity = item;
                        model.entity.iZan = zanclient.ZanCount(item.ID, item.FormType.Value);
                        //被留言人

                        ViewYogaUser yuser = clientUser.GetYogaUserById(item.ToUid.Value);
                        if (yuser != null)
                        {
                            model.ToUser = yuser.NickName;
                            model.UserType = yuser.UserType;
                        }
                        //留言人
                        ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                        if (usermodel != null)
                        {
                            model.FromUser = usermodel.NickName;
                            model.UserType = usermodel.UserType;
                        }
                        if (item.FormType == 0)
                        {
                            //习练者头像
                            using (YogaUserDetailServiceClient clientDet = new YogaUserDetailServiceClient())
                            {
                                ViewYogaUserDetail ViewDet = new ViewYogaUserDetail();
                                if (item.FromUid != 0)
                                    model.DisplayImg = clientDet.GetYogaUserDetailById(item.FromUid.Value).DisplayImg;
                                model.sUrl = "/YogaUserDetail/Details/" + item.FromUid.Value;
                            }
                        }
                        else
                        {
                            //导师头像
                            using (YogisModelsServiceClient clientDet = new YogisModelsServiceClient())
                            {
                                ViewYogisModels ViewDet = new ViewYogisModels();
                                if (item.FromUid != 0)
                                    model.DisplayImg = clientDet.GetYogisModelsById(item.FromUid.Value).DisplayImg;
                                model.sUrl = "/YogisModels/Details/" + item.FromUid.Value;
                            }
                        }
                        //回复
                        List<ViewtMessage> listM = clientMsg.GettMessageParentID(item.ID);
                        List<ViewtMessageGroup> entitylist = new List<ViewtMessageGroup>();
                        foreach (var it in listM)
                        {
                            ViewtMessageGroup entityMsg = new ViewtMessageGroup();
                            entityMsg.entity = it;
                            //被留言人

                            ViewYogaUser yuser2 = clientUser.GetYogaUserById(it.ToUid.Value);
                            if (yuser2 != null)
                                entityMsg.ToUser = yuser2.NickName;
                            //留言人
                            ViewYogaUser usermodel2 = clientUser.GetYogaUserById(it.FromUid.Value);
                            if (usermodel2 != null)
                                entityMsg.FromUser = usermodel2.NickName;

                            entitylist.Add(entityMsg);

                        }
                        model.msgList = entitylist;
                        listGroupRec.Add(model);
                    }

                    ViewBag.RecInfo = listGroupRec;

                    #endregion
                    Session["levelid"] = strRec;
                    Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l2 = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.RecInfo, page, pagesize, rcount);
                    if (Request.IsAjaxRequest())
                        return PartialView("PartialRec", l2);
                }

            }
            if (Request.IsAjaxRequest())
            {
                Session["levelid"] = 0;
                return PartialView("PartialRec", l);
            }
            return View(l);
        }
        public PartialViewResult GetRec(int id, int page = 1)
        {
            #region 推荐
            int rcount = 0;
            int pagesize = 10;
            List<ViewtMessage> recommendEntity = new List<ViewtMessage>();
            recommendEntity = clientMsg.GettMessageUidList(id, 1, page, pagesize, out rcount);
            List<ViewtMessageGroup> listGroupRec = new List<ViewtMessageGroup>();

            foreach (var item in recommendEntity)
            {
                ViewtMessageGroup model = new ViewtMessageGroup();

                model.entity = item;
                //被留言人

                ViewYogaUser yuser = clientUser.GetYogaUserById(item.ToUid.Value);
                if (yuser != null)
                    model.ToUser = yuser.NickName;
                //留言人
                ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                if (usermodel != null)
                    model.FromUser = usermodel.NickName;
                if (item.FormType == 0)
                {
                    //习练者头像
                    using (YogaUserDetailServiceClient clientDet = new YogaUserDetailServiceClient())
                    {
                        ViewYogaUserDetail ViewDet = new ViewYogaUserDetail();
                        if (item.FromUid != 0)
                            model.DisplayImg = clientDet.GetYogaUserDetailById(item.FromUid.Value).DisplayImg;
                        model.sUrl = "/YogaUserDetail/Details/" + item.FromUid.Value;
                    }
                }
                else
                {
                    //导师头像
                    using (YogisModelsServiceClient clientDet = new YogisModelsServiceClient())
                    {
                        ViewYogisModels ViewDet = new ViewYogisModels();
                        if (item.FromUid != 0)
                            model.DisplayImg = clientDet.GetYogisModelsById(item.FromUid.Value).DisplayImg;
                        model.sUrl = "/YogisModels/Details/" + item.FromUid.Value;
                    }
                }
                //回复
                List<ViewtMessage> listM = clientMsg.GettMessageParentID(item.ID);
                List<ViewtMessageGroup> entitylist = new List<ViewtMessageGroup>();
                foreach (var it in listM)
                {
                    ViewtMessageGroup entityMsg = new ViewtMessageGroup();
                    entityMsg.entity = it;
                    //被留言人

                    ViewYogaUser yuser2 = clientUser.GetYogaUserById(it.ToUid.Value);
                    if (yuser2 != null)
                        entityMsg.ToUser = yuser2.NickName;
                    //留言人
                    ViewYogaUser usermodel2 = clientUser.GetYogaUserById(it.FromUid.Value);
                    if (usermodel2 != null)
                        entityMsg.FromUser = usermodel2.NickName;

                    entitylist.Add(entityMsg);

                }
                model.msgList = entitylist;
                listGroupRec.Add(model);
            }
            Session["levelid"] = 1;
            #endregion
            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> lrec = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(listGroupRec, page, pagesize, rcount);
            ViewBag.rcount = rcount;
            return PartialView("PartialRec", lrec);
        }

        public ActionResult PicList(int id, int page = 1)
        {
            ///Follow iType
            ViewBag.UserType = 1;

            ViewBag.id = id;

            int strUid = user.Uid;
            int iLoginID = user.Uid;//登录用户ID
            ViewBag.iLoginID = user.Uid;

            #region 导师专页 基本信息

            ViewYogisModels temp = new ViewYogisModels();

            temp = client.GetYogisModelsById(id);
            ///昵称
            ViewBag.NickName = clientUser.GetYogaUserById(temp.UID).NickName;

            strUid = temp.UID;
            ViewBag.strUid = temp.UID;
            ///位置
            string strCountryID = "";
            string strProvinceID = "";
            string strCityID = "";
            string strDistrictID = "";
            if (temp.CountryID != null && temp.CountryID != 0)
            {
                strCountryID = GetItemName(temp.CountryID.Value) + "· ";
            }
            if (temp.ProvinceID != null && temp.ProvinceID != 0)
            {
                strProvinceID = GetItemName(temp.ProvinceID.Value) + " · ";
            }
            if (temp.CityID != null && temp.CityID != 0)
            {
                strCityID = GetItemName(temp.CityID.Value) + " ·";
            }
            if (temp.DistrictID != null && temp.DistrictID != 0)
            {
                strDistrictID = GetItemName(temp.DistrictID.Value);
            }

            ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
            ///流派
            if (!string.IsNullOrEmpty(temp.YogaTypeid))
            {
                string[] ids = temp.YogaTypeid.Split(',');
                foreach (var i in ids)
                {
                    ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                }
            }
            else ViewBag.YogaTypeid = "";
            ViewBag.listGroup = temp;
            #endregion

            //推荐的数据            
            using (tZanModelsServiceClient zclient = new tZanModelsServiceClient())
            {
                List<ViewtZanModels> listz = zclient.GettZanUid(id);
                ViewBag.tzancount = listz.Count();
            }
            //关注 粉丝 人气
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }

            #region 相册

            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();

            listPic = clientPic.GetUidList(id);


            #endregion
            return View(listPic);
        }

        public ActionResult PicList2(int id, int page = 1)
        {
            ///Follow iType
            ViewBag.UserType = 1;

            ViewBag.id = id;

            int strUid = user.Uid;
            int iLoginID = user.Uid;//登录用户ID
            ViewBag.iLoginID = user.Uid;

            #region 导师专页 基本信息

            ViewYogisModels temp = new ViewYogisModels();

            temp = client.GetYogisModelsById(id);
            ///昵称
            ViewBag.NickName = clientUser.GetYogaUserById(temp.UID).NickName;

            strUid = temp.UID;
            ViewBag.strUid = temp.UID;
            ///位置
            string strCountryID = "";
            string strProvinceID = "";
            string strCityID = "";
            string strDistrictID = "";
            if (temp.CountryID != null && temp.CountryID != 0)
            {
                strCountryID = GetItemName(temp.CountryID.Value) + "· ";
            }
            if (temp.ProvinceID != null && temp.ProvinceID != 0)
            {
                strProvinceID = GetItemName(temp.ProvinceID.Value) + " · ";
            }
            if (temp.CityID != null && temp.CityID != 0)
            {
                strCityID = GetItemName(temp.CityID.Value) + " ·";
            }
            if (temp.DistrictID != null && temp.DistrictID != 0)
            {
                strDistrictID = GetItemName(temp.DistrictID.Value);
            }

            ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
            ///流派
            if (!string.IsNullOrEmpty(temp.YogaTypeid))
            {
                string[] ids = temp.YogaTypeid.Split(',');
                foreach (var i in ids)
                {
                    ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                }
            }
            else ViewBag.YogaTypeid = "";
            ViewBag.listGroup = temp;
            #endregion

            //推荐的数据            
            using (tZanModelsServiceClient zclient = new tZanModelsServiceClient())
            {
                List<ViewtZanModels> listz = zclient.GettZanUid(id);
                ViewBag.tzancount = listz.Count();
            }
            //关注 粉丝 人气
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }

            #region 相册

            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();

            listPic = clientPic.GetUidList(id);


            #endregion
            return View(listPic);
        }
        //
        // GET: /YogaGuru/Create

        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// 地区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetItemName(int id)
        {
            ViewYogaDicItem list = new ViewYogaDicItem();

            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                list = client.GetYogaDicItemById(id);

            }
            if (list != null)
                return list.ItemName;
            else
                return "";
        }
        //
        // POST: /YogaGuru/Create

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
        // GET: /YogaGuru/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /YogaGuru/Edit/5

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
        // GET: /YogaGuru/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /YogaGuru/Delete/5

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
        /// Action：获取图片文件
        /// </summary>
        public FileContentResult GetImg(string url, int width = 100, int height = 100, string mode = "Cut", bool isChange = true)
        {
            string path = Server.MapPath(url);
            if (System.IO.File.Exists(path))
            {
                byte[] bb = CommonInfo.MakeThumbnail(path, width, height, mode, isChange);
                FileContentResult r = File(bb, "image/jpg");
                return r;
            }
            else
            {
                return null;
            }
        }
    }
}
