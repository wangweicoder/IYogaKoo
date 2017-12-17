using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using IYogaKoo.ViewModel.Commons.Enums;
using Commons.Helper;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class AdminController : Controller
    {
        ///获取用户信息cookie 
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser;
        tMessageServiceClient clientMsg; 
        YogaUserDetailServiceClient userDetclient;
        method method;
        public AdminController()
        {
            ViewBag.user = user;
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();
            clientMsg = new tMessageServiceClient(); 
            userDetclient = new YogaUserDetailServiceClient();
            method = new method();
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
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 通用 留言/评论
        /// </summary>
        /// <returns></returns>
        //public ActionResult PartialMessage(int id,int page=1)
        //{
        //    #region 留言/评论
        //    int rcount = 0;
        //    int pagesize = 10;
        //    List<ViewtMessage> msgEntity = new List<ViewtMessage>();
        //    msgEntity = clientMsg.GettMessageUidList(id, 0, page, pagesize, out rcount);
        //    List<ViewtMessageGroup> listGroupMsg = new List<ViewtMessageGroup>();

        //    #region

        //    foreach (var item in msgEntity)
        //    {
        //        ViewtMessageGroup model = new ViewtMessageGroup();

        //        model.entity = item;
        //        //被留言人

        //        ViewYogaUser yuser = clientUser.GetYogaUserById(item.ToUid.Value);
        //        if (yuser != null)
        //            model.ToUser = yuser.NickName;
        //        //留言人
        //        ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
        //        if (usermodel != null)
        //            model.FromUser = usermodel.NickName;
        //        if (item.FormType == 0)
        //        {
        //            //习练者头像
        //            using (YogaUserDetailServiceClient clientDet = new YogaUserDetailServiceClient())
        //            {
        //                ViewYogaUserDetail ViewDet = new ViewYogaUserDetail();
        //                if (item.FromUid != 0)
        //                {
        //                    ViewDet = clientDet.GetYogaUserDetailById(item.FromUid.Value);
        //                    if (ViewDet != null)
        //                    {
        //                        model.DisplayImg = ViewDet.DisplayImg;
        //                    }
        //                    model.sUrl = "/YogaUserDetail/Details/" + item.FromUid.Value;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //导师头像
        //            using (YogisModelsServiceClient clientDet = new YogisModelsServiceClient())
        //            {
        //                ViewYogisModels ViewDet = new ViewYogisModels();
        //                if (item.FromUid != 0)
        //                {
        //                    ViewDet = clientDet.GetYogisModelsById(item.FromUid.Value);
        //                    if (ViewDet != null)
        //                    {

        //                        model.DisplayImg = ViewDet.DisplayImg;
        //                    }
        //                    model.sUrl = "/YogisModels/Details/" + item.FromUid.Value;
        //                }

        //            }
        //        }
        //        //strPic
        //        //回复
        //        List<ViewtMessage> listM = clientMsg.GettMessageParentID(item.ID);
        //        List<ViewtMessageGroup> entitylist = new List<ViewtMessageGroup>();
        //        foreach (var it in listM)
        //        {
        //            ViewtMessageGroup entityMsg = new ViewtMessageGroup();
        //            entityMsg.entity = it;
        //            //被留言人

        //            ViewYogaUser yuser2 = clientUser.GetYogaUserById(it.ToUid.Value);
        //            if (yuser2 != null)
        //                entityMsg.ToUser = yuser2.NickName;
        //            //留言人
        //            ViewYogaUser usermodel2 = clientUser.GetYogaUserById(it.FromUid.Value);
        //            if (usermodel2 != null)
        //                entityMsg.FromUser = usermodel2.NickName;

        //            entitylist.Add(entityMsg);

        //        }
        //        model.msgList = entitylist;
        //        listGroupMsg.Add(model);
        //    }

        //    #endregion

        //    ViewBag.MsgInfo = listGroupMsg;
        //    ViewBag.rcount = rcount;

        //    Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> messlist = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.MsgInfo, page, pagesize, rcount);
        //    //if (Request.IsAjaxRequest())
        //    //{
        //    //    return PartialView("PartialMessage", messlist);
        //    //}
        //    #endregion
        //    return PartialView("PartialMessage", messlist);
        //}
        
        public ActionResult Add()
        {
            ClassServiceClient classService = new ClassServiceClient();
            for (int i = 0; i < 10; i++)
            {
                ViewClass vc = new ViewClass();
                vc.TopicIds = "1,11,1";
                vc.Name = "活动名称";
                vc.Start = DateTime.Now.ToString();
                vc.Duration = 3;
                vc.DurationUnit = (int)TimeUnit.小时;
                vc.AreaID = 82;
                vc.Address = "奥森公园";
                vc.Max = 100;
                vc.Summary = "这是一个测试活动";
                vc.Content = "这是活动的图文详细介绍";
                vc.Banner = "http://www.iyogakoo.com/files/avatar/original/100004.JPG";
                vc.ClassStatus = (int)ClassStatus.审核中;
                vc.CreateTime = DateTime.Now;
                vc.UpdateTime = DateTime.Now;

                classService.Add(vc);
                if (vc.Id > 0)
                {

                }
            }
            return View();
        }
    }
}
