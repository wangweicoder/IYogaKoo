using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zzfIBM.WebControls.Mvc;
using Commons.Helper;
using System.Text;
using System.Xml;
using IYogaKoo.ViewModel.Commons.Enums;

namespace IYogaKoo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        /// <summary>
        /// 用户登录信息
        /// </summary>
        tUserLoginInfoServiceClient userloginclient;
        YogisModelsServiceClient client;
        YogaUserDetailServiceClient clienuser;
        YogaUserServiceClient clientU;
        YogaArticleServiceClient clientArt;
        ClassServiceClient clientClass;
        YogaDicItemServiceClient dicclient;
        tKeyWordServiceClient keyclient;
        method method;
        public HomeController()
        {
            userloginclient = new tUserLoginInfoServiceClient();
            client = new YogisModelsServiceClient();
            clienuser = new YogaUserDetailServiceClient();
            clientU = new YogaUserServiceClient();
            clientArt = new YogaArticleServiceClient();
            clientClass = new ClassServiceClient();
            method = new method();
            dicclient = new YogaDicItemServiceClient();
            keyclient = new tKeyWordServiceClient();
            ViewBag.user = user;
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = clienuser.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else
            {
                ViewYogisModels temp = new ViewYogisModels();
                temp = client.GetYogisModelsById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.YogisLevel;
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
        //[OutputCache(CacheProfile = "SqlDependencyCache")]
        public ActionResult Index(int page = 1)
        {
             
            #region 瑜伽习练者
            List<ViewYogaUser> list = new List<ViewYogaUser>();
            //GetYogaUserXiLian
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                list = client.GetYogaUserPageList(0).Where(x => x.UserType == 0 && x.delState == 0).ToList();
            }
            List<ViewUserDetailGroup> listDeGroup = new List<ViewUserDetailGroup>();
            foreach (var item in list)
            {
                ViewUserDetailGroup model = new ViewUserDetailGroup();

                model.VyList = item;
                //粉丝
                ViewFollow viewMoel = new ViewFollow();
                using (FollowServiceClient followClient = new FollowServiceClient())
                {
                    model.FollowCount = followClient.GetFollowByCount(item.Uid);
                }


                using (YogaUserDetailServiceClient uclient = new YogaUserDetailServiceClient())
                {
                    model.VDetailsList = uclient.GetYogaUserDetailById((int)item.Uid);
                }

                listDeGroup.Add(model);
            }
            ViewBag.list3Group = listDeGroup.Take(24).
                OrderByDescending(x => x.FollowCount).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VDetailsList.DisplayImg))
                .ToList();
            #endregion
            #region 瑜伽导师
            List<ViewYogisModels> listEntity = new List<ViewYogisModels>();
            using (YogisModelsServiceClient uclient = new YogisModelsServiceClient())
            {
                listEntity = uclient.GetYogisModelsList();
            }
            List<ViewUserModelsGroup> listMGroup = new List<ViewUserModelsGroup>();
            foreach (var item in listEntity)
            {
                ViewUserModelsGroup model = new ViewUserModelsGroup();
                model.VmList = item;
                //粉丝
                ViewFollow viewMoel = new ViewFollow();
                using (FollowServiceClient followClient = new FollowServiceClient())
                {
                    model.FollowCount = followClient.GetFollowByCount(item.UID);
                }
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    model.VyList = client.GetYogaUserById(item.UID);
                }

                listMGroup.Add(model);
            }
            //瑜伽导师
            ViewBag.list2Group = listMGroup.Take(30).
                OrderByDescending(x => x.FollowCount).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.DisplayImg)).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.CoverImg)).
                OrderByDescending(x => !string.IsNullOrEmpty(x.VmList.YogisDepict)).ToList();
            #endregion
            #region 瑜伽达人
            List<ViewYogaUser> listDoyen = new List<ViewYogaUser>();
            //GetYogaUserXiLian
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                listDoyen = client.GetYogaUserPageList(0).Where(x => x.UStatus != 2 && x.delState == 0).ToList();
            }
            List<ViewDoyenGroup> listDoyenGroup = new List<ViewDoyenGroup>();
            foreach (var item in listDoyen)
            {
                ViewDoyenGroup model = new ViewDoyenGroup();
                if (item.UserType == 0)//习练者
                {
                    #region
                   
                    using (YogaUserDetailServiceClient uclient = new YogaUserDetailServiceClient())
                    {
                        ViewYogaUserDetail umodel = uclient.GetYogaUserDetailById((int)item.Uid);
                        if (umodel != null)
                        {
                            if (!string.IsNullOrEmpty(umodel.DisplayImg) && !string.IsNullOrEmpty(umodel.Covimg))
                            {
                                model.imgurl = CommonInfo.GetDisplayImg(umodel.DisplayImg);
                                model.userurl = "/YogaUserDetail/Details/";
                                model.uid = umodel.UID;
                                //登录表
                                using (YogaUserServiceClient client = new YogaUserServiceClient())
                                {
                                    model.nickname = client.GetYogaUserById(item.Uid).NickName;
                                }
                                //粉丝                           
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowCount = followClient.GetFollowByCount(item.Uid);
                                }
                                listDoyenGroup.Add(model);
                            }
                        }
                    }
                    #endregion
                }
                else if (item.UserType == 1)//导师
                {
                    using (YogisModelsServiceClient mlient = new YogisModelsServiceClient())
                    {
                        #region
                       
                        ViewYogisModels mmodel = mlient.GetYogisModelsById((int)item.Uid);
                        if (mmodel != null)
                        {
                            if (!string.IsNullOrEmpty(mmodel.DisplayImg) && !string.IsNullOrEmpty(mmodel.CoverImg)
                                && !string.IsNullOrEmpty(mmodel.YogisDepict) && mmodel.YogisLevel != 4)
                            {
                                model.imgurl = CommonInfo.GetDisplayImg(mmodel.DisplayImg);
                                model.userurl = "/YogisModels/Details/";
                                model.uid = mmodel.UID;
                                //登录表
                                using (YogaUserServiceClient client = new YogaUserServiceClient())
                                {
                                    model.nickname = client.GetYogaUserById(item.Uid).NickName;
                                }
                                //粉丝                            
                                using (FollowServiceClient followClient = new FollowServiceClient())
                                {
                                    model.FollowCount = followClient.GetFollowByCount(item.Uid);
                                }
                                listDoyenGroup.Add(model);
                            }
                        }
                        #endregion
                    }
                }
            }
            ViewBag.listGroup = listDoyenGroup.Take(18).ToList();
            #endregion
            //banner
            List<ViewtBanner> listBanner = new List<ViewtBanner>();
            listBanner = method.listBanner(0);
            ViewBag.listBanner = listBanner;
            //活动预告主推活动
            ViewBag.MainBanner = method.listBanner(2).First() ;
            //活动预告工作坊活动
            ViewBag.WorkBanner = method.listBanner(3).ToList();
            //活动预告课程活动
            ViewBag.SubjectBanner = method.listBanner(4).ToList();
            //活动预告培训活动
            ViewBag.TrainedBanner = method.listBanner(5).ToList();
            //活动预告主题活动
            ViewBag.ThemeBanner = method.listBanner(6).ToList();
           

            //活动类型
            using (YogaDicItemServiceClient dclient = new YogaDicItemServiceClient())
            {
                ViewBag.dList = dclient.GetDicId(114);
            }
            //活动预告
            using (ClassServiceClient client = new ClassServiceClient())
            {
                ViewBag.ClassesAdvance= client.GetClassesAdvance();
            }
            //using (ClassServiceClient cclient = new ClassServiceClient())
            //{ 
            //    cclient.get
            //}

            //ViewBag.CompletedClass = new ClassServiceClient().GetClasses(8, 1, 8, new string[] { "3", DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), "DESC" });
            ViewBag.CompletedClass = new ClassServiceClient().GetClassesHaveReport();

            return View(list);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="urlContent"></param>
        /// <returns></returns>
        public ActionResult Search(int? id, int? iType, string urlContent = "", int page = 1)
        {
             
            #region 添加搜索关健字-qiqi 2015-12-22
            if (!string.IsNullOrEmpty(urlContent.Trim()))
            {
                ViewtKeyWord keyEntity = new ViewtKeyWord();
                keyEntity.CreateTime = DateTime.Now;
                keyEntity.sWord = urlContent;
                if (user != null)
                    keyEntity.Uid = user.Uid;
                else
                    keyEntity.Uid = 0;

                keyclient.Add(keyEntity);
            }
            #endregion
            //if (!string.IsNullOrEmpty(urlContent))
            //{
            return SearchMethod(urlContent, page);
            //}
            //else
            //{
            //    ViewBag.NoMessage = "抱歉，没有找到相关的信息	";
            //    return View();
            //}
        }

        /// <summary>
        /// 搜索核心,
        /// 搜索分类0全部，1习练者，
        /// </summary>
        /// <param name="urlContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private ActionResult SearchMethod(string urlContent, int page)
        {
            List<ViewSearchGroup> groups = new List<ViewSearchGroup>();
            //设置检索条件
            string searchtype = "0";
            DateTime? datetime;
            int gender = 2;
            int level = 0;
            int lp = 0;
            int topicid = 0;
            SetSearchCondition(out searchtype, out datetime, out gender, out level, out lp,out topicid);

            ViewSearchGroup searchgroup = null;  //搜索类          
            int count = 0; //结果数量
            int classsize = searchtype == "0" ? 40 : 8;//每个分类中显示条数
            int pagesize = 8;//每页条数
            int total = 0;//总条数
            CacheHelper.RemoveAllCache("SearchResult");
            //设置搜索分类
            StringBuilder sb = new StringBuilder();

            ViewBag.strWhere = urlContent;
            //习练者1
            if (searchtype.Equals("0") || searchtype.Equals("1"))
            {
                List<ViewYogaUserDetail> listxlz = new List<ViewYogaUserDetail>();
                listxlz = clienuser.GetYogaUserList(urlContent, 0, 0, 0, 0, lp, level, gender, page, classsize, out count);
                if (searchtype.Equals("0"))
                    total += listxlz.Count();
                else
                    total = count;
                foreach (ViewYogaUserDetail u in listxlz)
                {
                    searchgroup = new ViewSearchGroup();
                    ViewYogaUser unick = clientU.GetYogaUserById(u.UID);
                    if (unick != null)
                        searchgroup.Title = unick.NickName;
                    else
                        searchgroup.Title = u.RealName_cn;
                    searchgroup.Content = u.GudWords;
                    searchgroup.Displayimg = CommonInfo.GetDisplayImg(u.DisplayImg); ;
                    searchgroup.ID = u.UID;
                    searchgroup.Date = u.CreateTime;
                    searchgroup.SearchType = "习练者";
                    searchgroup.Url = string.Format("/YogaUserDetail/Details/{0}", u.UID);
                    groups.Add(searchgroup);
                }
                if (listxlz != null && listxlz.Count > 0)
                    sb.Append("1,习练者|");
                else
                {
                    if (searchtype.Equals("1"))
                        sb.Append("1,习练者|");
                }
            }
            //导师2
            if (searchtype.Equals("0") || searchtype.Equals("2"))
            {
                List<ViewYogisModels> listds = new List<ViewYogisModels>();
                listds = client.GetYogisModelsList(urlContent, 0, 0, 0, 0, lp, level, gender, page, classsize, out count);
                if (searchtype.Equals("0"))
                    total += listds.Count();
                else
                    total = count;

                foreach (ViewYogisModels u in listds)
                {
                    searchgroup = new ViewSearchGroup();
                    searchgroup.Title = u.RealName;
                    searchgroup.Content = u.GudWords;
                    searchgroup.Displayimg = CommonInfo.GetDisplayImg(u.DisplayImg);
                    searchgroup.ID = u.UID;
                    searchgroup.Date = u.CreateDate;
                    searchgroup.SearchType = "导师";
                    if (u.YogisLevel == 4)
                    {
                        searchgroup.Url = string.Format("/Yogaguru/Details/{0}", u.UID);
                    }
                    else
                    {
                        searchgroup.Url = string.Format("/YogisModels/Details/{0}", u.UID);
                    }
                    groups.Add(searchgroup);
                }
                if (listds != null && listds.Count > 0)
                    sb.Append("2,导师|");
                else
                {
                    if (searchtype.Equals("2"))
                        sb.Append("2,导师|");
                }
            }

            //机构3
            if (searchtype.Equals("0") || searchtype.Equals("3"))
            {
                List<ViewCenters> listcenter = new List<ViewCenters>();
                using (CentersServiceClient clientcenter = new CentersServiceClient())
                {
                    listcenter = clientcenter.GetCentersPageList(urlContent, 0, 0, 0, 0, lp, "0", page, classsize, out count);
                }
                if (searchtype.Equals("0"))
                    total += listcenter.Count();
                else
                    total = count;
                foreach (ViewCenters u in listcenter)
                {
                    searchgroup = new ViewSearchGroup();
                    searchgroup.Title = u.CenterName;
                    searchgroup.Content = u.CenterProfile;
                    searchgroup.Displayimg = u.CenterPortraint;
                    searchgroup.ID = u.CenterId;
                    searchgroup.Date = u.CreateDate;
                    string centertype = string.Empty;
                    switch (u.CenterType)
                    {
                        case "1":
                            centertype = "会馆";
                            break;
                        case "2":
                            centertype = "学院";
                            break;
                        case "3":
                            centertype = "工作室";
                            break;
                        default:
                            break;
                    }
                    searchgroup.SearchType = centertype;
                    searchgroup.Url = string.Format("/Mechanism/Details/{0}", u.CenterId);
                    groups.Add(searchgroup);
                }
                if (listcenter != null && listcenter.Count > 0)
                    sb.Append("3,机构|");
                else
                {
                    if (searchtype.Equals("3"))
                        sb.Append("3,机构|");
                }
            }

            //文章4
            if (searchtype.Equals("0") || searchtype.Equals("4"))
            {
                List<ViewYogaArticle> listarticle = new List<ViewYogaArticle>();
                listarticle = clientArt.GetYogaArticlePageList(urlContent, datetime, page, classsize, out count);
                if (searchtype.Equals("0"))
                    total += listarticle.Count();
                else
                    total = count;
                foreach (ViewYogaArticle u in listarticle)
                {
                    searchgroup = new ViewSearchGroup();
                    searchgroup.Title = u.ArticleTitle;
                    searchgroup.Content = u.ArticleCon;
                    searchgroup.Displayimg = u.PicPath;
                    searchgroup.ID = u.ID;
                    searchgroup.Date = u.CreateTime;
                    searchgroup.SearchType = "文章";
                    searchgroup.Url = string.Format("/Login/ArticleDetail/{0}", u.ID);
                    groups.Add(searchgroup);
                }
                if (listarticle != null && listarticle.Count > 0)
                    sb.Append("4,文章|");
                else
                {
                    if (searchtype.Equals("4"))
                        sb.Append("4,文章|");
                }
            }
            //日志5
            if (searchtype.Equals("0") || searchtype.Equals("5"))
            {
                List<ViewtWriteLog> listlog = new List<ViewtWriteLog>();
                using (tWriteLogServiceClient clientlog = new tWriteLogServiceClient())
                {
                    listlog = clientlog.GettWriteLogPageList(0, urlContent, datetime, page, classsize, out count);
                }
                if (searchtype.Equals("0"))
                    total += listlog.Count();
                else
                    total = count;
                foreach (ViewtWriteLog u in listlog)
                {
                    searchgroup = new ViewSearchGroup();
                    searchgroup.Title = u.sTitle;
                    searchgroup.Content = u.sContent;
                    string tempdisplayimg = string.Empty;
                    ViewYogisModels models = client.GetYogisModelsById((int)u.Uid);
                    if (models != null)
                    {
                        tempdisplayimg = CommonInfo.GetDisplayImg(models.DisplayImg);
                    }
                    else
                    {
                        ViewYogaUserDetail ud = clienuser.GetYogaUserDetailById((int)u.Uid);
                        if (ud != null)
                        {
                            tempdisplayimg = CommonInfo.GetDisplayImg(ud.DisplayImg);
                        }
                    }
                    searchgroup.Displayimg = tempdisplayimg;
                    searchgroup.ID = u.ID;
                    searchgroup.Date = u.CreateDate;
                    searchgroup.SearchType = "日志";
                    searchgroup.Url = string.Format("/tWriteLog/Details/{0}", u.ID);
                    groups.Add(searchgroup);
                }
                if (listlog != null && listlog.Count > 0)
                    sb.Append("5,日志|");
                else
                {
                    if (searchtype.Equals("5"))
                        sb.Append("5,日志|");
                }
            }

            //活动6
            if (searchtype.Equals("0") || searchtype.Equals("6"))
            {
                int code = 10;//搜索
                List<ViewClass> listclass = new List<ViewClass>();
                string args = "" + urlContent + "," + topicid + ",";
                using (ClassServiceClient classClient = new ClassServiceClient())
                {
                    PageResult<ViewClass> classlist = classClient.GetClasses(code, page, classsize, args.Split(','));
                    listclass = classlist.Objects;
                }
                if (searchtype.Equals("0"))
                    total += listclass.Count();
                else
                    total = listclass.Count();

                foreach (ViewClass u in listclass)
                {
                    searchgroup = new ViewSearchGroup();
                    searchgroup.Title = u.Name;
                    searchgroup.Content = u.Summary;
                    string tempdisplayimg = string.Empty;

                    searchgroup.Displayimg = u.Banner;
                    searchgroup.ID = u.Id;
                    searchgroup.Date = u.CreateTime;
                    searchgroup.SearchType = "活动";
                    searchgroup.Url = string.Format("/class/viewactivity/{0}", u.Id);
                    groups.Add(searchgroup);
                }
                if (listclass != null && listclass.Count > 0)
                    sb.Append("6,活动|");
                else
                {
                    if (searchtype.Equals("6"))
                        sb.Append("6,活动|");
                }
            }
           

            //条件输出(保证在全部搜索时，筛选中保留筛选条件)
            if (searchtype.Equals("0"))
            {
                //设置缓存
                CacheHelper.SetCache("SearchResult", groups);
                if (datetime == null && gender == 2 && level == 0 && lp == 0)
                {
                    ViewBag.Puttyps = SetSearchCondition(sb.ToString());
                    CacheHelper.SetCache("SearchTypes", sb.ToString());
                }
                else
                {
                    if (CacheHelper.GetCache("SearchTypes") != null)
                    {
                        ViewBag.Puttyps = SetSearchCondition(CacheHelper.GetCache("SearchTypes").ToString());
                    }
                }
            }
            else
            {
                ViewBag.Puttyps = SetSearchCondition(sb.ToString());
            }
            //ViewBag.Puttyps = SetSearchCondition(sb.ToString());
            //条件分类
            ViewBag.SearchType = sb.ToString();

            //设置搜索条件


            //设置分页
            if (searchtype.Equals("0"))
            {
                groups = groups.OrderByDescending(g => g.Date).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }

            Webdiyer.WebControls.Mvc.PagedList<ViewSearchGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewSearchGroup>(groups, page, pagesize, total);
            ViewBag.Total = total;
            if (Request.IsAjaxRequest())
                return PartialView("_ArticleList", l);
            return View(l);
        }

        /// <summary>
        /// 得到检索条件，进行查询
        /// </summary>
        /// <param name="searchType">检索分类</param>
        private void SetSearchCondition(out string searchType, out DateTime? datetime, out int gender, out int level, out int lp,out int topicid)
        {
            //分类
            searchType = "0";
            if (!string.IsNullOrEmpty(Request.Form["hid_searchtype"]))
            {
                searchType = Request.Form["hid_searchtype"];
            }
            //性别
            gender = 2;
            if (!string.IsNullOrEmpty(Request.Form["hidGender"]))
            {
                gender = Convert.ToInt32(Request.Form["hidGender"]);
            }
            //级别
            level = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidlevel"]))
            {
                level = Convert.ToInt32(Request.Form["hidlevel"]);
            }
            //流派
            lp = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidmovement"]))
            {
                lp = Convert.ToInt32(Request.Form["hidmovement"]);
            }
            //form中获得值name，值在xml中配置
            datetime = null;
            if (!string.IsNullOrEmpty(Request.Form["hidcreatedate"]))
            {
                string d = Request.Form["hidcreatedate"];
                switch (d)
                {
                    case "1d":
                        datetime = DateTime.Now.AddDays(-1);
                        break;
                    case "2d":
                        datetime = DateTime.Now.AddDays(-2);
                        break;
                    case "3d":
                        datetime = DateTime.Now.AddDays(-2);
                        break;
                    case "1w":
                        datetime = DateTime.Now.AddDays(-7);
                        break;
                    case "1m":
                        datetime = DateTime.Now.AddMonths(-1);
                        break;
                    case "3m":
                        datetime = DateTime.Now.AddMonths(-3);
                        break;
                    case "6m":
                        datetime = DateTime.Now.AddMonths(-6);
                        break;
                    case "1y":
                        datetime = DateTime.Now.AddYears(-1);
                        break;
                    case "3y":
                        datetime = DateTime.Now.AddYears(-3);
                        break;
                    default:
                        break;
                }
            }
            //课程主题
            topicid = 0;
            if (!string.IsNullOrEmpty(Request.Form["hidtopicid"]))
            {
                topicid = Convert.ToInt32(Request.Form["hidtopicid"]);
            }
        }

        /// <summary>
        /// 设置检索分类条件,输出
        /// </summary>
        /// <param name="searchType">检索值</param> 
        /// <param name="searchType">检索分类</param>
        public List<ViewSearchTypeGroup> SetSearchCondition(string searchtype)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(Server.MapPath("/Content/XML/SearchCondition.xml"));
            XmlNode root = xd.DocumentElement;

            List<ViewSearchTypeGroup> types = new List<ViewSearchTypeGroup>();
            List<ViewTypes> childtypes;
            ViewSearchTypeGroup t;
            ViewTypes childt;
            string[] arrtype = searchtype.Split('|');
            //保证只留重复项
            bool isOvertwo = false;
            if (arrtype.Length > 2)
            {
                isOvertwo = true;
            }
            string arrtypechi;
            for (int i = 0; i < arrtype.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrtype[i]))
                {
                    //编号
                    arrtypechi = arrtype[i].Split(',')[0];

                    //过滤相同项
                    bool iscontains = true;
                    foreach (ViewSearchTypeGroup tempt in types)
                    {
                        XmlNodeList type = root.SelectSingleNode("//SearchType//T[@id='" + arrtypechi + "']").ChildNodes;
                        foreach (XmlNode childnode in type)
                        {
                            if (tempt.id.Equals(childnode.InnerText))
                            {
                                tempt.isKep += tempt.isKep;
                                iscontains = false;
                                break;
                            }
                            else
                            {
                                // tempt.isKep = 1;
                            }
                        }
                    }
                    //分类
                    if (iscontains)
                    {
                        XmlNodeList type = root.SelectSingleNode("//SearchType//T[@id='" + arrtypechi + "']").ChildNodes;
                        foreach (XmlNode node in type)
                        {
                            XmlNode zhonglei = root.SelectSingleNode("//SearchClass//C[@id='" + node.InnerText + "']");
                            if (zhonglei != null)
                            {
                                t = new ViewSearchTypeGroup();
                                childtypes = new List<ViewTypes>();
                                t.id = zhonglei.Attributes["id"].Value;
                                t.Title = zhonglei.Attributes["name"].Value;
                                t.type = zhonglei.Attributes["type"].Value;
                                t.hidname = zhonglei.Attributes["hidname"].Value; //动态赋值
                                //分类选项
                                foreach (XmlNode childenode in zhonglei.ChildNodes)
                                {
                                    childt = new ViewTypes();
                                    //设置选中状态
                                    if (!string.IsNullOrEmpty(Request.Form[zhonglei.Attributes["hidname"].Value]))
                                    {
                                        string s = Request.Form[zhonglei.Attributes["hidname"].Value];
                                        if (s.Equals(childenode.Attributes["value"].Value))
                                        {
                                            childt.on = "on";
                                            t.hidValue = s;
                                        }
                                    }
                                    else
                                    {
                                        childt.on = childenode.Attributes["selected"] == null ? "" : childenode.Attributes["selected"].Value;
                                    }
                                    childt.name = childenode.InnerText;
                                    childt.vlaue = childenode.Attributes["value"].Value;
                                    childtypes.Add(childt);
                                }
                                t.ts = childtypes;
                                t.isKep = 1;
                                types.Add(t);
                            }
                        }
                    }
                }
            }

            //过滤非共同项
            if (isOvertwo)
            {
                return types.Where(p => p.isKep == arrtype.Length - 1).ToList();
            }
            else
            {
                return types.ToList();
            }
        }

        /// <summary>
        /// 点击分页操作，读取缓存数据不重新检索
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchView(int page = 1)
        {
            string pagerOrcondition = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["hid_pagerorcondition"]))
            {
                pagerOrcondition = Request.Form["hid_pagerorcondition"];
            }
            //搜索类型
            string searchtype = Request.Form["hid_searchtype"];
            //只有是全部搜索、点击分页操作时从缓存中读取数据
            if (!pagerOrcondition.Equals("condition") || (searchtype.Equals("0") && page != 1))
            {
                if (CacheHelper.GetCache("SearchTypes") != null)
                {

                    ViewBag.Puttyps = SetSearchCondition(CacheHelper.GetCache("SearchTypes").ToString());//重新设置检索条件,需要以后修改为合理方案

                    int pagesize = 8;
                    object obj = CacheHelper.GetCache("SearchResult");
                    List<ViewSearchGroup> groups = (List<ViewSearchGroup>)obj;
                    int total = groups.Count;
                    ViewBag.Total = total;
                    if (obj != null)
                    {
                        groups = groups.OrderByDescending(g => g.Date).Skip((page - 1) * pagesize).Take(pagesize).ToList();
                        Webdiyer.WebControls.Mvc.PagedList<ViewSearchGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewSearchGroup>(groups, page, pagesize, total);
                        if (Request.IsAjaxRequest())
                            return PartialView("_ArticleList", l);
                        return RedirectToAction("Search");
                    }
                    else
                    {
                        return RedirectToAction("Search");
                    }
                }
                else
                {
                    return RedirectToAction("Search");
                }
            }
            else
            {
                string strWhere = Request.Form["hid_strwhere"];
                //重新筛选
                return SearchMethod(strWhere, page);
            }
        }


        /// <summary>
        ///  查询列表页
        /// </summary>
        /// <param name="iType">1找活动  2找导师  3找同修  4找资料</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult SearchList(int id, int page = 1)
        {
            //int count = 0;
            //string hidContnet = "";
            //if (!string.IsNullOrEmpty(Request.Form["hidContnet"]))
            //{
            //    hidContnet = Request.Form["hidContnet"].ToString();
            //}

            //int Gender = 2;
            //if (!string.IsNullOrEmpty(Request.Form["Gender"]))
            //{
            //    Gender = Convert.ToInt32(Request.Form["Gender"]);
            //}
            //int YogisLevel = 0;
            //if (!string.IsNullOrEmpty(Request.Form["YogisLevel"]))
            //{
            //    YogisLevel = Convert.ToInt32(Request.Form["YogisLevel"]);
            //}
            //string YogaTypeid = "";
            //if (!string.IsNullOrEmpty(Request.Form["YogaTypeid"]))
            //{
            //    YogaTypeid = Request.Form["YogaTypeid"].ToString();
            //}


            //if (!string.IsNullOrEmpty(Request.Form["id"]))
            //{
            //    id = Convert.ToInt32(Request.Form["id"]);
            //}
            //int hidiType = 0;
            //if (!string.IsNullOrEmpty(Request.Form["hidiType"]))
            //{
            //    hidiType = Convert.ToInt32(Request.Form["hidiType"]);
            //}


            //List<ViewYogisModels> list = new List<ViewYogisModels>();

            //if (hidiType != 0)
            //{
            //    #region

            //    switch (id)
            //    {
            //        case 0:

            //            break;
            //        case 1:
            //            // 找同修  
            //            List<ViewYogaUserDetail> listuser = new List<ViewYogaUserDetail>();

            //            listuser = clienuser.GetYogaUserList(hidContnet, Gender, YogaTypeid, hidiType, page, 10, out count);
            //            PagedList<ViewYogaUserDetail> pagelistuDetails = new PagedList<ViewYogaUserDetail>(listuser, page, 10, count);
            //            ViewBag.hidid = 1;
            //            ViewBag.listGroup = pagelistuDetails;
            //            ViewBag.Title = "同修";
            //            break;
            //        case 2:

            //            #region

            //            // 找导师

            //            list = client.GetYogisModelsList(hidContnet, Gender, YogisLevel, YogaTypeid, hidiType, page, 10, out count);
            //            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            //            ViewBag.hidid = 2;
            //            ViewBag.listGroup = pagelist;
            //            ViewBag.Title = "导师";
            //            #endregion

            //            break;

            //        case 3:

            //            break;
            //        case 4:


            //            break;
            //    }

            //    #endregion
            //}
            //else
            //{
            //    switch (id)
            //    {
            //        case 0:

            //            break;
            //        case 1:
            //            // 找同修  
            //            List<ViewYogaUserDetail> listuser = new List<ViewYogaUserDetail>();

            //            listuser = clienuser.GetYogaUserList(hidContnet, Gender, YogaTypeid, page, 10, out count);
            //            PagedList<ViewYogaUserDetail> pagelistuDetails = new PagedList<ViewYogaUserDetail>(listuser, page, 10, count);
            //            ViewBag.hidid = 1;
            //            ViewBag.listGroup = pagelistuDetails;
            //            ViewBag.Title = "同修";
            //            break;
            //        case 2:

            //            #region

            //            // 找导师

            //            list = client.GetYogisModelsList(hidContnet, Gender, YogisLevel, YogaTypeid, page, 10, out count);
            //            PagedList<ViewYogisModels> pagelist = new PagedList<ViewYogisModels>(list, page, 10, count);

            //            ViewBag.hidid = 2;
            //            ViewBag.listGroup = pagelist;
            //            ViewBag.Title = "导师";

            //            #endregion

            //            break; 

            //        case 3:  
            //            //活动
            //            PageResult<ViewClass> pr = clientClass.GetClasses(-1, page, 10, new string[] {hidContnet,YogaTypeid });
            //            PagedList<ViewClass> classPage = new PagedList<ViewClass>(pr.Objects, page, 10,pr.RecordCount);
            //            ViewBag.hidid = 3;
            //            ViewBag.listGroup = classPage;
            //            ViewBag.Title = "活动";
            //         break; 
            //        case 3:

            //                //活动
            //                PageResult<ViewClass> pr = clientClass.GetClasses(-1, page, 10, new string[] { hidContnet, YogaTypeid });
            //                PagedList<ViewClass> classPage = new PagedList<ViewClass>(pr.Objects, page, 10, pr.RecordCount);
            //                ViewBag.hidid = 3;
            //                ViewBag.listGroup = classPage;
            //                ViewBag.Title = "活动";
            //             break; 
            //        case 4:

            //            //找资料
            //            List<ViewYogaArticle> listArticle = new List<ViewYogaArticle>();

            //            listArticle = clientArt.GetYogaArticlePageList(hidContnet, page, 10, out count);
            //            PagedList<ViewYogaArticle> pagelistArt = new PagedList<ViewYogaArticle>(listArticle, page, 10, count);

            //            ViewBag.hidid = 4;
            //            ViewBag.listGroup = pagelistArt;
            //            ViewBag.Title = "资料";
            //            break;
            //    }

            //} 
            //switch (id)
            //{
            //    case 1:
            //    case 2:
            //        return PartialView("_ArticleList", ViewBag.listGroup);
            //}

            return PartialView("_ArticleList");


            //  return PartialView("SearchList");

        }
        //
        // GET: /Home/Details/5
        /// <summary>
        /// 退出
        /// </summary>
        public void Exitlogin()
        {
            ViewtUserLoginInfo userloginInfo = new ViewtUserLoginInfo();
            try
            { 
                string cookiename_login = "iyoga";
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookiename_login];

                if (cookie == null)
                {
                    userloginInfo = method.GetLoginInfo(user.Uid,user.NickName, UserLogin.退出成功.ToString(),false); 
                    userloginclient.Add(userloginInfo);

                    Tools.WriteTextLog("Login", user.NickName + " ： 退出登录");
                    Response.Redirect("~/Home/Index");
                }
                else
                {
                    userloginInfo = method.GetLoginInfo(user.Uid, user.NickName, UserLogin.退出成功.ToString(), false); 
                    userloginclient.Add(userloginInfo);
                    Tools.WriteTextLog("Login", user.NickName + " ： 退出登录");
                    DateTime dt = cookie.Expires;
                    Response.Cookies["iyoga"].Expires = DateTime.Now.AddDays(-1);

                    Response.Redirect("~/Home/Index");
                }
            }
            catch
            {
                userloginInfo = method.GetLoginInfo(user.Uid, user.NickName, UserLogin.退出失败.ToString(), false); 
                userloginclient.Add(userloginInfo);
            }
        }
        /// <summary>
        /// 是否已经提交过升级导师审核
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string isTeachProf(string uid)
        {
            if (uid != "" && uid != null)
            {
                LevelOrderServiceClient client = new LevelOrderServiceClient();
                ViewLevelOrder model = new ViewLevelOrder();
                model = client.GetUid(int.Parse(uid));
                if (model != null)
                {
                    if (model.OrderState == "申请中")
                        return "1";
                    else
                        return "0";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        public string isTeachProf2(string uid)
        {
            ///以前的
            if (uid != "" && uid != null)
            {
                YogiProfileServiceClient client = new YogiProfileServiceClient();
                ViewYogiProfile model = new ViewYogiProfile();
                model = client.GetYogiProfileById(int.Parse(uid));
                if (model != null)
                    return "1";
                else
                    return "0";
            }
            return "0";
        }
        /// <summary>
        /// 检查身份证号码
        /// </summary>        
        [HttpPost]
        public JsonResult ExistIdCard(string idcardnum)
        {
            ViewYogisModels model = new ViewYogisModels();
            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                model = client.ExistIdCard(idcardnum);
                if (model != null)
                    return Json(new { code = 0 });
                else
                    return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 申请成为导师页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyTeachAgreement()
        {
            ViewBag.user = user;
            List<ViewYogaArticle> alist = new List<ViewYogaArticle>();
            using (YogaArticleServiceClient client = new YogaArticleServiceClient())
            {
                alist = client.GetYogaArticleClassID(9);
            }
            ViewBag.Artlist = alist;
            ViewYogaArticle model = new ViewYogaArticle();
            if (Request.QueryString["id"] != null)
            {
                int id = int.Parse(Request.QueryString["id"]);
                if (id != 0)
                {
                    using (YogaArticleServiceClient client = new YogaArticleServiceClient())
                    {
                        model = client.GetYogaArticleById((int)id);
                    }
                }
            }
            else//点升级导师，没id的情况
            {
                using (YogaArticleServiceClient client = new YogaArticleServiceClient())
                {
                    model = client.GetYogaArticleById(16);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyTeachAgreement(int? id)
        {
            if (!string.IsNullOrEmpty(Request.Form["isAgree"]))
            {
                ViewYogisModels model = new ViewYogisModels();

                using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                {
                    #region

                    model = client.GetYogisModelsById(user.Uid);
                    if (model == null && user.Uid != 0)
                    {
                        //添加 ： 先取userDetails中的  

                        ViewYogaUserDetail userDetails = new ViewYogaUserDetail();
                        using (YogaUserDetailServiceClient clientDetails = new YogaUserDetailServiceClient())
                        {
                            userDetails = clientDetails.GetYogaUserDetailById(user.Uid);
                            model = new ViewYogisModels();

                            model.UID = userDetails.UID;
                            model.Gender = userDetails.Gender;
                            model.DisplayImg = userDetails.DisplayImg;
                            model.RealName = userDetails.RealName_cn;
                            model.CountryID = userDetails.CountryID;
                            model.ProvinceID = userDetails.ProvinceID;
                            model.CityID = userDetails.CityID;
                            model.DistrictID = userDetails.DistrictID;
                            model.Nationality = userDetails.Nationality == null ? "" : userDetails.Nationality.Value.ToString();
                            model.CreateDate = DateTime.Now;
                            model.YogaTypeid = userDetails.YogaTypeid;
                            model.Street = userDetails.Street;
                            model.IsAcceptAgreement = true;
                            model.CenterID = "0";
                            model.TeachYogis = "0";

                            client.Add(model);//设置同意
                        }

                    }
                    #endregion

                }
                return RedirectToAction("ApplyPerfect", "Home", new { id = model.UID });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 完善导师信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyPerfect(int id)
        {
            ViewBag.user = user;
            ViewYogisModels model = new ViewYogisModels();
            ViewYogaUserDetail udmodel = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient uclient = new YogaUserDetailServiceClient())
            {
                udmodel = uclient.GetYogaUserDetailById(id);
            }
            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                model = client.GetYogisModelsById(id);
                if (model.CountryID == null || model.CountryID == 0)
                {
                    model.CountryID = udmodel.CountryID;
                }
                if (model.ProvinceID == null || model.ProvinceID == 0)
                {
                    model.ProvinceID = udmodel.ProvinceID;
                }
                if (model.CityID == null || model.CityID == 0)
                {
                    model.CityID = udmodel.CityID;
                }
                if (model.DistrictID == null || model.DistrictID == 0)
                {
                    model.DistrictID = udmodel.DistrictID;
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
            }
            return View(model);

        }
        [HttpPost, ValidateInput(false)]
        public JsonResult ApplyPerfect(ViewYogisModels model)
        {
            try
            {
                using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                {
                    model = client.GetYogisModelsById(model.UID);
                    model.YID = Convert.ToInt32(Request.Form["YID"]);
                    model.RealName = Request.Form["RealName"].ToString();
                    model.Gender = Request.Form["Gender"] == null ? 0 : Convert.ToInt32(Request.Form["Gender"].ToString());
                    model.IdType = Request.Form["IdType"].ToString();
                    model.IdCardNum = Request.Form["IdCardNum"].ToString();
                    model.CenterID = Request.Form["hCenterID"].ToString().TrimEnd(',') == "" ? Request.Form["CenterID"].ToString().TrimEnd(',') : Request.Form["hCenterID"].ToString().TrimEnd(',');
                    model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                    model.TeachYogis = Request.Form["hTeachYogis"].ToString().TrimEnd(',') == "" ? Request.Form["TeachYogis"].ToString().TrimEnd(',') : Request.Form["hTeachYogis"].ToString().TrimEnd(',');
                    model.YogisDepict = Request.Form["YogisDepict"].ToString();
                    model.StartTeachYear = Convert.ToDateTime(Request.Form["StartTeachYear"].ToString());
                    model.Nationality = Request.Form["ddlNationality"].ToString();
                    model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
                    model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
                    model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
                    model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
                    model.Street = Request.Form["Street"].ToString();
                    model.UpgradeDate = DateTime.Now;
                    model.YogiStatus = 0;//申请中
                    model.IsAcceptAgreement = true;
                    model.YogisLevel = Convert.ToInt32(Request.Form["YogisLevel"].ToString());
                    if (model.YogisLevel == 1)
                    {
                        model.Leveldetails = "10";
                    }
                    else if (model.YogisLevel == 2)
                    {
                        model.Leveldetails = "15";
                    }
                    else if (model.YogisLevel == 3)
                    {
                        model.Leveldetails = "20";
                    }
                    else
                    {
                        model.Leveldetails = "10";
                    }
                    model.delState = 0;
                    client.Update(model); 
                }
                //添加到站内信-qiqi 2015-12-18
                using (tInstationInfoServiceClient tinclient = new tInstationInfoServiceClient())
                {
                    ViewYogaDicItem dicEntity = dicclient.GetById(2567);
                    ViewtInstationInfo tinEntity = new ViewtInstationInfo();
                    tinEntity.Uid = user.Uid;
                    tinEntity.CreateTime = DateTime.Now;
                    tinEntity.CreateUser = user.Uid.ToString();
                    tinEntity.ifDel = false; 
                    tinEntity.loginType = 0;
                    tinEntity.iType = dicEntity.ID ;
                    tinEntity.sContent = dicEntity.ItemValue;
                    tinclient.Add(tinEntity);
                }
                return Json(new { code = 0 });
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("Login", ex.Message);
                return Json(new { code = 1 });
            }
        }

        public ActionResult BottomNavigation(int id)
        {
            ViewBag.BottomID = id;
            List<ViewYogaArticle> list=clientArt.GetYogaArticleClassID(8);
            foreach (ViewYogaArticle i in list)
            {
                if (i.ArticleTitle.Trim() == "加入我们")
                    ViewBag.JoinUs = i;
                if (i.ArticleTitle.Trim() == "关于我们")
                    ViewBag.AboutUs = i;
                if (i.ArticleTitle.Trim() == "联系我们")
                    ViewBag.ContactUs = i;
            }
            return View();
        }
         
    }
}
