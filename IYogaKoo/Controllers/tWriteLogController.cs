using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class tWriteLogController : Controller
    {
        //
        // GET: /tWriteLog/
        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();

        YogaUserServiceClient clientUser;
        List<ViewtWriteLog> list;
        tWriteLogServiceClient logClient;
        YogisModelsServiceClient client;
        ClassServiceClient classclient;
        YogaUserDetailServiceClient udclient;
        tMessageServiceClient clientMsg;
        tLearingServiceClient learclient;
        YogaPictureServiceClient picclient;
        method method;
        tZanModelsServiceClient zanclient;
        ClassReportServiceClient classRepotclient;
        public tWriteLogController()
        {
            ViewBag.user = user;
            clientUser = new YogaUserServiceClient();
            list = new List<ViewtWriteLog>();
            logClient = new tWriteLogServiceClient();
            client = new YogisModelsServiceClient();
            classclient = new ClassServiceClient();
            udclient = new YogaUserDetailServiceClient();
            clientMsg = new tMessageServiceClient();
            picclient = new YogaPictureServiceClient();
            classclient = new ClassServiceClient();
            method = new method();
            classRepotclient = new ClassReportServiceClient();
            zanclient = new tZanModelsServiceClient();
            learclient = new tLearingServiceClient();
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
        public ActionResult Index(int? uid, int page = 1)
        {

            int count = 0;
            if (uid != null)
            {
                list = logClient.GettWriteLogPageList(uid.Value, page, 10, out count);
            }
            else
            {
                list = logClient.GettWriteLogPageList(user.Uid, page, 10, out count);
            }
            PagedList<ViewtWriteLog> pagelist = new PagedList<ViewtWriteLog>(list, page, 10, count);

            List<ViewtWriteLogGroup> listGroup = new List<ViewtWriteLogGroup>();
            foreach (var item in list)
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
                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            return View(pagelist);
        }

        public ActionResult logIndex(int year = 0, int month = 0, int page = 1)
        {
            int count = 0;
            int pagesize = 14;
            ViewBag.year = year;
            ViewBag.month = month;

            list = logClient.GettWriteLogPageList(user.Uid, year, month, page, pagesize, out count);

            if (year == 0)
            {
                list = logClient.GettWriteLogPageList(user.Uid, page, 20, out count);
            }
            ViewBag.id = user.Uid;
            Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog>(list, page, pagesize, count);

            List<ViewtWriteLogGroup> listGroup = new List<ViewtWriteLogGroup>();
            foreach (var item in list)
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
                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            if (Request.IsAjaxRequest())
            {
                return PartialView("logIndexList", pagelist);
            }
            return View(pagelist);

        }


        /// <summary>
        /// 日历控件获取当前月发布文章的天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns> 
        public ActionResult getCalandDay(int year, int month)
        {
            StringBuilder output = new StringBuilder("");

            DateTime dt = System.DateTime.Now;

            list = logClient.GettWriteLogPageList(user.Uid, year, month);
            int i = 0;

            output.Append("new Array(");

            foreach (var cc in list)
            {

                if (i == 0)

                    output.Append("{ hDongD:" + cc.CreateDate.Value.Day + ", hDongUrl: '/tWriteLog/logIndex?year=" + cc.CreateDate.Value.Year + "&month=" + cc.CreateDate.Value.Month + "&day=" + cc.CreateDate.Value.Day + "' }");

                else
                {

                    string curDay = cc.CreateDate.Value.Day.ToString();

                    string resultDay = output.ToString();

                    string[] str = resultDay.Split(','); //得到一个str的数组{“1”，”2“，“3”，”4“，“5”，”6“}

                    Boolean c = true;

                    foreach (string s in str)
                    {

                        if (s == curDay) c = false;

                    }

                    if (c)
                    {

                        // output.Append(",{ hDongD:" + cc.CreateDate.Value.Day + ", hDongUrl: '/" + cc.ID + "--" + cc.CreateDate.Value + "' }");
                        output.Append(",{ hDongD:" + cc.CreateDate.Value.Day + ", hDongUrl: '/tWriteLog/logIndex?year=" + cc.CreateDate.Value.Year + "&month=" + cc.CreateDate.Value.Month + "&day=" + cc.CreateDate.Value.Day + "' }");

                    }

                }

                i++;

            }

            output.Append(");");

            return Json(output.ToString());

            //return Json("new Array({ hDongD: 4, hDongUrl: 'http://www.jb51.net' },  { hDongD: 14, hDongUrl: 'http://play.jb51.net' });");

        }

        //
        // GET: /tWriteLog/Details/5

        public ActionResult Details(int id)
        {

            //ViewtWriteLog model=  logClient.GetById(id);
            //if (model != null)
            //{
            //    ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
            //    return View(model);
            //}
            #region 上一篇文章和下一篇文章
            ViewtWriteLog model = logClient.GetById(id);
            if (model != null)
            {
                //ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
                //用pre和next变量分别存放上一篇文章和下一篇文章的id号
                int pre = 0, next = 0, i = 0, j;
                //计算总记录数
                int num = logClient.GettWriteLogQuiltUidList(user.Uid).Count();
                int[] a = new int[num];
                var query = from c in logClient.GettWriteLogQuiltUidList(user.Uid) select c.ID;
                //将所有的文章id号全部放入一个数组中
                foreach (var item in query)
                {
                    a[i] = Convert.ToInt32(item);
                    i++;
                }
                //循环，获取上一篇和下一篇文章的ID号，分别放入变量pre和next中
                for (j = 0; j < num; j++)
                {
                    if (a[j] == id)
                    {
                        if (j != 0) pre = a[j - 1];
                        if (j != num - 1) next = a[j + 1];
                    }
                }
                //获取上一篇文章的标题
                if (pre == 0)
                {
                    ViewBag.preTitle = "已经是第一篇";
                    ViewBag.pre = id;
                }
                else
                {
                    ViewBag.preTitle = logClient.GettWriteLogQuiltUidList(user.Uid).Where(c => c.ID == pre).Single().sTitle;
                    ViewBag.pre = pre;
                }
                //获取下一篇文章的标题
                if (next == 0)
                {
                    ViewBag.nextTitle = "已经是最后一篇";
                    ViewBag.next = id;
                }
                else
                {
                    ViewBag.nextTitle = logClient.GettWriteLogQuiltUidList(user.Uid).Where(c => c.ID == next).Single().sTitle;
                    ViewBag.next = next;
                }

                return View(model);
            }
            #endregion
            return View();
        }
        /// <summary>
        /// 查看其他人相册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OtherPics(int id, int imgtypeid = -1)
        {
            ViewBag.imgtypeid = imgtypeid;
            ViewBag.otherGender = method.Gender(id);
            ViewBag.id = id;//专页用户编号           
            int usertype = clientUser.GetYogaUserById(id).UserType.Value;
            if (usertype == 1)
            {
                ViewYogisModels tempm = new ViewYogisModels();
                tempm = client.GetYogisModelsById(id);
                ViewBag.YogisLevel = tempm.YogisLevel;//是否 大师=4
            }
            using (FollowServiceClient clientFoll = new FollowServiceClient())
            {
                ViewFollow vm = clientFoll.GetFollowById(user.Uid, id);
                ViewBag.isfollow = vm == null ? false : vm.isfollow;
            }

            #region

            PicListGroup entity = new PicListGroup();

            List<PicListGroup> list = new List<PicListGroup>();

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + id);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = id.ToString();
                entity.pictitle = subdir.Name;
                List<ViewYogaPicture> piclist = picclient.GetBackUidList(id, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else
                {
                    entity.picsrc = "../../Content/Front/images/filebackimg.png";
                }
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" + id + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                list.Add(entity);
            }
            #endregion
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = picclient.GetUidList(id);
            ViewBag.piclistAll = piclistAll;

            return View(list.Take(4).ToList());

        }

        public ActionResult OtherPicsDetails(string pirctureName, int id)
        {
            ViewBag.pictitle = pirctureName;
            ViewBag.id = id;//专页用户编号     
            int usertype = clientUser.GetYogaUserById(id).UserType.Value;
            using (FollowServiceClient clientFoll = new FollowServiceClient())
            {
                ViewFollow vm = clientFoll.GetFollowById(user.Uid, id);
                ViewBag.isfollow = vm == null ? false : vm.isfollow;
            }
            List<ViewYogaPicture> picsList = null;
            using (YogaPictureServiceClient picClient = new YogaPictureServiceClient())
            {
                picsList = picClient.GetBackUidList(id, pirctureName);// picClient.GetUidList(id); 
            }
            //start
            PicListGroup entity = new PicListGroup();

            List<PicListGroup> listGroup = new List<PicListGroup>();

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + id);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = id.ToString();
                entity.pictitle = subdir.Name;
                List<ViewYogaPicture> piclist = picclient.GetBackUidList(id, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else
                {
                    entity.picsrc = "../../Content/Front/images/filebackimg.png";
                }
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" + id + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                listGroup.Add(entity);
            }
            ViewBag.listGroup = listGroup.Take(4).ToList();
            //end
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = picclient.GetUidList(id);
            ViewBag.piclistAll = piclistAll;
            return View(picsList);
        }
        public ActionResult OtherPicsFileInfo(int id)
        {
            ViewBag.id = id;//专页用户编号   
            ViewBag.otherGender = method.Gender(id);

            PicListGroup entity = new PicListGroup();
            List<PicListGroup> list = new List<PicListGroup>();
            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + id);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = id.ToString();
                entity.pictitle = subdir.Name;
                List<ViewYogaPicture> piclist = picclient.GetBackUidList(id, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else
                {
                    entity.picsrc = "../../Content/Front/images/filebackimg.png";
                }
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" +id + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                list.Add(entity);
            }
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = picclient.GetUidList(id);
            ViewBag.piclistAll = piclistAll;

            return View(list);
        }

        /// <summary>
        /// 查看其他人活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OtherClass(int uid, int type = 1)
        {
            ViewBag.otherGender = method.Gender(uid);
            ViewBag.id = uid;
            int usertype = clientUser.GetYogaUserById(uid).UserType.Value;
            if (usertype == 1)
            {
                ViewYogisModels tempm = new ViewYogisModels();
                tempm = client.GetYogisModelsById(uid);
                ViewBag.YogisLevel = tempm.YogisLevel;//是否 大师=4
            }
            List<ViewClass> classess = new List<ViewClass>();
            List<ViewInterestedClass> Inteseclassess = new List<ViewInterestedClass>();
            List<ViewOrder> orders = new List<ViewOrder>();
            //我发起的活动
            using (ClassServiceClient classclient = new ClassServiceClient())
            {
                classess = classclient.GetClassesByUid(uid);
                //测试用
                //classess = classclient.GetClassesByUid(100316);
            }
            //我参加的活动
            using (OrderServiceClient sc = new OrderServiceClient())
            {
                orders = sc.GetOrdersByuid(uid);
                //测试用
                //orders = sc.GetOrdersByuid(100290);
            }
            //我感兴趣的活动
            using (InterestServiceClient classclient = new InterestServiceClient())
            {
                Inteseclassess = classclient.GetListClassByUId(uid);
                //测试用
                //Inteseclassess = classclient.GetListClassByUId(100321);
            }

            /*个数*/
            ViewBag.count1 = classess.Count();
            ViewBag.count2 = orders.Count();
            ViewBag.count3 = Inteseclassess.Count();
            //我发起的活动
            if (type == 1)
            {
                OrderServiceClient sc = new OrderServiceClient();

                foreach (ViewClass vo in classess)
                {
                    vo.IfEdit = false;
                    vo.IfDel = false;
                    vo.IfReback = false;
                }
            }

            //我参加的活动
            if (type == 2)
            {
                classess = new List<ViewClass>();
                ViewClass vc = null;
                foreach (ViewOrder vo in orders)
                {
                    vc = new ViewClass();
                    vc.Id = vo.Class.Id;
                    vc.Banner = vo.Class.Banner;
                    vc.Name = vo.Class.Name;
                    vc.IfDel = false;
                    vc.IfEdit = false;
                    vc.IfReback = false;
                    classess.Add(vc);
                }
            }


            //我感兴趣的活动    
            if (type == 3)
            {
                classess = new List<ViewClass>();
                ViewClass vc = null;
                foreach (ViewInterestedClass ic in Inteseclassess)
                {
                    vc = new ViewClass();
                    vc.Id = ic.Class.Id;
                    vc.Banner = ic.Class.Banner;
                    vc.Name = ic.Class.Name;
                    vc.IfDel = false;
                    vc.IfEdit = false;
                    vc.IfReback = false;
                    classess.Add(vc);
                }
            }
            if (Request.IsAjaxRequest())
            { return PartialView("OtherClassList", classess); }
            else
            { return View(classess); }

        }
        #region 登录人的活动,编辑，删除
        /// <summary>
        /// 本人活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyOtherClass(int type = 1, int page = 1)
        {

            //表示是订单分页
            if (page != 1) type = 4;

            ViewBag.id = user.Uid;
            List<ViewClass> classess = new List<ViewClass>();
            List<ViewInterestedClass> Inteseclassess = new List<ViewInterestedClass>();
            List<ViewOrder> orders = new List<ViewOrder>();
            //我发起的活动
            using (ClassServiceClient classclient = new ClassServiceClient())
            {
                classess = classclient.GetClassesByUid(user.Uid);
                //测试用
                //classess = classclient.GetClassesByUid(100316);
            }
            //我参加的活动
            using (OrderServiceClient sc = new OrderServiceClient())
            {
                orders = sc.GetOrdersByuid(user.Uid);
                //测试用
                //orders = sc.GetOrdersByuid(100290);
            }
            //我感兴趣的活动
            using (InterestServiceClient classclient = new InterestServiceClient())
            {
                Inteseclassess = classclient.GetListClassByUId(user.Uid);
                //测试用
                //Inteseclassess = classclient.GetListClassByUId(100321);
            }

            #region 订单
            string whereStr = "";

            string Phone = "";
            if (!string.IsNullOrEmpty(Request.Form["Phone"]))
            {
                Phone = Request.Form["Phone"].ToString().Trim();
                whereStr += "Phone!" + Phone + ",";
            }

            string CreateTime = "";
            if (!string.IsNullOrEmpty(Request.Form["CreateTime"]))
            {
                CreateTime = Request.Form["CreateTime"].ToString();
                whereStr += "CreateTime!" + CreateTime + ",";
            }
            string EndTime = "";
            if (!string.IsNullOrEmpty(Request.Form["EndTime"]))
            {
                EndTime = Request.Form["EndTime"].ToString();
                whereStr += "EndTime!" + EndTime + ",";
            }
            int UserId = user.Uid;
            whereStr += "UserId!" + UserId + ",";

            OrderServiceClient client = new OrderServiceClient();
            int pagesize = 12;
            int count = 0;
            var list = client.GetOrder(whereStr, page, pagesize, out count);
            ViewBag.count4 = count;

            #endregion

            /*个数*/
            ViewBag.count1 = classess.Count();
            ViewBag.count2 = orders.Count();
            ViewBag.count3 = Inteseclassess.Count();
            //我发起的活动
            if (type == 1)
            {
                OrderServiceClient sc = new OrderServiceClient();

                foreach (ViewClass vo in classess)
                {
                    vo.IfEdit = false;
                    vo.IfDel = false;
                    vo.IfReback = false;
                    //已经有人订单
                    if (sc.GetOrdersByclassid(vo.Id) == 0)
                    {
                        if (vo.ClassStatus == (int)ClassStatus.审核中)
                        {
                            vo.IfDel = true;
                            vo.IfReback = true;
                        }
                        vo.IfEdit = true;
                    }

                }
            }

            //我参加的活动
            if (type == 2)
            {
                classess = new List<ViewClass>();
                ViewClass vc = null;
                foreach (ViewOrder vo in orders)
                {
                    vc = new ViewClass();
                    vc.Id = vo.Class.Id;
                    vc.Banner = vo.Class.Banner;
                    vc.Name = vo.Class.Name;
                    vc.IfDel = true;
                    vc.IfEdit = false;
                    vc.IfReback = false;
                    classess.Add(vc);
                }
            }


            //我感兴趣的活动    
            if (type == 3)
            {
                classess = new List<ViewClass>();
                ViewClass vc = null;
                foreach (ViewInterestedClass ic in Inteseclassess)
                {
                    vc = new ViewClass();
                    vc.Id = ic.Class.Id;
                    vc.Banner = ic.Class.Banner;
                    vc.Name = ic.Class.Name;
                    vc.IfDel = true;
                    vc.IfEdit = false;
                    vc.IfReback = false;
                    classess.Add(vc);
                }
            }
            //我的订单
            if (type == 4)
            {
                Webdiyer.WebControls.Mvc.PagedList<ViewOrder> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewOrder>(list, page, pagesize, count);
                //if (Request.IsAjaxRequest())
                //{
                //return PartialView("OrderList", pagelist);
                return RedirectToAction("Order");
                //}
            }
            if (Request.IsAjaxRequest())
            { return PartialView("MyOtherClassList", classess); }
            else
            { return View(classess); }

        }
        public ActionResult Order(int page = 1)
        {
            string whereStr = "";

            string Phone = "";
            if (!string.IsNullOrEmpty(Request.Form["Phone"]))
            {
                Phone = Request.Form["Phone"].ToString().Trim();
                whereStr += "Phone!" + Phone + ",";
            }

            string CreateTime = "";
            if (!string.IsNullOrEmpty(Request.Form["CreateTime"]))
            {
                CreateTime = Request.Form["CreateTime"].ToString();
                whereStr += "CreateTime!" + CreateTime + ",";
            }
            string EndTime = "";
            if (!string.IsNullOrEmpty(Request.Form["EndTime"]))
            {
                EndTime = Request.Form["EndTime"].ToString();
                whereStr += "EndTime!" + EndTime + ",";
            }
            int UserId = user.Uid;
            whereStr += "UserId!" + UserId + ",";

            OrderServiceClient client = new OrderServiceClient();
            int pagesize = 12;
            int count = 0;
            var list = client.GetOrder(whereStr, page, pagesize, out count);
            Webdiyer.WebControls.Mvc.PagedList<ViewOrder> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewOrder>(list, page, pagesize, count);
            //return PartialView("OrderList", pagelist);
            if (Request.IsAjaxRequest())
            { return PartialView("OrderList", pagelist); }
            else
            { return View(pagelist); }
        }

        [HttpPost]
        public JsonResult ClassEdit(int id, FormCollection collection)
        {
            try
            {
                ViewClass model = new ViewClass();
                model.Name = collection["Name"].ToString();

                //classclient.Edit(model);
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }

            return Json(new { code = 0 });
        }

        [HttpPost]
        public JsonResult ClassDel(int id)
        {
            try
            {
                classclient.Delete(id.ToString());
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }

            return Json(new { code = 0 });
        }
        /// <summary>
        /// 取消兴趣
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelInterestClass(int userid, int classid)
        {
            int result = 0;
            using (InterestServiceClient interClient = new InterestServiceClient())
            {
                result = interClient.DeleteNO(userid, classid);
            }
            return result;
        }

        /// <summary>
        /// 撤销活动
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public int CancleDoClass(int classid)
        {
            int result = 0;
            try
            {
                using (ClassServiceClient csc = new ClassServiceClient())
                {
                    ViewClass c = csc.Get(classid);
                    if (c != null)
                    {
                        c.ClassStatus = (int)ClassStatus.撤销;
                        result = csc.Edit(c);
                    }
                }
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public int DelDoClass(int classid)
        {
            int result = 0;
            try
            {
                using (ClassServiceClient csc = new ClassServiceClient())
                {
                    ViewClass c = csc.Get(classid);
                    if (c != null)
                    {
                        c.IsDeleted = true;
                        result = csc.Edit(c);
                    }
                }
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }
        /// <summary>
        /// 取消参加活动
        /// </summary>
        /// <returns></returns>
        public int DelJoinClass(int userid, int classid)
        {
            int result = 0;
            using (OrderServiceClient JoinClient = new OrderServiceClient())
            {
                result = JoinClient.DeleteNO(userid, classid);
            }
            return result;
        }

        /// <summary>
        /// 修改活动
        /// </summary>
        /// <param name="id">活动ID</param>
        /// <returns></returns>
        public ActionResult MyEditClass(int id)
        {
            ViewClass model = new ViewClass();
            ClassServiceClient client = new ClassServiceClient();
            model = client.Get(id);

            ViewBag.Title = "编辑活动";
            ViewBag.Id = id;
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            ViewData["YogaTopic"] = (from topic in (dicClient.Dics(d => d.DicId == CommonInfo.Topic))
                                     select new SelectListItem()
                                     {
                                         Text = topic.ItemName,
                                         Value = topic.ID.ToString(),
                                         Selected = model.TopicIds.Split(',').Contains(topic.ID.ToString())
                                     }).ToList();
            //逐层获取地理位置区域
            List<DistrictModel> DistrictModelList = client.GetDistrictModel(model.AreaID);
            ViewBag.DistrictModelList = Newtonsoft.Json.JsonConvert.SerializeObject(DistrictModelList);

            //获取活动关联的老师并拼成HTML展示在页面
            ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
            List<ViewClassTeacher> ClassTeacherModel = subClient.GetClass_Id(id);
            string html = "";
            if (ClassTeacherModel != null)
            {
                foreach (var item in ClassTeacherModel)
                {
                    html += "<li class=teacher id=teacher_" + item.Id + ""
                    + " teacherid=" + item.TeacherId + "><div class=teacher-info><div class=name><img src=" + item.Avatar + "> "
                    + item.Name + "<span>" + item.Gender + "</span> <span>"
                    + item.Country + "</span> <span>" + item.YogaSystem + "</span></div></div><div class=teacher-close>&nbsp;</div></li>";

                }
            }
            ViewData["html"] = new MvcHtmlString(html);
            string centerHtml = "";
            CentersServiceClient centerClient = new CentersServiceClient();
            if (!string.IsNullOrWhiteSpace(model.CenterID))
            {
                var centerList = centerClient.GetCentersListByClassCenterID(model.CenterID);
                foreach (var dataItem in centerList)
                {
                    centerHtml += "<li class=Center id=CenterID_" + dataItem.CenterId + " CenterID=" + dataItem.CenterId
                        + "><div class=Center-info><div class=name><img src=" + dataItem.CenterPortraint + "/> " + dataItem.CenterName
                        + " </div></div><div class=Center-close>&nbsp;</div></li>";
                }
            }
            ViewData["centerHtml"] = new MvcHtmlString(centerHtml);
            return View(model);
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public int MyEditClass1(ViewClass entity)
        {
            ClassServiceClient client = new ClassServiceClient();
            ViewClass model = client.Get(entity.Id);
            model.Address = entity.Address;
            model.AreaID = entity.AreaID;
            model.Banner = entity.Banner;
            model.ClassType = entity.ClassType;
            model.ClassStatus = 1;
            model.Content = entity.Content;
            model.Creater = entity.Creater;
            model.CreateTime = DateTime.Now;
            model.Discount = entity.Discount;
            model.Duration = entity.Duration;
            model.DurationUnit = entity.DurationUnit;
            model.InterestCount = entity.InterestCount;
            model.Max = entity.Max;
            model.Name = entity.Name;
            model.Price = entity.Price;
            model.Start = entity.Start;
            model.Summary = entity.Summary;
            model.Tags = entity.Tags;
            model.TopicIds = entity.TopicIds;
            ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
            List<ViewClassTeacher> list = subClient.GetClass_Id(entity.Id);
            if (!string.IsNullOrEmpty(entity.TeacherIds) && entity.Id > 0)
            {
                subClient.EditTeachers(entity.Id, entity.TeacherIds);
            }
            int result = client.Edit(model);
            return result;
            //ClassServiceClient client = new ClassServiceClient();
            //ViewClass model = client.Get(entity.Id);
            //model.Address = entity.Address;
            //model.AreaID = entity.AreaID;
            //model.Banner = entity.Banner;
            //model.ClassType = entity.ClassType;
            //model.Content = entity.Content;
            //model.Creater = entity.Creater;
            //model.CreateTime = DateTime.Now;
            //model.Discount = entity.Discount;
            //model.Duration = entity.Duration;
            //model.DurationUnit = entity.DurationUnit;
            //model.InterestCount = entity.InterestCount;
            //model.Max = entity.Max;
            //model.Name = entity.Name;
            //model.Price = entity.Price;
            //model.Start = entity.Start;
            //model.Summary = entity.Summary;
            //model.Tags = entity.Tags;
            //model.TopicIds = entity.TopicIds;
            //model.CloseTime = entity.CloseTime;
            //model.CenterID = entity.CenterID;
            //if (entity.DurationUnit == 1)
            //    entity.EndTime = Convert.ToDateTime(entity.Start).AddHours(entity.Duration);
            //else
            //    entity.EndTime = Convert.ToDateTime(entity.Start).AddDays(entity.Duration);
            //ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
            //List<ViewClassTeacher> list = subClient.GetClass_Id(entity.Id);
            //if (!string.IsNullOrEmpty(entity.TeacherIds) && entity.Id > 0)
            //{
            //    subClient.EditTeachers(entity.Id, entity.TeacherIds);
            //}
            //int result = client.Edit(model);
            //return result;
        }
        #endregion

        public ActionResult logDetails(int id, int page = 1)
        {
            #region  日志
            int rcount = 0;
            int pagesize = 4;
            List<ViewtMessageGroup> mesgsList = listMessage(id, page, pagesize, out rcount);
            ViewBag.rcount = rcount;
            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(mesgsList, page, pagesize, rcount);

            #endregion
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialMsg", l);
            }
            else
            {
                ViewBag.id = user.Uid;
                ViewBag.logid = id;
                string myselefDisplayimg = string.Empty;
                #region 登录者的级别
                if (user.UserType == 0)
                {
                    ViewYogaUserDetail userdetail = udclient.GetYogaUserDetailById(user.Uid);
                    if (userdetail != null)
                    {
                        ViewBag.DisplayImg = CommonInfo.GetDisplayImg(userdetail.DisplayImg);
                    }
                }
                else//导师级别
                {
                    ViewYogisModels vyogism = new ViewYogisModels();
                    vyogism = client.GetYogisModelsById(user.Uid);
                    if (vyogism != null)
                    {
                        ViewBag.level = vyogism.YogisLevel;
                        ViewBag.DisplayImg = CommonInfo.GetDisplayImg(vyogism.DisplayImg);
                    }
                }
                #endregion

                #region 上一篇文章和下一篇文章
                ViewtWriteLog model = logClient.GetById(id);

                //ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
                //用pre和next变量分别存放上一篇文章和下一篇文章的id号
                int pre = 0, next = 0, i = 0, j;
                //计算总记录数
                int num = logClient.GettWriteLogQuiltUidList(user.Uid).Count();
                int[] a = new int[num];
                var query = from c in logClient.GettWriteLogQuiltUidList(user.Uid) select c.ID;
                //将所有的文章id号全部放入一个数组中
                foreach (var item in query)
                {
                    a[i] = Convert.ToInt32(item);
                    i++;
                }
                //循环，获取上一篇和下一篇文章的ID号，分别放入变量pre和next中
                for (j = 0; j < num; j++)
                {
                    if (a[j] == id)
                    {
                        if (j != 0) pre = a[j - 1];
                        if (j != num - 1) next = a[j + 1];
                    }
                }
                //获取上一篇文章的标题
                if (pre == 0)
                {
                    ViewBag.preTitle = "已经是第一篇";
                    ViewBag.pre = id;
                }
                else
                {
                    ViewBag.preTitle = logClient.GettWriteLogQuiltUidList(user.Uid).Where(c => c.ID == pre).Single().sTitle;
                    ViewBag.pre = pre;
                }
                //获取下一篇文章的标题
                if (next == 0)
                {
                    ViewBag.nextTitle = "已经是最后一篇";
                    ViewBag.next = id;
                }
                else
                {
                    ViewBag.nextTitle = logClient.GettWriteLogQuiltUidList(user.Uid).Where(c => c.ID == next).Single().sTitle;
                    ViewBag.next = next;
                }
                AddReadNum(id);
                ViewBag.logdetail = model;
                #endregion
                return View(l);
            }
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
                int Uid = Convert.ToInt32(Request.Form["uid"]);
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

            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 评论/留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewtMessageGroup> listMessage(int id, int page, int pagesize, out int rcount)
        {
            #region 留言
            if (page == 0) page = 1;
            List<ViewtMessage> msgEntity = new List<ViewtMessage>();
            //msgEntity = msgclient.GettMessageUid(id, 0);
            msgEntity = clientMsg.GettMessageUidList(id, 4, page, pagesize, out rcount);
            List<ViewtMessageGroup> listGroupMsg = new List<ViewtMessageGroup>();

            foreach (var item in msgEntity)
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

                #region 头像

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

                #endregion

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
            #endregion

            return listGroupMsg;
        }

        public ActionResult OtherIndex(int? uid, int year = 0, int month = 0, int page = 1, string desc = "createtime")
        {
            ViewBag.otherGender = method.Gender(uid.Value);
            int logpagesize = 14;
            if (year != 0)
            {
                int count = 0;

                if (uid != null)
                {
                    list = logClient.GettWriteLogPageList(uid.Value, year, month, page, logpagesize, out count);
                }
                else
                {
                    list = logClient.GettWriteLogPageList(user.Uid, year, month, page, logpagesize, out count);
                }
                Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog>(list, page, logpagesize, count);

                return PartialView("OtherIndexList", pagelist);
            }
            else
            {

                ViewBag.id = uid.Value;

                using (FollowServiceClient clientFoll = new FollowServiceClient())
                {
                    ViewFollow vm = clientFoll.GetFollowById(user.Uid, uid.Value);
                    ViewBag.isfollow = vm == null ? false : vm.isfollow;
                }
                int count = 0;
                if (uid != null)
                {
                    list = logClient.GettWriteLogPageList(uid.Value, page, logpagesize, out count);
                }
                else
                {
                    list = logClient.GettWriteLogPageList(user.Uid, page, logpagesize, out count);
                }
                Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewtWriteLog>(list, page, logpagesize, count);


                ViewBag.listGroup = pagelist;
                int id = uid.Value;//专页用户编号
                int usertype = clientUser.GetYogaUserById(id).UserType.Value;
                if (usertype == 0)
                {
                    #region 习练者专页 基本信息

                    ViewYogaUserDetail temp = new ViewYogaUserDetail();

                    using (YogaUserDetailServiceClient userDetclient = new YogaUserDetailServiceClient())
                    {
                        temp = userDetclient.GetYogaUserDetailById(id);
                        if (temp != null)
                        {
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

                            ViewYogaUser userEntity1 = new ViewYogaUser();

                            userEntity1 = clientUser.GetYogaUserById(temp.UID);
                            ViewBag.NickName = userEntity1.NickName;//昵称
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 导师专页 基本信息

                    ViewYogisModels tempm = new ViewYogisModels();
                    using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                    {
                        tempm = client.GetYogisModelsById(id);
                        ViewBag.YogisLevel = tempm.YogisLevel;//是否 大师=4
                        ///昵称
                        ViewBag.NickName = clientUser.GetYogaUserById(tempm.UID).NickName;

                        ViewBag.strUid = tempm.UID;
                        ///位置
                        string strCountryID = "";
                        string strProvinceID = "";
                        string strCityID = "";
                        string strDistrictID = "";
                        if (tempm.CountryID != null && tempm.CountryID != 0)
                        {
                            strCountryID = GetItemName(tempm.CountryID.Value) + "·";
                        }
                        if (tempm.ProvinceID != null && tempm.ProvinceID != 0)
                        {
                            strProvinceID = GetItemName(tempm.ProvinceID.Value) + "·";
                        }
                        if (tempm.CityID != null && tempm.CityID != 0)
                        {
                            strCityID = GetItemName(tempm.CityID.Value) + "·";
                        }
                        if (tempm.DistrictID != null && tempm.DistrictID != 0)
                        {
                            strDistrictID = GetItemName(tempm.DistrictID.Value);
                        }

                        ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
                        ///流派
                        if (!string.IsNullOrEmpty(tempm.YogaTypeid))
                        {
                            string[] ids = tempm.YogaTypeid.Split(',');
                            foreach (var i in ids)
                            {
                                ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                            }
                        }
                        else ViewBag.YogaTypeid = "";
                        ViewBag.modelsDetails = tempm;
                    }
                    #endregion
                }
                //关注 粉丝 
                ViewFollow viewMoel = new ViewFollow();
                using (FollowServiceClient followClient = new FollowServiceClient())
                {
                    ViewBag.iCount = followClient.GetFollowByUid(id);
                    ViewBag.FCount = followClient.GetFollowByCount(id);
                }
                //推荐的数据 （人气）           
                using (tZanModelsServiceClient zclient = new tZanModelsServiceClient())
                {
                    List<ViewtZanModels> listz = zclient.GettZanUid(id);
                    ViewBag.tzancount = listz.Count();
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("OtherIndexList", pagelist);
                }
                return View(pagelist);
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
            string itemname = string.Empty;
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                list = client.GetYogaDicItemById(id);
            }
            if (list != null)
                itemname = list.ItemName;
            return itemname;
        }
        //
        // GET: /tWriteLog/Details/5

        public ActionResult OtherDetails(int id, int? uid, int page = 1)
        {
            ViewBag.otherGender = method.Gender(uid.Value);
            #region  日志
            int rcount = 0;
            int pagesize = 4;
            List<ViewtMessageGroup> mesgsList = listMessage(id, page, pagesize, out rcount);
            ViewBag.rcount = rcount;
            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(mesgsList, page, pagesize, rcount);

            #endregion
            if (Request.IsAjaxRequest())
            {
                return PartialView("OtherPartialMsg", l);
            }
            else
            {

                ViewBag.id = uid.Value;
                ViewBag.logid = id;
                int Uid = uid.Value;//专页用户编号
                int usertype = clientUser.GetYogaUserById(Uid).UserType.Value;
                using (FollowServiceClient clientFoll = new FollowServiceClient())
                {
                    ViewFollow vm = clientFoll.GetFollowById(user.Uid, Uid);
                    ViewBag.isfollow = vm == null ? false : vm.isfollow;
                }
                if (usertype == 0)
                {
                    #region 习练者专页 基本信息

                    ViewYogaUserDetail temp = new ViewYogaUserDetail();

                    using (YogaUserDetailServiceClient userDetclient = new YogaUserDetailServiceClient())
                    {
                        temp = userDetclient.GetYogaUserDetailById(Uid);
                        if (temp != null)
                        {
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

                            ViewYogaUser userEntity1 = new ViewYogaUser();

                            userEntity1 = clientUser.GetYogaUserById(temp.UID);
                            ViewBag.NickName = userEntity1.NickName;//昵称
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 导师专页 基本信息

                    ViewYogisModels tempm = new ViewYogisModels();
                    using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                    {
                        tempm = client.GetYogisModelsById(Uid);
                        ViewBag.YogisLevel = tempm.YogisLevel;//是否 大师=4
                        ///昵称
                        ViewBag.NickName = clientUser.GetYogaUserById(tempm.UID).NickName;

                        ViewBag.strUid = tempm.UID;
                        ///位置
                        string strCountryID = "";
                        string strProvinceID = "";
                        string strCityID = "";
                        string strDistrictID = "";
                        if (tempm.CountryID != null && tempm.CountryID != 0)
                        {
                            strCountryID = GetItemName(tempm.CountryID.Value) + "·";
                        }
                        if (tempm.ProvinceID != null && tempm.ProvinceID != 0)
                        {
                            strProvinceID = GetItemName(tempm.ProvinceID.Value) + "·";
                        }
                        if (tempm.CityID != null && tempm.CityID != 0)
                        {
                            strCityID = GetItemName(tempm.CityID.Value) + "·";
                        }
                        if (tempm.DistrictID != null && tempm.DistrictID != 0)
                        {
                            strDistrictID = GetItemName(tempm.DistrictID.Value);
                        }

                        ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
                        ///流派
                        if (!string.IsNullOrEmpty(tempm.YogaTypeid))
                        {
                            string[] ids = tempm.YogaTypeid.Split(',');
                            foreach (var i in ids)
                            {
                                ViewBag.YogaTypeid += GetItemName(Convert.ToInt32(i)) + " ";
                            }
                        }
                        else ViewBag.YogaTypeid = "";
                        ViewBag.modelsDetails = tempm;
                    }
                    #endregion
                }
                //关注 粉丝 
                ViewFollow viewMoel = new ViewFollow();
                using (FollowServiceClient followClient = new FollowServiceClient())
                {
                    ViewBag.iCount = followClient.GetFollowByUid(Uid);
                    ViewBag.FCount = followClient.GetFollowByCount(Uid);
                }
                //推荐的数据 （人气）           
                using (tZanModelsServiceClient zclient = new tZanModelsServiceClient())
                {
                    List<ViewtZanModels> listz = zclient.GettZanUid(Uid);
                    ViewBag.tzancount = listz.Count();
                }
                #region 上一篇文章和下一篇文章
                ViewtWriteLog model = logClient.GetById(id);
                if (model.iReadNums == null)
                {
                    model.iReadNums = 0;
                }
                if (model != null)
                {
                    //ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
                    //用pre和next变量分别存放上一篇文章和下一篇文章的id号
                    int pre = 0, next = 0, i = 0, j;
                    //计算总记录数
                    int num = logClient.GettWriteLogQuiltUidList(uid.Value).Count();
                    int[] a = new int[num];
                    var query = from c in logClient.GettWriteLogQuiltUidList(uid.Value) select c.ID;
                    //将所有的文章id号全部放入一个数组中
                    foreach (var item in query)
                    {
                        a[i] = Convert.ToInt32(item);
                        i++;
                    }
                    //循环，获取上一篇和下一篇文章的ID号，分别放入变量pre和next中
                    for (j = 0; j < num; j++)
                    {
                        if (a[j] == id)
                        {
                            if (j != 0) pre = a[j - 1];
                            if (j != num - 1) next = a[j + 1];
                        }
                    }
                    //获取上一篇文章的标题
                    if (pre == 0)
                    {
                        ViewBag.preTitle = "已经是第一篇";
                        ViewBag.pre = id;
                    }
                    else
                    {
                        ViewBag.preTitle = logClient.GettWriteLogQuiltUidList(uid.Value).Where(c => c.ID == pre).Single().sTitle;
                        ViewBag.pre = pre;
                    }
                    //获取下一篇文章的标题
                    if (next == 0)
                    {
                        ViewBag.nextTitle = "已经是最后一篇";
                        ViewBag.next = id;
                    }
                    else
                    {
                        ViewBag.nextTitle = logClient.GettWriteLogQuiltUidList(uid.Value).Where(c => c.ID == next).Single().sTitle;
                        ViewBag.next = next;
                    }

                }
                AddReadNum(id);
                ViewBag.model = model;
                return View(l);
                #endregion
            }
        }

        /// <summary>
        /// 阅读量
        /// </summary>
        /// <param name="id"></param>
        private void AddReadNum(int id)
        {
            ViewtWriteLog log = logClient.GetById(id);
            if (log != null)
            {
                if (log.iReadNums == null)
                {
                    log.iReadNums = 0;
                }
                log.iReadNums++;
                logClient.Update(log);
            }
        }

        /// <summary>
        /// 转载
        /// </summary>
        /// <param name="id"></param>
        public int Reprint(int id)
        {
            int value = 0;
            ViewtWriteLog log = logClient.GetById(id);
            if (log != null)
            {
                log.Uid = user.Uid;
                log.iReadNums = 0;
                log.CreateDate = DateTime.Now;
                log.ifpush = false;
                value = logClient.Add(log);
            }
            return value;
        }

        /// <summary>
        /// 赞日志
        /// </summary>
        /// <param name="id"></param>
        public int AddZanLog(int logid, int ToUid)
        {
            int result = 0;
            using (tZanModelsServiceClient zanClient = new tZanModelsServiceClient())
            {
                ViewtZanModels zan = new ViewtZanModels();
                zan.iFromUid = user.Uid;
                zan.iToUid = logid;
                zan.ToUid = ToUid; //添加该日志的人的UID
                zan.iType = user.UserType;
                zan.CreateDate = DateTime.Now;
                zan.iToType = 3; //日志标识
                zan.loginType = 0;
                result = zanClient.Add(zan);
            }
            return result;
        }

        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="logid"></param>
        /// <returns>0赞过、1未赞</returns>
        public int ifZan(int logid)
        {
            int value = 0;
            using (tZanModelsServiceClient zanClient = new tZanModelsServiceClient())
            {
                ViewtZanModels zan = zanClient.GetByFromToUid(logid, user.Uid, 3);
                if (zan != null)
                {
                    value = 1;
                }
            }
            return value;
        }
        /// <summary>
        /// 赞数
        /// </summary>
        /// <param name="logid"></param> 
        public int GetZanCount(int logid)
        {
            int zanCount = 0;
            using (tZanModelsServiceClient zanClient = new tZanModelsServiceClient())
            {
                zanCount = zanClient.Count(logid, 0, 3);
            }
            return zanCount;
        }



        //
        // GET: /tWriteLog/Create

        public ActionResult Create()
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            if (user != null)
            {
                ViewBag.Name = user.NickName;
            }
            return View();
        }

        //
        // POST: /tWriteLog/Create

        [HttpPost, ValidateInput(false)]
        public JsonResult Create(ViewtWriteLog model)
        {
            try
            {
                // TODO: Add insert logic here

                model.ifShow = true;
                model.iReadNums = 0;
                model.Uid = user.Uid;
                model.CreateDate = DateTime.Now;
                model.ValueType = 0;
                logClient.Add(model);

                if (model.ifpush == true)
                {
                    if (!string.IsNullOrEmpty(Request.Form["iType"]))
                    {
                        ViewtLearing learEntity = new ViewtLearing();
                        learEntity.iType = Convert.ToInt32(Request.Form["iType"].ToString());
                        learEntity.sContent = model.sContent;
                        learEntity.Uid = model.Uid.ToString();
                        learEntity.CreateDate = DateTime.Now;
                        learEntity.sTitle = model.sTitle;
                        learEntity.ifexamine = false;
                        learEntity.UrlType = 0;
                        learEntity.sPic = method.getImgUrl(model.sContent)[0].ToString();

                        learclient.Add(learEntity);
                    }

                }
                //start 把sContent中图片添加到相册YogaPicture，类型：3

                Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
                var m = rg.Match(model.sContent);
                while (m.Success)
                {
                    ViewYogaPicture picModel = new ViewYogaPicture();
                    picModel.PictureOriginal = m.Groups[1].Value;//这里就是图片路径     
                    picModel.PictureType = 3;
                    picModel.CreateTime = DateTime.Now;
                    picModel.PictureName = "日志相册";
                    picModel.Uid = user.Uid;
                    picModel.CreateUser = user.Uid;
                    picModel.PictureContent = "日志相册";
                    picModel.iAudio = 1;
                    picModel.HitNum = 0;

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

                    picclient.Add(picModel);
                    m = m.NextMatch();
                }

                //end
                return Json(new { code = 0 });
                //return RedirectToAction("Index");
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //
        // GET: /tWriteLog/Edit/5

        public ActionResult Edit(int id)
        {
            ViewtWriteLog model = logClient.GetById(id);
            if (model != null)
            {
                ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
                return View(model);
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /tWriteLog/Edit/5

        [HttpPost, ValidateInput(false)]
        public JsonResult Edit(ViewtWriteLog model)
        {
            try
            {
                // TODO: Add update logic here
                string title = model.sTitle;
                string scontent = model.sContent;
                int id = model.ID;
                model = logClient.GetById(id);
                model.CreateDate = DateTime.Now;
                model.sContent = scontent;
                model.sTitle = title;

                logClient.Update(model);
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 个人专页中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult logEdit(int id)
        {
            ViewtWriteLog model = logClient.GetById(id);
            if (model != null)
            {
                ViewBag.Name = clientUser.GetYogaUserById(model.Uid.Value).NickName;
                return View(model);
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /tWriteLog/Edit/5

        [HttpPost, ValidateInput(false)]
        public JsonResult logEdit(ViewtWriteLog model)
        {
            try
            {
                // TODO: Add update logic here
                string title = model.sTitle;
                string scontent = model.sContent;
                int id = model.ID;
                model = logClient.GetById(id);
                model.CreateDate = DateTime.Now;
                model.sContent = scontent;
                model.sTitle = title;

                logClient.Update(model);
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //
        // GET: /tWriteLog/Delete/5

        public ActionResult Delete(int id)
        {
            #region （现在先不删除）删除sContent内容中的图片路径所在相册表中的图片信息--如果不删除：该信息没有来源--
            //ViewtWriteLog Entity = logClient.GetById(id);

            //Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            //var m = rg.Match(Entity.sContent);
            //while (m.Success)
            //{
            //    //m.Groups[1].Value;//这里就是图片路径     
            //    ViewYogaPicture picModel = picclient.ExistsPictureOriginal(user.Uid, m.Groups[1].Value);
            //    if (picModel != null)
            //    {
            //        picclient.Delete(picModel.Pid.ToString());
            //    }
            //    m = m.NextMatch();
            //}
            #endregion

            logClient.Delete(id.ToString());
            return RedirectToAction("logIndex");
        }

        //
        // POST: /tWriteLog/Delete/5

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
        /// 日志发表
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
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
                int Toid = Convert.ToInt32(Request.Form["Toid"]);
                string sContent = "";
                if (!string.IsNullOrEmpty(Request.Form["sContent"]))
                {
                    sContent = Request.Form["sContent"].ToString();
                }
                int ParentID = 0;
                if (!string.IsNullOrEmpty(Request.Form["parentID"]))
                {
                    ParentID = Convert.ToInt32(Request.Form["parentID"]);
                }
                int toType = 4;
                if (!string.IsNullOrEmpty(Request.Form["toType"]))
                {
                    toType = Convert.ToInt32(Request.Form["toType"]);
                }
                model = clientMsg.GettMessageDistinct(ToUid, sContent, user.Uid, ParentID);
                if (model != null)
                {
                    return Json(new { code = 2 });
                }
                else
                {
                    model = new ViewtMessage();
                    model.ToType = toType;
                    model.Toid = Toid;
                    model.ToUid = ToUid;
                    model.sContent = sContent;
                    model.ParentID = ParentID;
                    model.FromUid = user.Uid;
                    model.FormType = user.UserType;
                    model.CreateDate = DateTime.Now;
                    model.iZan = 0;
                    model.loginType = 0;
                    clientMsg.Add(model);

                    return Json(new { code = 0 });
                }

            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
    }
}
