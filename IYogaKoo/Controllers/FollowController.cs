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
    public class FollowController : Controller
    {
        //
        // GET: /Follow/
        ///获取用户信息cookie 
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        FollowServiceClient client;
        YogaUserServiceClient yuserclient;
        YogaUserDetailServiceClient userdetailsclient;
        YogisModelsServiceClient modelclient;
        List<ViewFollow> list;
        ViewFollow model;
        method method;
        public FollowController() {
            ViewBag.user = user; 
            client = new FollowServiceClient();
            yuserclient = new YogaUserServiceClient();
            modelclient = new YogisModelsServiceClient();
            userdetailsclient = new YogaUserDetailServiceClient();
            list = new List<ViewFollow>();
            model = new ViewFollow();
            method = new method();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = userdetailsclient.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = modelclient.GetYogisModelsById(user.Uid);
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
        /// <summary>
        /// 查看别人的瑜珈圈
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public ActionResult Index(int Uid)
        {
            ViewBag.id = Uid;

            #region 瑜伽圈

            list = client.GetFollowQuiltUidList(Uid);
             
            List<ViewFollowUserDetail> listGroup = new List<ViewFollowUserDetail>();
           
            foreach (var item in list)
            {
                //登录表                   
                ViewYogaUser umodel = yuserclient.GetYogaUserById(item.QuiltUid);
                ViewFollowUserDetail model = new ViewFollowUserDetail();
                model.FollowersName = umodel.NickName;
                model.flag = item.iType.Value;
                if (item.iType == 0)
                {
                    //习练者
                    ViewYogaUserDetail udmodel = userdetailsclient.GetYogaUserDetailById(item.QuiltUid);
                    model.spic = CommonInfo.GetDisplayImg(udmodel.DisplayImg);
                    model.userurl = "/YogaUserDetail/Details/";
                    model.uid = udmodel.UID;
                    //登录表                   
                    model.nickname = yuserclient.GetYogaUserById(item.QuiltUid).NickName;
                    //粉丝                  
                    model.FollowCount = client.GetFollowByCount(item.Uid);
                    model.FollowersCount = client.GetFollowByCount(item.QuiltUid);//你关注的人的粉丝
                    model.Leval = udmodel.Ulevel;
                    listGroup.Add(model);
                }
                else if (item.iType == 1)
                {
                    //导师
                    ViewYogisModels mmodel = modelclient.GetYogisModelsById(item.QuiltUid);
                    model.spic = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                    //登录表                   
                    model.nickname = yuserclient.GetYogaUserById(item.QuiltUid).NickName;
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
                    model.FollowCount = client.GetFollowByCount(item.Uid);
                    model.FollowersCount =client.GetFollowByCount(item.QuiltUid);//你关注的人的粉丝                    
                    model.Leval = mmodel.YogisLevel.Value;
                    listGroup.Add(model);
                }
            }

            ViewBag.listGroup = listGroup;

            #endregion
            return View(list);
        }
       
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="quertid">被关注者Uid</param>
        /// <param name="isf"></param>
        /// <param name="UserType">登录者ID</param>
        /// <returns></returns>
        [HttpGet]
        public string SetFollow(int quertid, string isf, int UserType)
        {
            int uid =user.Uid;
            ViewFollow model = new ViewFollow();
            model.Uid = uid;
            model.QuiltUid = quertid;
            model.iType = UserType;
            string restring = null;
            int reid=0;
            if (uid != quertid)
            {
                if (isf == "+关注")
                {
                    using (FollowServiceClient clent = new FollowServiceClient())
                    {
                        model = clent.GetFollowById(uid,quertid);
                        if (model != null)
                        {
                            //update
                            model.FollowDate = DateTime.Now;
                            model.isfollow = true;

                            clent.Update(model);
                        }
                        else
                        { 
                            //insert
                            model = new ViewFollow();
                            model.Uid = uid;
                            model.QuiltUid = quertid;
                            model.FollowDate = DateTime.Now;
                            model.isfollow = true;
                            model.iType = UserType;
                            model.QuiltCenterID = user.UserType;
                            model.loginType = 0;
                            clent.Add(model);
                        }
                        restring = "1"; 
                    }
                   
                }
                else if (isf == "已关注")
                {
                    using (FollowServiceClient clent = new FollowServiceClient())
                    {
                        model = clent.GetFollowById(uid, quertid);

                        model.isfollow = false;
                        model.FollowDate = DateTime.Now;
                        reid = clent.Update(model);

                        restring = "0"; 
                    }
                }
            }
            else
                restring = "3";//自己不需要关注自己
            return restring;

        }
        //
        // GET: /Follow/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Follow/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Follow/Create

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
        // GET: /Follow/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Follow/Edit/5

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
        // GET: /Follow/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Follow/Delete/5

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
