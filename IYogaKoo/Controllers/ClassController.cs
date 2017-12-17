using Commons.Helper;
using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Commons.Helper.WebHelper;
using WxPayAPI;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class ClassController : Controller
    {
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        ClassServiceClient client;
        YogaDicItemServiceClient dicclient;
        InterestServiceClient interclient;
        tMessageServiceClient msgclient;
        YogaUserServiceClient clientUser;
        YogaUserDetailServiceClient userDetclient;
        method method;

        public ClassController()
        {
            ViewBag.user = user;
            client = new ClassServiceClient();
            dicclient = new YogaDicItemServiceClient();
            interclient = new InterestServiceClient();
            msgclient = new tMessageServiceClient();
            clientUser = new YogaUserServiceClient();
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
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                using (YogisModelsServiceClient mclient = new YogisModelsServiceClient())
                {
                    vyogism = mclient.GetYogisModelsById(user.Uid);
                    if (vyogism != null)
                        ViewBag.level = vyogism.YogisLevel;
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

        #region 报名活动

        public ActionResult Apply(int id)
        {
            ViewBag.ClassID = id;
            return View();
        }

        [HttpPost]
        public JsonResult Apply(ViewOrder vo)
        {
            if (vo.Number > 0 && vo.Name != "" && vo.Phone != "")
            {
                vo.CreateTime = DateTime.Now;
                vo.Status = 0;
                vo.IsDeleted = false;
                vo.IsPaid = false;
                vo.PayAccount = "";
                vo.Payment = "";
                vo.UserId = user.Uid;
                Result result = new OrderServiceClient().Add(vo);
                return Json(result);
            }
            else
                return Json(new Result(1, "信息输入不完整"));
        }

        /// <summary>
        /// 校验是否需要支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Verify(int id)
        {
            Result result = new Result();
            var osc = new OrderServiceClient();
            ViewOrder order = osc.Get(id);
            if (order.Amount == 0)
            {
                result.Code = 0;
                result.Message = "报名成功！";
                result.Obj = 0;
            }
            else
                result.Obj = 1;
            //ViewBag.Result = result;
            return Json(result);
        }

        /// <summary>
        /// 验证是否支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsPay(int id)
        {
            Result result = new Result();
            var osc = new OrderServiceClient();
            ViewOrder order = osc.Get(id);
            if (order.Status == 2)
            {
                result.Code = 0;
                result.Message = "报名成功！";
                result.Obj = order.ClassId;//完成支付
            }
            else
            {
                result.Code = 1;
                result.Obj = order.ClassId; ;//没有完成支付
            }
            //ViewBag.Result = result;
            return Json(result);
        }

        //Pay
        public ActionResult WXNotify(int id)
        {
            Result result = new Result();


            var osc = new OrderServiceClient();
            ViewOrder order = osc.Get(id);
            //if (order.Amount == 0)
            //{
            //    result.Code = 1;
            //    result.Message = "报名成功！";
            //    ViewBag.Result = result;
            //    return View();
            //}
            string out_trade_no = "";
            WxPayData data = GetWXPayData(order, out out_trade_no);
            //ViewOrder order = new OrderServiceClient().Get(id, user.Uid);
            //OrderServiceClient osc = new OrderServiceClient();
            //ViewOrder order = new ViewOrder();
            //order = osc.Get(vo.Id);
            if (order.IsPaid)
            {
                result.Code = 1;
                result.Message = "已支付。";
            }
            else if ((DateTime.Now - order.CreateTime).Minutes > 15)
            {
                result.Code = 1;
                result.Message = "您的支付时间已超时，请重新下单并完成支付，谢谢。";
            }
            else if (data != null)
            {
                WxPayData payResult = WxPayApi.UnifiedOrder(data);
                result.Obj = payResult.ToPrintStr();
                if (payResult.GetValue("return_code").ToString().Contains("SUCCESS") && payResult.GetValue("result_code").ToString().Contains("SUCCESS"))
                {
                    result.Code = 0;
                    result.Message = "/Class/GetWXPayUrl?url=" + payResult.GetValue("code_url");
                    ViewBag.URL = result.Message;
                    //order = osc.Get(vo.Id);
                    order.IsPaid = false;
                    order.PayDomain = "wx";
                    order.Status = 1;
                    order.PayNO = out_trade_no;// WxPayApi.GenerateOutTradeNo(order.Id, order.CreateTime);
                    osc.updateEntity(order);
                }
                else
                {
                    result.Code = 1;
                    result.Message = payResult.GetValue("return_msg").ToString();
                }
            }
            else
            {
                result.Code = 1;
                result.Message = "无效的订单";
            }
            //result.Obj = new { OrderId = id, ClassId = order.ClassId };// order.ClassId;//订单id
            ViewBag.OrderId = id;
            ViewBag.ClassId = order.ClassId;
            //前台有附带更改
            ViewBag.Result = result; //"报名成功，程序猿正在加班完善支付功能！";

            return View();
        }
        [HttpPost]
        public ActionResult WXNotify()
        {
            #region
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            Log.Info(this.GetType().ToString(), "Check sign success");

            #endregion
            WriteLog("notify", "", data.ToXml() + "----" + data.GetValue("return_code"));
            try
            {
                ViewOrder order = new ViewOrder();
                if (data.GetValue("return_code").ToString() == ("SUCCESS"))
                {
                    order.Id = int.Parse(data.GetValue("attach").ToString());
                    OrderServiceClient osc = new OrderServiceClient();
                    order = osc.Get(order.Id);
                    order.PayTime = DateTime.ParseExact(data.GetValue("time_end").ToString(), "yyyyMMddHHmmss", null);
                    order.IsPaid = true;
                    order.Status = 2;
                    order.OpenID = data.GetValue("openid").ToString();
                    //osc.Edit(order);
                    osc.updateEntity(order);
                    data = new WxPayData();
                    data.SetValue("return_code", "SUCCESS");
                    data.SetValue("return_msg", "OK");
                    Response.Write(data.ToXml());
                    Response.End();
                    //Log.Error("成功", "");
                    //return RedirectToAction("viewactivity", "class", new { id = order.ClassId, isOrder = true }); //View();
                }
                else
                {
                    data = new WxPayData();
                    data.SetValue("return_code", "FAIL");
                    data.SetValue("return_msg", "FAIL");
                    Response.Write(data.ToXml());
                    Response.End();
                    //Log.Error("失败", "");
                    return View();
                }
            }
            catch (Exception ex)
            {
                WriteLog("notify", "", ex.Message);
                //Log.Error("错误", "");
            }

            return View();
        }

        WxPayData GetWXPayData(ViewOrder vo, out string out_trade_no)
        {
            //vo = new OrderServiceClient().Get(id, user.Uid);
            if (vo.Id > 0)
            {
                ViewClass vc = new ClassServiceClient().Get(vo.ClassId);
                WxPayData data = new WxPayData();
                data.SetValue("device_info", "WEB");
                data.SetValue("body", vc.Name);
                data.SetValue("detail", vc.Name + " " + vo.Number);
                data.SetValue("attach", vo.Id);
                out_trade_no = WxPayApi.GenerateOutTradeNo(vo.Id, vo.CreateTime);
                data.SetValue("out_trade_no", out_trade_no);
                data.SetValue("time_start", vo.CreateTime.ToString("yyyyMMddHHmmss"));//交易起始时间
                data.SetValue("time_expire", vo.CreateTime.AddMinutes(CommonInfo.Order_Timeout).ToString("yyyyMMddHHmmss"));
                data.SetValue("total_fee", (int)vo.Amount * 100);//微信钱的单位是分
                data.SetValue("trade_type", "NATIVE");
                data.SetValue("product_id", vc.Id);

                return data;
            }
            else
            {
                out_trade_no = "";
                return null;
            }
        }

        public ActionResult GetWXPayUrl(string url)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(url, Encoding.Default);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            return new ImageResult(image, ImageFormat.Png);
        }

        protected void WriteLog(string type, string className, string content)
        {
            string path = this.HttpContext.Server.MapPath("~/");
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = System.IO.File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + " " + type + " " + className + ": " + content;
            mySw.WriteLine(write_content);

            //关闭日志文件
            mySw.Close();
        }
        #endregion

        #region 活动查询
        public ActionResult Index()
        {
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            List<ViewYogaDicItem> dics = dicClient.GetSelectList(CommonInfo.Topic, true);
            ViewData["YogaTopic"] = (from topic in (dicClient.Dics(d => d.DicId == CommonInfo.Topic))
                                     select new SelectListItem()
                                     {
                                         Text = topic.ItemName,
                                         Value = topic.ID.ToString()
                                     }).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Index(int code, int page, int size, string args)
        {
            ClassServiceClient client = new ClassServiceClient();
            PageResult<ViewClass> pr = client.GetClasses(code, page, size, args.Split(','));
            return Json(pr);
        }
        public ActionResult ViewActivity(int id)
        {
            //, bool isOrder = false ViewBag.isOrder = isOrder;
            ViewBag.id = id;
            ClassServiceClient client = new ClassServiceClient();
            YogaDicItemServiceClient dclient = new YogaDicItemServiceClient();
            ViewClass model = client.Get(id);
            List<ViewYogaDicItem> dics = dclient.GetSelectList(model.TopicIds);
            model.TopicIds = "";
            //主题
            foreach (var item in dics)
            {
                if (model.TopicIds == "")
                    model.TopicIds = item.ItemName;
                else
                    model.TopicIds = model.TopicIds + " " + item.ItemName;
            }
            //发起人头像
            model.Poster = client.GetAvatars(model.UserId.ToString())[0];

            //兴趣
            InterestServiceClient interestClient = new InterestServiceClient();
            ViewBag.IsInterest = interestClient.Exists(id, user.Uid);
            ViewBag.InterestCount = interestClient.ClassInterests(id, 1, 10).RecordCount;

            ////老师粉丝
            //FollowServiceClient followClient = new FollowServiceClient();
            //int teacherFollowCount = 0;
            //foreach (var item in model.Teachers)
            //{
            //    followClient.GetFollowUidList(item.UserId, 1, 1, out teacherFollowCount);
            //    ViewData[item.UserId.ToString()] = teacherFollowCount;
            //}

            YogisModelsServiceClient ymClient = new YogisModelsServiceClient();
            //老师粉丝
            FollowServiceClient followClient = new FollowServiceClient();
            foreach (var item in model.Teachers)
            {
                //followClient.GetFollowUidList(item.UserId, 1, 1, out teacherFollowCount);
                //ViewData[item.UserId.ToString()] = teacherFollowCount;
                var ymModel = ymClient.GetById((int)item.TeacherId);
                int num = followClient.GetFollowByCount(ymModel.UID);
                ViewData[item.UserId.ToString()] = num;
            }
            //发起人粉丝
            ViewBag.Sponsor = followClient.GetFollowByCount(model.UserId);

            //参加
            OrderServiceClient orderClient = new OrderServiceClient();
            // 只查询出有效的订单
            PageResult<ViewOrder> orders = orderClient.GetByClass(id, 1, 6000);
            //等待加入
            #region
            string uids = "";
            int joinCount = 0;
            foreach (ViewOrder item in orders.Objects)
            {
                if (uids == "")
                    uids = item.UserId.ToString();
                else
                    uids += "," + item.UserId;
                joinCount += item.Number;
            }
            ViewBag.JoinCount = joinCount;
            List<UserListItem> orderAvatars = client.GetAvatars(uids == "" ? "0" : uids);
            List<UserListItem> allJoinAvatars = new List<UserListItem>();
            foreach (var item in orders.Objects)
            {
                foreach (var ui in orderAvatars)
                {
                    if (item.UserId == ui.ID)
                    {
                        for (int i = 0; i < item.Number; i++)
                        {
                            allJoinAvatars.Add(ui);
                        }
                    }
                }
            }
            ViewData["AllJoinAvatars"] = allJoinAvatars;
            #endregion

            return View(model);
        }

        public JsonResult GetClasses(int code, int page, int size, string args)
        {
            ClassServiceClient client = new ClassServiceClient();
            PageResult<ViewClass> pr = client.GetClasses(code, page, size, args.Split(','));
            return Json(pr, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 活动报道
        public ActionResult Reports()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Reports(int code, int page, int size, string args)
        {
            PageResult<ViewClass> pr = client.GetClasses(code, page, size, args.Split(','));
            return Json(pr);
        }
        public ActionResult ViewReport(int? classid, int id = 0)
        {
            ViewClass @class = null;
            ViewClassReport report = null;
            if (classid > 0)
            {
                @class = client.Get((int)classid);
                report = @class.Reports[0];
            }
            else if (id > 0)
            {
                ClassReportServiceClient rc = new ClassReportServiceClient();
                report = rc.Get(id);
                @class = client.Get(report.ClassId);
            }
            int nowIndex = 0;
            for (int i = 0; i < @class.Reports.Count; i++)
            {
                if (@class.Reports[i].Id == report.Id)
                {
                    nowIndex = i;
                    break;
                }
            }
            //更多中移除当前活动
            @class.Reports.RemoveAt(nowIndex);
            ViewBag.Class = @class;
            return View(report);
        }

        #endregion

        /// <summary>
        /// 回顾首页
        /// </summary>
        /// <param name="ClassStatus">2 活动预告 3 活动回顾</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ActivityReport(int ClassStatus, int page = 1)
        {
            ViewBag.ClassStatus = ClassStatus;
            List<ViewtBanner> listBanner = new List<ViewtBanner>();
            if (ClassStatus == 2)
            {
                listBanner = method.listBanner(2);
            }
            else if (ClassStatus == 3)
            {
                listBanner = method.listBanner(1);
            }
            ViewBag.listBanner = listBanner;
            int pagesize = 12;
            string OrderBy = string.Empty;
            string where = string.Empty;

            string strValue = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["litit"]))
            {
                strValue = Request.Form["litit"].ToString();
                //排序
                if (strValue == "OrderNums" || strValue == "InterNums")
                {
                    OrderBy = strValue;
                }

                //条件
                if (strValue == "590" || strValue == "100")
                {
                    where = "    where DistrictDicID =" + strValue + " ";
                }
                else if (strValue == "844" || strValue == "845")
                {
                    where = "    where AreaID =" + strValue + " ";
                }
                else if (strValue == "7" || strValue == "90" || strValue == "180")
                {
                    where = "    where convert(datetime,Start) >=convert(datetime,'" + DateTime.Now.AddDays(-Convert.ToInt32(strValue)).ToString() + "') ";
                }
                else if (strValue == "115" || strValue == "116" || strValue == "117")
                {
                    where = "    where TopicIds like '%" + strValue + "%' ";
                }
            }
            if (ClassStatus == 0) ClassStatus = 3;
            List<ViewClassGroup> list = client.GetClassHuiGuList(OrderBy, where, ClassStatus);
            PagedList<ViewClassGroup> pagelist = new PagedList<ViewClassGroup>(list, page, pagesize);
            Session["ClassStatus"] = ClassStatus;
            if (Request.IsAjaxRequest())
            {
                return PartialView("ActivityReportPartial", pagelist);
            }
            return View(pagelist);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditShare(int id)
        {
            ViewClass entity = new ViewClass();
            entity = client.Get(id);

            if (entity.iShareNums == null)
                entity.iShareNums = 1;
            else entity.iShareNums++;
            client.Edit(entity);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">报道ClassId</param>
        /// <param name="classReportId">报道id</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ActivityReportDetailsPage(int id, int classReportId, int page = 1)
        {
            ViewBag.id = id;
            int rcount = 0;
            int pagesize = 10;
            ViewClass entity = new ViewClass();
            entity = client.Get(id);

            if (entity.iReadNums == null)
                entity.iReadNums = 1;
            else entity.iReadNums++;
            client.Edit(entity);
            ViewBag.iShareNums = entity.iShareNums == null ? 0 : entity.iShareNums;

            ViewBag.Entity = entity;
            ViewBag.interCount = interclient.Count(id);//分享人数

            ViewBag.MsgInfo = method.listMessage(id, 3, page, out rcount);//评论 

            YogisModelsServiceClient ymClient = new YogisModelsServiceClient();
            //老师粉丝
            FollowServiceClient followClient = new FollowServiceClient();
            int teacherFollowCount = 0;
            foreach (var item in entity.Teachers)
            {
                //followClient.GetFollowUidList(item.UserId, 1, 1, out teacherFollowCount);
                //ViewData[item.UserId.ToString()] = teacherFollowCount;
                var ymModel = ymClient.GetById((int)item.TeacherId);
                int num = followClient.GetFollowByCount(ymModel.UID);
                ViewData[item.UserId.ToString()] = num;
            }
            //发起人粉丝
            ViewBag.Sponsor = followClient.GetFollowByCount(entity.UserId);

            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> messlist = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.MsgInfo, page, pagesize, rcount);
            if (Request.IsAjaxRequest())
                return PartialView("PartialMessage", messlist);

            ViewData["ClassStatus"] = Session["ClassStatus"];

            using (ClassReportServiceClient rClient = new ClassReportServiceClient())
            {
                List<ViewClassReport> classReportList = rClient.GetClassId(id);
                ViewBag.ClassReport = classReportList;
                ViewBag.ClassReportShow = new ViewClassReport();
                if (classReportList.Any())
                {
                    if (classReportId == -99)
                        ViewBag.ClassReportShow = classReportList.First();
                    else
                        ViewBag.ClassReportShow = classReportList.First(p => p.Id == classReportId);
                }
            }

            return View(messlist);
        }

        /// <summary>
        /// 活动主题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MvcHtmlString GetActivityThemeHtml()
        {
            List<ViewYogaDicItem> list = dicclient.GetDicId(114);
            string strHtml = string.Empty;
            for (var i = 0; i < list.Count(); i++)
            {
                strHtml += "<a tit='" + list[i].ID + "' href='javascript:;'>" + list[i].ItemName + "</a>";

            }
            // ViewData["funHtml"] = new MvcHtmlString(strHtml);
            return new MvcHtmlString(strHtml);

        }

        #region 添加活动
        public ActionResult AddActivity()
        {
            if (user.Uid == 0 || user.Uid.IsNullOrEmpty())
            {
                return Content("<script>if (confirm('你还没有登录，是否登录？')) { window.location='/Login/Login';  }</script>");
            }
            else
            {
                YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
                ViewData["YogaTopic"] = (from topic in (dicClient.Dics(d => d.DicId == CommonInfo.Topic))
                                         select new SelectListItem()
                                         {
                                             Text = topic.ItemName,
                                             Value = topic.ID.ToString()
                                         }).ToList();
                return View();
            }
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult AddActivity(ViewClass entity)
        {
            ClassServiceClient client = new ClassServiceClient();
            entity.Banner = "";
            entity.Discount = 0;
            entity.IsItem = false;
            entity.ItemClassID = 0;
            entity.ClassType = (int)ClassType.activity;
            entity.ClassStatus = (int)ClassStatus.审核中;
            entity.UserId = user.Uid;
            entity.UpdateTime = DateTime.Now;
            entity.IsDeleted = false;
            if (entity.DurationUnit == 1)
                entity.EndTime = Convert.ToDateTime(entity.Start).AddHours(entity.Duration);
            else
                entity.EndTime = Convert.ToDateTime(entity.Start).AddDays(entity.Duration);

            entity.Id = client.Add(entity);
            if (!string.IsNullOrEmpty(entity.TeacherIds) && entity.Id > 0)
            {
                ClassTeacherServiceClient subClient = new ClassTeacherServiceClient();
                subClient.AddTeachers(entity.Id, entity.TeacherIds);
            }
            return Json(entity);
        }

        /// <summary>
        ///  查询导师
        /// </summary>
        /// <param name="text"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public JsonResult GetYogis(string text, int page = 1, int size = 10)
        {
            YogisModelsServiceClient client = new YogisModelsServiceClient();
            PageResult<ViewClassTeacher> pr = new PageResult<ViewClassTeacher>();
            int records = 0;
            List<ViewYogisModels> yogis = client.GetYogisModelsList(text, 2, 0, "", page, size, out records);
            pr.Objects = (from y in yogis
                          select new ViewClassTeacher()
                          {
                              TeacherId = y.YID,
                              Country = (y.Nationality == "" ? CommonInfo.CountryZHID.ToString() : y.Nationality),
                              Name = (y.RealName ?? ""),
                              Gender = (y.Gender == 0 ? "女" : "男"),
                              YogaSystem = (string.IsNullOrEmpty(y.YogaTypeid) ? "0" : y.YogaTypeid),
                              Info = y.GudWords,
                              Avatar = y.DisplayImg
                          }).ToList();
            pr.Code = 0;
            pr.Index = page;
            pr.PageSize = size;
            pr.RecordCount = records;
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            foreach (ViewClassTeacher item in pr.Objects)
            {
                if (item.Country != null && item.Country != "0")
                    item.Country = dicClient.GetById(int.Parse(item.Country)).ItemName;
                else
                    item.Country = "";
                if (item.YogaSystem != "0")
                {
                    List<ViewYogaDicItem> systems = dicClient.GetSelectList(item.YogaSystem);
                    item.YogaSystem = "";
                    foreach (ViewYogaDicItem system in systems)
                    {
                        item.YogaSystem += system.ItemName + ",";
                    }
                    item.YogaSystem = item.YogaSystem.Replace(',', ' ');
                }
                else
                    item.YogaSystem = "";
            }
            return Json(pr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查找机构
        /// </summary>
        /// <param name="text"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public JsonResult GetCenter(string text, int page = 1, int size = 10)
        {
            //派别
            int typeid = 0;
            //瑜伽类别
            string id = "0";
            //国家
            int CountryID = 0;
            //省份
            int ProvinceID = 0;
            //城市
            int CityID = 0;
            //地区
            int DistrictID = 0;
            int count = 0;
            List<ViewCenters> list = new List<ViewCenters>();
            using (CentersServiceClient client = new CentersServiceClient())
            {
                list = client.GetCentersPageList(text, DistrictID, CityID, ProvinceID, CountryID, typeid, id, page, size, out count);
            }
            PageResult<ViewCenters> pr = new PageResult<ViewCenters>();
            pr.Objects = (from y in list
                          select new ViewCenters()
                          {
                              CenterId = y.CenterId,
                              CenterName = y.CenterName,
                              CenterPortraint = y.CenterPortraint
                          }).ToList();
            pr.Code = 0;
            pr.Index = page;
            pr.PageSize = size;
            pr.RecordCount = count;
            return Json(pr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加机构
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCenter(ViewCenters Model)
        {
            try
            {
                using (CentersServiceClient client = new CentersServiceClient())
                {
                    Model.CenterName = Request.Form["CenterName"].ToString();
                    Model.CenterState = 2;
                    Model.CenterSource = 2;

                    Model.CenterAddress = "";
                    Model.UpgradeDate = Model.CreateDate = DateTime.Now;

                    Model.DistrictID = 0;
                    Model.CityID = 0;
                    Model.ProvinceID = 0;
                    Model.CountryID = 0;
                    Model.CenterType = "";
                    Model.CenterBanner = "";
                    Model.CenterIntrodition = "";
                    Model.CenterPortraint = "";
                    string temptypeid = "";
                    string[] arrtypeid = temptypeid.Split(',');
                    string newtypeid = string.Empty;
                    for (int i = 0; i < arrtypeid.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(arrtypeid[i]))
                        {
                            arrtypeid[i] = "|" + arrtypeid[i] + "|";
                            newtypeid += arrtypeid[i] + ",";
                        }
                    }
                    Model.YogaTypeid = newtypeid;
                    Model.OpenTime = DateTime.Now.ToString("yyyy-MM-dd ");
                    Model.CloseTime = DateTime.Now.ToString("yyyy-MM-dd ");
                    client.Add(Model);
                    return Json(Model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddActivityBanner(int id)
        {
            ViewClass model = new ClassServiceClient().Get(id);
            return View(model);
        }

        [HttpPost]
        public JsonResult AddActivityBanner(int id, string banner)
        {
            ClassServiceClient client = new ClassServiceClient();

            ViewClass entity = client.Get(id);
            if (entity.Id > 0)
            {
                if (entity.UserId != user.Uid)
                    return Json(new Result(1, "这不是你的地盘"));
                entity.Banner = banner;
                int result = client.Edit(entity);
                if (result > 0)
                    return Json(new Result(0, "保存海报成功"));
            }
            return Json(new Result(0, "保存海报失败"));
        }

        public ActionResult AddActivityView(int id)
        {
            ViewBag.ClassId = id;
            return View();
        }
        [HttpPost]
        public string Upload()
        {
            string path = FileHelper.SaveImage(Request, "", "/files" + Request.Params["category"], true, 900);
            return path;
        }
        #endregion

        #region 兴趣
        public ActionResult Interests()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Interests(int page = 1, int size = 20)
        {
            return Json(new InterestServiceClient().Interests(user.Uid, page, size), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteInterest(int classId)
        {
            if (user.Uid > 0)
            {
                InterestServiceClient client = new InterestServiceClient();
                if (client.Exists(classId, user.Uid))
                {
                    int r = client.Delete(classId, user.Uid);
                    if (r > 0)
                        return Json(new Result(0, "这个兴趣已经消失了"));
                    else
                        return Json(new Result(1, "删除失败，请重试"));
                }
                else
                    return Json(new Result(1, "你似乎没有关注过这个活动"));
            }
            else
                return Json(new Result(1, "登陆后再试试吧"));

        }
        [HttpPost]
        public JsonResult AddInterest(int classId)
        {
            ViewInterestedClass model = new ViewInterestedClass();
            InterestServiceClient client = new InterestServiceClient();
            if (client.Exists(classId, user.Uid))
            {
                model.Id = client.Delete(classId, user.Uid);
            }
            else
            {
                model.ClassId = classId;
                model.UserId = user.Uid;
                model.Id = client.Add(model);
            }
            InterestServiceClient interestClient = new InterestServiceClient();
            int GetCount = interestClient.ClassInterests(classId, 1, 10).RecordCount;
            return Json(new { code = GetCount });
        }
        #endregion

        #region 导师
        public PartialViewResult _PartialTeacherWall()
        {
            return PartialView();
        }
        //添加活动导师
        public PartialViewResult _PartialAddClassTeacher()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "男", Value = "男" });
            items.Add(new SelectListItem() { Text = "女", Value = "女", Selected = true });
            ViewData["Gender"] = items;
            items = new List<SelectListItem>();
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            items = (from dic in dicClient.GetSelectList(CommonInfo.NationID, true) select new SelectListItem() { Selected = false, Text = dic.ItemName, Value = dic.ItemName }).ToList();
            ViewData["Country"] = items;
            items = (from dic in dicClient.GetSelectList(CommonInfo.YogaSystemID, true) select new SelectListItem() { Selected = false, Text = dic.ItemName, Value = dic.ItemName }).ToList();
            ViewData["YogaSystem"] = items;
            return PartialView();
        }
        [HttpPost]
        public JsonResult _PartialAddClassTeacher(ViewClassTeacher entity)
        {

            BasicInfo bi = Commons.Helper.Login.GetCurrentUser();
            if (bi.Uid > 0)
            {
                //添加到 YogaUser YogisModels

                ViewYogaUser item = new ViewYogaUser();

                ViewYogisModels model = new ViewYogisModels();

                #region 添加到 YogaUser

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

                #endregion

                using (YogaUserServiceClient clientuser = new YogaUserServiceClient())
                {
                    var yuEntity = clientuser.Return_AddUid(item);
                    model.UID = yuEntity.Uid;
                }
                model.RealName = entity.Name;
                model.DisplayImg = entity.Avatar;
                if (entity.Gender == "男")
                    model.Gender = 1;
                else if (entity.Gender == "女")
                    model.Gender = 0;

                model.CountryID = method.GetDicId(entity.Country);

                model.YogaTypeid = method.GetYogaTypeid(entity.YogaSystem).TrimEnd(',');
                model.CreateDate = DateTime.Now;
                model.YogiStatus = 1;//1=普通导师
                model.IsAcceptAgreement = true;
                model.delState = 0;
                //添加到 YogisModels
                using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
                {
                    model = clientModel.Add_Model(model);
                }

                ClassTeacherServiceClient client = new ClassTeacherServiceClient();
                entity.UserId = bi.Uid;
                entity.TeacherId = model.YID;
                entity.Id = client.Add(entity);
            }
            return Json(entity);
        }

        /// <summary>
        /// 用于后台的添加导师，不用获取Uid
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _PartialAddClassTeacherOfManager(ViewClassTeacher entity)
        {
            //BasicInfo bi = Commons.Helper.Login.GetCurrentUser();
            //if (bi.Uid > 0)
            //{
            ClassTeacherServiceClient client = new ClassTeacherServiceClient();
            //entity.UserId = bi.Uid;
            entity.Id = client.Add(entity);
            //}
            //添加到 YogaUser YogisModels
            #region
            //ViewYogaUser item = new ViewYogaUser();

            //ViewYogisModels model = new ViewYogisModels();

            //#region 添加到 YogaUser

            //item.RegDate = DateTime.Now;
            //item.UStatus = 0;
            //item.UEmail = "";
            //item.Uphone = "";
            //item.Pwd = "";
            //item.LastDate = DateTime.Now;
            //item.LoginTimes = 0;
            //item.LoginType = (int)LoginType.普通;
            //item.UserType = (int)UserType.瑜伽导师;
            //item.UStatus = (int)Ustatus.未激活;
            //item.IsAssessor = false;
            //item.IsWebworkers = false;

            //item.YogisModels = new List<YogisModels>();
            //item.WechatAuthCode = "";
            //item.WechatBack = "";
            //item.SinaAuthCode = "";
            //item.SinaBack = "";
            //item.QQAuthCode = "";
            //item.QQBack = "";
            //item.ValCode = "";
            //item.ValExpire = DateTime.Now.AddYears(2);

            //#endregion

            //using (YogaUserServiceClient clientuser = new YogaUserServiceClient())
            //{
            //    model.UID = clientuser.Return_AddUid(item);
            //}
            //model.RealName = entity.Name;
            //model.DisplayImg = entity.Avatar;
            //if (entity.Gender == "男")
            //    model.Gender = 1;
            //else if (entity.Gender == "女")
            //    model.Gender = 0;

            //model.CountryID = method.GetDicId(entity.Country);

            //model.YogaTypeid = method.GetYogaTypeid(entity.YogaSystem).Trim(',');
            //model.CreateDate = DateTime.Now;
            //model.YogiStatus = 1;//1=普通导师
            //model.IsAcceptAgreement = true;
            ////添加到 YogisModels
            //using (YogisModelsServiceClient clientModel = new YogisModelsServiceClient())
            //{
            //    clientModel.Add(model);
            //}
            #endregion
            return Json(entity);
        }

        [HttpPost]
        public JsonResult UpdateAvatar(int ClassTeacherId, string imgUrl)
        {
            ClassTeacherServiceClient client = new ClassTeacherServiceClient();
            ViewClassTeacher entity = client.GetById(ClassTeacherId);
            entity.Avatar = imgUrl;
            client.Edit(entity);
            TempData["HtmlClassTeacher"] = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            return Json(entity);
        }
        #endregion


    }
}