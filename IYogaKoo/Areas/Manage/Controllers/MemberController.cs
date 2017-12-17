using IYogaKoo.Client;
using IYogaKoo.Controllers;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class MemberController : BaseController
    {
        //
        // GET: /Manage/Member/
        YogiProfileServiceClient proClient;//= new YogaPictureServiceClient();
        YogisModelsServiceClient client;// = new YogisModelsServiceClient();
        YogaUserServiceClient userclient;
        YogaUserDetailServiceClient userDetclient;
        YogaPictureServiceClient picclient;
        CentersServiceClient cenclient;
        YogaDicItemServiceClient dicclient;
        LevelOrderServiceClient levelorderclient;
        public MemberController()
        {
            ///认证
            proClient = new YogiProfileServiceClient();
            client = new YogisModelsServiceClient();
            userclient = new YogaUserServiceClient();
            userDetclient = new YogaUserDetailServiceClient();
            picclient = new YogaPictureServiceClient();
            cenclient = new CentersServiceClient();
            dicclient = new YogaDicItemServiceClient();
            levelorderclient = new LevelOrderServiceClient();
        }

        #region 会员管理
        /// <summary>
        /// 会员列表
        /// </summary>
        /// <param name="emailOrPhone">注册邮箱/注册手机号</param>
        /// <param name="NickName">昵称</param>
        /// <param name="LoginTimes">登录次数</param>
        /// <param name="UserType">瑜友类别</param>
        /// <param name="UStatus">瑜友状态</param>
        /// /// <param name="UStatus">登录状态</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult Index(string UEmail, int? LoginTimes, int? UserType, int? UStatus, int? LoginType, int page = 1)
        {
            List<ViewYogaUser> list = new List<ViewYogaUser>();

            int count = 0;

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                list = client.BackGetPageList(UEmail, LoginTimes, UserType, UStatus, LoginType, page, 10, out count);
            }

            PagedList<ViewYogaUser> pagelist = new PagedList<ViewYogaUser>(list, page, 10, count);

            return PartialView("Index", pagelist);

        }
        /// <summary>
        /// 会员查询条件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult IndexSearch(int page = 1)
        {

            List<ViewYogaUser> list = new List<ViewYogaUser>();

            int count = 0;

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                list = client.GetogaUserGroupList(page, 10, out count);
            }

            Webdiyer.WebControls.Mvc.PagedList<ViewYogaUser> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewYogaUser>(list, page, 10, count);

            return View("IndexSearch", pagelist);
        }

        /// <summary>
        /// 会员详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            ViewYogaUser pro = userclient.GetById(id);

            return View(pro);
        }

        //
        // GET: /Manage/Member/Edit/5
        /// <summary>
        /// 会员编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.GetYogaUserById(id);
            }
            return View(model);
        }

        //
        // POST: /Manage/Member/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                ViewYogaUser model = userclient.GetById(Convert.ToInt32(collection["Uid"]));
                model.UEmail = collection["UEmail"];
                model.Uphone = collection["Uphone"];
                model.NickName = collection["NickName"];
                model.UStatus = Convert.ToInt32(collection["UStatus"]);
                model.UserType = Convert.ToInt32(collection["UserType"]);
                model.IsAssessor = Convert.ToBoolean(collection["IsAssessor"]);
                model.IsWebworkers = Convert.ToBoolean(collection["IsWebworkers"]);

                userclient.Update(model);
                return RedirectToAction("IndexSearch");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/Member/Delete/5
        /// <summary>
        /// 删除会员
        /// 假删除，修改状态为：delState=1
        /// </summary>
        /// <param name="id"></param>
        public ActionResult Delete(int id)
        {
            ViewYogaUser user = new ViewYogaUser();

            using (YogaUserServiceClient clientuser = new YogaUserServiceClient())
            {
                user = clientuser.GetById(id);
                user.delState = 1;
                user.UStatus = 2;//2＝冻结
                clientuser.Update(user);
            }
            return View("IndexSearch");
        }

        //
        // POST: /Manage/Member/Delete/5

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

        #endregion

        #region 升级导师

        /// <summary>
        /// 升级导师查询条件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns> 
        public ActionResult UpDaoShiIndexSearch(int page = 1)
        {
            List<ViewYogisModels> list = new List<ViewYogisModels>();

            int count = 0;

            list = client.GetYogisModelsPageListUp(page, 10, out count);

            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            List<ViewBackUserModelsGroup> listGroup = new List<ViewBackUserModelsGroup>();

            foreach (var item in list)
            {
                ViewBackUserModelsGroup model = new ViewBackUserModelsGroup();

                model.VYogisModels = item;

                using (CentersServiceClient uclient = new CentersServiceClient())
                {
                    #region 会馆
                    if (!string.IsNullOrEmpty(item.CenterID))
                    {
                        string[] cenlist = item.CenterID.Split(',');

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
                            model.CentersName = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(item.YogaTypeid))
                    {
                        string[] YogaTypeidlist = item.YogaTypeid.Split(',');

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
                            model.LiuPai = strYogaTypeidValue;
                        }
                    }
                    #endregion

                    #region 导师列表

                    if (!string.IsNullOrEmpty(item.TeachYogis))
                    {
                        string[] TeachYogislist = item.TeachYogis.Split(',');
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
                        model.TeachersName = strTeachYogisValue;

                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(item.Leveldetails))
                {
                    int score = 0;
                    string[] tempscore = item.Leveldetails.Split(',');
                    for (int i = 0; i < tempscore.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempscore[i]))
                        {
                            score += Convert.ToInt32(tempscore[i]);
                        }
                    }
                    //根据分数评级别

                    if (score > 6 && score < 12)
                    {
                        model.UpTeacher = "普通导师";
                    }
                    else if (score >= 12 && score < 18)
                    {
                        model.UpTeacher = "中级导师";
                    }
                    else if (score >= 18 && score <= 24)
                    {
                        model.UpTeacher = "高级导师";
                    }
                    else
                        model.UpTeacher = "大师";
                }
                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return View(pagelist);

        }
        /// <summary>
        /// 升级导师列表
        /// </summary>
        /// <param name="RealName"></param>
        /// <param name="CenterID"></param>
        /// <param name="YogaTypeid"></param>
        /// <param name="YogisLevel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult UpDaoShiIndex(string RealName, string CenterID, string YogaTypeid, int? YogisLevel, int page = 1)
        {
            List<ViewYogisModels> list = new List<ViewYogisModels>();

            int count = 0;

            if (!string.IsNullOrEmpty(CenterID))
            {
                ViewCenters entity = cenclient.GetCentersByCenterName(CenterID);
                if (entity != null)
                {
                    CenterID = entity.CenterId.ToString();
                }

            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                ViewYogaDicItem entityDic = dicclient.GetYogaDicItemByItemName(YogaTypeid);
                if (entityDic != null)
                {
                    YogaTypeid = entityDic.ID.ToString();
                }

            }

            list = client.BackPageList(RealName, CenterID, YogaTypeid, 0, YogisLevel, page, 10, out count);

            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            List<ViewBackUserModelsGroup> listGroup = new List<ViewBackUserModelsGroup>();

            foreach (var item in list)
            {
                ViewBackUserModelsGroup model = new ViewBackUserModelsGroup();

                model.VYogisModels = item;

                using (CentersServiceClient uclient = new CentersServiceClient())
                {
                    #region 会馆
                    if (!string.IsNullOrEmpty(item.CenterID))
                    {
                        string[] cenlist = item.CenterID.Split(',');

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
                            model.CentersName = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(item.YogaTypeid))
                    {
                        string[] YogaTypeidlist = item.YogaTypeid.Split(',');

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
                            model.LiuPai = strYogaTypeidValue;
                        }
                    }
                    #endregion

                    #region 导师列表
                    if (!string.IsNullOrEmpty(item.TeachYogis))
                    {
                        string[] TeachYogislist = item.TeachYogis.Split(',');
                        List<ViewYogisModels> listcenter3 = new List<ViewYogisModels>();

                        listcenter3 = client.GetYogisModelsList();
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            foreach (var itemModel in listcenter3)
                            {
                                if (k.ToString() == itemModel.YID.ToString())
                                {
                                    strTeachYogisValue += itemModel.RealName + ',';
                                }
                            }
                        }
                        model.TeachersName = strTeachYogisValue;

                    }
                    #endregion
                }
                #region 申请为
                if (!string.IsNullOrEmpty(item.Leveldetails))
                {
                    int score = 0;
                    string[] tempscore = item.Leveldetails.Split(',');
                    for (int i = 0; i < tempscore.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempscore[i]))
                        {
                            score += Convert.ToInt32(tempscore[i]);
                        }
                    }
                    //根据分数评级别

                    if (score > 6 && score < 12)
                    {
                        model.UpTeacher = "普通导师";
                    }
                    else if (score >= 12 && score < 18)
                    {
                        model.UpTeacher = "中级导师";
                    }
                    else if (score >= 18 && score <= 24)
                    {
                        model.UpTeacher = "高级导师";
                    }
                    else
                        model.UpTeacher = "大师";
                }
                #endregion
                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return PartialView("UpDaoShiIndex", pagelist);

        }

        #endregion

        #region 导师
        //
        // GET: /Manage/Member/Details/5

        /// <summary>
        /// 导师分页查询条件
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DaoShiIndexSearch(int page = 1)
        {
            List<ViewYogisModels> list = new List<ViewYogisModels>();

            int count = 0;

            list = client.GetYogisModelsPageList(page, 10, out count);

            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            List<ViewBackUserModelsGroup> listGroup = new List<ViewBackUserModelsGroup>();

            foreach (var item in list)
            {
                ViewBackUserModelsGroup model = new ViewBackUserModelsGroup();

                model.VYogisModels = item;

                using (CentersServiceClient uclient = new CentersServiceClient())
                {
                    #region 会馆
                    if (!string.IsNullOrEmpty(item.CenterID))
                    {
                        string[] cenlist = item.CenterID.Split(',');

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
                            model.CentersName = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(item.YogaTypeid))
                    {
                        string[] YogaTypeidlist = item.YogaTypeid.Split(',');

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
                            model.LiuPai = strYogaTypeidValue;
                        }
                    }
                    #endregion

                    #region 导师列表
                    if (!string.IsNullOrEmpty(item.TeachYogis))
                    {
                        string[] TeachYogislist = item.TeachYogis.Split(',');
                        List<ViewYogisModels> listcenter3 = new List<ViewYogisModels>();

                        listcenter3 = client.GetYogisModelsList();
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            foreach (var itemModel in listcenter3)
                            {
                                if (k.ToString() == itemModel.YID.ToString())
                                {
                                    strTeachYogisValue += itemModel.RealName + ',';
                                }
                            }
                        }
                        model.TeachersName = strTeachYogisValue;

                    }
                    #endregion
                }

                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;
            return View(pagelist);
        }
        /// <summary>
        /// 导师分页 
        /// </summary>
        /// <param name="strName">真实姓名</param>
        /// <param name="CenterID">会馆</param>
        /// <param name="YogaTypeid">流派</param> 
        /// <param name="YogisLevel">状态级别</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult DaoShiIndex(string RealName, string CenterID, string YogaTypeid, int? YogisLevel, int page = 1)
        {
            List<ViewYogisModels> list = new List<ViewYogisModels>();

            int count = 0;

            if (!string.IsNullOrEmpty(CenterID))
            {
                ViewCenters entity = cenclient.GetCentersByCenterName(CenterID);
                if (entity != null)
                {
                    CenterID = entity.CenterId.ToString();
                }

            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                ViewYogaDicItem entityDic = dicclient.GetYogaDicItemByItemName(YogaTypeid);
                if (entityDic != null)
                {
                    YogaTypeid = entityDic.ID.ToString();
                }

            }

            list = client.BackPageList(RealName, CenterID, YogaTypeid, 1, YogisLevel, page, 10, out count);

            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            List<ViewBackUserModelsGroup> listGroup = new List<ViewBackUserModelsGroup>();

            foreach (var item in list)
            {
                ViewBackUserModelsGroup model = new ViewBackUserModelsGroup();

                model.VYogisModels = item;

                using (CentersServiceClient uclient = new CentersServiceClient())
                {
                    #region 会馆
                    if (!string.IsNullOrEmpty(item.CenterID))
                    {
                        string[] cenlist = item.CenterID.Split(',');

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
                            model.CentersName = strCentValue;
                        }

                    }
                    #endregion

                    #region 流派
                    if (!string.IsNullOrEmpty(item.YogaTypeid))
                    {
                        string[] YogaTypeidlist = item.YogaTypeid.Split(',');

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
                            model.LiuPai = strYogaTypeidValue;
                        }
                    }
                    #endregion

                    #region 导师列表
                    if (!string.IsNullOrEmpty(item.TeachYogis))
                    {
                        string[] TeachYogislist = item.TeachYogis.Split(',');
                        List<ViewYogisModels> listcenter3 = new List<ViewYogisModels>();

                        listcenter3 = client.GetYogisModelsList();
                        string strTeachYogisValue = "";
                        foreach (var k in TeachYogislist)
                        {
                            foreach (var itemModel in listcenter3)
                            {
                                if (k.ToString() == itemModel.YID.ToString())
                                {
                                    strTeachYogisValue += itemModel.RealName + ',';
                                }
                            }
                        }
                        model.TeachersName = strTeachYogisValue;

                    }
                    #endregion
                }

                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return PartialView("DaoShiIndex", pagelist);

        }
        /// <summary>
        /// 导师详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DaoShiDetails(int? LevelOrderid, int id)
        {
            if (LevelOrderid != null)
            {
                ViewBag.LevelOrderid = LevelOrderid;
                ViewLevelOrder levorder = levelorderclient.GetById(LevelOrderid.Value);
                if (levorder != null)
                {
                    ViewBag.levorder = levorder;
                }
            }
            else {
                ViewBag.LevelOrderid = 0;
            }
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
            ViewYogiProfile pro = proClient.GetYogiProfileById(id);
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


        #endregion

        #region 添加/编辑 删除导师

        /// <summary>
        /// 删除导师
        /// 假删除，修改状态为：delState=1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DaoShiDelete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ViewYogisModels model = new ViewYogisModels();

                using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                {
                    model = client.GetYogisModelsById(id);
                    model.delState = 1;
                    client.Update(model);
                }
                ViewYogaUser user = new ViewYogaUser();

                using (YogaUserServiceClient clientuser = new YogaUserServiceClient())
                {
                    user = clientuser.GetYogaUserById(id);
                    user.delState = 1;
                    user.UStatus = 2;//2＝冻结
                    clientuser.Update(user);
                }

                return RedirectToAction("DaoShiIndex");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// 添加导师
        /// </summary>
        /// <returns></returns>
        public ActionResult DaoShiCreate()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult DaoShiCreate(ViewYogisModels model)
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
                item.UserType = (int)UserType.瑜伽导师;
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
                item.delState = 0;
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    model.UID = client.Return_AddUid(item).Uid;
                }
                #endregion

                model.StartTeachYear = Convert.ToDateTime(Request.Form["StartTeachYear"].ToString());
                model.Nationality = Request.Form["ddlNationality"].ToString();

                if (Request.Form["ddlCountryID"] != null && Request.Form["ddlCountryID"].ToString() != "")
                {
                    model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
                }
                else
                {
                    model.CountryID = 0;
                }
                if (Request.Form["ddlProvinceID"] != null && Request.Form["ddlProvinceID"].ToString() != "")
                {
                    model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
                }
                else
                {
                    model.ProvinceID = 0;
                }
                if (Request.Form["ddlCityID"] != null && Request.Form["ddlCityID"].ToString() != "")
                {
                    model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
                }
                else
                {
                    model.CityID = 0;
                }
                if (Request.Form["ddlDistrictID"] != null && Request.Form["ddlDistrictID"].ToString() != "")
                {
                    model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
                }
                else
                { model.DistrictID = 0; }
                model.CenterID = Request.Form["hCenterID"].ToString().TrimEnd(',') == "" ? Request.Form["CenterID"].ToString().TrimEnd(',') : Request.Form["hCenterID"].ToString().TrimEnd(',');
                model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                model.CreateDate = DateTime.Now;
                model.YogiStatus = 1;//1=普通导师
                model.IsAcceptAgreement = true;
                model.delState = 0;
                //
                using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
                {
                    clientModel.Add(model);
                }

                // return RedirectToAction("DaoShiIndex");
                return Json(new { code = 0, Uid = model.UID });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 添加导师相册
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDaoShiPic(int uid)
        {
            ViewBag.Uid = uid;
            List<SelectListItem> selectlist = listselect(GetAllFolder(uid));
            ViewData["PictureName"] = selectlist;

            return View();
        }
        public string GetAllFolder(int uid)
        {
            string folder_Names = "";
            string uploadPathNext = string.Empty;
            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + uid);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string uploadPathdefault = Server.MapPath("~/Files/PirtureType/2/" + uid + "/default");
            if (!Directory.Exists(uploadPathdefault))
            {
                Directory.CreateDirectory(uploadPathdefault);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                folder_Names += subdir.Name + ",";
                //
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");

                //if (files.Count() > 0)
                //{
                //    foreach (var subdirNext in files)
                //    {
                //        //判断文件夹中是否存在图片 
                //        List<ViewYogaPicture> list = picclient.GetListWhere(uid, subdirNext.Name);
                //        if (list.Count == 0)
                //        {
                //            PubClass.FileDel(uploadPathNext + "\\" + subdirNext.ToString());
                //        }
                //    }
                //}
            }
            return folder_Names;
        }
        public List<SelectListItem> listselect(string fileInfo)
        {
            string[] ids = fileInfo.Split(',');
            List<SelectListItem> selectlist = new List<SelectListItem>
            {
                 new SelectListItem { Text = "请选择相册", Value = "",Selected=true},
            };
            foreach (var i in ids)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    selectlist.Add(new SelectListItem { Text = i, Value = i, Selected = false });
                }
            }
            if (selectlist.Count() == 0)
            {
                selectlist.Add(new SelectListItem { Text = "请选择", Value = "", Selected = true });
            }
            return selectlist;
        }
        [HttpPost]
        public JsonResult CreateDaoShiPic(YogaPicture model)
        {
            try
            {
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

                            picModel.Uid = model.Uid;

                            picModel.PictureType = 2;
                            try
                            {
                                picModel.PictureContent = strpicContent[i];
                            }
                            catch
                            {
                                picModel.PictureContent = "";
                            }
                            picModel.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            picModel.CreateUser = 0;//登录用户ID
                            picModel.PictureName = model.PictureName;
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
                            picModel.iAudio = 0;
                            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                            {
                                clientpic.Add(picModel);
                            }
                        }
                        #endregion
                    }
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }
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
        public ActionResult DaoShiEdit(int id)
        {
            string strNickName = "";
            ViewYogaUser list = new ViewYogaUser();

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                if (client.GetYogaUserById(id) != null)
                {
                    strNickName = client.GetYogaUserById(id).NickName;
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
        public JsonResult DaoShiEdit(FormCollection collection)
        {
            try
            {
                ViewYogisModels model = new ViewYogisModels();
                using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
                {
                    model = clientModel.GetYogisModelsById(Convert.ToInt32(collection["UID"].ToString()));

                    model.RealName = collection["RealName"].ToString();
                    model.Gender = Convert.ToInt32(collection["Gender"].ToString());
                    model.Street = collection["Street"].ToString();
                    model.YogisLevel = Convert.ToInt32(collection["YogisLevel"].ToString());

                    model.YogisDepict = collection["YogisDepict"].ToString();
                    // TODO: Add insert logic here
                    model.StartTeachYear = Convert.ToDateTime(collection["StartTeachYear"].ToString());
                    model.Nationality = collection["ddlNationality"].ToString();
                    if (collection["ddlCountryID"] != null && collection["ddlCountryID"].ToString() != "")
                    {
                        model.CountryID = Convert.ToInt32(collection["ddlCountryID"].ToString());
                    }
                    else
                    {
                        model.CountryID = 0;
                    }
                    if (collection["ddlProvinceID"] != null && collection["ddlProvinceID"].ToString() != "")
                    {
                        model.ProvinceID = Convert.ToInt32(collection["ddlProvinceID"].ToString());
                    }
                    else
                    {
                        model.ProvinceID = 0;
                    }
                    if (collection["ddlCityID"] != null && collection["ddlCityID"].ToString() != "")
                    {
                        model.CityID = Convert.ToInt32(collection["ddlCityID"].ToString());
                    }
                    else
                    {
                        model.CityID = 0;
                    }
                    if (collection["ddlDistrictID"] != null && collection["ddlDistrictID"].ToString() != "")
                    {
                        model.DistrictID = Convert.ToInt32(collection["ddlDistrictID"].ToString());
                    }
                    else
                    {
                        model.DistrictID = 0;
                    }
                    model.CenterID = collection["hCenterID"].ToString().TrimEnd(',') == "" ? collection["CenterID"].ToString().TrimEnd(',') : collection["hCenterID"].ToString().TrimEnd(',');
                    model.YogaTypeid = collection["hYogaTypeid"].ToString().TrimEnd(',') == "" ? collection["YogaTypeid"].ToString().TrimEnd(',') : collection["hYogaTypeid"].ToString().TrimEnd(',');
                    model.CreateDate = DateTime.Now;
                    model.IsAcceptAgreement = true;
                    //

                    clientModel.Update(model);
                }

                //呢称修改
                ViewYogaUser mm = new ViewYogaUser();
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    mm = client.GetYogaUserById(Convert.ToInt32(collection["UID"].ToString()));
                    mm.NickName = collection["nickName"].ToString();
                    client.Update(mm);
                }
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }

        }

        #endregion

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <returns></returns>
        public JsonResult AudioNO(int LevelOrderid,int uid, string sMemo)
        {
            try
            {
                // TODO: Add insert logic here 
                ViewLevelOrder levorder = levelorderclient.GetById(LevelOrderid);
                levorder.UpdateTime = DateTime.Now;
                levorder.Reason = sMemo;
                levorder.OrderState = LevelOrderState.未通过.ToString();
                levelorderclient.Update(levorder);
                   
                //添加到站内信-qiqi 2015-12-18
                using (tInstationInfoServiceClient tinclient = new tInstationInfoServiceClient())
                { 
                    ViewtInstationInfo tinEntity = new ViewtInstationInfo();
                    tinEntity.Uid = uid;
                    tinEntity.CreateTime = DateTime.Now;
                    tinEntity.CreateUser = "100316";
                    tinEntity.ifDel = false;
                    tinEntity.loginType = 0;
                    tinEntity.iType = 2569;
                    tinEntity.sContent = sMemo;
                    tinclient.Add(tinEntity);
                }
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        public JsonResult AudioNO2(int uid, string sMemo)
        {
            try
            {
                //以前的
                // TODO: Add insert logic here 

                ViewYogiProfile model = new ViewYogiProfile();

                model = proClient.GetYogiProfileById(uid);
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.sMemo))
                    {
                        ViewYogisModels model2 = new ViewYogisModels();
                        model2 = client.GetYogisModelsById(uid);
                        model2.YogiStatus = 1;
                        client.Update(model2);
                    }
                }
                else
                {
                    return Json(new { code = 2 });
                }
                model.sMemo = sMemo;
                model.UpgradeDate = DateTime.Now;
                proClient.Update(model);

                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <returns></returns>
        public JsonResult AudioYes(int LevelOrderid,int uid)
        {
            try
            {
                // TODO: Add insert logic here
                ViewYogaUser user = new ViewYogaUser();
                user = userclient.GetYogaUserById(uid);
                if (user != null)
                {
                    user.UserType = 1;//1＝瑜伽导师
                    user.delState = 0;
                    userclient.Update(user);
                }
              
                ViewYogisModels model = new ViewYogisModels();
                model = client.GetYogisModelsById(uid);
                model.YogiStatus = 1;//导师状态1=普通导师;  
                //查询订单状态
                ViewLevelOrder levorder =levelorderclient.GetById(LevelOrderid);
                if (levorder.TargetLevel == TeacherLevel.初级老师.ToString()) 
                {
                    model.YogisLevel = 1;
                }
                else if (levorder.TargetLevel == TeacherLevel.中级老师.ToString())
                {
                    model.YogisLevel = 2;
                }
                else if (levorder.TargetLevel == TeacherLevel.高级老师.ToString())
                {
                    model.YogisLevel = 3;
                }
                client.Update(model);//修改YogisModels状态

                levorder.UpdateTime = DateTime.Now;
                levorder.OrderState = LevelOrderState.通过.ToString();
                levelorderclient.Update(levorder);

                //审核通过 删除习练者信息
                //userDetclient.Delete(model.UID.ToString());

                //添加到站内信-qiqi 2015-12-18
                using (tInstationInfoServiceClient tinclient = new tInstationInfoServiceClient())
                {
                    ViewYogaDicItem dicEntity = dicclient.GetById(2568);
                    ViewtInstationInfo tinEntity = new ViewtInstationInfo();
                    tinEntity.Uid = uid;
                    tinEntity.CreateTime = DateTime.Now;
                    tinEntity.CreateUser = "100316";
                    tinEntity.ifDel = false;
                    tinEntity.loginType = 0;
                    tinEntity.iType = dicEntity.ID;
                    tinEntity.sContent = dicEntity.ItemValue;
                    tinclient.Add(tinEntity);
                }

                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }

        }
        public JsonResult AudioYes2(int uid)
        {
            try
            {
                // TODO: Add insert logic here
                ViewYogaUser user = new ViewYogaUser();
                user = userclient.GetYogaUserById(uid);
                if (user != null)
                {
                    user.UserType = 1;//1＝瑜伽导师
                    user.delState = 0;
                    userclient.Update(user);
                }
              
                ViewYogisModels model = new ViewYogisModels();
                model = client.GetYogisModelsById(uid);
                model.YogiStatus = 1;//导师状态1=普通导师;   
                //认证
                ViewYogiProfile pro = new ViewYogiProfile();
                pro = proClient.GetYogiProfileById(uid);

                //区分习练者升级导师，导师级别升级
                if (pro != null)
                {
                    if (string.IsNullOrEmpty(pro.sMemo))
                    {
                        ViewYogaUserDetail udmodel = new ViewYogaUserDetail();
                        udmodel = userDetclient.GetYogaUserDetailById(uid);
                        model.DisplayImg = udmodel.DisplayImg;
                        model.CoverImg = udmodel.Covimg;
                        model.YogaTypeid = udmodel.YogaTypeid;
                        model.CountryID = udmodel.CountryID;
                        model.CityID = udmodel.CityID;
                        model.DistrictID = udmodel.DistrictID;
                        model.ProvinceID = udmodel.ProvinceID;

                        model.BecomeYogisTime = DateTime.Now;//升级为导师的时间
                        //习练者-〉导师
                        client.Update(model);
                    }
                    else
                    {
                        #region 导师级别升级

                        string leveldetail = model.Leveldetails;
                        int score = 0;
                        string[] tempscore = leveldetail.Split(',');
                        for (int i = 0; i < tempscore.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(tempscore[i]))
                            {
                                score += Convert.ToInt32(tempscore[i]);
                            }
                        }
                        //根据分数评级别
                        int newlevel = 1;
                        if (score > 6 && score < 12)
                        {
                            newlevel = 1;
                        }
                        else if (score >= 12 && score < 18)
                        {
                            newlevel = 2;
                        }
                        else if (score >= 18 && score <= 24)
                        {
                            newlevel = 3;
                        }
                        model.YogisLevel = newlevel;
                        client.Update(model);
                        #endregion
                    }
                    pro.sMemo = "审核通过";
                    proClient.Update(pro);
                }
                else {
                    return Json(new { code =2 });
                }
               

                //审核通过 删除习练者信息
                //userDetclient.Delete(model.UID.ToString());

                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }

        }
        #region 编辑头像封面
        //
        // GET: /Manage/Member/Create

        /// <summary>
        /// 编辑头像
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ClassTeacherId">ClassTeacher表ID</param>
        /// <returns></returns>
        public ActionResult CreateDisplayImg(int uid, int? ClassTeacherId)
        {
            ViewBag.Uid = uid;
            ViewYogisModels model = client.GetYogisModelsById(uid);
            ViewBag.ClassTeacherId = ClassTeacherId;
            return View(model);
        }

        //
        // POST: /Manage/Member/Create

        [HttpPost]
        public ActionResult CreateDisplayImg(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                //int iUid =Convert.ToInt32(collection["UID"].ToString()); 
                //string strDisplayImg = collection["DisplayImg"].ToString();
                //ViewYogisModels model = client.GetYogisModelsById(iUid);
                //model.DisplayImg = strDisplayImg;
                //client.Update(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// 编辑封面
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult CreateCoverImg(int uid)
        {
            ViewBag.Uid = uid;
            ViewYogisModels model = client.GetYogisModelsById(uid);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCoverImg(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                int iUid = Convert.ToInt32(collection["UID"].ToString());
                string strCoverImg = collection["CoverImg"].ToString();
                ViewYogisModels model = client.GetYogisModelsById(iUid);
                model.CoverImg = strCoverImg;
                client.Update(model);
                return RedirectToAction("DaoShiIndex");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        [HttpPost]
        public JsonResult Info(FormCollection collection)
        {
            try
            {
                string txtpicName = collection["txtpicName"];
                string Uid = collection["Uid"];
                string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + Uid);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);

                }
                if (!Directory.Exists(uploadPath + txtpicName))
                {
                    Directory.CreateDirectory(uploadPath + "/" + txtpicName);
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }

            }
            catch (Exception)
            {
                return Json(new { code = 1 });
            }

        }

        #region 习练者信息

        /// <summary>
        /// 习练者查询条件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult UserDetailsSearch(int page = 1)
        {
            List<ViewYogaUserDetail> list = new List<ViewYogaUserDetail>();
            int count = 0;
            int pagesize = 10;

            list = userDetclient.GetYogaUserDetailPageList(page, pagesize, out count);
            PagedList<ViewYogaUserDetail> pagelist = new PagedList<ViewYogaUserDetail>(list, page, pagesize, count);

            List<ViewUserDetailGroup> users = new List<ViewUserDetailGroup>();
            ViewUserDetailGroup user = new ViewUserDetailGroup();
            ViewYogaDicItem dicitem = null;
            foreach (var c in list)
            {
                user = new ViewUserDetailGroup();
                user.VDetailsList = c;
                //昵称
                ViewYogaUser yogauser = userclient.GetById(c.UID);
                if (yogauser != null)
                {
                    user.VyList = yogauser;
                }
                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (c.CountryID != null && c.CountryID != 0)
                {
                    strCountryID = GetItemName(c.CountryID.Value) + "·";
                }
                if (c.ProvinceID != null && c.ProvinceID != 0)
                {
                    strProvinceID = GetItemName(c.ProvinceID.Value) + "·";
                }
                if (c.CityID != null && c.CityID != 0)
                {
                    strCityID = GetItemName(c.CityID.Value) + "·";
                }
                if (c.DistrictID != null && c.DistrictID != 0)
                {
                    strDistrictID = GetItemName(c.DistrictID.Value);
                }

                user.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;

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

                users.Add(user);
            }

            ViewBag.listusers = users;

            return View(pagelist);
        }
        /// <summary>
        /// 习练者列表
        /// </summary>
        /// <param name="RealName_cn">昵称/中文名称/外文名称</param> 
        /// <param name="Ulevel">级别</param>
        /// <param name="YogaTypeid">流派</param>
        /// <param name="Nationality">国籍</param>
        /// <param name="CountryID">国家</param> 
        /// <param name="ProvinceID">省</param>
        /// <param name="CityID">市</param>
        /// <param name="DistrictID">区</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult UserDetailsIndex(string RealName_cn, int? Ulevel, string YogaTypeid,
            int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID, int page = 1)
        {
            List<ViewYogaUserDetail> list = new List<ViewYogaUserDetail>();
            int count = 0;
            int pagesize = 10;
            if (!string.IsNullOrEmpty(RealName_cn))
            {
                ViewYogaUser userEntity = userclient.ExistNickName(RealName_cn);
                if (userEntity != null)
                {
                    RealName_cn = userEntity.Uid.ToString();
                }
            }

            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                ViewYogaDicItem entity = dicclient.GetYogaDicItemByItemName(YogaTypeid);
                if (entity != null)
                    YogaTypeid = entity.ID.ToString();
            }

            list = userDetclient.BackGetPageList(RealName_cn, Ulevel, YogaTypeid, Nationality, CountryID, ProvinceID, CityID, DistrictID, page, pagesize, out count);
            PagedList<ViewYogaUserDetail> pagelist = new PagedList<ViewYogaUserDetail>(list, page, pagesize, count);

            List<ViewUserDetailGroup> users = new List<ViewUserDetailGroup>();
            ViewUserDetailGroup user = new ViewUserDetailGroup();
            ViewYogaDicItem dicitem = null;
            foreach (var c in list)
            {
                user = new ViewUserDetailGroup();
                user.VDetailsList = c;
                //昵称
                ViewYogaUser yogauser = userclient.GetById(c.UID);
                if (yogauser != null)
                {
                    user.VyList = yogauser;
                }
                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (c.CountryID != null && c.CountryID != 0)
                {
                    strCountryID = GetItemName(c.CountryID.Value) + "·";
                }
                if (c.ProvinceID != null && c.ProvinceID != 0)
                {
                    strProvinceID = GetItemName(c.ProvinceID.Value) + "·";
                }
                if (c.CityID != null && c.CityID != 0)
                {
                    strCityID = GetItemName(c.CityID.Value) + "·";
                }
                if (c.DistrictID != null && c.DistrictID != 0)
                {
                    strDistrictID = GetItemName(c.DistrictID.Value);
                }

                user.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;

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

                users.Add(user);
            }

            ViewBag.listusers = users;

            return PartialView("UserDetailsIndex", pagelist);
        }

        /// <summary>
        /// 编辑习练者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        public ActionResult UserDetailsEdit(int id)
        {
            ViewYogaUserDetail entity = userDetclient.GetById(id);

            return View(entity);
        }

        [HttpPost]
        public ActionResult UserDetailsEdit(int id, FormCollection collection)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return View("UserDetailsSearch");
        }
        /// <summary>
        /// 习练者详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserDetails(int id)
        {
            ViewYogaUserDetail temp = userDetclient.GetById(id);
            #region
            ///位置

            string strCountryID = string.Empty;
            string strProvinceID = string.Empty;
            string strCityID = string.Empty;
            string strDistrictID = string.Empty;
            if (temp.CountryID > 0)
            {
                strCountryID = GetItemName(temp.CountryID.Value) + "·";
            }
            if (temp.ProvinceID > 0)
            {
                strProvinceID = GetItemName(temp.ProvinceID.Value) + "·";
            }
            if (temp.CityID > 0)
            {
                strCityID = GetItemName(temp.CityID.Value) + "·";
            }
            if (temp.DistrictID > 0)
            {
                strDistrictID = GetItemName(temp.DistrictID.Value);
            }

            ViewBag.Address = strCountryID + strProvinceID + strCityID + strDistrictID;
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
            if (temp.Nationality > 0)
            {
                ViewBag.Nationality = GetItemName(temp.Nationality.Value);
            }
            ViewYogaUser userEntity = new ViewYogaUser();
            //昵称
            userEntity = userclient.GetById(temp.UID);
            if (userEntity != null)
                ViewBag.NickName = userEntity.NickName;
            else ViewBag.NickName = "";

            #endregion

            return View(temp);
        }

        /// <summary>
        /// 删除习练者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteUserDetails(int id)
        {
            ViewYogaUserDetail model = userDetclient.GetById(id);
            model.delState = 1;
            try
            {
                userDetclient.Update(model);            
                return Json(new { code = 0 },JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

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

        #region 升级老师订单
        public ActionResult UpgradeTeacherList(int page = 1)
        {
            string Name="";
            if (!string.IsNullOrEmpty(Request.Form["Name"]))
                Name = Request.Form["Name"];

            string OrderState="";
            if (!string.IsNullOrEmpty(Request.Form["OrderState"]))
                OrderState = Request.Form["OrderState"];

            string OrderType = "";
            if (!string.IsNullOrEmpty(Request.Form["OrderType"]))
                OrderType = Request.Form["OrderType"];

            string TargetLevel = "";
            if (!string.IsNullOrEmpty(Request.Form["TargetLevel"]))
                TargetLevel = Request.Form["TargetLevel"];

            
            int count = 0;
            //问题：如果用昵称查询 ：用户修改昵称时，是否修改该表
            List<ViewLevelOrder> list = levelorderclient.BackGetOrdersPageList(Name, OrderState, TargetLevel, OrderType, page, 10, out count);
            PagedList<ViewLevelOrder> pagelist = new PagedList<ViewLevelOrder>(list, page, 10, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartTeacherList", pagelist);
            }
            return View(pagelist);
        }

        public ActionResult DelLevelOrder(int id)
        {
             ViewLevelOrder entity = levelorderclient.GetById(id);
             entity.OrderDel = 1;
             levelorderclient.Update(entity);
             return RedirectToAction("UpgradeTeacherList", "Member"); 
        }

        #endregion
    }
}
