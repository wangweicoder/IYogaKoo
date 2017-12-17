using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace IYogaKoo.Controllers
{
    public class YogaUserDetailController : Controller
    {
        //
        // GET: /YogaUserDetail/
        ///获取用户信息cookie 
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser;
        tMessageServiceClient clientMsg;
        tWriteLogServiceClient logClient;
        YogaPictureServiceClient clientPic;
        YogaUserDetailServiceClient userDetclient;
        YogaDicItemServiceClient dicclient;
        FollowServiceClient clientfollow;
        LevelOrderServiceClient levelorderclient;
        /// <summary>
        /// 我感兴趣的活动
        /// </summary>
        InterestServiceClient interclient;
        /// <summary>
        /// 我报名的活动
        /// </summary>
        OrderServiceClient orderclient;
        ClassServiceClient classclient;
        CentersServiceClient cenclient;
        method method;
        tZanModelsServiceClient zanclient;
        tLearingServiceClient learnclient;
        public YogaUserDetailController()
        {
            ViewBag.user = user;
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();
            clientMsg = new tMessageServiceClient();
            logClient = new tWriteLogServiceClient();
            clientPic = new YogaPictureServiceClient();
            userDetclient = new YogaUserDetailServiceClient();
            dicclient = new YogaDicItemServiceClient();
            clientfollow = new FollowServiceClient();
            interclient = new InterestServiceClient();
            orderclient = new OrderServiceClient();
            classclient = new ClassServiceClient();
            method = new method();
            zanclient = new tZanModelsServiceClient();
            levelorderclient = new LevelOrderServiceClient();
            cenclient = new CentersServiceClient();
            learnclient = new tLearingServiceClient();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = userDetclient.GetYogaUserDetailById(user.Uid);
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
        // GET: /Mechanism/

        public ActionResult Index(int page = 1)
        {

            //派别
            int typeid = 0;
            if (!string.IsNullOrEmpty(Request.Form["typeid"]))
            {
                typeid = Convert.ToInt32(Request.Form["typeid"]);
            }
            int level = 0;
            //瑜伽类别
            if (!string.IsNullOrEmpty(Request.Form["levelid"]))
            {
                level = Convert.ToInt32(Request.Form["levelid"]);
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

            string name = string.Empty;

            List<ViewYogaUserDetail> list = new List<ViewYogaUserDetail>();
            int count = 0;
            int pagesize = 12;
            List<ViewYogaUser> listuser = clientUser.BackGetPageList(0);

            string idsUId = string.Join(",",listuser.Select(a=>a.Uid));

            list = userDetclient.GetYogaUserList("", DistrictID, CityID, ProvinceID, CountryID, typeid, level, 2, out count);

            List<ViewUserDetailGroup> users = new List<ViewUserDetailGroup>();
            ViewUserDetailGroup user = new ViewUserDetailGroup();
            ViewYogaDicItem dicitem = null;
            foreach (ViewYogaUserDetail c in list)
            {
                foreach (var ik in idsUId.Split(','))
                {
                    if (c.UID == Convert.ToInt32(ik))
                    {
                        #region

                        c.DisplayImg = CommonInfo.GetDisplayImg(c.DisplayImg);
                        user = new ViewUserDetailGroup();
                        user.VDetailsList = c;
                        //派别
                        string typename = string.Empty;
                        if (c.YogaTypeid != null)
                        {
                            string[] arrtypeid = c.YogaTypeid.Split(',');

                            for (int i = 0; i < arrtypeid.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(arrtypeid[i]))
                                {
                                    int result = 0;
                                    if (Int32.TryParse(arrtypeid[i], out result))
                                    {
                                        dicitem = dicclient.GetYogaDicItemById(result);
                                        if (dicitem != null)
                                        {
                                            typename += dicitem.ItemName + ",";
                                        }
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(typename))
                            {
                                typename = typename.Substring(0, typename.Length - 1);
                            }
                        }
                        user.usertype = typename;

                        //昵称
                        ViewYogaUser u = clientUser.GetYogaUserById(c.UID);
                        user.VyList = u;
                        //粉丝
                        user.FollowCount = clientfollow.GetFollowByCount(c.UID);
                        users.Add(user);
                        #endregion
                    }
                
                }
               
            }
            count = users.Count();
            users = users.
                OrderByDescending(x => x.FollowCount).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VDetailsList.DisplayImg))
                .Skip((page - 1) * pagesize).Take(pagesize).ToList();
           
             Webdiyer.WebControls.Mvc.PagedList<ViewUserDetailGroup> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewUserDetailGroup>(users, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexList", pagelist);
            }
            return View(pagelist);
        }

        //
        // GET: /YogaUserDetail/Details/5

        public ActionResult Details(int id, int page = 1)
        {
            try
            {
                ///Follow iType
                ViewBag.UserType = 0;
                ViewBag.id = id;
                int strUid = 0;
                int iLoginID = user.Uid;//登录用户ID
                ViewBag.iLoginID = user.Uid;

                #region 习练者专页 基本信息


                ViewYogaUserDetail temp = new ViewYogaUserDetail();

                temp = userDetclient.GetYogaUserDetailById(id);

                #region 习练者基本信息
                strUid = temp.UID;
                ViewBag.strUid = temp.UID;
                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (temp.CountryID != null && temp.CountryID != 0)
                {
                    strCountryID = GetItemName(temp.CountryID.Value) + "·";
                }
                if (temp.ProvinceID != null && temp.ProvinceID != 0)
                {
                    strProvinceID = GetItemName(temp.ProvinceID.Value) + "·";
                }
                if (temp.CityID != null && temp.CityID != 0)
                {
                    strCityID = GetItemName(temp.CityID.Value) + "·";
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
                ViewBag.UserDetail = temp;
                #endregion

                ViewYogaUser userEntity = clientUser.GetYogaUserById(temp.UID);
                if (userEntity != null)
                {
                    ViewBag.NickName = userEntity.NickName;//昵称 
                }
                if (user.Uid == id)
                {
                    ViewBag.Gender = 2;
                }
                else
                {
                    ViewBag.Gender = temp.Gender.Value;
                }
                #endregion

                //关注 粉丝 
                ViewFollow viewMoel = new ViewFollow();
                using (FollowServiceClient followClient = new FollowServiceClient())
                {
                    ViewBag.iCount = followClient.GetFollowByUid(id);
                    ViewBag.FCount = followClient.GetFollowByCount(id);
                }
                #region 瑜伽习练圈

                List<ViewFollow> listFollow = new List<ViewFollow>();
                using (FollowServiceClient client = new FollowServiceClient())
                {
                    listFollow = client.GetFollowUidQuiltList(id);

                    ViewFollow vm = client.GetFollowById(iLoginID, strUid);
                    ViewBag.isfollow = vm == null ? false : vm.isfollow;
                }
                List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();
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
                            ViewYogaUserDetail udmodel = userDetclient.GetYogaUserDetailById(Convert.ToInt32(item));
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

                ViewBag.list2Group = list2Group.Take(8);

                #endregion

                #region 相册

                List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();

                int piccount = 0;
                listPic = clientPic.GetYogaPicturePageList(id, 1, 8, out piccount);
                if (listPic.Count() > 0)
                {
                    ViewBag.listPic = listPic;
                }
                #endregion


                int rcount = 0;
                int pagesize = 10;

                ViewBag.MsgInfo = method.listMessage(id, 0, page, out rcount);//留言 评论
                ViewBag.rcount = rcount;

                #region 我的日志列表
                int count = 0;
                List<ViewtWriteLog> listwriteLog = logClient.GettWriteLogPageList(id, 1, 6, out count);

                List<ViewtWriteLogGroup> listLog = new List<ViewtWriteLogGroup>();
                foreach (var item in listwriteLog)
                {
                    ViewtWriteLogGroup model = new ViewtWriteLogGroup();
                    model.entity = item;
                    ViewYogaUser uEntity = clientUser.GetYogaUserById(item.Uid.Value);
                    if (uEntity != null)
                    {
                        model.UserName = uEntity.NickName;
                    }
                    else
                    {
                        model.UserName = "";
                    }

                    listLog.Add(model);
                }
                ViewBag.listLog = listLog;
                #endregion

                #region 我的活动
                string classIds = string.Empty;

                List<ViewInterestedClass> interlist = interclient.GetClassId(id);//我感兴趣的活动
                if (interlist.Count() > 0)
                {
                    for (int i = 0; i < interlist.Count(); i++)
                    {
                        classIds += interlist[i].ClassId + ",";
                    }
                }
                List<ViewOrder> orderlist = orderclient.GetClassId(id);
                if (orderlist.Count() > 0)
                {
                    for (int i = 0; i < orderlist.Count(); i++)
                    {
                        classIds += orderlist[i].ClassId + ",";
                    }
                }
                List<ViewClass> classlist = classclient.GetClassesByZhuanYe(id, 0, 0, 2); //classclient.GetClassesByUid(classIds, id).Take(8).ToList();

                ViewBag.classlist = classlist;

                #endregion

                Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.MsgInfo, page, pagesize, rcount);
                if (Request.IsAjaxRequest())
                    return PartialView("PartialMsg", l);
                return View(l);
            } 
            catch (Exception ex)
            {
                Tools.WriteTextLog("习练者主页 报错：",ex.Message);
                return View();
            }
        }


        public ActionResult PicList(int id, int page = 1)
        {

            ///Follow iType
            ViewBag.UserType = 0;
            ViewBag.id = id;
            int strUid = 0;
            int iLoginID = user.Uid;//登录用户ID
            ViewBag.iLoginID = user.Uid;

            #region 习练者专页 基本信息


            ViewYogaUserDetail temp = new ViewYogaUserDetail();

            temp = userDetclient.GetYogaUserDetailById(id);

            #region 习练者基本信息
            strUid = temp.UID;
            ViewBag.strUid = temp.UID;
            ///位置
            string strCountryID = "";
            string strProvinceID = "";
            string strCityID = "";
            string strDistrictID = "";
            if (temp.CountryID != null && temp.CountryID != 0)
            {
                strCountryID = GetItemName(temp.CountryID.Value) + "·";
            }
            if (temp.ProvinceID != null && temp.ProvinceID != 0)
            {
                strProvinceID = GetItemName(temp.ProvinceID.Value) + "·";
            }
            if (temp.CityID != null && temp.CityID != 0)
            {
                strCityID = GetItemName(temp.CityID.Value) + "·";
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
            ViewBag.UserDetail = temp;
            #endregion

            ViewYogaUser userEntity = new ViewYogaUser();

            userEntity = clientUser.GetYogaUserById(temp.UID);
            ViewBag.NickName = userEntity.NickName;//昵称

            #endregion

            //关注 粉丝 
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }
            #region 瑜伽习练圈

            List<ViewFollow> listFollow = new List<ViewFollow>();
            using (FollowServiceClient client = new FollowServiceClient())
            {
                listFollow = client.GetFollowQuiltUidList(id);

                ViewFollow vm = client.GetFollowById(iLoginID, strUid);
                ViewBag.isfollow = vm == null ? false : vm.isfollow;
            }
            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();

            foreach (var item in listFollow)
            {
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                if (item.iType == 0)
                {
                    //习练者
                    ViewYogaUserDetail udmodel = userDetclient.GetYogaUserDetailById(item.QuiltUid);
                    model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                    model.userurl = "/YogaUserDetail/Details/";
                    model.uid = udmodel.UID;
                    //登录表                   
                    model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                    //粉丝                  
                    using (FollowServiceClient followClient = new FollowServiceClient())
                    {
                        model.FollowCount = followClient.GetFollowByCount(item.Uid);
                    }
                    list2Group.Add(model);
                }
                else if (item.iType == 1)
                {
                    //导师
                    ViewYogisModels mmodel = client.GetYogisModelsById(item.QuiltUid);
                    model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                    //登录表                   
                    model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
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
                    //粉丝                  
                    using (FollowServiceClient followClient = new FollowServiceClient())
                    {
                        model.FollowCount = followClient.GetFollowByCount(item.Uid);
                    }
                    list2Group.Add(model);
                }
            }

            ViewBag.list2Group = list2Group;

            #endregion

            #region 相册

            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();

            listPic = clientPic.GetUidList(id);

            #endregion
            return View(listPic);
        }
        public ActionResult UserDetails(int id)
        {

            ///Follow iType
            ViewBag.UserType = 0;

            ViewBag.id = id;
            int strUid = 0;
            int iLoginID = user.Uid;//登录用户ID
            ViewBag.iLoginID = user.Uid;

            #region 习练者专页 基本信息


            ViewYogaUserDetail temp = new ViewYogaUserDetail();

            temp = userDetclient.GetYogaUserDetailById(id);

            #region 习练者基本信息
            try
            {
                strUid = temp.UID;
                ViewBag.strUid = temp.UID;
            }
            catch { }
            ///位置
            string strCountryID = "";
            string strProvinceID = "";
            string strCityID = "";
            string strDistrictID = "";
            if (temp.CountryID != null && temp.CountryID != 0)
            {
                strCountryID = GetItemName(temp.CountryID.Value) + "·";
            }
            if (temp.ProvinceID != null && temp.ProvinceID != 0)
            {
                strProvinceID = GetItemName(temp.ProvinceID.Value) + "·";
            }
            if (temp.CityID != null && temp.CityID != 0)
            {
                strCityID = GetItemName(temp.CityID.Value) + "·";
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
            #endregion

            ViewYogaUser userEntity = new ViewYogaUser();

            userEntity = clientUser.GetYogaUserById(temp.UID);
            ViewBag.NickName = userEntity.NickName;//昵称

            #endregion

            //关注 粉丝 
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }
            #region 瑜伽习练圈

            List<ViewFollow> listFollow = new List<ViewFollow>();
            using (FollowServiceClient client = new FollowServiceClient())
            {
                listFollow = client.GetFollowQuiltUidList(id);

                ViewFollow vm = client.GetFollowById(iLoginID, strUid);
                ViewBag.isfollow = vm == null ? false : vm.isfollow;
            }
            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();

            foreach (var item in listFollow)
            {
                ViewYogaUser umodel = clientUser.GetYogaUserById(item.QuiltUid);
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = umodel.NickName;
                model.flag = item.iType.Value;
                if (item.iType == 0)
                {
                    //习练者
                    ViewYogaUserDetail udmodel = userDetclient.GetYogaUserDetailById(item.QuiltUid);
                    if (udmodel != null)
                    {
                        model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                        model.userurl = "/YogaUserDetail/Details/";
                        model.uid = udmodel.UID;
                        //登录表                   
                        model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                        //粉丝                  
                        using (FollowServiceClient followClient = new FollowServiceClient())
                        {
                            model.FollowCount = followClient.GetFollowByCount(item.Uid);
                            model.FollowersCount = followClient.GetFollowByCount(item.QuiltUid);
                        }
                        model.Leval = udmodel.Ulevel;
                        list2Group.Add(model);
                    }
                }
                else if (item.iType == 1)
                {
                    //导师
                    ViewYogisModels mmodel = client.GetYogisModelsById(item.QuiltUid);
                    if (mmodel != null)
                    {
                        model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                        //登录表                   
                        model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
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
                        //粉丝                  
                        using (FollowServiceClient followClient = new FollowServiceClient())
                        {
                            model.FollowCount = followClient.GetFollowByCount(item.Uid);
                            model.FollowersCount = followClient.GetFollowByCount(item.QuiltUid);//你关注的人的粉丝
                        }
                        model.Leval = mmodel.YogisLevel.Value;
                        list2Group.Add(model);
                    }
                }
            }

            ViewBag.list2Group = list2Group;

            #endregion

            #region 相册

            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();

            listPic = clientPic.GetUidList(id);
            if (listPic.Count() > 0)
            {
                ViewBag.listPic = listPic;
            }
            #endregion

            #region 留言


            List<ViewtMessage> msgEntity = new List<ViewtMessage>();
            msgEntity = clientMsg.GettMessageUid(id, 0);
            List<ViewtMessageGroup> listGroupMsg = new List<ViewtMessageGroup>();

            foreach (var item in msgEntity)
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
                        {
                            ViewDet = clientDet.GetYogaUserDetailById(item.FromUid.Value);
                            if (ViewDet != null)
                            {
                                model.DisplayImg = ViewDet.DisplayImg;
                            }
                            model.sUrl = "/YogaUserDetail/Details/" + item.FromUid.Value;
                        }
                    }
                }
                else
                {
                    //导师头像
                    using (YogisModelsServiceClient clientDet = new YogisModelsServiceClient())
                    {
                        ViewYogisModels ViewDet = new ViewYogisModels();
                        if (item.FromUid != 0)
                        {
                            ViewDet = clientDet.GetYogisModelsById(item.FromUid.Value);
                            if (ViewDet != null)
                            {

                                model.DisplayImg = ViewDet.DisplayImg;
                            }
                            model.sUrl = "/YogisModels/Details/" + item.FromUid.Value;
                        }

                    }
                }
                //strPic
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
                listGroupMsg.Add(model);
            }

            ViewBag.MsgInfo = listGroupMsg;

            #endregion

            #region 我的日志列表
            int count = 0;
            List<ViewtWriteLog> listwriteLog = logClient.GettWriteLogPageList(id, 1, 7, out count);

            List<ViewtWriteLogGroup> listLog = new List<ViewtWriteLogGroup>();
            foreach (var item in listwriteLog)
            {
                ViewtWriteLogGroup model = new ViewtWriteLogGroup();
                model.entity = item;
                ViewYogaUser uEntity = clientUser.GetYogaUserById(item.Uid.Value);
                if (uEntity != null)
                {
                    model.UserName = uEntity.NickName;
                }
                else
                {
                    model.UserName = "";
                }

                listLog.Add(model);
            }
            ViewBag.listLog = listLog;
            #endregion
            return View(temp);
        }

        /// <summary>
        /// 赞同
        /// </summary>
        /// <returns></returns>
        public JsonResult iZan()
        {
            try
            {
                ViewtZanModels zanEntity = new ViewtZanModels();
                // TODO: Add update logic here
                int Uid = Convert.ToInt32(Request.Form["uid"]);//主键ID
                int iToType = Convert.ToInt32(Request.Form["UserType"]);
                zanEntity = zanclient.GetExists(user.Uid, Uid, user.UserType.Value, iToType);
                if (zanEntity == null)
                {
                    zanEntity = new ViewtZanModels();
                    zanEntity.iToUid = Uid;//被赞人
                    zanEntity.iFromUid = user.Uid;//登录人
                    zanEntity.iType = user.UserType;
                    zanEntity.iToType = iToType;
                    zanEntity.CreateDate = DateTime.Now;
                    zanclient.Add(zanEntity);
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 2 });//已经赞过
                }
                //zanclient
                //int fromuid=Convert.ToInt32(Request.Form["fromuid"]);
                //int ParentID = 0;
                //ViewtMessage model = clientMsg.GettMessageOnly(Uid, fromuid, ParentID);
                //ViewtMessage model = clientMsg.GetById(Uid);
                //if (model.iZan == null)
                //{
                //    model.iZan = 1;
                //}
                //else
                //{
                //    model.iZan = model.iZan + 1;
                //}
                //clientMsg.Update(model);
                //return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 地区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetItemName(int id)
        {
            ViewYogaDicItem list = new ViewYogaDicItem();
            if (id > 0)
            {
                using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
                {
                    list = client.GetYogaDicItemById(id);
                }
                return list.ItemName;
            }
            else
                return "";

        }
        //
        // GET: /YogaUserDetail/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /YogaUserDetail/Create

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
        // GET: /YogaUserDetail/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /YogaUserDetail/Edit/5

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
        // GET: /YogaUserDetail/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /YogaUserDetail/Delete/5

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
        /// 添加留言
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult AddMessageInfo(int totype)
        {
            try
            {
                // TODO: Add delete logic here

                ViewtMessage model = new ViewtMessage();
                int ToUid = 0;

                string strContent = Request.Form["sContent"].ToString();
                int Toid = Convert.ToInt32(Request.Form["hidid"]);
                int FromUid = user.Uid;
                if (Request.Form["hidType"].ToString() == "1")
                {
                    //hidType=1 表示习练者/导师 ToUid=Toid ;否则不同
                    ToUid = Toid;
                }
                else
                {
                    // totype  =  0：留言；1：推荐； //--  2 学习互动留言；3 活动留言 ；4：日志留言；5 会馆留言
                    if (totype == 1 || totype == 0)
                    {
                        ToUid = Toid;
                    }
                    else if (totype == 4)
                    {
                        ViewtWriteLog logentity = logClient.GetById(Toid);
                        if (logentity != null)
                        {
                            ToUid = logentity.Uid.Value;
                        }
                    }
                    else if (totype == 3)
                    {
                        ViewClass classentity = classclient.Get(Toid);
                        if (classentity != null)
                        {
                            ToUid = classentity.UserId;
                        }
                    }
                    else if (totype == 2)
                    {
                        ViewtLearing learnentity = learnclient.GetById(Toid);
                        if (learnentity != null)
                        {
                            ToUid = Convert.ToInt32(learnentity.Uid); ;
                        }
                    }
                    else if (totype == 5)
                    {
                        ViewCenters cenentity = cenclient.GetById(Toid);
                        if (cenentity != null)
                        {
                            if (!string.IsNullOrEmpty(cenentity.Uid))
                            {
                                ToUid = Convert.ToInt32(cenentity.Uid);
                            }
                            else
                            {
                                ToUid = int.Parse(ConfigurationManager.AppSettings["BACK_POSTER"]);
                            }
                        }

                    }

                }
                model = clientMsg.GettMessageDistinct(Toid, strContent, FromUid);
                if (model == null)
                {
                    model = new ViewtMessage();
                    //相册
                    model.photo = Request.Form["Diploma"];
                    model.ToType = totype;
                    model.sContent = strContent;
                    model.ToUid = ToUid;
                    model.Toid = Toid;
                    model.FromUid = FromUid;
                    model.CreateDate = DateTime.Now;
                    model.ParentID = 0;
                    model.FormType = user.UserType;//0 , 1
                    model.loginType = 0;//默认值 与站内信表中的字段loginType一样
                    clientMsg.Add(model);

                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 2 });//重复
                }
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 回复发表
        /// </summary>
        /// <returns></returns>
        public JsonResult AddFaBiaoInfo(ViewtMessage model)
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
                int totype = Convert.ToInt32(Request.Form["totype"]);
                if (!string.IsNullOrEmpty(Request.Form["parentID"]))
                {
                    ParentID = Convert.ToInt32(Request.Form["parentID"]);
                }
                model = clientMsg.GettMessageDistinct(ToUid, sContent, user.Uid, ParentID);
                if (model != null)
                {
                    return Json(new { code = 2 });
                }
                else
                {
                    model = new ViewtMessage();
                    model.ToUid = ToUid;
                    model.sContent = sContent;
                    model.ParentID = ParentID;
                    model.FromUid = user.Uid;
                    model.iZan = 0;
                    model.ToType = totype;
                    model.CreateDate = DateTime.Now;
                    clientMsg.Add(model);

                    return Json(new { code = 0 });
                }

            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 回复删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult delFaBiao(int id)
        {
            try
            {
                // TODO: Add delete logic here
                clientMsg.Delete(id.ToString());
                return Json(new { code = 0 });

            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 级别认证
        /// </summary>
        /// <returns></returns>
        public ActionResult LevelAuthentication()
        {

            ViewYogaUserDetail model = userDetclient.GetYogaUserDetailById(user.Uid);
            return View(model);
        }
        [HttpPost]
        public string LevelAuthentication1(ViewYogaUserDetail model)
        {
            try
            {
                // TODO: Add delete logic here

                #region 注释
                //级别分为：初级练习者、中级习练者、高级习练者：
                //选项的分值：
                //A: 1  B:2   C:3  D: 4
                //初级： 4-8分（不含8分）
                //中级：8-12分（含12分）
                //高级：13-16分 
                #endregion

                int iIsUserWeeknumber = 0;
                if (model.IsUserWeeknumber != null)
                {
                    iIsUserWeeknumber = Convert.ToInt32(model.IsUserWeeknumber);
                }
                int isContent = 0;
                if (model.sContent != null)
                {
                    isContent = Convert.ToInt32(model.sContent);
                }
                int iknowledge = 0;
                if (model.knowledge != null)
                {
                    iknowledge = Convert.ToInt32(model.knowledge);
                }
                int StartYear = 0;
                if (model.StartYear != null)
                {
                    StartYear = model.StartYear.Value;
                }
                int Nums = iIsUserWeeknumber + isContent + iknowledge + StartYear;

                ViewYogaUserDetail entity = userDetclient.GetById(model.ID);
                entity.IsUserWeeknumber = iIsUserWeeknumber.ToString();
                entity.sContent = isContent.ToString();
                entity.knowledge = iknowledge.ToString();
                entity.StartYear = StartYear;
                string level = "0";
                if (Nums <= 4)
                {
                    entity.Ulevel = 0;
                    userDetclient.Update(entity);
                }
                if (Nums > 4 && Nums < 8)
                {
                    entity.Ulevel = 1;
                    userDetclient.Update(entity);
                }
                else if (Nums > 8 && Nums <= 12)
                {
                    entity.Ulevel = 2;
                    userDetclient.Update(entity);
                }
                else if (Nums > 13 && Nums <= 16)
                {
                    entity.Ulevel = 3;
                    userDetclient.Update(entity);
                }
                level = entity.Ulevel.ToString();

                //添加升级订单表
                AddLevelOrder(Nums);

                return level;

            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        /// <summary>
        /// 习练者到习练者  添加级别订单
        /// </summary>
        private void AddLevelOrder(int score)
        {
            DateTime now = DateTime.Now;
            using (LevelOrderServiceClient loClient = new LevelOrderServiceClient())
            {
                ViewLevelOrder viewLO = new ViewLevelOrder();
                viewLO.LevelOrderID = now.ToString("yyMMddHHmmssfff");
                viewLO.UID = user.Uid;
                viewLO.Name = user.NickName;
                viewLO.OrderType = LevelOrderType.习练者到习练者.ToString();
                viewLO.OrderState = LevelOrderState.通过.ToString();
                viewLO.OrderScore = score.ToString();
                viewLO.OrderDel = 0;
                viewLO.OriginalLevel = CommonInfo.GetCurrentLevel(user);//当前用户level
                viewLO.TargetLevel = CommonInfo.GetLevelbyScore(score, 0);
                viewLO.CreateTime = viewLO.UpdateTime = now;
                loClient.Add(viewLO);
            }

        }

        /// <summary>
        /// 隐私设置
        /// </summary>
        /// <returns></returns>
        public ActionResult PrivacySettings()
        {

            ViewYogaUserDetail model = userDetclient.GetYogaUserDetailById(user.Uid);
            return View(model);
        }
        [HttpPost]
        public ActionResult PrivacySettings(ViewYogaUserDetail model)
        {
            try
            {
                // TODO: Add delete logic here
                model = userDetclient.GetById(model.ID);

                #region 是否公开

                if (!string.IsNullOrEmpty(Request.Form["RealName_cn"]))
                {
                    model.RealName_cn = Request.Form["RealName_cn"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["RealName_en"]))
                {
                    model.RealName_en = Request.Form["RealName_en"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["BirthYear"]))
                {
                    model.BirthYear = Convert.ToInt32(Request.Form["BirthYear"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["BirthMonth"]))
                {
                    model.BirthMonth = Convert.ToInt32(Request.Form["BirthMonth"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["BirthDay"]))
                {
                    model.BirthDay = Convert.ToInt32(Request.Form["BirthDay"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["MSN"]))
                {
                    model.MSN = Request.Form["MSN"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["QQ"]))
                {
                    model.QQ = Request.Form["QQ"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["Postal"]))
                {
                    model.Postal = Request.Form["Postal"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["IsPostal"]))
                {
                    model.IsPostal = Convert.ToInt32(Request.Form["IsPostal"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["Zip"]))
                {
                    model.Zip = Request.Form["Zip"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["Tel"]))
                {
                    model.Tel = Request.Form["Tel"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["Mobile"]))
                {
                    model.Mobile = Request.Form["Mobile"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["Weibo"]))
                {
                    model.Weibo = Request.Form["Weibo"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["Profile"]))
                {
                    model.Profile = Request.Form["Profile"].ToString();
                }

                if (!string.IsNullOrEmpty(Request.Form["PersonalURL"]))
                {
                    model.PersonalURL = Request.Form["PersonalURL"].ToString();
                } if (!string.IsNullOrEmpty(Request.Form["StartYear"]))
                {
                    model.StartYear = Convert.ToInt32(Request.Form["StartYear"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["IsUserYogaType"]))
                {
                    model.IsUserYogaType = Request.Form["IsUserYogaType"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["IsUserYogaPlace"]))
                {
                    model.IsUserYogaPlace = Request.Form["IsUserYogaPlace"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Form["IsYogisMap"]))
                {
                    model.IsYogisMap = true;
                }
                else model.IsYogisMap = false;

                if (Request.Form["IsRealName_cn"] != null) model.IsRealName_cn = 1;
                else model.IsRealName_cn = 0;
                if (Request.Form["IsRealName_en"] != null) model.IsRealName_en = 1;
                else model.IsRealName_en = 0;
                if (Request.Form["IsBirthYear"] != null) model.IsBirthYear = 1;
                else model.IsBirthYear = 0;
                if (Request.Form["IsBirthMonth"] != null) model.IsBirthMonth = 1;
                else model.IsBirthMonth = 0;
                if (Request.Form["IsBirthDay"] != null) model.IsBirthDay = 1;
                else model.IsBirthDay = 0;
                if (Request.Form["IsNationality"] != null) model.IsNationality = 1;
                else model.IsNationality = 0;
                if (Request.Form["IsMsn"] != null) model.IsMsn = 1;
                else model.IsMsn = 0;
                if (Request.Form["IsQQ"] != null) model.IsQQ = 1;
                else model.IsQQ = 0;
                if (Request.Form["IsPostal"] != null) model.IsPostal = 1;
                else model.IsPostal = 0;
                if (Request.Form["IsZip"] != null) model.IsZip = 1;
                else model.IsZip = 0;
                if (Request.Form["IsTel"] != null) model.IsTel = 1;
                else model.IsQQ = 0;
                if (Request.Form["IsMobile"] != null) model.IsMobile = 1;
                else model.IsMobile = 0;
                if (Request.Form["IsWeibo"] != null) model.IsWeibo = 1;
                else model.IsWeibo = 0;
                if (Request.Form["IsPersonalURL"] != null) model.IsPersonalURL = 1;
                else model.IsPersonalURL = 0;

                #endregion
                userDetclient.Update(model);
                //return RedirectToAction("PrivacySettings");
                return Content("ture");

            }
            catch
            {
                return View();
            }
        }


        public ActionResult UserDetailEdit(int id)
        {
            ViewBag.id = user.Uid;
            int iUserid = user.Uid;

            ViewYogaUserDetail model = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                model = client.GetYogaUserDetailById(iUserid);
                if (!string.IsNullOrEmpty(model.YogaTypeid))
                {
                    string[] YogaTypeidlist = model.YogaTypeid.Split(',');
                    #region 流派

                    List<ViewYogaDicItem> listcenter2 = new List<ViewYogaDicItem>();
                    using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                    {
                        listcenter2 = YogaDicItemServiceClient.GetYogaDicItemList();
                        string strYogaTypeidValue = "";
                        foreach (var j in YogaTypeidlist)
                        {
                            foreach (var itemDic in listcenter2)
                            {
                                if (j.ToString() == itemDic.ID.ToString())
                                {
                                    strYogaTypeidValue += itemDic.ItemName + ',';
                                }
                            }
                        }
                        ViewBag.YogaTypeidValue = strYogaTypeidValue;
                    }
                    #endregion
                }
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult UserDetailEdit(ViewYogaUserDetail model)
        {
            ViewYogaUser muser = new ViewYogaUser();
            //呢称修改
            if (!string.IsNullOrEmpty(Request.Form["nickName"]))
            {
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    muser = client.GetYogaUserById(model.UID);

                    muser.NickName = Request.Form["nickName"].ToString();

                    HttpCookie cookie = Request.Cookies["iyoga"];
                    cookie["NickName"] = AES.Encode(Request.Form["nickName"].ToString(), "iyoga");
                    user.NickName = AES.Decode(cookie["NickName"], "iyoga");
                    Commons.Helper.Login.CreateLoginInfo(user, true);
                    client.Update(muser);
                }
            }
            ViewYogaUserDetail mm = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                mm = client.GetById(Convert.ToInt32(Request.Form["ID"].ToString()));
            }
            mm.CreateTime = mm.CreateTime;
            mm.UpgradeDate = DateTime.Now;
            mm.Uscore = 0;
            mm.RealName_cn = model.RealName_cn;
            mm.RealName_en = model.RealName_en;
            mm.Gender = model.Gender;
            mm.GudWords = model.GudWords;
            mm.Street = model.Street;
            mm.Covimg = model.Covimg;
            #region 是否公开

            mm.Nationality = Convert.ToInt32(Request.Form["ddlNationality"].ToString());
            mm.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
            mm.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
            mm.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
            mm.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
            mm.LocationID = 0;
            mm.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');

            #endregion

            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
            {
                clientModels.Update(mm);
            }
            return Json(new { code = 0 });

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

        /// <summary>
        /// 习练者个人设置页面
        /// </summary>
        /// <param name="iType">传值判断显示</param>
        /// <returns></returns>
        public ActionResult PersonalSetUp(string iType)
        {
            //start判断是否是审核中
            LevelOrderServiceClient client = new LevelOrderServiceClient();
            ViewLevelOrder model = new ViewLevelOrder();
            model = client.GetUid(user.Uid);
            if (model != null)
            {
                if (model.OrderState == "申请中")
                    ViewBag.OrderState = "1";
                else
                    ViewBag.OrderState = "0";
            }
            else
            {
                ViewBag.OrderState = "0";
            }
            //end
            ViewBag.iType = iType;
            #region 习练者
            ViewBag.UserType = 0;
            ViewBag.id = user.Uid;
            int id = user.Uid;//登录用户ID 
            ViewYogaUserDetail temp = new ViewYogaUserDetail();
            temp = userDetclient.GetYogaUserDetailById(id);
            #region 习练者基本信息
            ViewBag.strUid = temp.UID;
            ///位置
            string strCountryID = "";
            string strProvinceID = "";
            string strCityID = "";
            string strDistrictID = "";
            if (temp.CountryID != null && temp.CountryID != 0)
            {
                strCountryID = method.GetItemName(temp.CountryID.Value) + "·";
            }
            if (temp.ProvinceID != null && temp.ProvinceID != 0)
            {
                strProvinceID = method.GetItemName(temp.ProvinceID.Value) + "·";
            }
            if (temp.CityID != null && temp.CityID != 0)
            {
                strCityID = method.GetItemName(temp.CityID.Value) + "·";
            }
            if (temp.DistrictID != null && temp.DistrictID != 0)
            {
                strDistrictID = method.GetItemName(temp.DistrictID.Value);
            }

            ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
            ///流派
            if (!string.IsNullOrEmpty(temp.YogaTypeid))
            {
                string[] ids = temp.YogaTypeid.Split(',');
                foreach (var i in ids)
                {
                    ViewBag.YogaTypeid += method.GetItemName(Convert.ToInt32(i)) + " ";
                }
            }
            else ViewBag.YogaTypeid = "";
            #endregion

            ViewYogaUser userEntity = new ViewYogaUser();
            userEntity = clientUser.GetYogaUserById(temp.UID);
            ViewBag.NickName = userEntity.NickName;//昵称

            //关注 粉丝 
            ViewFollow viewMoel = new ViewFollow();
            ViewBag.iCount = clientfollow.GetFollowByUid(id);
            ViewBag.FCount = clientfollow.GetFollowByCount(id);

            #region 我的问题

            tQuestionServiceClient questionClient = new tQuestionServiceClient();
            string whereStr = "";
            int count = 0;
            whereStr += "IsDelete!" + false + ",";
            whereStr += "Uid!" + user.Uid + ",";
            List<ViewtQuestion> myQuestionList = questionClient.GetList(whereStr, 1, 150, out count).OrderByDescending(p => p.QuestionTime).ToList();
            ViewBag.myQuestionList = myQuestionList;
            #endregion


            return View(temp);
            #endregion


        }

        public ActionResult PractitionersUpgrade(string msg)
        {
            if (msg == "0")
            {
                ViewBag.msg = "基础习练者";
            }
            else if (msg == "1")
            {
                ViewBag.msg = "初级习练者";
            }
            else if (msg == "2")
            {
                ViewBag.msg = "中级习练者";
            }
            else if (msg == "3")
            {
                ViewBag.msg = "高级习练者";
            }
            ViewLevelOrder entity = levelorderclient.GetUid(user.Uid);
            int iInfo = 0;
            switch (entity.TargetLevel)
            {
                #region 习练者升级
                case "初级习练者":
                    iInfo = 1;

                    DataTable listuser1 = userDetclient.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser1;
                    break;
                case "中级习练者":
                    iInfo = 2;

                    DataTable listuser2 = userDetclient.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser2;
                    break;
                case "高级习练者":
                    iInfo = 3;

                    DataTable listuser3 = userDetclient.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser3;
                    break;
                #endregion

            }

            #region 同级别导师

            DataTable listMs = client.GetSamelevelSupervisor(1);
            ViewBag.listMs = listMs;

            #endregion

            return View();
        }

        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalHelpCenters()
        {

            return View();
        }
    }
}
