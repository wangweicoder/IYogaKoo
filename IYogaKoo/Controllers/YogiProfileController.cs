using Commons.Helper;
using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace IYogaKoo.Controllers
{
    public class YogiProfileController : Controller
    {
        //
        // GET: /YogiProfile/
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogiProfileServiceClient client ;
        YogisModelsServiceClient mclient = new YogisModelsServiceClient();
        method method;
        public YogiProfileController()
        {
            ViewBag.user = user;
            client = new YogiProfileServiceClient();
            method = new Commons.Helper.method();
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

        //
        // GET: /YogiProfile/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
        /// <summary>
        /// 升级导师审核中页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Audit()
        { 
            #region 登录者的级别
            if (user.UserType == 0)
            {

            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = mclient.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                    ViewBag.level = vyogism.YogisLevel;
            }
            #endregion
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
            ViewYogiProfile pro = proClient.GetYogiProfileById(id);   
            if (pro != null)
            {
                ViewBag.pro = pro;
            }
            //using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
            //{
            //    List<ViewYogaPicture> pic = clientpic.GetUidList(id);
            //    if (pic != null)
            //    {
            //        ViewBag.Pic = pic;
            //    }
            //}
            return View(model);
        }
        //
        // GET: /YogiProfile/Create
        /// <summary>
        /// info不为空时，修改审核页面的信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult Create(string info)
        {
            if (!string.IsNullOrEmpty(info))
            {
                info = "editInfo";
                ViewBag.info = info;
            }
            ViewYogiProfile model = new ViewYogiProfile();
             
                model = client.GetYogiProfileById(user.Uid);
                if (model != null)
                {
                    Tools.WriteTextLog("YogiProfile/Create ", model.UID.ToString());
                    return View(model); 
                }
                else
                {
                    Tools.WriteTextLog("YogiProfile/Create ", "model为空");
                    return View();
                }
        }

        //
        // POST: /YogiProfile/Create

        [HttpPost]
        public JsonResult Create(ViewYogiProfile model)
        {
            try
            {
                model.UID = user.Uid;
                model.CreateDate = DateTime.Now;
                model.UpgradeDate = DateTime.Now;
                using (YogiProfileServiceClient client = new YogiProfileServiceClient())
                {
                    ViewYogiProfile  modelInfo = client.GetYogiProfileById(user.Uid);
                    if (modelInfo != null)
                    {
                        model.ProfileID = modelInfo.ProfileID;
                        client.Update(model);
                    }
                    else
                    {
                        client.Add(model);
                    }

                }
                //添加升级订单表
                AddLevelOrder();
                return Json(new { code = 0 });

                //return RedirectToAction("Index");
            }
            catch
            {
                return Json(new { code = "添加失败！" });
            }
        }

        /// <summary>
        /// 习练者到老师  添加级别订单
        /// </summary>
        private void AddLevelOrder()
        {
            DateTime now = DateTime.Now;
            using (LevelOrderServiceClient loClient = new LevelOrderServiceClient())
            {
                ViewLevelOrder viewLO = new ViewLevelOrder();
                viewLO.LevelOrderID = now.ToString("yyMMddHHmmssfff");
                viewLO.UID = user.Uid;
                viewLO.Name = user.NickName;
                viewLO.OrderType = LevelOrderType.习练者到老师.ToString();
                viewLO.OrderState = LevelOrderState.申请中.ToString();
                viewLO.OrderScore = "-0";
                viewLO.OrderDel = 0;
                viewLO.OriginalLevel = CommonInfo.GetCurrentLevel(user);//当前用户level
                viewLO.TargetLevel = TeacherLevel.初级老师.ToString();
                viewLO.CreateTime = viewLO.UpdateTime = now;
                loClient.Add(viewLO);
            }
        }

        //
        // GET: /YogiProfile/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /YogiProfile/Edit/5

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
        // GET: /YogiProfile/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /YogiProfile/Delete/5

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
