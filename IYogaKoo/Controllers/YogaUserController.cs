using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zzfIBM.WebControls.Mvc;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using Commons.Helper;

namespace IYogaKoo.Controllers
{
    public class YogaUserController : Controller
    {
        //
        // GET: /YogaUser/
        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser;
        FollowServiceClient clientFoll;
        YogaUserDetailServiceClient clientDetail;
        method method;
        public YogaUserController()
        {
            ViewBag.user = user;
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();
            clientFoll = new FollowServiceClient();
            clientDetail = new YogaUserDetailServiceClient();
            method = new method();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = clientDetail.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                {
                    ViewBag.level = temp.Ulevel;
                    ViewBag.Gender = temp.Gender;
                }
            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = client.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                {
                    ViewBag.level = vyogism.YogisLevel;
                    ViewBag.Gender = vyogism.Gender;
                }
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
        public ActionResult Index(int page = 1)
        {
            List<ViewYogaUser> list = new List<ViewYogaUser>();

            int count = 0;

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                list = client.GetogaUserGroupList(page, 10, out count);
            }

            PagedList<ViewYogaUser> pagelist = new PagedList<ViewYogaUser>(list, page, 10, count);

            List<ViewUserModelsGroup> listGroup = new List<ViewUserModelsGroup>();

            foreach (var item in list)
            {
                ViewUserModelsGroup model = new ViewUserModelsGroup();

                model.VyList = item;

                using (YogisModelsServiceClient uclient = new YogisModelsServiceClient())
                {
                    model.VmList = uclient.GetById((int)item.Uid);
                }

                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return View(pagelist);

        }

        /// <summary>
        ///关注列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FollowList(string typeid, int page = 1)
        {
            #region 关注列表
            int pagesize = 12;
            int count = 0;
            List<ViewFollow> listFollow = new List<ViewFollow>();

            ViewBag.dcount = clientFoll.GetFollowQuiltUidList(user.Uid).Where(a => a.iType == 1).Count();
            ViewBag.xcount = clientFoll.GetFollowQuiltUidList(user.Uid).Where(a => a.iType == 0).Count();

            ViewBag.fscount = clientFoll.GetFollowByQuiltUid(user.Uid).Count();
            ViewBag.dcount2 = clientFoll.GetFollowByQuiltUid(user.Uid).Where(a => a.QuiltCenterID == 1).Count();
            ViewBag.xcount2 = clientFoll.GetFollowByQuiltUid(user.Uid).Where(a => a.QuiltCenterID == 0).Count();


            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();
            string strRec = "2";
            if (Session["Backtypeid"] == null)
            {
                if (!string.IsNullOrEmpty(Request.Form["typeid"]))
                {
                    strRec = Request.Form["typeid"];
                }
                if (!string.IsNullOrEmpty(typeid))
                {
                    strRec = typeid;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Form["typeid"]))
                {
                    strRec = Session["Backtypeid"].ToString();
                }
                else
                {
                    strRec = Request.Form["typeid"];
                }
                if (string.IsNullOrEmpty(typeid))
                {
                    strRec = Session["Backtypeid"].ToString();
                }
                else
                {
                    strRec = typeid;
                }
            }
            ViewBag.typeid = strRec;


            switch (strRec)
            {
                case "0":
                    #region 习练者关注

                    listFollow = clientFoll.GetFollowUidList(0, user.Uid, page, pagesize, out count);
                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        //习练者
                        ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.QuiltUid);
                        model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                        model.userurl = "/YogaUserDetail/Details/";
                        model.uid = udmodel.UID;
                        model.gender = udmodel.Gender.ToString();
                        model.Asign = udmodel.GudWords;
                        model.flag = 0;
                        //登录表                   
                        model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
                        model.Leval = udmodel.Ulevel;
                        list2Group.Add(model);
                    }

                    #endregion

                    break;
                case "1":
                    #region 导师关注
                    listFollow = clientFoll.GetFollowUidList(1, user.Uid, page, pagesize, out count);
                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        //导师
                        
                        ViewYogisModels mmodel = client.GetYogisModelsById(item.QuiltUid);
                        if (mmodel != null)
                        {
                            model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                            model.gender = mmodel.Gender.ToString();
                            model.Asign = mmodel.GudWords;
                            model.flag = 1;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
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
                            model.Leval = mmodel.YogisLevel == null ? 0 : mmodel.YogisLevel.Value;
                            list2Group.Add(model);
                        }
                    }

                    #endregion

                    break;
                case "2":
                    #region 全部关注
                    listFollow = clientFoll.GetFollowUidList(user.Uid, page, pagesize, out count);

                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser usermodel = clientUser.GetYogaUserById(item.QuiltUid);
                        if (usermodel.UserType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.QuiltUid);
                            if (udmodel != null)
                            {
                                model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                                model.userurl = "/YogaUserDetail/Details/";
                                model.uid = udmodel.UID;
                                model.gender = udmodel.Gender.ToString();
                                model.Asign = udmodel.GudWords;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
                                model.Leval = udmodel.Ulevel;
                                model.flag = 0;
                                list2Group.Add(model);
                            }
                        }
                        else if (usermodel.UserType == 1)
                        {
                            //导师
                            ViewYogisModels mmodel = client.GetYogisModelsById(item.QuiltUid);
                            if (mmodel != null)
                            {
                                model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                                model.gender = mmodel.Gender.ToString();
                                model.Asign = mmodel.GudWords;
                                model.flag = 1;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
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
                                model.Leval = mmodel.YogisLevel == null ? 0 : mmodel.YogisLevel.Value;
                                list2Group.Add(model);
                            }
                        }
                    }

                    #endregion
                    break;
                case "4":
                    #region 粉丝
                    listFollow = clientFoll.GetFollowQuiltUidList(user.Uid, page, pagesize, out count);
                    ViewBag.fscount = count;

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, user.Uid);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel.UserType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                            model.userurl = "/YogaUserDetail/Details/";
                            model.uid = udmodel.UID;
                            model.gender = udmodel.Gender.ToString();
                            model.Asign = udmodel.GudWords;
                            model.Leval = udmodel.Ulevel;
                            model.flag = 0;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (udmodel.CountryID != null && udmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                            }
                            if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                            }
                            if (udmodel.CityID != null && udmodel.CityID != 0)
                            {
                                strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                            }
                            if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(udmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                            list2Group.Add(model);
                        }
                        else if (umodel.UserType == 1)
                        {
                            //导师
                            ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                            model.gender = mmodel.Gender.ToString();
                            model.Profile = mmodel.YogisDepict;
                            model.Leval = mmodel.YogisLevel.Value;
                            model.flag = 1;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (mmodel.CountryID != null && mmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                            }
                            if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                            }
                            if (mmodel.CityID != null && mmodel.CityID != 0)
                            {
                                strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                            }
                            if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(mmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                            list2Group.Add(model);
                        }
                    }
                    #endregion
                    break;
                case "5":
                    #region 导师粉丝
                    listFollow = clientFoll.GetFollowQuiltUidList(1, user.Uid, page, pagesize, out count);
                    ViewBag.fscount = count;

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, user.Uid);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel.UserType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                            model.userurl = "/YogaUserDetail/Details/";
                            model.uid = udmodel.UID;
                            model.gender = udmodel.Gender.ToString();
                            model.Asign = udmodel.GudWords;
                            model.Leval = udmodel.Ulevel;
                            model.flag = 0;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (udmodel.CountryID != null && udmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                            }
                            if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                            }
                            if (udmodel.CityID != null && udmodel.CityID != 0)
                            {
                                strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                            }
                            if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(udmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                            list2Group.Add(model);
                        }
                        else if (umodel.UserType == 1)
                        {
                            //导师
                            ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                            model.gender = mmodel.Gender.ToString();
                            model.Profile = mmodel.YogisDepict;
                            model.Leval = mmodel.YogisLevel.Value;
                            model.flag = 1;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (mmodel.CountryID != null && mmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                            }
                            if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                            }
                            if (mmodel.CityID != null && mmodel.CityID != 0)
                            {
                                strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                            }
                            if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(mmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                            list2Group.Add(model);
                        }
                    }
                    #endregion
                    break;
                case "6":
                    #region 习练者粉丝

                    listFollow = clientFoll.GetFollowQuiltUidList(0, user.Uid, page, pagesize, out count);
                    ViewBag.fscount = count;

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, user.Uid);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel.UserType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                            model.userurl = "/YogaUserDetail/Details/";
                            model.uid = udmodel.UID;
                            model.gender = udmodel.Gender.ToString();
                            model.Asign = udmodel.GudWords;
                            model.Leval = udmodel.Ulevel;
                            model.flag = 0;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (udmodel.CountryID != null && udmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                            }
                            if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                            }
                            if (udmodel.CityID != null && udmodel.CityID != 0)
                            {
                                strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                            }
                            if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(udmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                            list2Group.Add(model);
                        }
                        else if (umodel.UserType == 1)
                        {
                            //导师
                            ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                            model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                            model.gender = mmodel.Gender.ToString();
                            model.Profile = mmodel.YogisDepict;
                            model.Leval = mmodel.YogisLevel.Value;
                            model.flag = 1;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                            //粉丝 ,关注                
                            using (FollowServiceClient followClient = new FollowServiceClient())
                            {
                                model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                model.FollowCount = followClient.GetFollowByUid(item.Uid);
                            }
                            ///位置
                            string strCountryID = "";
                            string strProvinceID = "";
                            string strCityID = "";
                            string strDistrictID = "";
                            if (mmodel.CountryID != null && mmodel.CountryID != 0)
                            {
                                strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                            }
                            if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                            {
                                strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                            }
                            if (mmodel.CityID != null && mmodel.CityID != 0)
                            {
                                strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                            }
                            if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                            {
                                strDistrictID = GetItemName(mmodel.DistrictID.Value);
                            }
                            model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                            list2Group.Add(model);
                        }
                    }
                    #endregion
                    break;
            }
            Session["Backtypeid"] = strRec; //分页用到
            Webdiyer.WebControls.Mvc.PagedList<ViewFollowUserDetail> followlist = new Webdiyer.WebControls.Mvc.PagedList<ViewFollowUserDetail>(list2Group, page, pagesize, count);
            if (Request.IsAjaxRequest())
                return PartialView("PartialFollow", followlist);
            //else
            //{
            //    Session["itype"] = null;
            //}
            #endregion
            return View(followlist);
        }
        /// <summary>
        /// 他人关注列表
        /// </summary>
        /// <param name="id">他人UID</param>
        /// <param name="page">typeid 2 全部； 1 导师 ；0 习练者；4 粉丝</param>
        /// <returns></returns>
        public ActionResult otherFollowList(int id, string typeid, int page = 1)
        {
            ViewBag.otherGender = method.Gender(id);
            ViewBag.id = id;
            int usertype = clientUser.GetYogaUserById(id).UserType.Value;
            if (usertype == 1)
            {
                ViewYogisModels tempm = new ViewYogisModels();
                tempm = client.GetYogisModelsById(id);
                ViewBag.YogisLevel = tempm.YogisLevel;//是否 大师=4
            }
            #region 他人关注列表

            int pagesize = 12;
            int count = 0;

            List<ViewFollow> listFollow = new List<ViewFollow>();
            ///导师量
            ViewBag.dcount = clientFoll.GetFollowQuiltUidList(id).Where(a => a.iType == 1).Count();
            ///习练者量
            ViewBag.xcount = clientFoll.GetFollowQuiltUidList(id).Where(a => a.iType == 0).Count();
            ///粉线量
            ViewBag.fscount = clientFoll.GetFollowByCount(id);

            ViewBag.dcount2 = clientFoll.GetFollowByQuiltUid(id).Where(a => a.QuiltCenterID == 1).Count();
            ViewBag.xcount2 = clientFoll.GetFollowByQuiltUid(id).Where(a => a.QuiltCenterID == 0).Count();

            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();
            string strRec = "2";
            if (!string.IsNullOrEmpty(Request.Form["typeid"]))
            {
                strRec = Request.Form["typeid"];
            }
            if (!string.IsNullOrEmpty(typeid))
            {
                strRec = typeid;
            }
            ViewBag.typeid = strRec;
            switch (strRec)
            {
                case "0":
                    #region 习练者关注

                    listFollow = clientFoll.GetFollowUidList(0, id, page, pagesize, out count);
                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        //习练者
                        ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.QuiltUid);
                        if (udmodel != null)
                        {
                            model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                            model.userurl = "/YogaUserDetail/Details/";
                            model.uid = udmodel.UID;
                            model.gender = udmodel.Gender.ToString();
                            model.Asign = udmodel.GudWords;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
                            list2Group.Add(model);
                        }
                    }

                    #endregion
                    break;

                case "1":
                    #region 导师关注
                    listFollow = clientFoll.GetFollowUidList(1, id, page, pagesize, out count);
                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        //导师
                        ViewYogisModels mmodel = client.GetYogisModelsById(item.QuiltUid);
                        if (mmodel != null)
                        {
                            model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                            model.gender = mmodel.Gender.ToString();
                            model.Asign = mmodel.GudWords;
                            //登录表                   
                            model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
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
                            list2Group.Add(model);
                        }
                    }
                    #endregion
                    break;

                case "2":
                    //全部关注
                    listFollow = clientFoll.GetFollowUidList(id, page, pagesize, out count);
                    #region 全部关注列表foreace
                    foreach (var item in listFollow)
                    {
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        if (item.iType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.QuiltUid);
                            if (udmodel != null)
                            {
                                model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                                model.userurl = "/YogaUserDetail/Details/";
                                model.uid = udmodel.UID;
                                model.gender = udmodel.Gender.ToString();
                                model.Asign = udmodel.GudWords;
                                model.Leval = udmodel.Ulevel;
                                model.flag = 0;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
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
                                model.gender = mmodel.Gender.ToString();
                                model.Asign = mmodel.GudWords;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.QuiltUid).NickName;
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
                                model.Leval = mmodel.YogisLevel.Value;
                                model.flag = 1;
                                model.uid = mmodel.UID;
                                list2Group.Add(model);
                            }
                        }
                    }
                    #endregion

                    break;

                case "4":
                    #region 粉丝
                    listFollow = clientFoll.GetFollowQuiltUidList(id, page, pagesize, out count);

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, id);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel != null)
                        {
                            if (umodel.UserType == 0)
                            {
                                #region 习练者
                                ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                                model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                                model.userurl = "/YogaUserDetail/Details/";
                                model.uid = udmodel.UID;
                                model.gender = udmodel.Gender.ToString();
                                model.Asign = udmodel.GudWords;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                //粉丝 ,关注                
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                    model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                }
                                ///位置
                                string strCountryID = "";
                                string strProvinceID = "";
                                string strCityID = "";
                                string strDistrictID = "";
                                if (udmodel.CountryID != null && udmodel.CountryID != 0)
                                {
                                    strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                                }
                                if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                                {
                                    strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                                }
                                if (udmodel.CityID != null && udmodel.CityID != 0)
                                {
                                    strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                                }
                                if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                                {
                                    strDistrictID = GetItemName(udmodel.DistrictID.Value);
                                }
                                model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                                list2Group.Add(model);

                                #endregion
                            }
                            else if (umodel.UserType == 1)
                            {
                                #region

                                //导师
                                ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                                model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                                model.gender = mmodel.Gender.ToString();
                                model.Profile = mmodel.YogisDepict;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                //粉丝 ,关注                
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                    model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                }
                                ///位置
                                string strCountryID = "";
                                string strProvinceID = "";
                                string strCityID = "";
                                string strDistrictID = "";
                                if (mmodel.CountryID != null && mmodel.CountryID != 0)
                                {
                                    strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                                }
                                if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                                {
                                    strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                                }
                                if (mmodel.CityID != null && mmodel.CityID != 0)
                                {
                                    strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                                }
                                if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                                {
                                    strDistrictID = GetItemName(mmodel.DistrictID.Value);
                                }
                                model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                                list2Group.Add(model);

                                #endregion
                            }
                        }

                    }

                    #endregion
                    break;
                case "5":
                    #region 导师粉丝
                    listFollow = clientFoll.GetFollowQuiltUidList(1, id, page, pagesize, out count);
                    ViewBag.fscount = count;

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, user.Uid);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel != null)
                        {
                            if (umodel.UserType == 0)
                            {
                                #region 习练者
                                ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                                if (udmodel != null)
                                {
                                    model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                                    model.userurl = "/YogaUserDetail/Details/";
                                    model.uid = udmodel.UID;
                                    model.gender = udmodel.Gender.ToString();
                                    model.Asign = udmodel.GudWords;
                                    model.Leval = udmodel.Ulevel;
                                    model.flag = 0;
                                    //登录表                   
                                    model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                    //粉丝 ,关注                
                                    using (FollowServiceClient followClient = new FollowServiceClient())
                                    {
                                        model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                        model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                    }
                                    ///位置
                                    string strCountryID = "";
                                    string strProvinceID = "";
                                    string strCityID = "";
                                    string strDistrictID = "";
                                    if (udmodel.CountryID != null && udmodel.CountryID != 0)
                                    {
                                        strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                                    }
                                    if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                                    {
                                        strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                                    }
                                    if (udmodel.CityID != null && udmodel.CityID != 0)
                                    {
                                        strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                                    }
                                    if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                                    {
                                        strDistrictID = GetItemName(udmodel.DistrictID.Value);
                                    }
                                    model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                                    list2Group.Add(model);
                                }
                                #endregion
                            }
                            else if (umodel.UserType == 1)
                            {
                                #region 导师
                                ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                                if (mmodel != null)
                                {
                                    model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                                    model.gender = mmodel.Gender.ToString();
                                    model.Profile = mmodel.YogisDepict;
                                    model.Leval = mmodel.YogisLevel.Value;
                                    model.flag = 1;
                                    //登录表                   
                                    model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                    //粉丝 ,关注                
                                    using (FollowServiceClient followClient = new FollowServiceClient())
                                    {
                                        model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                        model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                    }
                                    ///位置
                                    string strCountryID = "";
                                    string strProvinceID = "";
                                    string strCityID = "";
                                    string strDistrictID = "";
                                    if (mmodel.CountryID != null && mmodel.CountryID != 0)
                                    {
                                        strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                                    }
                                    if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                                    {
                                        strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                                    }
                                    if (mmodel.CityID != null && mmodel.CityID != 0)
                                    {
                                        strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                                    }
                                    if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                                    {
                                        strDistrictID = GetItemName(mmodel.DistrictID.Value);
                                    }
                                    model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                                    list2Group.Add(model);
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    break;
                case "6":
                    #region 习练者粉丝
                    listFollow = clientFoll.GetFollowQuiltUidList(0, id, page, pagesize, out count);
                    ViewBag.fscount = count;

                    foreach (var item in listFollow)
                    {
                        ViewFollow vm = clientFoll.GetFollowById(item.Uid, user.Uid);
                        ViewBag.isfollow = vm == null ? false : vm.isfollow;
                        ViewFollowUserDetail model = new ViewFollowUserDetail();
                        ViewYogaUser umodel = clientUser.GetYogaUserById(item.Uid);
                        if (umodel.UserType == 0)
                        {
                            //习练者
                            ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.Uid);
                            if (udmodel != null)
                            {
                                model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                                model.userurl = "/YogaUserDetail/Details/";
                                model.uid = udmodel.UID;
                                model.gender = udmodel.Gender.ToString();
                                model.Asign = udmodel.GudWords;
                                model.Leval = udmodel.Ulevel;
                                model.flag = 0;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                //粉丝 ,关注                
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                    model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                }
                                ///位置
                                string strCountryID = "";
                                string strProvinceID = "";
                                string strCityID = "";
                                string strDistrictID = "";
                                if (udmodel.CountryID != null && udmodel.CountryID != 0)
                                {
                                    strCountryID = GetItemName(udmodel.CountryID.Value) + "   ";
                                }
                                if (udmodel.ProvinceID != null && udmodel.ProvinceID != 0)
                                {
                                    strProvinceID = GetItemName(udmodel.ProvinceID.Value) + "  ";
                                }
                                if (udmodel.CityID != null && udmodel.CityID != 0)
                                {
                                    strCityID = GetItemName(udmodel.CityID.Value) + "  ";
                                }
                                if (udmodel.DistrictID != null && udmodel.DistrictID != 0)
                                {
                                    strDistrictID = GetItemName(udmodel.DistrictID.Value);
                                }
                                model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
                                list2Group.Add(model);
                            }
                        }
                        else if (umodel.UserType == 1)
                        {
                            //导师
                            ViewYogisModels mmodel = client.GetYogisModelsById(item.Uid);
                            if (mmodel != null)
                            {
                                model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                                model.gender = mmodel.Gender.ToString();
                                model.Profile = mmodel.YogisDepict;
                                model.Leval = mmodel.YogisLevel.Value;
                                model.flag = 1;
                                //登录表                   
                                model.nickname = clientUser.GetYogaUserById(item.Uid).NickName;
                                //粉丝 ,关注                
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowersCount = followClient.GetFollowByCount(item.Uid);
                                    model.FollowCount = followClient.GetFollowByUid(item.Uid);
                                }
                                ///位置
                                string strCountryID = "";
                                string strProvinceID = "";
                                string strCityID = "";
                                string strDistrictID = "";
                                if (mmodel.CountryID != null && mmodel.CountryID != 0)
                                {
                                    strCountryID = GetItemName(mmodel.CountryID.Value) + "   ";
                                }
                                if (mmodel.ProvinceID != null && mmodel.ProvinceID != 0)
                                {
                                    strProvinceID = GetItemName(mmodel.ProvinceID.Value) + "  ";
                                }
                                if (mmodel.CityID != null && mmodel.CityID != 0)
                                {
                                    strCityID = GetItemName(mmodel.CityID.Value) + "  ";
                                }
                                if (mmodel.DistrictID != null && mmodel.DistrictID != 0)
                                {
                                    strDistrictID = GetItemName(mmodel.DistrictID.Value);
                                }
                                model.ressname = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                                list2Group.Add(model);
                            }
                        }
                    }
                    #endregion
                    break;
            }


            Webdiyer.WebControls.Mvc.PagedList<ViewFollowUserDetail> followlist = new Webdiyer.WebControls.Mvc.PagedList<ViewFollowUserDetail>(list2Group, page, pagesize, count);
            if (Request.IsAjaxRequest())
                return PartialView("PartialotherFollow", followlist);
            else
            {
                // Session["itypeOther"] = null;
            }
            #endregion
            return View(followlist);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <returns></returns>
        public JsonResult Cancelf()
        {
            try
            {
                int uid = Convert.ToInt32(Request.Form["uid"]);
                int quid = Convert.ToInt32(Request.Form["quid"]);
                ViewFollow model = new ViewFollow();
                model = clientFoll.GetFollowById(uid, quid);
                if (model != null)
                {
                    //update
                    model.FollowDate = DateTime.Now;
                    model.isfollow = false;

                    clientFoll.Update(model);

                }
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 移除粉丝
        /// </summary>
        /// <returns></returns>
        public JsonResult Cancelfs()
        {
            try
            {
                int uid = Convert.ToInt32(Request.Form["uid"]);
                int quid = Convert.ToInt32(Request.Form["quid"]);
                ViewFollow model = new ViewFollow();
                model = clientFoll.GetFollowById(quid, uid);
                if (model != null)
                {
                    //update
                    model.FollowDate = DateTime.Now;
                    model.isfollow = false;

                    clientFoll.Update(model);

                }
                return Json(new { code = 0 });
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

        // GET: /YogaUser/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /YogaUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /YogaUser/Create

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
        // GET: /YogaUser/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /YogaUser/Edit/5

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
        // GET: /YogaUser/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /YogaUser/Delete/5

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
