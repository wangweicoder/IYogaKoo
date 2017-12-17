using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class tInstationInfoController : Controller
    {
        //站内信
        // GET: /tInstationInfo/
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        method method;
        tInstationInfoServiceClient client;
        List<ViewtInstationInfo> list;

        /// <summary>
        /// 被赞
        /// </summary>
        tZanModelsServiceClient zanclient;
        /// <summary>
        /// 关注
        /// </summary> 
        FollowServiceClient followclient;
        /// <summary>
        /// 评论
        /// </summary>
        tMessageServiceClient messageclient;
        tWriteLogServiceClient writelogclient;
        YogaUserServiceClient clientUser;
        YogisModelsServiceClient clientModel; 
        YogaUserDetailServiceClient udclient;
        int count = 0;
        public tInstationInfoController()
        {
            ViewBag.user = user;
            method = new method();
            client = new tInstationInfoServiceClient();
            list = new List<ViewtInstationInfo>();
            zanclient = new tZanModelsServiceClient();
            followclient = new FollowServiceClient();
            messageclient = new tMessageServiceClient();
            clientUser = new YogaUserServiceClient();
            clientModel = new YogisModelsServiceClient();
            udclient = new YogaUserDetailServiceClient();
            writelogclient = new tWriteLogServiceClient();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = udclient.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                {
                    ViewBag.level = temp.Ulevel;
                    ViewBag.Gender = temp.Gender;
                }
            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = clientModel.GetYogisModelsById(user.Uid);

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
        //站内信
        public ActionResult Index(int page=1)
        {
            List<ViewtInstationInfo> listWhere0 = client.GetPageListWhereUidAndloginType(user.Uid, 0,out count);
            if (count > 0)
            {
                //第一次登录
                foreach (ViewtInstationInfo item in listWhere0)
                { 
                    item.loginType=1;
                    client.Update(item);
                }
            }
            else
            {  
                List<ViewtInstationInfo> listWhere1 = client.GetPageListWhereUidAndloginType(user.Uid, 1, out count);
                if (count > 0)
                {
                    foreach (ViewtInstationInfo item in listWhere1)
                    {
                        item.loginType = 2;
                        client.Update(item);
                    }
                }
            }

            list = client.GetPageList(user.Uid, page, 10, out count);
            PagedList<ViewtInstationInfo> pagelist = new PagedList<ViewtInstationInfo>(list, page, 10, count);　
            return View(pagelist);
        }

        //被关注  
        public ActionResult FollowIndex(int page = 1)
        {
            List<ViewFollow> listWhere0 = followclient.GetFollowQuiltUidList(user.Uid, 0,out count);
            if (count > 0)
            {
                //第一次登录
                foreach (ViewFollow item in listWhere0)
                {
                    item.loginType = 1;
                    followclient.Update(item);
                }
            }
            else
            {
                List<ViewFollow> listWhere1 = followclient.GetFollowQuiltUidList(user.Uid, 1, out count);
                if (count > 0)
                {
                    foreach (ViewFollow item in listWhere1)
                    {
                        item.loginType = 2;
                        followclient.Update(item);
                    }
                }
            }
           
            List<ViewFollow> followlist = followclient.GetFollowQuiltUidList(user.Uid,page,10,out count);
            PagedList<ViewFollow> pagelist = new PagedList<ViewFollow>(followlist, page, 10, count);
            #region
            List<ViewFollowUserDetail> listFollowGroup = new List<ViewFollowUserDetail>();
            foreach (var item in followlist)
            {

                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.Uid);

                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = userEntity.NickName;//昵称

                model.flag = item.iType.Value;
                model.CreateTime = item.FollowDate;
                model.iNew = item.loginType.Value;
                if (userEntity.UserType == 0)
                {
                    //习练者
                    ViewYogaUserDetail udmodel = udclient.GetYogaUserDetailById(item.Uid);
                    model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                    model.userurl = "/YogaUserDetail/Details/";
                    model.uid = udmodel.UID;

                    listFollowGroup.Add(model);
                }
                else if (userEntity.UserType == 1)
                {
                    //导师
                    ViewYogisModels mmodel = clientModel.GetYogisModelsById(item.Uid);
                    model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg); 
                    
                    if (mmodel.YogisLevel == 4)
                    {
                        model.userurl = "/YogaGuru/Details/";
                        model.nickname = mmodel.RealName;
                    }
                    else
                    {
                        model.userurl = "/YogisModels/Details/";
                    }
                    
                    model.uid = mmodel.UID;

                    listFollowGroup.Add(model);
                }
            }

            ViewBag.listFollowGroup = listFollowGroup;
             
            #endregion

            return View("FollowIndex", pagelist);
        }

        //被赞
        public ActionResult ZanIndex(int page = 1)
        {
            List<ViewtZanModels> listWhere0 = zanclient.GetByFromUidList(user.Uid, 0, out count);
            if (count > 0)
            {
                //第一次登录
                foreach (ViewtZanModels item in listWhere0)
                {
                    item.loginType = 1;
                    zanclient.Update(item);
                }
            }
            else
            {
                List<ViewtZanModels> listWhere1 = zanclient.GetByFromUidList(user.Uid, 1, out count);
                if (count > 0)
                {
                    foreach (ViewtZanModels item in listWhere1)
                    {
                        item.loginType = 2;
                        zanclient.Update(item);
                    }
                }
            }
             
            List<ViewtZanModels> Zanlist = zanclient.GetToUidList(user.Uid);
            PagedList<ViewtZanModels> pagelist = new PagedList<ViewtZanModels>(Zanlist, page, 10, count);
            #region
            List<ViewFollowUserDetail> listFollowGroup = new List<ViewFollowUserDetail>();
            foreach (var item in Zanlist)
            {
                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.iFromUid.Value);
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = userEntity.NickName;//昵称
                model.flag = item.iType.Value;
                model.CreateTime = item.CreateDate;
                model.iNew = item.loginType.Value;
                switch (item.iToType)
                {
                    #region

                    
                    case 0://"0习练者 
                        
                        ViewYogaUserDetail udmodel = udclient.GetYogaUserDetailById(item.iFromUid.Value);
                        model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                        model.userurl = "/YogaUserDetail/Details/";
                        model.uid = udmodel.UID;

                        listFollowGroup.Add(model);

                        break;
                    case 1://1导师
                        
                        ViewYogisModels mmodel = clientModel.GetYogisModelsById(item.iFromUid.Value);
                        model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);

                        if (mmodel.YogisLevel == 4)
                        {
                            model.userurl = "/YogaGuru/Details/";
                            model.nickname = mmodel.RealName;
                        }
                        else
                        {
                            model.userurl = "/YogisModels/Details/";
                        }

                        model.uid = mmodel.UID;

                        listFollowGroup.Add(model);
                        break;
                    #endregion

                    case 2://2 学习互动（社区） 

                        break;
                    case 3://3 日志标识" 日志表加个字段ID 表示赞的主键
                       ViewtWriteLog entity = new ViewtWriteLog();
                       ViewtZanModels zanentity= zanclient.GetByiToType(item.iToType.Value);
                       if (zanentity != null)
                       {  
                           if (zanentity.ToUid != user.Uid)
                           {
                               entity = writelogclient.GetById(zanentity.iToUid.Value);
                               model.Profile = entity.sTitle;//标题
                               #region
                               model.ID = zanentity.iToUid.Value;
                               if (userEntity.UserType == 0)
                               {
                                   ViewYogaUserDetail udmodel3 = udclient.GetYogaUserDetailById(item.iFromUid.Value);
                                   model.spic = CommonInfo.GetDisplayImg(udmodel3.DisplayImg);
                                   model.userurl = "/YogaUserDetail/Details/";
                                   model.uid = udmodel3.UID;

                                   listFollowGroup.Add(model);
                               }
                               else
                               {
                                   ViewYogisModels mmodel3 = clientModel.GetYogisModelsById(item.iFromUid.Value);
                                   model.spic = CommonInfo.GetDisplayImg(mmodel3.DisplayImg);

                                   if (mmodel3.YogisLevel == 4)
                                   {
                                       model.userurl = "/YogaGuru/Details/";
                                       model.nickname = mmodel3.RealName;
                                   }
                                   else
                                   {
                                       model.userurl = "/YogisModels/Details/";
                                   }

                                   model.uid = mmodel3.UID;

                                   listFollowGroup.Add(model);
                               }

                               #endregion
                                
                           }
                            
                       }
                       
                        break;
                }
                
            }

            ViewBag.listFollowGroup = listFollowGroup;

            #endregion

            return View(pagelist);
        }
        /// <summary>
        /// 收到的评论
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult MessageIndex(int page=1)
        {
            ViewBag.FromType = 0;
            int iToType = 0;//表示空
            if (!string.IsNullOrEmpty(Request.QueryString["ToType"]))
            {
                iToType = Convert.ToInt32(Request.QueryString["ToType"]);
            }
            List<ViewtMessage> listWhere0 = messageclient.GetPageListWhereUidAndloginType(user.Uid, 0, out count);
            if (count > 0)
            {
                //第一次登录
                foreach (ViewtMessage item in listWhere0)
                {
                    item.loginType = 1;
                    messageclient.Update(item);
                }
            }
            else
            {
                List<ViewtMessage> listWhere1 = messageclient.GetPageListWhereUidAndloginType(user.Uid, 1, out count);
                if (count > 0)
                {
                    foreach (ViewtMessage item in listWhere1)
                    {
                        item.loginType = 2;
                        messageclient.Update(item);
                    }
                }
            }

            #region 收到的评论

            List<ViewtMessage> entitylist = messageclient.GetByMessage(iToType,user.Uid, 0);
            PagedList<ViewtMessage> pagelist = new PagedList<ViewtMessage>(entitylist, page, 10, count);

            List<ViewFollowUserDetail> listFollowGroup = new List<ViewFollowUserDetail>();
            foreach (var item in entitylist)
            {
                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.FromUid.Value);
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = userEntity.NickName;//昵称 
                model.CreateTime = item.CreateDate;
                model.iNew = item.loginType.Value;
                model.Profile = item.sContent;
                // 0：留言；1：推荐；2 学习互动留言；3 活动留言 ；4：日志留言；5 会馆留言
                if (item.ToType == 1 || item.ToType == 0)
                {
                    model.messType = "我";
                }
                else if (item.ToType == 2) 
                {  
                    model.messType = "我的学习互动";
                }
                else if (item.ToType == 3)
                { 
                    model.messType = "我的活动";
                }
                else if (item.ToType == 4)
                { 
                    model.messType = "我的日志";
                }
                else if (item.ToType == 5)
                { 
                    model.messType = "我的会馆";
                }
                #region
                
                if (userEntity.UserType == 0)
                {
                    ViewYogaUserDetail udmodel3 = udclient.GetYogaUserDetailById(item.FromUid.Value);
                    model.spic = method.DisplayImg(udmodel3.DisplayImg);
                    model.userurl = "/YogaUserDetail/Details/";
                    model.uid = udmodel3.UID;

                    listFollowGroup.Add(model);
                }
                else
                {
                    ViewYogisModels mmodel3 = clientModel.GetYogisModelsById(item.FromUid.Value);
                    model.spic = method.DisplayImg(mmodel3.DisplayImg);

                    if (mmodel3.YogisLevel == 4)
                    {
                        model.userurl = "/YogaGuru/Details/";
                        model.nickname = mmodel3.RealName;
                    }
                    else
                    {
                        model.userurl = "/YogisModels/Details/";
                    }

                    model.uid = mmodel3.UID;

                    listFollowGroup.Add(model);
                }

                #endregion

            }
             
            ViewBag.listFollowGroup = listFollowGroup;

            #endregion

            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialMessage", pagelist);
            }
            return View(pagelist);
        }

       
        /// <summary>
        /// 发出的评论
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult FromMessageIndex(int page = 1)
        {
            ViewBag.FromType = 1;
            int iToType = 0;//表示空
            if (!string.IsNullOrEmpty(Request.QueryString["ToType"]))
            {
                iToType = Convert.ToInt32(Request.QueryString["ToType"]);
            }
            List<ViewtMessage> listWhere0 = messageclient.GetPageListWhereFormUidAndloginType(user.Uid, 0, out count);
            if (count > 0)
            {
                //第一次登录
                foreach (ViewtMessage item in listWhere0)
                {
                    item.loginType = 1;
                    messageclient.Update(item);
                }
            }
            else
            {
                List<ViewtMessage> listWhere1 = messageclient.GetPageListWhereFormUidAndloginType(user.Uid, 1, out count);
                if (count > 0)
                {
                    foreach (ViewtMessage item in listWhere1)
                    {
                        item.loginType = 2;
                        messageclient.Update(item);
                    }
                }
            }
            #region 发出的评论

            List<ViewFollowUserDetail> listFollowGroup = new List<ViewFollowUserDetail>();

            List<ViewtMessage> entitylist2 = messageclient.GetByMessageFromUid(iToType,user.Uid, 0);
            PagedList<ViewtMessage> pagelist2 = new PagedList<ViewtMessage>(entitylist2, page, 10, count);

            foreach (var item in entitylist2)
            {
                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.ToUid.Value);
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = userEntity.NickName;//昵称 
                model.CreateTime = item.CreateDate;
                model.iNew = item.loginType.Value;
                model.Profile = item.sContent;

                //if (item.ToType == 1 || item.ToType == 0)
                //{
                //    model.messType = "我";
                //}
                //else 
                if (item.ToType == 2)
                {
                    // 0：留言；1：推荐；2 学习互动留言；3 活动留言 ；4：日志留言；5 会馆留言
                    model.messType = "的学习互动";
                }
                else if (item.ToType == 3)
                {
                    model.messType = "的活动";
                }
                else if (item.ToType == 4)
                {
                    model.messType = "的日志";
                }
                else if (item.ToType == 5)
                {
                    model.messType = "的会馆";
                }
                #region

                if (userEntity.UserType == 0)
                {
                    ViewYogaUserDetail udmodel3 = udclient.GetYogaUserDetailById(item.ToUid.Value);
                    model.spic = method.DisplayImg(udmodel3.DisplayImg);
                    model.userurl = "/YogaUserDetail/Details/";
                    model.uid = udmodel3.UID;

                    listFollowGroup.Add(model);
                }
                else
                {
                    ViewYogisModels mmodel3 = clientModel.GetYogisModelsById(item.ToUid.Value);
                    model.spic = method.DisplayImg(mmodel3.DisplayImg);

                    if (mmodel3.YogisLevel == 4)
                    {
                        model.userurl = "/YogaGuru/Details/";
                        model.nickname = mmodel3.RealName;
                    }
                    else
                    {
                        model.userurl = "/YogisModels/Details/";
                    }

                    model.uid = mmodel3.UID;

                    listFollowGroup.Add(model);
                }

                #endregion

            }

            #endregion

            ViewBag.listFollowGroup = listFollowGroup;
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialMessage", pagelist2);
            }
            return View(pagelist2);
        }
    }
}
