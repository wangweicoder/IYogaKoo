using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using zzfIBM.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class YogisModelsController : Controller
    {

        //
        // GET: /YogisModels/

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
        CentersServiceClient cenclient;
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
        method method;
        tZanModelsServiceClient zanclient;
        public YogisModelsController()
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
            interclient = new InterestServiceClient();
            orderclient = new OrderServiceClient();
            classclient = new ClassServiceClient();
            method = new method();
            zanclient = new tZanModelsServiceClient();
            cenclient = new CentersServiceClient();
            levelorderclient = new LevelOrderServiceClient();
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

            List<ViewYogisModels> list = new List<ViewYogisModels>();
            int count = 0;
            int pagesize = 12;

            list = client.GetYogisModelsList("", DistrictID, CityID, ProvinceID, CountryID, typeid, level, 2, out count);

            List<ViewUserModelsGroup> users = new List<ViewUserModelsGroup>();
            ViewUserModelsGroup user = new ViewUserModelsGroup();
            ViewYogaDicItem dicitem = null;
            foreach (ViewYogisModels c in list)
            {
                c.DisplayImg = CommonInfo.GetDisplayImg(c.DisplayImg);
                user = new ViewUserModelsGroup();
                user.VmList = c;
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
                                int a = result;
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
                //ViewYogaUser u = clientUser.GetYogaUserById(c.UID);
                //user.VyList = u;

                //粉丝
                user.FollowCount = clientFoll.GetFollowByCount(c.UID);
                users.Add(user);
            }
            users = users.OrderByDescending(x => x.FollowCount).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.DisplayImg)).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.CoverImg)).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.YogisDepict)).Skip((page - 1) * pagesize).Take(pagesize).ToList();

            Webdiyer.WebControls.Mvc.PagedList<ViewUserModelsGroup> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewUserModelsGroup>(users, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexList", pagelist);
            }
            return View(pagelist);

        }

        //
        // GET: /YogisModels/Details/5

        /// <summary>
        ///  瑜伽达人/导师 主页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Details(int id, int page = 1)
        {
            try
            {
                ///Follow iType
                ViewBag.UserType = 1;

                ViewBag.id = id;
                int strUid = user.Uid;
                // int iLoginID = user.Uid;//登录用户ID
                ViewBag.iLoginID = user.Uid;

                #region 导师专页 基本信息

                ViewYogisModels temp = new ViewYogisModels();

                temp = client.GetYogisModelsById(id);

                ViewBag.listGroup = temp;

                ///昵称
                ViewBag.NickName = clientUser.GetYogaUserById(temp.UID).NickName;
                if (user.Uid == id)//自己
                {
                    ViewBag.Gender = 2;
                }
                else
                {
                    ViewBag.Gender = temp.Gender.Value;
                }
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
                #endregion

                #region 流派 / 影响他的导师 / 所在会馆
                if (!string.IsNullOrEmpty(temp.YogaTypeid))
                {
                    string[] ids = temp.YogaTypeid.Split(',');
                    foreach (var i in ids)
                    {
                        ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                    }
                }
                else ViewBag.YogaTypeid = "";
                //影响他的导师 TeachYogis
                List<ViewYogisModels> listModels = new List<ViewYogisModels>();
                if (!string.IsNullOrEmpty(temp.TeachYogis))
                {
                    string[] idsTeachYogis = temp.TeachYogis.Split(',');
                    foreach (var i in idsTeachYogis)
                    {
                        int TeachYogisId = Convert.ToInt32(i);
                        ViewYogisModels viewEntity = client.GetById(TeachYogisId);
                        if (viewEntity != null)
                            listModels.Add(viewEntity);
                    }
                }
                ViewBag.listModels = listModels;
                //所在会馆 CenterID
                List<ViewCenters> listCenters = new List<ViewCenters>();
                if (!string.IsNullOrEmpty(temp.CenterID))
                {
                    string[] idsCenterID = temp.CenterID.Split(',');
                    foreach (var i in idsCenterID)
                    {
                        int CenterIDId = Convert.ToInt32(i);
                        ViewCenters viewCenters = cenclient.GetById(CenterIDId);
                        if (viewCenters != null)
                            listCenters.Add(viewCenters);
                    }
                }
                ViewBag.listCenters = listCenters;
                //end

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

                ViewFollow vm = clientFoll.GetFollowById(user.Uid, strUid);
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

                ViewBag.list2Group = list2Group.Take(8);

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
                int mcount = 0;
                int pagesize = 10;

                ViewBag.Msg = method.listMessage(id, 0, page, out mcount);//留言 评论          



                #region 推荐
                //if (!string.IsNullOrEmpty(Request.QueryString["levelid"]))
                //{
                //    totype = Convert.ToInt32(Request.QueryString["levelid"]);
                //}
                int rcount = 0;
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

                ViewBag.Recinfo = listGroupRec;
                ////推荐的数据            
                //using (tZanModelsServiceClient zclient = new tZanModelsServiceClient())
                //{
                //    List<ViewtZanModels> listz = zclient.GettZanUid(id);
                //   ViewBag.tzancount = listz.Count();
                //}           
                #endregion

                #region 我的日志列表
                int count = 0;
                List<ViewtWriteLog> listwriteLog = logClient.GettWriteLogPageList(id, 1, 7, out count);

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
                //List<ViewClass> classlist = classclient.GetClassesByUid(classIds, id).Take(8).ToList();
                List<ViewClass> classlist = classclient.GetClassesByZhuanYe(id, temp.YID, 0, 1);
                ViewBag.classlist = classlist;

                #endregion

                #region 相似导师

                List<ViewYogisModels> Modelslist = client.GetYogisModelsByYogaTypeid(id, temp.YogaTypeid, 4);
                ViewBag.Modelslist = Modelslist;

                #endregion

                ViewBag.url = Request.Url.AbsolutePath;
                ViewBag.mcount = mcount;
                ViewBag.rcount = rcount;
                Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.Msg, page, pagesize, mcount);
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
            catch (Exception ex)
            {
                Tools.WriteTextLog("导师主页 报错：", ex.Message);
                return View();
            }
        }
        public ActionResult ModelsDetails(int id)
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
            ViewBag.temp = temp;
            try
            {
                ///昵称
                ViewYogaUser youser = clientUser.GetYogaUserById(temp.UID);
                if (youser != null)
                    ViewBag.NickName = youser.NickName;

                strUid = temp.UID;
                ViewBag.strUid = temp.UID;

                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (temp.CountryID != null && temp.CountryID != 0)
                {
                    strCountryID = GetItemName(temp.CountryID.Value) + " ·  ";
                }
                if (temp.ProvinceID != null && temp.ProvinceID != 0)
                {
                    strProvinceID = GetItemName(temp.ProvinceID.Value) + " · ";
                }
                if (temp.CityID != null && temp.CityID != 0)
                {
                    strCityID = GetItemName(temp.CityID.Value) + " · ";
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
            }
            catch { }
            #endregion

            //关注 粉丝 人气
            ViewFollow viewMoel = new ViewFollow();
            using (FollowServiceClient followClient = new FollowServiceClient())
            {
                ViewBag.iCount = followClient.GetFollowByUid(id);
                ViewBag.FCount = followClient.GetFollowByCount(id);
            }
            ViewBag.tzancount = clientMsg.GettMessageUid(id, 1).Count();
            #region 瑜伽圈

            List<ViewFollow> listFollow = new List<ViewFollow>();

            listFollow = clientFoll.GetFollowQuiltUidList(id);

            ViewFollow vm = clientFoll.GetFollowById(iLoginID, strUid);
            ViewBag.isfollow = vm == null ? false : vm.isfollow;

            List<ViewFollowUserDetail> list2Group = new List<ViewFollowUserDetail>();

            foreach (var item in listFollow)
            {
                //登录表                   
                ViewYogaUser umodel = clientUser.GetYogaUserById(item.QuiltUid);

                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = umodel.NickName;
                model.flag = item.iType.Value;
                if (umodel.UserType == 0)
                {
                    //习练者
                    ViewYogaUserDetail udmodel = clientDetail.GetYogaUserDetailById(item.QuiltUid);
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
                else if (umodel.UserType == 1)
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
                        model.FollowCount = followClient.GetFollowByCount(item.Uid);//你的粉丝
                        model.FollowersCount = followClient.GetFollowByCount(item.QuiltUid);//你关注的人的粉丝
                    }
                    model.Leval = mmodel.YogisLevel.Value;
                    list2Group.Add(model);
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

                string result = "";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = jss.Serialize(listPic);

                ViewBag.listPicString = result;

            }

            #endregion


            return View(temp);
        }

        /// <summary>
        /// 赞同
        /// </summary>
        /// <returns></returns>
        public JsonResult Zan()
        {
            try
            {
                int Uid = Convert.ToInt32(Request.Form["uid"]);
                ViewYogisModels model = client.GetYogisModelsById(Uid);
                if (model.iZan == null)
                {
                    model.iZan = 0;
                }
                else
                {
                    model.iZan = model.iZan + 1;
                }
                client.Update(model);
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
        // GET: /YogisModels/Create

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Create(ViewYogisModels model)
        {
            try
            {
                #region
                ViewYogaUser item = new ViewYogaUser();

                item.RegDate = DateTime.Now;
                item.UStatus = 0;
                item.UEmail = "";
                item.Uphone = "";
                item.Pwd = "";
                item.LastDate = DateTime.Now;
                item.LoginTimes = 0;
                item.LoginType = (int)LoginType.普通;
                item.UserType = (int)UserType.瑜伽会员;
                item.UStatus = (int)Ustatus.未激活;
                item.IsAssessor = false;
                item.IsWebworkers = false;

                item.YogisModels = new List<YogisModels>();
                item.WechatAuthCode = "";
                item.WechatBack = "";
                item.SinaAuthCode = "";
                item.SinaBack = "";
                item.QQAuthCode = "";
                item.QQBack = "";
                item.ValCode = "";
                item.ValExpire = DateTime.Now.AddYears(2);

                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    model.UID = client.Return_AddUid(item).Uid;
                }
                #endregion
                model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
                model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
                model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
                model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());

                // model.CenterID = Request.Form["hCenterID"].ToString().TrimEnd(',') == "" ? Request.Form["CenterID"].ToString().TrimEnd(',') : Request.Form["hCenterID"].ToString().TrimEnd(',');
                // model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                model.CreateDate = DateTime.Now;
                model.YogiStatus = 1;//1=普通导师
                model.IsAcceptAgreement = true;
                model.delState = 0;
                //
                using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
                {
                    clientModel.Add(model);
                }

                // TODO: Add insert logic here
                //if (!string.IsNullOrEmpty(Request.Form["Diploma"]))
                //{
                //    //相册
                //    string[] strPic = Request.Form["Diploma"].ToString().Split(';');
                //    string[] strpicContent = Request.Form["PictureContent"].ToString().TrimEnd('|').Split('|');

                //    ViewYogaPicture picModel = new ViewYogaPicture();
                //    for (int i = 0; i < strPic.Length - 1; i++)
                //    {
                //        #region
                //        if (!string.IsNullOrEmpty(strPic[i]))
                //        {
                //            picModel.PictureOriginal = strPic[i];

                //            picModel.Uid = model.UID;

                //            picModel.PictureType = 2;
                //            try
                //            {
                //                picModel.PictureContent = strpicContent[i];
                //            }
                //            catch
                //            {
                //                picModel.PictureContent = "";
                //            }
                //            picModel.CreateTime = DateTime.Now;
                //            picModel.CreateUser = user.Uid;//登录用户ID
                //            picModel.PictureName = "";
                //            picModel.PictureSmall = "";

                //            picModel.AlbumId = 0;
                //            picModel.EvaluateId = 0;
                //            picModel.Comid = 0;
                //            picModel.PictureLarge = "";
                //            picModel.PictureMiddle = "";
                //            picModel.PircureSize = "";
                //            picModel.CommentCount = 0;
                //            picModel.LikeCount = 0;
                //            picModel.NotLikeCount = 0;
                //            picModel.CommentLimite = 0;
                //            picModel.LastChangeTime = DateTime.Now;
                //            picModel.HitNum = 0;

                //            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                //            {
                //                clientpic.Add(picModel);
                //            }
                //        }
                //        #endregion
                //    }

                //}
                // return RedirectToAction("DaoShiIndex");
                ViewClassTeacher entity = new ViewClassTeacher();
                entity.Name = model.RealName;
                entity.Gender = model.Gender == 1 ? "男" : "女";
                entity.Country = Request.Form["ddlCountryText"].ToString();
                entity.Info = "";
                entity.IsDeleted = false;
                entity.CreateTime = DateTime.Now;
                entity.UserId = model.UID;
                //entity.TeacherId = model.YID;
                entity.Avatar = "";
                BasicInfo bi = Commons.Helper.Login.GetCurrentUser();
                if (bi.Uid > 0)
                {
                    ClassTeacherServiceClient client = new ClassTeacherServiceClient();
                    entity.UserId = bi.Uid;
                    entity.Id = client.Add(entity);
                }

                return Json(new { code = 0, Uid = model.UID, ClassTeacherId = entity.Id });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        ///编辑导师
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {

            //ModelsDetails(id);
            string strNickName = "";
            ViewYogaUser list = new ViewYogaUser();

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                if (client.GetYogaUserById(user.Uid) != null)
                {
                    strNickName = client.GetYogaUserById(user.Uid).NickName;
                }
            }
            ViewBag.NiceName = strNickName;
            ViewYogisModels model = new ViewYogisModels();
            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                model = client.GetYogisModelsById(id);

                #region 会馆
                if (!string.IsNullOrEmpty(model.CenterID))
                {
                    string[] cenlist = model.CenterID.Split(',');

                    List<ViewCenters> listcenter = new List<ViewCenters>();
                    using (CentersServiceClient CentersServiceClient = new CentersServiceClient())
                    {
                        listcenter = CentersServiceClient.GetCentersUid();

                        string strCentValue = "";
                        foreach (var i in cenlist)
                        {
                            foreach (var itemCenter in listcenter)
                            {
                                if (i.ToString() == itemCenter.CenterId.ToString())
                                {
                                    strCentValue += itemCenter.CenterName + ',';
                                }
                            }
                        }
                        ViewBag.CentValue = strCentValue;
                    }
                }
                #endregion

                #region 流派
                if (!string.IsNullOrEmpty(model.YogaTypeid))
                {
                    string[] YogaTypeidlist = model.YogaTypeid.Split(',');

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
                }
                #endregion

                #region 导师

                if (!string.IsNullOrEmpty(model.TeachYogis))
                {
                    string[] TeachYogislist = model.TeachYogis.Split(',');
                    ViewYogisModels model3 = new ViewYogisModels();
                    string strTeachYogisValue = "";
                    foreach (var k in TeachYogislist)
                    {
                        model3 = client.GetById(Convert.ToInt32(k));
                        if (model3 != null)
                        {
                            strTeachYogisValue += model3.RealName + ',';
                        }

                    }
                    ViewBag.TeachYogisValue = strTeachYogisValue;
                }

                #endregion

            }
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                List<ViewYogaPicture> pic = clientpic.GetUidList(id);
                if (pic != null)
                {
                    ViewBag.Pic = pic;
                }
            }

            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Edit(ViewYogisModels model)
        {
            try
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
                // TODO: Add insert logic here
                model.StartTeachYear = Convert.ToDateTime(Request.Form["StartTeachYear"].ToString());
                model.Nationality = Request.Form["ddlNationality"].ToString();
                model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
                model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
                model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
                model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());

                model.CenterID = Request.Form["hCenterID"].ToString().TrimEnd(',') == "" ? Request.Form["CenterID"].ToString().TrimEnd(',') : Request.Form["hCenterID"].ToString().TrimEnd(',');
                model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                model.CreateDate = DateTime.Now;
                model.YogiStatus = 1;//1=普通导师
                model.IsAcceptAgreement = true;
                model.delState = 0;
                //
                using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
                {
                    clientModel.Update(model);
                }

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
                            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                            {
                                #region info

                                picModel.PictureOriginal = strPic[i];

                                picModel.Uid = model.UID;

                                picModel.PictureType = 2;
                                try
                                {
                                    picModel.PictureContent = strpicContent[i];
                                }
                                catch
                                {
                                    picModel.PictureContent = "";
                                }
                                picModel.CreateTime = DateTime.Now;
                                picModel.CreateUser = user.Uid;//登录用户ID
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

                                #endregion

                                List<ViewYogaPicture> list = clientpic.GetUidList(model.UID);
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
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }

        }


        //
        // GET: /YogisModels/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /YogisModels/Delete/5

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
        /// 升级导师审核中页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Audit()
        {

            int id = user.Uid;
            ViewYogisModels model = new ViewYogisModels();

            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                model = client.GetYogisModelsById(id);
                if (model != null)
                {
                    #region
                    //昵称

                    using (YogaUserServiceClient YogaUserServiceClient = new YogaUserServiceClient())
                    {
                        ViewBag.NickName = YogaUserServiceClient.GetYogaUserById(model.UID).NickName ?? "";
                    }


                    #region 会馆
                    if (!string.IsNullOrEmpty(model.CenterID))
                    {
                        string[] cenlist = model.CenterID.Split(',');

                        List<ViewCenters> listcenter = new List<ViewCenters>();
                        using (CentersServiceClient CentersServiceClient = new CentersServiceClient())
                        {
                            listcenter = CentersServiceClient.GetCentersUid();

                            string strCentValue = "";
                            foreach (var i in cenlist)
                            {
                                foreach (var itemCenter in listcenter)
                                {
                                    if (i.ToString() == itemCenter.CenterId.ToString())
                                    {
                                        strCentValue += itemCenter.CenterName + ',';
                                    }
                                }
                            }
                            ViewBag.CentValue = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(model.YogaTypeid))
                    {
                        string[] YogaTypeidlist = model.YogaTypeid.Split(',');

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
                    }
                    #endregion

                    #region 导师列表

                    if (!string.IsNullOrEmpty(model.TeachYogis))
                    {
                        string[] TeachYogislist = model.TeachYogis.Split(',');
                        ViewYogisModels model3 = new ViewYogisModels();
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            model3 = client.GetById(Convert.ToInt32(k));
                            if (model3 != null)
                            {
                                strTeachYogisValue += model3.RealName + ',';
                            }

                        }
                        ViewBag.TeachYogisValue = strTeachYogisValue;

                    }
                    #endregion

                    //国籍
                    if (!string.IsNullOrEmpty(model.Nationality))
                    {
                        if (model.Nationality != "0")
                        {
                            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                            {
                                ViewBag.Nationality = YogaDicItemServiceClient.GetYogaDicItemById(Convert.ToInt32(model.Nationality)).ItemName ?? "";
                            }
                        }
                    }


                    //国家

                    if (model.CountryID != null && model.CountryID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.CountryID = YogaDicItemServiceClient.GetYogaDicItemById(model.CountryID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.CountryID = "";

                    //地址；  省 城市 城区 
                    if (model.ProvinceID != null && model.ProvinceID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.ProvinceID = YogaDicItemServiceClient.GetYogaDicItemById(model.ProvinceID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.ProvinceID = "";
                    if (model.CityID != null && model.CityID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.CityID = YogaDicItemServiceClient.GetYogaDicItemById(model.CityID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.CityID = "";
                    if (model.DistrictID != null && model.DistrictID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.DistrictID = YogaDicItemServiceClient.GetYogaDicItemById(model.DistrictID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.DistrictID = "";
                    if (ViewBag.ProvinceID != "")
                    {
                        ViewBag.Address = ViewBag.ProvinceID + "--" + ViewBag.CityID + "--" + ViewBag.DistrictID;
                    }
                    else
                        ViewBag.Address = "";
                    #endregion
                }
                else
                {
                    ViewBag.CentValue = "";
                    ViewBag.YogaTypeidValue = "";
                    ViewBag.TeachYogisValue = "";

                }

            }
            //ViewYogiProfile pro = proClient.GetYogiProfileById(id);
            YogiProfileServiceClient proClient = new YogiProfileServiceClient();
            ViewYogiProfile pro = new ViewYogiProfile();
            pro = proClient.GetYogiProfileById(id);
            if (pro != null)
            {
                ViewBag.pro = pro;
            }
            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            {
                List<ViewYogaPicture> pic = clientpic.GetUidList(id);
                if (pic != null)
                {
                    ViewBag.Pic = pic;
                }
            }
            return View(model);
        }
        /// <summary>
        /// 级别认证
        /// </summary>
        /// <returns></returns>
        public ActionResult LevelAuthentication()
        {

            ViewYogisModels model = client.GetYogisModelsById(user.Uid);
            return View(model);
        }
        [HttpPost]
        public ActionResult LevelAuthentication(ViewYogisModels model)
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

                int yearNums = 0;
                if (!string.IsNullOrEmpty(Request.Form["yearNums"]))
                {
                    yearNums = Convert.ToInt32(Request.Form["yearNums"]);
                }
                int yogaPinl = 0;
                if (!string.IsNullOrEmpty(Request.Form["yogaPinl"]))
                {
                    yogaPinl = Convert.ToInt32(Request.Form["yogaPinl"]);
                }
                int JiaoNums = 0;
                if (!string.IsNullOrEmpty(Request.Form["JiaoNums"]))
                {
                    JiaoNums = Convert.ToInt32(Request.Form["JiaoNums"]);
                }
                int yogaUserPl = 0;
                if (!string.IsNullOrEmpty(Request.Form["yogaUserPl"]))
                {
                    yogaUserPl = Convert.ToInt32(Request.Form["yogaUserPl"]);
                }
                int yogaContent = 0;
                if (!string.IsNullOrEmpty(Request.Form["yogaContent"]))
                {
                    yogaContent = Convert.ToInt32(Request.Form["yogaContent"]);
                }
                int yogaknowledge = 0;
                if (!string.IsNullOrEmpty(Request.Form["yogaknowledge"]))
                {
                    yogaknowledge = Convert.ToInt32(Request.Form["yogaknowledge"]);
                }

                int Nums = yearNums + yogaPinl + JiaoNums + yogaUserPl + yogaContent + yogaknowledge;

                ViewYogisModels entity = client.GetById(model.YID);
                entity.Leveldetails = yearNums + "," + yogaPinl + "," + JiaoNums + "," + yogaUserPl + "," + yogaContent + "," + yogaknowledge;
                int level = 0;
                int? oldlevel = entity.YogisLevel;
                if (Nums > 6 && Nums < 12)
                {
                    level = 1;
                    //entity.YogisLevel = 1;
                    client.Update(entity);
                }
                else if (Nums >= 12 && Nums < 18)
                {
                    level = 2;
                    //entity.YogisLevel = 2;
                    client.Update(entity);
                }
                else if (Nums >= 18 && Nums <= 24)
                {
                    level = 3;
                    //entity.YogisLevel = 3;
                    client.Update(entity);
                }

                if (oldlevel != null)
                {
                    //升级判断
                    if (level <= oldlevel)
                    {
                        //级别低于原级别，升级失败
                        level = 0;
                    }
                }
                else
                {
                    level = -1;
                }

                //添加升级订单表
                AddLevelOrder(Nums);

                //更新Proile
                //using (YogiProfileServiceClient profileclient = new YogiProfileServiceClient())
                //{
                //    ViewYogiProfile profile= profileclient.GetYogiProfileById(model.UID);
                //    if (profile == null)
                //    {
                //        profile = new ViewYogiProfile();
                //        profile.sMemo = "";
                //        profileclient.Add(profile);
                //    }
                //    else
                //    {
                //        profile.sMemo = "";
                //        profileclient.Update(profile);
                //    }
                //}
                //return Content("<script>alert('提交成功！');window.location.href='/YogisModels/LevelAuthentication'</script>");
                return Content(level.ToString());
            }
            catch
            {
                return View();
            }

        }

        /// <summary>
        /// 老师到老师  添加级别订单
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
                viewLO.OrderType = LevelOrderType.老师到老师.ToString();
                viewLO.OrderState = LevelOrderState.申请中.ToString();
                viewLO.OrderScore = score.ToString();
                viewLO.OrderDel = 0;
                viewLO.OriginalLevel = CommonInfo.GetCurrentLevel(user);//当前用户level
                viewLO.TargetLevel = CommonInfo.GetLevelbyScore(score, 0);
                viewLO.CreateTime = viewLO.UpdateTime = now;
                loClient.Add(viewLO);
            }

        }

        /// <summary>
        /// 升级导师审核中页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditEdit()
        {

            int id = user.Uid;
            ViewBag.id = id;
            ViewYogisModels model = new ViewYogisModels();

            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                model = client.GetYogisModelsById(id);

                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.Leveldetails))
                    {
                        string[] scores = model.Leveldetails.Split(',');
                        int Nums = 0;
                        string templevel = "";
                        for (int i = 0; i < scores.Length; i++)
                        {
                            Nums += Convert.ToInt32(scores[i].ToString());
                        }
                        if (Nums > 6 && Nums < 12)
                        {
                            templevel = "初";

                        }
                        else if (Nums >= 12 && Nums < 18)
                        {
                            templevel = "中";

                        }
                        else if (Nums >= 18 && Nums <= 24)
                        {
                            templevel = "高";
                        }
                        ViewBag.level = templevel;
                    }
                    else
                    {
                        ViewBag.level = "初 ";
                    }
                    #region
                    //昵称

                    using (YogaUserServiceClient YogaUserServiceClient = new YogaUserServiceClient())
                    {
                        ViewBag.NickName = YogaUserServiceClient.GetYogaUserById(model.UID).NickName ?? "";
                    }


                    #region 会馆
                    if (!string.IsNullOrEmpty(model.CenterID))
                    {
                        string[] cenlist = model.CenterID.Split(',');

                        List<ViewCenters> listcenter = new List<ViewCenters>();
                        using (CentersServiceClient CentersServiceClient = new CentersServiceClient())
                        {
                            listcenter = CentersServiceClient.GetCentersUid();

                            string strCentValue = "";
                            foreach (var i in cenlist)
                            {
                                foreach (var itemCenter in listcenter)
                                {
                                    if (i.ToString() == itemCenter.CenterId.ToString())
                                    {
                                        strCentValue += itemCenter.CenterName + ',';
                                    }
                                }
                            }
                            ViewBag.CentValue = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(model.YogaTypeid))
                    {
                        string[] YogaTypeidlist = model.YogaTypeid.Split(',');

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
                    }
                    #endregion

                    #region 导师列表

                    if (!string.IsNullOrEmpty(model.TeachYogis))
                    {
                        string[] TeachYogislist = model.TeachYogis.Split(',');
                        ViewYogisModels model3 = new ViewYogisModels();
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            model3 = client.GetById(Convert.ToInt32(k));
                            if (model3 != null)
                            {
                                strTeachYogisValue += model3.RealName + ',';
                            }

                        }
                        ViewBag.TeachYogisValue = strTeachYogisValue;

                    }
                    #endregion

                    //国籍
                    if (string.IsNullOrEmpty(model.Nationality))
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.Nationality = YogaDicItemServiceClient.GetYogaDicItemById(Convert.ToInt32(model.Nationality)).ItemName ?? "";
                        }
                    }


                    //国家

                    if (model.CountryID != null && model.CountryID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.CountryID = YogaDicItemServiceClient.GetYogaDicItemById(model.CountryID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.CountryID = "";

                    //地址；  省 城市 城区 
                    if (model.ProvinceID != null && model.ProvinceID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.ProvinceID = YogaDicItemServiceClient.GetYogaDicItemById(model.ProvinceID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.ProvinceID = "";
                    if (model.CityID != null && model.CityID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.CityID = YogaDicItemServiceClient.GetYogaDicItemById(model.CityID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.CityID = "";
                    if (model.DistrictID != null && model.DistrictID != 0)
                    {
                        using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                        {
                            ViewBag.DistrictID = YogaDicItemServiceClient.GetYogaDicItemById(model.DistrictID.Value).ItemName ?? "";
                        }
                    }
                    else ViewBag.DistrictID = "";

                    ViewBag.Address = ViewBag.ProvinceID + "--" + ViewBag.CityID + "--" + ViewBag.DistrictID;
                    #endregion
                }
                else
                {
                    ViewBag.CentValue = "";
                    ViewBag.YogaTypeidValue = "";
                    ViewBag.TeachYogisValue = "";

                }

            }
            YogiProfileServiceClient proClient = new YogiProfileServiceClient();
            ViewYogiProfile pro = new ViewYogiProfile();
            pro = proClient.GetYogiProfileById(id);
            if (pro != null)
            {
                ViewBag.pro = pro;
            }

            return View(model);
        }


        [HttpPost]
        public JsonResult DoAuditEdit()
        {
            try
            {
                using (YogiProfileServiceClient pclient = new YogiProfileServiceClient())
                {
                    ViewYogiProfile modelInfo = pclient.GetYogiProfileById(user.Uid);
                    if (modelInfo != null)
                    {
                        modelInfo.IDPhoto = Request.Form["IDPhoto"];
                        modelInfo.IDScan = Request.Form["IDScan"];
                        modelInfo.IDScanF = Request.Form["IDScanF"];
                        modelInfo.Diploma = Request.Form["Diploma"];
                        modelInfo.sMemo = "审核中";
                        pclient.Update(modelInfo);
                    }
                    else
                    {
                        modelInfo = new ViewYogiProfile();
                        modelInfo.ProfileID = modelInfo.ProfileID;
                        modelInfo.IDPhoto = Request.Form["IDPhoto"];
                        modelInfo.IDScan = Request.Form["IDScan"];
                        modelInfo.IDScanF = Request.Form["IDScanF"];
                        modelInfo.Diploma = Request.Form["Diploma"];
                        modelInfo.sMemo = "审核中";
                        modelInfo.UID = user.Uid;
                        modelInfo.CreateDate = DateTime.Now;
                        modelInfo.UpgradeDate = DateTime.Now;
                        pclient.Add(modelInfo);
                    }
                }

                ViewYogisModels model = client.GetYogisModelsById(user.Uid);
                if (model != null)
                {
                    model.YogiStatus = 0;
                    client.Update(model);
                }

                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = "添加失败！" });
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

        /// <summary>
        /// 导师个人设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalSetUp(string iType, string info, int page = 1)
        {
            if (!string.IsNullOrEmpty(info))
            {
                info = "editInfo";
                ViewBag.info = info;
            }
            ViewBag.iType = iType;
            ViewBag.NickName = user.NickName;//昵称

            ViewYogisModels temp = new ViewYogisModels();

            #region 去掉冗余图片
            string uploadPath = Server.MapPath("~/Files/avatar/original/" + user.Uid);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dirNext = new DirectoryInfo(uploadPath);
            FileInfo[] files = dirNext.GetFiles("*.*");
            if (files.Count() > 0)
            {
                foreach (var subdirNext in files)
                {
                    //判断文件夹中是否存在图片 
                    ViewYogisModels list = client.GetWhere(user.Uid, subdirNext.Name);
                    if (list == null)
                    {
                        PubClass.FileDel(uploadPath + "\\" + subdirNext.ToString());
                    }
                }
            }
            #endregion

            #region 导师专页 基本信息

            temp = client.GetYogisModelsById(user.Uid);
            if (temp != null)
            {
                #region 会馆
                if (!string.IsNullOrEmpty(temp.CenterID))
                {
                    string[] cenlist = temp.CenterID.Split(',');

                    List<ViewCenters> listcenter = new List<ViewCenters>();
                    using (CentersServiceClient CentersServiceClient = new CentersServiceClient())
                    {
                        listcenter = CentersServiceClient.GetCentersUid();

                        string strCentValue = "";
                        foreach (var i in cenlist)
                        {
                            foreach (var itemCenter in listcenter)
                            {
                                if (i.ToString() == itemCenter.CenterId.ToString())
                                {
                                    strCentValue += itemCenter.CenterName + ',';
                                }
                            }
                        }
                        ViewBag.CentValue = strCentValue;
                    }
                }
                #endregion

                #region 流派
                //if (!string.IsNullOrEmpty(temp.YogaTypeid))
                //{
                //    string[] YogaTypeidlist = temp.YogaTypeid.Split(',');

                //    List<ViewYogaDicItem> listcenter2 = new List<ViewYogaDicItem>();
                //    using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                //    {
                //        listcenter2 = YogaDicItemServiceClient.GetYogaDicItemList();
                //        string strYogaTypeidValue = "";
                //        foreach (var j in YogaTypeidlist)
                //        {
                //            foreach (var itemDic in listcenter2)
                //            {
                //                if (j.ToString() == itemDic.ID.ToString())
                //                {
                //                    strYogaTypeidValue += itemDic.ItemName + ',';
                //                }
                //            }
                //        }
                //        ViewBag.YogaTypeidValue = strYogaTypeidValue;
                //    }
                //}
                #endregion

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

                #region 导师列表

                if (!string.IsNullOrEmpty(temp.TeachYogis))
                {
                    string[] TeachYogislist = temp.TeachYogis.Split(',');
                    ViewYogisModels model3 = new ViewYogisModels();
                    using (YogisModelsServiceClient mClient = new YogisModelsServiceClient())
                    {
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            model3 = mClient.GetById(Convert.ToInt32(k));
                            if (model3 != null)
                            {
                                strTeachYogisValue += model3.RealName + ',';
                            }

                        }
                        ViewBag.TeachYogisValue = strTeachYogisValue;
                    }
                }
                #endregion

                #region 位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (temp.CountryID != null && temp.CountryID != 0)
                {
                    strCountryID = method.GetItemName(temp.CountryID.Value) + " ·  ";
                }
                if (temp.ProvinceID != null && temp.ProvinceID != 0)
                {
                    strProvinceID = method.GetItemName(temp.ProvinceID.Value) + " · ";
                }
                if (temp.CityID != null && temp.CityID != 0)
                {
                    strCityID = method.GetItemName(temp.CityID.Value) + " · ";
                }
                if (temp.DistrictID != null && temp.DistrictID != 0)
                {
                    strDistrictID = method.GetItemName(temp.DistrictID.Value);
                }

                ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
                #endregion


            }

            #endregion

            //关注 粉丝 人气
            ViewFollow viewMoel = new ViewFollow();
            ViewBag.iCount = clientFoll.GetFollowByUid(user.Uid);
            ViewBag.FCount = clientFoll.GetFollowByCount(user.Uid);
            ViewBag.tzancount = clientMsg.GettMessageUid(user.Uid, 1).Count();

            #region 我的课程
            //-99表示是直接从齿轮处“课程设置”点击进来的
            if (page == -99)
            {
                page = 1;
                ClassDetailServiceClient classDetailClient = new ClassDetailServiceClient();
                int pagesize = 10;
                int count = 0;
                List<ViewClassDetail> classDetailList = classDetailClient.GetClassDetail("", page, pagesize, out count);
                Webdiyer.WebControls.Mvc.PagedList<ViewClassDetail> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewClassDetail>(classDetailList, page, pagesize, count);
                ViewBag.ClassDetailList = pagelist;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("MyClassDetailList", pagelist);//pagelist
                }
                //ViewBag.Type = "ViewClassDetail";
                //return View(pagelist);
            }
            else
                ViewBag.ClassDetailList = new Webdiyer.WebControls.Mvc.PagedList<ViewClassDetail>(new List<ViewClassDetail> (),1,10);
            #endregion

            ViewBag.Type = "ViewYogisModels";
            return View(temp);

        }
        /// <summary>
        /// 恭喜您已经完成导师升级流程
        /// </summary>
        /// <returns></returns>
        public ActionResult InstructorUpgrade()
        {
            ViewLevelOrder entity = levelorderclient.GetUid(user.Uid);
            int iInfo = 0;
            int iType = 0;//判断升级为0 习练者； 1 导师
            switch (entity.TargetLevel)
            {
                #region 习练者升级
                case "初级习练者":
                    iInfo = 1;
                    iType = 0;
                    DataTable listuser1 = clientDetail.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser1;
                    break;
                case "中级习练者":
                    iInfo = 2;
                    iType = 0;
                    DataTable listuser2 = clientDetail.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser2;
                    break;
                case "高级习练者":
                    iInfo = 3;
                    iType = 0;
                    DataTable listuser3 = clientDetail.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = listuser3;
                    break;
                #endregion

                #region 老师升级

                case "初级老师":
                    iInfo = 1;
                    iType = 1;
                    DataTable list = client.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = list;
                    break;
                case "中级老师":
                    iInfo = 2;
                    iType = 1;
                    DataTable list2 = client.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = list2;
                    break;
                case "高级老师":
                    iInfo = 3;
                    iType = 1;
                    DataTable list3 = client.GetSamelevelSupervisor(iInfo);
                    ViewBag.list = list3;
                    break;
                #endregion
            }
            ViewBag.iType = iType;
            return View();
        }

        public ActionResult MyClassDetailList(int page = 1)
        {
            ClassDetailServiceClient client = new ClassDetailServiceClient();
            int pagesize = 10;
            int count = 0;
            List<ViewClassDetail> list = client.GetClassDetail("", page, pagesize, out count);
            Webdiyer.WebControls.Mvc.PagedList<ViewClassDetail> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewClassDetail>(list, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("MyClassDetailList", pagelist);
            }
            return View();
        }
    }
}
