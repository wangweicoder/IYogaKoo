using Commons.Helper;
using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using zzfIBM.WebControls.Mvc;
using Commons.Helper.WebHelper;
namespace IYogaKoo.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 获取登录/注册信息
        /// </summary>
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        /// <summary>
        /// 用户登录信息
        /// </summary>
        tUserLoginInfoServiceClient userloginclient ;
        ViewtUserLoginInfo userloginInfo;

        YogaUserDetailServiceClient client;
        YogisModelsServiceClient modclient;

        YogaUserServiceClient clientUser;
        tMessageServiceClient clientMsg;
        FollowServiceClient clientFoll;
        tWriteLogServiceClient logClient;
        YogaPictureServiceClient clientPic;
        YogaDicItemServiceClient dicclient;
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
       
        public LoginController()
        {
            ViewBag.user = user;
            client = new YogaUserDetailServiceClient();
            modclient = new YogisModelsServiceClient();
            userloginclient = new tUserLoginInfoServiceClient();
            clientUser = new YogaUserServiceClient();
            clientMsg = new tMessageServiceClient();
            clientFoll = new FollowServiceClient();
            logClient = new tWriteLogServiceClient();
            clientPic = new YogaPictureServiceClient();
            dicclient = new YogaDicItemServiceClient();
            interclient = new InterestServiceClient();
            orderclient = new OrderServiceClient();
            classclient = new ClassServiceClient();
            method = new method();
            userloginInfo = new ViewtUserLoginInfo();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = client.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = modclient.GetYogisModelsById(user.Uid);
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
        //
        // GET: /Login/ 
        public ActionResult Login()
        {
            //string returnUrl = Request.Params["returnUrl"];
            //returnUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "" });
            //ViewYogaUser model = new ViewYogaUser
            //{
            //    ReturnUrl = returnUrl
            //};
            // return View(model);
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(int i = 0)
        {
            string strUserName = "";
            string strPwd = "";

            try
            {
               
                bool bl = false;
                if (Request.Form["Email"] == null || Request.Form["Email"].ToString() == "")
                {
                    return Json(new { info = "用户名不能为空!" });
                }
                else
                {
                    strUserName = Request.Form["Email"].ToString();
                }
                if (Request.Form["pwd"] == null || Request.Form["pwd"].ToString() == "")
                {
                    return Json(new { info = "密码不能为空!" });
                }
                else
                {
                    strPwd = Request.Form["pwd"].ToString();
                }
                if (Request.Form["RremeberMe"] != null && Request.Form["RremeberMe"].ToString() != "")
                {
                    bl = true;
                }
                
                ViewYogaUser yuser = new ViewYogaUser();
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    //APP：电话+InputType=1 判断是否存在  如果存在取salt +输入的密码  生成88位密码与  该表中的pw比较  成功则登录
                    //判断APP 的用户登录
                    yuser = client.GetAppOrPc(strUserName);
                    if (yuser !=null)
                    {
                        string pwdApp = SHA512Encrypt.SHA512En(strPwd, yuser.salt);
                        if (pwdApp == yuser.Pwd)
                        {

                            #region 更新部分信息

                            yuser.LoginTimes = yuser.LoginTimes + 1;
                            yuser.LastDate = DateTime.Now;
                            yuser.LastIP = Commons.Helper.Login.GetCurrentIP();

                            client.Update(yuser);

                            //保存cookie
                            BasicInfo binfo = new BasicInfo()
                            {
                                Uid = yuser.Uid,
                                Pwd = yuser.Pwd,
                                UEmail = yuser.UEmail,
                                Uphone = yuser.Uphone,
                                ValExpire = yuser.ValExpire,
                                ValCode = yuser.ValCode,
                                LastIP = yuser.LastIP,
                                NickName = yuser.NickName,
                                LoginTimes = yuser.LoginTimes,
                                LoginType = yuser.LoginType,
                                UserType = yuser.UserType
                            };
                            Commons.Helper.Login.CreateLoginInfo(binfo, bl);
                            #endregion

                            Tools.WriteTextLog("Login", yuser.NickName + " ： App用户登录成功");

                            return Json(new { code = 0 });

                        }
                        else {
                            return Json(new { code = 1 }); 
                        }
                          
                    }
                    else
                    {
                        //PC 的用户登录
                        string pwdstr = AlipayMD5.GetMD5(strPwd);
                        yuser = new ViewYogaUser();
                        yuser = client.CheckUser(strUserName, pwdstr);

                        if (yuser == null)
                        {
                            userloginInfo = method.GetLoginInfo(0,strUserName, UserLogin.登录失败.ToString(), true); 
                            userloginclient.Add(userloginInfo);

                            return Json(new { code = 1 });
                        }
                        else
                        {
                            userloginInfo = method.GetLoginInfo(yuser.Uid,strUserName, UserLogin.登录成功.ToString(), true); 
                            userloginclient.Add(userloginInfo);

                            #region 更新部分信息

                            yuser.LoginTimes = yuser.LoginTimes + 1;
                            yuser.LastDate = DateTime.Now;
                            yuser.LastIP = Commons.Helper.Login.GetCurrentIP();

                            client.Update(yuser);

                            //保存cookie
                            BasicInfo binfo = new BasicInfo()
                            {
                                Uid = yuser.Uid,
                                Pwd = yuser.Pwd,
                                UEmail = yuser.UEmail,
                                Uphone = yuser.Uphone,
                                ValExpire = yuser.ValExpire,
                                ValCode = yuser.ValCode,
                                LastIP = yuser.LastIP,
                                NickName = yuser.NickName,
                                LoginTimes = yuser.LoginTimes,
                                LoginType = yuser.LoginType,
                                UserType = yuser.UserType
                            };
                            Commons.Helper.Login.CreateLoginInfo(binfo, bl);
                            #endregion
                             
                            Tools.WriteTextLog("Login", yuser.NickName + " ： 登录成功");

                            return Json(new { code = 0 });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                userloginInfo = method.GetLoginInfo(0,strUserName, UserLogin.登录失败.ToString(), true); 
                userloginclient.Add(userloginInfo);
                Tools.WriteTextLog("Login", ex.Message);
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            ViewYogaArtClass list = new ViewYogaArtClass();

            List<ViewArticleClassGroup> listGroup = new List<ViewArticleClassGroup>();

            using (YogaArtClassServiceClient client = new YogaArtClassServiceClient())
            {
                ViewArticleClassGroup model = new ViewArticleClassGroup();

                list = client.GetYogaArtClassByClassName("协议");

                model.artclass = list;

                ViewYogaArticle listArticle = new ViewYogaArticle();
                using (YogaArticleServiceClient clientC = new YogaArticleServiceClient())
                {
                    List<ViewYogaArticle> li = clientC.GetYogaArticleClassID(list.ID).ToList();
                    if (li.Count() > 0)
                    {
                        ViewBag.ID = li.FirstOrDefault().ID;
                        ViewBag.ArticleTitle = li.FirstOrDefault().ArticleTitle;
                    }
                    else
                    {
                        ViewBag.ID = "";
                        ViewBag.ArticleTitle = "";
                    }

                }
            }
            return View();
        }

        #region 手机注册功能
        /// <summary>
        /// 手机注册功能
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PhoneReg(ViewYogaUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.Uphone == null)
                {
                    return View();
                }
                else
                {
                    user.Uphone = Server.HtmlEncode(user.Uphone);
                }
                user.UEmail = "";
                user.Pwd = AlipayMD5.GetMD5(user.Pwd);
                user.RegDate = DateTime.Now;
                user.LastDate = DateTime.Now;
                user.LastIP = Request.UserHostAddress;
                user.LoginTimes = 0;
                user.WechatAuthCode = "";
                user.WechatBack = "";
                user.UserType = (int)UserType.瑜伽会员;
                user.LoginType = (int)LoginType.普通;
                user.UStatus = (int)Ustatus.开启;
                user.InputType = 0;
                user.delState = 0;
                ViewYogaUser model = new ViewYogaUser();
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    ViewYogaUser modelInfo = client.ExistPhoneReg(user.Uphone, user.NickName);
                    if (modelInfo == null)
                    {
                        #region 添加注册信息

                        client.Add(user);
                        model = client.ExistPhoneReg(user.Uphone, user.NickName);
                        if (model != null)
                        {
                            //注册成功：--添加到Details表
                            ViewYogaUserDetail modelDetail = new ViewYogaUserDetail();
                            modelDetail.UID = model.Uid;
                            modelDetail.Gender = 0;
                            modelDetail.CreateTime = DateTime.Now;
                            modelDetail.Ulevel = (int)MemberLevel.基础习练者;
                            modelDetail.Uscore = 0;
                            using (YogaUserDetailServiceClient clientDetails = new YogaUserDetailServiceClient())
                            {
                                clientDetails.Add(modelDetail);
                            }
                            //保存cookie
                            BasicInfo binfo = new BasicInfo()
                            {
                                Uid = model.Uid,
                                Pwd = model.Pwd,
                                UEmail = model.UEmail,
                                Uphone = model.Uphone,
                                ValExpire = model.ValExpire,
                                ValCode = model.ValCode,
                                LastIP = model.LastIP,
                                NickName = model.NickName,
                                LoginTimes = model.LoginTimes,
                                LoginType = model.LoginType,
                                UserType = model.UserType

                            };

                            Commons.Helper.Login.CreateLoginInfo(binfo, false);

                            Response.Write("<script>alert('注册成功'!)</script>");
                            Response.Redirect("~/Home/Index");
                        }
                        else
                        {
                            Response.Write("<script>alert('注册失败，请重试'!)</script>");
                            return View("Register");
                        }

                        #endregion
                    }
                    else
                    {
                        Response.Write("<script>alert('该手机已注册！');</script>");
                        return View("Register");
                    }
                }

            }
            return View(user);
        }

        /// <summary>
        /// 即时发送，切换不同的短信提供商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type">0注册，1：找回，3：修改手机号</param>
        [HttpGet]
        public string getyzm(string phone)
        {
            string smsType = "hx1";
            string strRes = null;
            if (!string.IsNullOrEmpty(phone))
            {
                if (smsType.Equals("hx"))
                {
                    //华兴软通
                    strRes = sendSmsByhxrt(phone);
                }
                else
                {
                    //容联
                    strRes = sendSmsByrl(phone);
                }
                return strRes;
            }
            return strRes;
        }

        #region 华兴软通    hx
        private string sendSmsByhxrt(string phoneNumber)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");
            string yzm = GenerateCheckCode().ToString();
            //以下参数为所需要的参数，测试时请修改
            string strReg = "101100-WEB-HUAX-514726";   //注册号（由华兴软通提供）
            string strPwd = "KZPGMWQC";                 //密码（由华兴软通提供）
            string strSourceAdd = "";                   //子通道号，可为空（预留参数）
            string strPhone = phoneNumber;//手机号码，多个手机号用半角逗号分开，最多1000个,15601051155
            //string strContent = HttpUtility.UrlEncode("ASP.net" + DateTime.Now.ToString(), myEncode);
            string strContent = HttpUtility.UrlEncode(@"尊敬的会员您好，本次登录验证码为：" + yzm + "，请注意保管,不要将密码泄露给他人，以免造成损失！", myEncode);
            //短信内容
            // "http://www.stongnet.com/sdkhttp/sendsms.aspx";  //华兴软通发送短信地址
            string url = ConfigurationManager.AppSettings["url"].ToString();

            //要发送的内容
            string strSend = "reg=" + strReg + "&pwd=" + strPwd + "&sourceadd=" + strSourceAdd +
                             "&phone=" + strPhone + "&content=" + strContent;

            string resstr = HttpSend.postSend(url, strSend);
            //string resstr = "result=0&message=短信发送成功&smsid=2015040117024435";
            string[] SstrRes = resstr.Split('&');
            string strRes = SstrRes[0].Substring(SstrRes[0].IndexOf('=') + 1, 1);
            return strRes;
        }

        #endregion

        #region 容联‘云通信'短信  rl
        private string sendSmsByrl(string phoneNumber)
        {
            string yzm = GenerateCheckCode().ToString();
            string ret = null;

            CCPRestSDK api = new CCPRestSDK();
            //ip格式如下，不带https://
            bool isInit = api.init("sandboxapp.cloopen.com", "8883");

            api.setAccount("8a48b5514fba2f87014fceacf6dd2c0b", "f7bc97b08e784bccaa63c4eb7ecd2143");
            api.setAppId("8a48b551511a2cec0151233da7d91bfc");

            try
            {
                if (isInit)
                {
                    //模版id  实际 50364，测试 1
                    Dictionary<string, object> retData = api.SendTemplateSMS(phoneNumber, "50364", new string[] { yzm });
                    ret = getDictionaryData(retData);
                    ret = ret.Substring(11, 6);
                    if (ret.Equals("000000"))
                    {
                        ret = "0";
                    }
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            return ret;
        }


        private string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
        #endregion

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckCode()
        {
            //创建整型型变量
            int number;
            //创建字符型变量
            char code;
            //创建字符串变量并初始化为空
            string checkCode = String.Empty;
            //创建Random对象
            Random random = new Random();
            //使用For循环生成6个数字
            for (int i = 0; i < 6; i++)
            {
                //生成一个随机数
                number = random.Next();
                //将数字转换成为字符型
                code = (char)('0' + (char)(number % 10));

                checkCode += code.ToString();
            }
            //将生成的随机数添加到Cookies中
            Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));
            // ViewBag.checkCode = checkCode;
            //返回字符串
            return checkCode;
        }
        [HttpGet]
        public string Nickname(string nickname)
        {
            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.ExistNickName(nickname);
                if (model != null) return "false";
                else return "true";
            }
        }

        [HttpGet]
        public string Phone(string Uphone)
        {
            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.ExistUphone(Uphone);
                if (model != null) return "false";
                else return "true";
            }
        }
        /// <summary>
        /// 验证码确认
        /// </summary>
        public string RegCAPTCHA(string CAPTCHA)
        {
            try
            {
                string c = Request.Cookies.Get("checkcode").Value.ToString();

                if (CAPTCHA == c)
                {
                    return "true";
                }
            }
            catch (Exception c)
            {
                return "false";
                throw c;
            }

            return "false";
        }

        #endregion

        #region 邮箱注册功能


        [HttpGet]
        public string email(string UEmail)
        {
            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.ExistEmail(UEmail);
                if (model != null) return "false";
                else return "true";
            }
        }

        /// <summary>
        /// 邮箱注册功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmailReg(ViewYogaUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.UEmail == null)
                {
                    return View();
                }
                else
                {
                    user.UEmail = Server.HtmlEncode(user.UEmail);
                }
                user.Uphone = "";
                user.Pwd = AlipayMD5.GetMD5(Request.Form["password"]);
                user.RegDate = DateTime.Now;
                user.LastDate = DateTime.Now;
                user.LastIP = Request.UserHostAddress;
                user.LoginTimes = 0;
                user.WechatAuthCode = "";
                user.WechatBack = "";
                user.LoginType = (int)LoginType.普通;
                user.UserType = (int)UserType.瑜伽会员;
                user.UStatus = (int)Ustatus.未激活;
                user.IsAssessor = false;
                user.IsWebworkers = false;
                user.SinaAuthCode = "";
                user.SinaBack = "";
                user.QQAuthCode = "";
                user.QQBack = "";
                user.ValCode = "";
                user.ValExpire = DateTime.Now;
                user.YogisModels = new List<YogisModels>();
                user.InputType = 0;
                user.delState = 0;
                ViewYogaUser model = new ViewYogaUser();
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    ViewYogaUser modelInfo = client.ExistEmailReg(user.UEmail, user.NickName);
                    if (modelInfo == null)
                    {
                        #region 添加注册信息

                        client.Add(user);
                        model = client.ExistEmailReg(user.UEmail, user.NickName);
                        if (model != null)
                        {
                            //注册成功：--添加到Details表
                            ViewYogaUserDetail modelDetail = new ViewYogaUserDetail();
                            modelDetail.UID = model.Uid;
                            modelDetail.CreateTime = DateTime.Now;
                            modelDetail.Ulevel = (int)MemberLevel.基础习练者;
                            modelDetail.Uscore = 0;
                            using (YogaUserDetailServiceClient clientDetails = new YogaUserDetailServiceClient())
                            {
                                clientDetails.Add(modelDetail);
                            }
                            string strUrl = "~/Login/ValidateEmail?Email=" + Server.HtmlEncode(user.UEmail)
                                + "&nickname=" + Server.HtmlEncode(user.NickName) + "&uid=" + Server.HtmlEncode(modelDetail.UID.ToString());


                            Response.Redirect(strUrl);
                        }
                        else
                        {
                            Response.Write("<script>alert('注册失败，请重试'!)</script>");
                        }

                        #endregion
                    }
                    else
                    {
                        Response.Write("<script>alert('该邮箱已注册！');</script>");

                        if (!string.IsNullOrEmpty(modelInfo.ValCode)) return View("Login");
                        else return View("ValidateEmail");
                    }
                }

            }
            return View(user);
        }

        /// <summary>
        /// 验证邮件功能
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateEmail()
        {
            string email = string.Empty;
            string nickname = string.Empty;
            string uid = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["Email"]))
            {
                email = Server.HtmlDecode(Request.QueryString["Email"]);
                ViewBag.email = email;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["nickname"]))
            {
                nickname = Server.HtmlDecode(Request.QueryString["nickname"]);
                ViewData["nickname"] = nickname;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                uid = Server.HtmlDecode(Request.QueryString["uid"]);
                ViewData["uid"] = uid;
            }

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(nickname) && !string.IsNullOrEmpty(email))
            {
                if (SendValidateEmail(uid, nickname, email))
                {
                    string loginEmail = email.Substring(email.IndexOf('@') + 1);
                    ViewBag.loginEmail = getEmServer(loginEmail);
                }
            }
            else
            {
                Response.Redirect("~/Home/Index");
            }
            return View();
        }
        /// <summary>
        /// 返回邮箱登录地址
        /// </summary>
        /// <param name="loginEmail"></param>
        /// <returns></returns>
        private string getEmServer(string loginEmail)
        {
            string EmServer = null;
            switch (loginEmail)
            {
                case "qq.com": EmServer = "http://mail.qq.com"; break;
                case "126.com": EmServer = "http://mail.126.com/"; break;
                case "163.com": EmServer = "http://mail.163.com/"; break;
                default: EmServer = "https://www.baidu.com/s?wd=%E9%82%AE%E7%AE%B1%E7%99%BB%E5%BD%95"; break;
            }
            return EmServer;
        }
        /// <summary>
        /// 发送Email功能
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool SendValidateEmail(string uid, string nickname, string email)
        {
            SendEmailModel sendEmail = new SendEmailModel();
            sendEmail.Title = "来自iyogakoo注册验证";
            sendEmail.To = email;
            sendEmail.SenderEmail = null;
            sendEmail.SenderName = "iyogakoo";
            string code = Guid.NewGuid().ToString();

            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.GetYogaUserById(int.Parse(uid));
                model.ValCode = code;
                model.ValExpire = DateTime.Now.AddDays(1);
                client.Update(model);

                if (!string.IsNullOrEmpty(client.ExistValCode(code).ValCode))
                {
                    string validateUrl = Request.Url.AbsoluteUri;
                    if (validateUrl.ToLower().Contains("sendvalidateemail"))
                    {
                        int lindex = validateUrl.ToLower().LastIndexOf("?");
                        validateUrl = validateUrl.ToLower().Substring(0, lindex);
                        validateUrl = validateUrl.ToLower().Replace("sendvalidateemail", "confirmEmail?code=" + code);
                    }
                    else
                    {
                        int lindex = validateUrl.ToLower().LastIndexOf("?");
                        validateUrl = validateUrl.ToLower().Substring(0, lindex);
                        validateUrl = validateUrl.ToLower().Replace("validateemail", "confirmEmail?code=" + code);
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<p>" + nickname + "，您好！</P><br/>");
                    sb.Append("<p>感谢您加入iyogakoo爱瑜伽酷 。</p>");
                    sb.Append("<p>请验证您的电邮地址，开始使用 iyogakoo.com：</p>");
                    sb.Append("<p><strong><a href='" + validateUrl + "'>" + validateUrl + "</a></strong></p>");
                    sb.Append("<br/><p>（哎呀！如果这不是您的电邮，别担心，我们不会再发邮件给您）</p>");
                    sb.Append("<br/><p>此致，</p>");
                    sb.Append("<p>iyogakoo团队</p>");
                    sendEmail.Body = sb.ToString();

                    if (MailHelper.SendNetMail(sendEmail))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 收件者--确认邮箱
        /// </summary>
        /// <param name="code">确认码</param>
        /// <returns></returns>
        public ActionResult ConfirmEmail(string code)
        {
            ViewYogaUser model = new ViewYogaUser();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                model = client.ExistValCode(code);
                if (model == null || model.ValExpire < DateTime.Now)
                {
                    ViewBag.IsConfirm = false;
                    ViewBag.Email = model.UEmail;
                    ViewBag.Result = "验证失败，验证邮件可能已过期，请重新发送验证邮件。";
                    ViewData["nickname"] = model.NickName;
                    ViewData["uid"] = model.Uid;
                }
                else
                {
                    #region

                    ViewBag.Result = "账号验证成功，可以去随便看看了。";
                    ViewBag.IsConfirm = true;
                    ViewBag.Email = model.UEmail;
                    ViewBag.Uid = model.Uid;
                    ViewData["nickname"] = model.NickName; 
                    //保存cookie                
                    BasicInfo binfo = new BasicInfo()
                    {
                        Uid = model.Uid,
                        Pwd = model.Pwd,
                        UEmail = model.UEmail,
                        Uphone = model.Uphone,
                        ValExpire = model.ValExpire,
                        ValCode = model.ValCode,
                        LastIP = model.LastIP,
                        NickName = model.NickName,
                        LoginTimes = model.LoginTimes,
                        LoginType = model.LoginType,
                        UserType = model.UserType

                    };
                    Commons.Helper.Login.CreateLoginInfo(binfo, false);
                    #endregion
                    //修改状态
                    model.UStatus = (int)Ustatus.开启;
                    client.Update(model);
                }
                #region 瑜伽导师
                List<ViewYogisModels> listEntity = new List<ViewYogisModels>();
                using (YogisModelsServiceClient uclient = new YogisModelsServiceClient())
                {
                    listEntity = uclient.GetYogisModelsList();
                }
                List<ViewUserModelsGroup> listMGroup = new List<ViewUserModelsGroup>();
                foreach (var item in listEntity)
                {
                    ViewUserModelsGroup modelu = new ViewUserModelsGroup();
                    modelu.VmList = item;
                    //粉丝
                    ViewFollow viewMoel = new ViewFollow();
                    using (FollowServiceClient followClient = new FollowServiceClient())
                    {
                        modelu.FollowCount = followClient.GetFollowByCount(item.UID);
                    }
                    using (YogaUserServiceClient clientu = new YogaUserServiceClient())
                    {
                        modelu.VyList = clientu.GetYogaUserById(item.UID);
                    }

                    listMGroup.Add(modelu);
                }
                //瑜伽导师
                ViewBag.list2Group = listMGroup.Where(x => !string.IsNullOrEmpty(x.VmList.DisplayImg) &&
                    !string.IsNullOrEmpty(x.VmList.CoverImg) && !string.IsNullOrEmpty(x.VmList.YogisDepict)).OrderByDescending(x => x.FollowCount).Take(7).ToList();
                
                #endregion
                ClassServiceClient classClient = new ClassServiceClient();
                string args = ",,2,,undefined,undefined";
                ViewBag.WorkBanner = classClient.GetClasses(-3, 1, 7, args.Split(','));
            }
            return View();
        }

        /// <summary>
        /// 邮箱验证码确认
        /// </summary>
        public string RegemailCAPTCHA(string eCAPTCHA)
        {
            string c = Request.Cookies.Get("emailcheckcode").Value.ToString();
            if (eCAPTCHA == c)
            {
                return "true";
            }
            return "false";
        }
        #endregion

        #region  第三方登录


        /// <summary>
        /// 跳转第三方登录页面
        /// </summary>
        /// <param name="name"></param>
        public void GoPartner(string name)
        {
            Session["loginType"] = name;
            IoAuthLogin login = Commons.Helper.Login.GetOuathObject(name);
            string url = login.GetLoginUrl(Response.Cookies);
            Response.Redirect(url);
        }
        /// <summary>
        /// 第三方登录，注册
        /// </summary>
        public void OauthBack()
        {
            /*
         * 第三方流程
         * 获取code
         * 获取token
         * 获取UID
         * 判断是否注册
         * 判断昵称，邮箱，注册
         * 登陆成功
         */
            string loginType = Session["loginType"].ToString();
            //string loginType = "3";
            IoAuthLogin login = Commons.Helper.Login.GetOuathObject(loginType);

            ViewYogaUser lr = login.IsRegister(Request.QueryString, Request.Cookies);
            if (lr != null)
            {
                //已注册，生成cookie
                BasicInfo binfo = new BasicInfo()
                {
                    Uid = lr.Uid,
                    Pwd = lr.Pwd,
                    UEmail = lr.UEmail,
                    Uphone = lr.Uphone,
                    ValExpire = lr.ValExpire,
                    ValCode = lr.ValCode,
                    LastIP = lr.LastIP,
                    NickName = lr.NickName,
                    LoginTimes = lr.LoginTimes,
                    LoginType = lr.LoginType,
                    UserType = lr.UserType

                };
                Commons.Helper.Login.CreateLoginInfo(binfo, false);

                //登陆成功，跳转首页
                Response.Redirect("~/Home/Index");
            }
            else
            {
                //获取第三方的用户信息
                OauthInfo oi = login.GetUser();
                //没有注册,跳转到补充注册页面
                Response.Redirect("~/login/OauthReg?uid=" + oi.AuthCode + "&nickname=" + oi.NickName + "&back=" + oi.ChatBack + "&type=" + ((int)oi.LoginType).ToString() + "&photo=" + oi.Avatar);
            }
        }
        //第三方注册
        public ActionResult OauthReg(string uid, string nickname, string back, string type, string photo)
        {
            BasicInfo lr = new BasicInfo();
            lr.WechatAuthCode = uid;
            lr.NickName = nickname;
            lr.Avatar = photo;
            //ur.Place = place;
            lr.WechatBack = back;
            lr.LoginType = int.Parse(type);
            ViewYogaUser model = new ViewYogaUser()
            {
                Uid = lr.Uid,
                Pwd = lr.Pwd,
                UEmail = lr.UEmail,
                Uphone = lr.Uphone,
                ValExpire = lr.ValExpire,
                ValCode = lr.ValCode,
                LastIP = lr.LastIP,
                NickName = lr.NickName,
                LoginTimes = lr.LoginTimes,
                LoginType = lr.LoginType,
                UserType = lr.UserType,
                WechatAuthCode = lr.WechatAuthCode,
                WechatBack = lr.WechatBack
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult OauthReg(ViewYogaUser lr)
        {
            lr.LastDate = DateTime.Now;
            lr.LastIP = Request.UserHostAddress;
            lr.LoginTimes = 0;
            lr.RegDate = DateTime.Now;
            lr.UStatus = (int)Ustatus.开启;
            lr.UserType = (int)UserType.瑜伽会员;
            lr.Pwd = AlipayMD5.GetMD5(lr.Pwd);
            lr.LoginType = int.Parse(Request.QueryString["type"].ToString());

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                try
                {
                    lr.Uid = client.Return_AddUid(lr).Uid;
                    ViewYogaUserDetail modelDetails = new ViewYogaUserDetail()
                    {
                        UID = lr.Uid,
                        Gender = 0,
                        CreateTime = DateTime.Now,
                        Ulevel = 0,
                        Uscore = 0,
                        DisplayImg = Request.QueryString["photo"].ToString()
                    };
                    using (YogaUserDetailServiceClient clientDet = new YogaUserDetailServiceClient())
                    {
                        try
                        {
                            clientDet.Add(modelDetails);
                        }
                        catch
                        {
                            Response.Redirect("~/");
                        }

                    }
                }
                catch
                {
                    Response.Redirect("~/");
                }

            }

            //已注册，生成cookie
            BasicInfo binfo = new BasicInfo()
            {
                Uid = lr.Uid,
                Pwd = lr.Pwd,
                UEmail = lr.UEmail,
                Uphone = lr.Uphone,
                ValExpire = lr.ValExpire,
                ValCode = lr.ValCode,
                LastIP = lr.LastIP,
                NickName = lr.NickName,
                LoginTimes = lr.LoginTimes,
                LoginType = lr.LoginType,
                UserType = lr.UserType
            };
            Commons.Helper.Login.CreateLoginInfo(binfo, false);

            Response.Redirect("~/");
            return View();
        }

        #endregion

        #region 导师头像管理

        public ActionResult ModelsTopPic()
        {
            #region 登录者的级别
            if (user.UserType == 0)
            {

            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = modclient.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                    ViewBag.level = vyogism.YogisLevel;
            }
            #endregion
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
                    ViewYogisModels list = modclient.GetWhere(user.Uid, subdirNext.Name);
                    if (list == null)
                    {
                        PubClass.FileDel(uploadPath + "\\" + subdirNext.ToString());
                    }
                }
            }
            #endregion
            ViewYogisModels listModel = new ViewYogisModels();
            using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
            {
                listModel = clientModels.GetYogisModelsById(user.Uid);

                return View(listModel);

            }
        }

        [HttpPost]
        public JsonResult ModelsTopPic(int? id)
        {
            string DisplayImg = Request.Form["DisplayImg"].ToString();

            int icount = 0;//修改成功 
            ViewYogisModels listModel = new ViewYogisModels();
            using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
            {
                listModel = clientModels.GetYogisModelsById(user.Uid);
                listModel.DisplayImg = DisplayImg;

                icount = clientModels.Update(listModel);

            }
            return Json(new { code = icount });

        }

        #endregion

        #region 习练者头像管理

        public ActionResult TopPicJquery()
        {
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
                    // subdirNext.Name
                    ViewYogaUserDetail list = client.GetWhere(user.Uid, subdirNext.Name);
                    if (list == null)
                    {
                        PubClass.FileDel(uploadPath + "\\" + subdirNext.ToString());
                    }
                }
            }
            //
            ViewYogaUserDetail listModel = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
            {
                listModel = clientModels.GetYogaUserDetailById(user.Uid);
                if (listModel != null)
                {
                    return View(listModel);
                }
                else
                {
                    return View();

                }
            }
        }


        #endregion
        #region 忘记密码

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public ActionResult LostPassword()
        {
            ViewBag.Result = false;
            return View();
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LostPassword(string email)
        {
            ViewBag.Result = false;
            if (string.IsNullOrWhiteSpace(email))
                ViewBag.Error = "请输入电邮！";
            else
            {
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    ViewYogaUser model = client.ExistEmail(email);
                    if (model != null)
                    {
                        //发送修改密码链接
                        SendEmailModel sendEmail = new SendEmailModel();
                        sendEmail.Title = "忘记密码了？";
                        sendEmail.To = email;
                        sendEmail.SenderEmail = null;
                        sendEmail.SenderName = "iyogakoo";
                        string code = Guid.NewGuid().ToString();

                        model.ValCode = code;
                        model.ValExpire = DateTime.Now.AddDays(1);

                        if (client.Update(model) == 0)
                        {
                            string validateUrl = Request.Url.AbsoluteUri.ToLower();
                            validateUrl = validateUrl.Replace("lostpassword", "resetpassword?id=" + model.Uid + "&code=" + code);
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<p>" + model.NickName + "，您好！</P><br/>");
                            sb.Append("<p>我们收到了重设 Iyogakoo 密码的请求。</p>");
                            sb.Append("<p>如果您未发出请求，请不要担心，只需忽略此电邮，任何内容均不会更改。</p>");
                            sb.Append("<p>如果确实是您请求重设 Iyogakoo 账户密码，点击以下链接即可：</p>");
                            sb.Append("<p><strong><a href='" + validateUrl + "'>" + validateUrl + "</a></strong></p>");
                            sb.Append("<br/><p>（哎呀！如果这不是您的电邮，别担心，我们不会再发邮件给您）</p>");
                            sb.Append("<br/><p>此致，</p>");
                            sb.Append("<p>iyogakoo团队</p>");
                            sendEmail.Body = sb.ToString();
                            if (MailHelper.SendNetMail(sendEmail))
                                ViewBag.Result = true;
                        }
                        else ViewBag.Error = "邮箱验证发送失败，请重试。";
                    }
                    else
                        ViewBag.Error = "我们没有找到与该电邮地址有关的注册账户，无法取回密码。";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult LostPhone(string phone, string pcode)
        {
            ViewBag.Result = false;
            if (string.IsNullOrWhiteSpace(phone))
                ViewBag.Error = "请输入手机号！";
            else
            {
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    ViewYogaUser model = client.ExistUphone(phone);
                    try
                    {
                        string checkcode = Request.Cookies.Get("checkcode").Value.ToString();

                        if (model != null && pcode == checkcode)
                        {
                            //发送修改密码链接   
                            if (client.Update(model) == 0)
                            {
                                ViewBag.Result = true;
                                return Json(new { code = 0, uid = model.Uid });
                            }
                            else
                            {
                                ViewBag.Error = "手机号验证码不正确，请重试。";
                                return Json(new { code = 1 });
                            }
                        }
                        else
                        {
                            ViewBag.Error = "手机号验证码不正确，无法取回密码。";
                            return Json(new { code = 1 });
                        }
                    }
                    catch (Exception e)
                    {
                        Tools.WriteTextLog("Login-LostPhone", e.Message);
                    }
                }
            }
            return View();
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ActionResult ResetPassword(string id, string code)
        {
            ViewBag.IsConfirm = false;
            if (code == null)
            {
                ViewBag.Error = "验证码正确，可以重置密码。";
            }
            else
            {
                ViewYogaUser model = new ViewYogaUser();
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    model = client.ExistValCode(code);
                    if (model == null || model.ValExpire < DateTime.Now)
                    {
                        ViewBag.IsConfirm = false;
                        ViewBag.Error = "验证失败，验证邮件可能已过期，请重新发送验证邮件。";
                    }
                }

            }
            return View();
        }
        /// <summary>
        /// 重置密码
        /// </summary>       
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(string password)
        {
            ViewBag.IsConfirm = false;
            string uid = Request.QueryString["id"].ToString();
            if (string.IsNullOrWhiteSpace(password))
                ViewBag.Error = "请输入密码。";
            else
            {
                ViewYogaUser model = new ViewYogaUser();
                using (YogaUserServiceClient clientModels = new YogaUserServiceClient())
                {
                    model = clientModels.GetYogaUserById(int.Parse(uid));
                    model.Pwd = AlipayMD5.GetMD5(password);
                    if (clientModels.Update(model) == 0)//修改成功 
                    {
                        ViewBag.Error = "密码重置成功，可以去随便看看了。";
                        ViewBag.IsConfirm = true;

                        //保存cookie                
                        BasicInfo binfo = new BasicInfo()
                        {
                            Uid = model.Uid,
                            Pwd = model.Pwd,
                            UEmail = model.UEmail,
                            Uphone = model.Uphone,
                            ValExpire = model.ValExpire,
                            ValCode = model.ValCode,
                            LastIP = model.LastIP,
                            NickName = model.NickName,
                            LoginTimes = model.LoginTimes,
                            LoginType = model.LoginType,
                            UserType = model.UserType

                        };
                        Commons.Helper.Login.CreateLoginInfo(binfo, false);

                    }
                }
            }
            return View();
        }

        #endregion
        /// <summary>
        /// 修改密码
        /// </summary>
        public ActionResult UpdatePwd()
        {
            try
            {

                ViewYogaUser Model = new ViewYogaUser();
                using (YogaUserServiceClient clientModels = new YogaUserServiceClient())
                {
                    Model = clientModels.GetYogaUserById(user.Uid);
                    ViewBag.Uphone = Model.Uphone;
                    ViewBag.UEmail = Model.UEmail;

                }

                ViewYogisModels listModel = new ViewYogisModels();
                using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                {
                    listModel = client.GetYogisModelsById(user.Uid);
                    if (listModel != null)
                    {
                        ViewBag.RealName = listModel.RealName;
                        ViewBag.IdType = listModel.IdType;
                        ViewBag.IdCardNum = listModel.IdCardNum;
                    }
                    else
                    {
                        ViewBag.RealName = "";
                        ViewBag.IdType = 0;
                        ViewBag.IdCardNum = "";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("UpdatePwd", ex.Message);
                return View();
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePwd(int? id)
        {
            try
            {
                string strOldPwd = Request.Form["OldPwd"].ToString();//旧密码 
                string strNewPwd = Request.Form["NewPwd"].ToString();//新密码 

                int icount = 2;//修改成功 
                ViewYogaUser listModel = new ViewYogaUser();
                using (YogaUserServiceClient clientModels = new YogaUserServiceClient())
                {
                    listModel = clientModels.GetYogaUserById(user.Uid);
                    if (listModel.Pwd == AlipayMD5.GetMD5(strOldPwd))
                    {
                        listModel.Pwd = AlipayMD5.GetMD5(strNewPwd);
                        icount = clientModels.Update(listModel);
                    }
                }
                Tools.WriteTextLog("UpdatePwd", "密码修改成功！");
                return Json(new { code = icount });
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("UpdatePwd", ex.Message);
                return Json(new { code = 1 });
            }
            //return Json(new { code = icount});

        }
        /// <summary>
        /// 修改电话
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUphone()
        {
            try
            {
                string strUphone = Request.Form["NewUphone"].ToString();//电话

                int icount = 0;//修改成功 
                ViewYogaUser listModel = new ViewYogaUser();
                using (YogaUserServiceClient clientModels = new YogaUserServiceClient())
                {
                    listModel = clientModels.GetYogaUserById(user.Uid);
                    listModel.Uphone = strUphone;
                    icount = clientModels.Update(listModel);

                }
                return Json(new { code = icount });
                Tools.WriteTextLog("UpdateUphone", "电话修改成功！");
                return Json(new { code = icount });
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("UpdateUphone", ex.Message);
                return Json(new { code = 1 });
            }

        }

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUEmail()
        {
            try
            {
                string strUEmail = Request.Form["NewUEmail"].ToString();//邮箱

                int icount = 0;//修改成功 
                ViewYogaUser listModel = new ViewYogaUser();
                using (YogaUserServiceClient clientModels = new YogaUserServiceClient())
                {
                    listModel = clientModels.GetYogaUserById(user.Uid);
                    listModel.UEmail = strUEmail;
                    icount = clientModels.Update(listModel);

                }
                Tools.WriteTextLog("UpdateUEmail", "邮箱修改成功！");
                return Json(new { code = icount });
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("UpdateUEmail", "邮箱修改失败：" + ex.Message);
                return Json(new { code = 1 });
            }

        }
        /// <summary>
        /// 修改证件信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateIdType()
        {
            try
            {
                string IdType = Request.Form["IdType"].ToString();// 类型
                string IdCardNum = Request.Form["IdCardNum"].ToString();//号码

                int icount = 0;//修改成功 
                ViewYogisModels listModel = new ViewYogisModels();
                using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
                {
                    listModel = clientModels.GetYogisModelsById(user.Uid);
                    listModel.IdType = IdType;
                    listModel.IdCardNum = IdCardNum;
                    icount = clientModels.Update(listModel);

                }
                Tools.WriteTextLog("UpdateIdType", "证件修改成功！");
                return Json(new { code = icount });
            }
            catch (Exception ex)
            {
                Tools.WriteTextLog("UpdateIdType", "证件修改失败：" + ex.Message);
                return Json(new { code = 1 });
            }

        }

        #region 导师信息
        /// <summary>
        /// 编辑导师信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            ViewBag.id = user.Uid;
            ViewBag.NickName = user.NickName;
            ViewYogisModels listModel = new ViewYogisModels();
            using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
            {
                listModel = clientModels.GetYogisModelsById(user.Uid);
                if (listModel != null)
                {
                    #region 会馆
                    if (!string.IsNullOrEmpty(listModel.CenterID))
                    {
                        string[] cenlist = listModel.CenterID.Split(',');

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
                    if (!string.IsNullOrEmpty(listModel.YogaTypeid))
                    {
                        string[] YogaTypeidlist = listModel.YogaTypeid.Split(',');

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

                    if (!string.IsNullOrEmpty(listModel.TeachYogis))
                    {
                        string[] TeachYogislist = listModel.TeachYogis.Split(',');
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
            }

            return View(listModel);
        }

        /// <summary>
        /// 删除封面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delCoverImgModels(int id)
        {
            if (Request.Form["iType"].ToString() == "0")
            {
                ViewYogaUserDetail model = new ViewYogaUserDetail();
                using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
                {
                    model = client.GetYogaUserDetailById(id);
                    model.Covimg = "";
                    client.Update(model);
                }
            }
            else
            {
                ViewYogisModels listModel = new ViewYogisModels();
                using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
                {
                    listModel = clientModels.GetYogisModelsById(id);
                    listModel.CoverImg = "";
                    clientModels.Update(listModel);
                }
            }
            return Json(new { code = 0 });
        }
        #region  下拉列表

        public JsonResult GetAreaInfo(int id)
        {
            List<ViewYogaDicItem> dic = new List<ViewYogaDicItem>();
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                if (id != 0)
                    dic = YogaDicItemServiceClient.GetDicId(id);
            }

            return Json(dic);
        }
        /// <summary>
        /// 国籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetNationalityInfo()
        {
            List<ViewYogaDicItem> dic = new List<ViewYogaDicItem>();
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                dic = YogaDicItemServiceClient.GetDicId(103);
            }

            return Json(dic);
        }
        /// <summary>
        /// 国家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetCountryInfo()
        {
            List<ViewYogaDicItem> dic = new List<ViewYogaDicItem>();
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                dic = YogaDicItemServiceClient.GetDicId(65);
            }

            return Json(dic);
        }
        #endregion

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public JsonResult Method()
        {
            ViewYogisModels model = GetRequest();
            model.YogisDepict = Request.Form["hYogisDepict"].ToString();
            int iCount = 0;//修改成功
            using (YogisModelsServiceClient clientModels = new YogisModelsServiceClient())
            {
                iCount = clientModels.Update(model);
            }
            ViewYogaUser mm = new ViewYogaUser();
            //呢称修改
            if (!string.IsNullOrEmpty(Request.Form["nickName"]))
            {
                using (YogaUserServiceClient client = new YogaUserServiceClient())
                {
                    mm = client.GetYogaUserById(model.UID);

                    mm.NickName = Request.Form["nickName"].ToString();

                    HttpCookie cookie = Request.Cookies["iyoga"];
                    cookie["NickName"] = AES.Encode(Request.Form["nickName"].ToString(), "iyoga");
                    user.NickName = AES.Decode(cookie["NickName"], "iyoga");

                    Commons.Helper.Login.CreateLoginInfo(user, true);
                    //Commons.Helper.Login.GetCurrentUser();
                    iCount = client.Update(mm);
                }
            }
            return Json(new { code = iCount });
        }
        private ViewYogisModels GetRequest()
        {
            string Info = "";
            ViewYogisModels model = new ViewYogisModels();
            try
            {
                model.YID = Convert.ToInt32(Request.Form["YID"]);
                model.UID = Convert.ToInt32(Request.Form["UID"]);

                if (!string.IsNullOrEmpty(Request.Form["Gender"]))
                {
                    model.Gender = Convert.ToInt32(Request.Form["Gender"]);

                }

                model.DisplayImg = modclient.GetYogisModelsById(model.UID).DisplayImg;// Request.Form["DisplayImg"].ToString();
                model.RealName = Request.Form["RealName"].ToString();
                model.CenterID = Request.Form["hCenterID"].ToString().TrimEnd(',') == "" ? Request.Form["CenterID"].ToString().TrimEnd(',') : Request.Form["hCenterID"].ToString().TrimEnd(',');
                model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
                model.TeachYogis = Request.Form["hTeachYogis"].ToString().TrimEnd(',') == "" ? Request.Form["TeachYogis"].ToString().TrimEnd(',') : Request.Form["hTeachYogis"].ToString().TrimEnd(',');
                model.EachClassCost = Request.Form["EachClassCost"].ToString();
                model.StartTeachYear = Convert.ToDateTime(Request.Form["StartTeachYear"].ToString());
                model.Nationality = Request.Form["ddlNationality"].ToString();
                model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
                model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
                model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
                model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
                model.Street = Request.Form["Street"].ToString();
                model.iRate = Convert.ToInt32(Request.Form["ddliRate"].ToString());
                model.CoverImg = Request.Form["CoverImg"].ToString();
                model.YogiStatus = Convert.ToInt32(Request.Form["YogiStatus"].ToString());
                model.YogisLevel = Convert.ToInt32(Request.Form["YogisLevel"].ToString());
                model.GudWords = Request.Form["GudWords"].ToString();
                model.delState = 0;
            }
            catch (Exception ex)
            {
                Info = ex.Message;
            }
            return model;
        }


        /// <summary>
        /// 会馆列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadCenterList(int page = 1)
        {
            List<ViewCenters> list = new List<ViewCenters>();
            int count = 0;
            using (CentersServiceClient CentersServiceClient = new CentersServiceClient())
            {
                list = CentersServiceClient.GetCentersPageList(page, 10, "0", out count);
            }
            Webdiyer.WebControls.Mvc.PagedList<ViewCenters> pageslist = new Webdiyer.WebControls.Mvc.PagedList<ViewCenters>(list, page, 10, count);
            return View(pageslist);
        }
        /// <summary>
        /// 流派列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadYogaTypeidList(int page = 1)
        {
            List<ViewYogaDicItem> list = new List<ViewYogaDicItem>();
            int count = 0;
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                list = YogaDicItemServiceClient.GetYogaDicItemPageList("", 63, page, 10, out count);
            }
            Webdiyer.WebControls.Mvc.PagedList<ViewYogaDicItem> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewYogaDicItem>(list, page, 10, count);
            return View(pagelist);
        }
        /// <summary>
        /// 导师列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTeachYogisList(int page = 1, string text = null)
        {
            List<ViewYogisModels> list = new List<ViewYogisModels>();

            int count = 0;

            using (YogisModelsServiceClient client = new YogisModelsServiceClient())
            {
                //list = client.GetYogisModelsPageList(page, 9, out count);
                list = client.GetYogisModelsList(text, 2, 0, "", page, 9, out count);
            }

            Webdiyer.WebControls.Mvc.PagedList<ViewYogisModels> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewYogisModels>(list, page, 9, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("GetTeachYogisListChild", pagelist);
            }
            return View(pagelist);
        }

        #endregion


        #region 习练者基本信息

        public ActionResult UserDetailEdit()
        {
            ViewBag.id = user.Uid;
            int iUserid = user.Uid;
            #region 登录者的级别
            if (user.UserType == 0)
            {
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
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                using (YogisModelsServiceClient ymclient = new YogisModelsServiceClient())
                {
                    vyogism = ymclient.GetYogisModelsById(user.Uid);
                    if (vyogism != null)
                        ViewBag.level = vyogism.YogisLevel;
                }
            }
            #endregion
            ViewYogaUserDetail model = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                model = client.GetYogaUserDetailById(iUserid);
                if (!string.IsNullOrEmpty(model.YogaTypeid))
                {
                    string[] YogaTypeidlist = model.YogaTypeid.Split(',');
                    #region 流派

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
                    #endregion
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UserDetailEdit(ViewYogaUserDetail model)
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
            ViewYogaUserDetail mm = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                mm = client.GetById(model.ID);
            }
            mm.CreateTime = mm.CreateTime;
            mm.UpgradeDate = DateTime.Now;
            mm.Uscore = 0;
            mm.RealName_cn = model.RealName_cn;
            mm.RealName_en = model.RealName_en;
            mm.Gender = model.Gender;
            mm.GudWords = model.GudWords;
            mm.Street = model.Street;
            mm.Covimg = model.Covimg;
            #region 是否公开

            mm.Nationality = Convert.ToInt32(Request.Form["ddlNationality"].ToString());
            mm.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
            mm.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
            mm.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
            mm.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
            mm.LocationID = 0;
            mm.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');

            #endregion

            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
            {
                clientModels.Update(mm);
            }
            return RedirectToAction("UserDetailEdit", "Login", new { id = model.UID });

        }

        [HttpPost]
        public ActionResult UserDetailEditper(ViewYogaUserDetail model)
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
            ViewYogaUserDetail mm = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                mm = client.GetById(model.ID);
            }
            mm.CreateTime = mm.CreateTime;
            mm.UpgradeDate = DateTime.Now;
            mm.Uscore = 0;
            mm.RealName_cn = model.RealName_cn;
            mm.RealName_en = model.RealName_en;
            mm.Gender = model.Gender;
            mm.GudWords = model.GudWords;
            mm.Street = model.Street;
            mm.Covimg = model.Covimg;
            #region 是否公开

            mm.Nationality = Convert.ToInt32(Request.Form["ddlNationality"].ToString());
            mm.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
            mm.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
            mm.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
            mm.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
            mm.LocationID = 0;
            mm.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');

            #endregion

            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
            {
                clientModels.Update(mm);
            }
            return Content("true");//RedirectToAction("PersonalSetUp", "YogaUserDetail", new { id = model.UID });

        }
        #endregion

        #region 习练者全部信息

        public ActionResult UserEdit()
        {
            ViewBag.id = user.Uid;
            int iUserid = user.Uid;
            ViewYogaUserDetail model = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                model = client.GetYogaUserDetailById(iUserid);
                if (!string.IsNullOrEmpty(model.YogaTypeid))
                {
                    string[] YogaTypeidlist = model.YogaTypeid.Split(',');
                    #region 流派

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
                    #endregion
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult UserEdit(ViewYogaUserDetail model)
        {
            ViewYogaUserDetail mm = new ViewYogaUserDetail();
            using (YogaUserDetailServiceClient client = new YogaUserDetailServiceClient())
            {
                mm = client.GetYogaUserDetailById(model.UID);
            }
            model.CreateTime = mm.CreateTime;
            model.UpgradeDate = DateTime.Now;

            #region 是否公开

            if (Request.Form["IsRealName_cn"] != null) model.IsRealName_cn = 1;
            else model.IsRealName_cn = 0;
            if (Request.Form["IsRealName_en"] != null) model.IsRealName_en = 1;
            else model.IsRealName_en = 0;
            if (Request.Form["IsBirthYear"] != null) model.IsBirthYear = 1;
            else model.IsBirthYear = 0;
            if (Request.Form["IsBirthMonth"] != null) model.IsBirthMonth = 1;
            else model.IsBirthMonth = 0;
            if (Request.Form["IsBirthDay"] != null) model.IsBirthDay = 1;
            else model.IsBirthDay = 0;
            if (Request.Form["IsNationality"] != null) model.IsNationality = 1;
            else model.IsNationality = 0;
            if (Request.Form["IsQQ"] != null) model.IsQQ = 1;
            else model.IsQQ = 0;
            if (Request.Form["IsPostal"] != null) model.IsPostal = 1;
            else model.IsPostal = 0;
            if (Request.Form["IsZip"] != null) model.IsZip = 1;
            else model.IsZip = 0;
            if (Request.Form["IsTel"] != null) model.IsTel = 1;
            else model.IsQQ = 0;
            if (Request.Form["IsMobile"] != null) model.IsMobile = 1;
            else model.IsMobile = 0;
            if (Request.Form["IsWeibo"] != null) model.IsWeibo = 1;
            else model.IsWeibo = 0;
            if (Request.Form["IsPersonalURL"] != null) model.IsPersonalURL = 1;
            else model.IsPersonalURL = 0;

            model.Nationality = Convert.ToInt32(Request.Form["ddlNationality"].ToString());
            model.CountryID = Convert.ToInt32(Request.Form["ddlCountryID"].ToString());
            model.ProvinceID = Convert.ToInt32(Request.Form["ddlProvinceID"].ToString());
            model.CityID = Convert.ToInt32(Request.Form["ddlCityID"].ToString());
            model.DistrictID = Convert.ToInt32(Request.Form["ddlDistrictID"].ToString());
            model.YogaTypeid = Request.Form["hYogaTypeid"].ToString().TrimEnd(',') == "" ? Request.Form["YogaTypeid"].ToString().TrimEnd(',') : Request.Form["hYogaTypeid"].ToString().TrimEnd(',');
            model.LocationID = 0;//Request.Form["hLocationID"].ToString()=="" ?0: Convert.ToInt32(Request.Form["hLocationID"].ToString());

            #endregion

            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
            {
                clientModels.Update(model);
            }
            return RedirectToAction("Details", "YogaUserDetail", new { id = model.UID });

        }

        #endregion


        /// <summary>
        /// 协议
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ArticleDetail(int id)
        {
            ViewYogaArticle model = new ViewYogaArticle();
            using (YogaArticleServiceClient client = new YogaArticleServiceClient())
            {
                model = client.GetYogaArticleById(id);
            }
            return View(model);

        }

        /// <summary>
        /// 个人设置
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonelSetUp(int id)
        {
            if (user.UserType == 0)
            {
                #region 习练者
                ViewBag.UserType = 0;

                ViewBag.id = id;
                int strUid = 0;
                int iLoginID = user.Uid;//登录用户ID
                ViewBag.iLoginID = user.Uid;

                ViewYogaUserDetail temp = new ViewYogaUserDetail();

                temp = client.GetYogaUserDetailById(id);

                #region 习练者基本信息
                try
                {
                    strUid = temp.UID;
                    ViewBag.strUid = temp.UID;
                }
                catch { }
                ///位置
                string strCountryID = "";
                string strProvinceID = "";
                string strCityID = "";
                string strDistrictID = "";
                if (temp.CountryID != null && temp.CountryID != 0)
                {
                    strCountryID = method.GetItemName(temp.CountryID.Value) + "·";
                }
                if (temp.ProvinceID != null && temp.ProvinceID != 0)
                {
                    strProvinceID = method.GetItemName(temp.ProvinceID.Value) + "·";
                }
                if (temp.CityID != null && temp.CityID != 0)
                {
                    strCityID = method.GetItemName(temp.CityID.Value) + "·";
                }
                if (temp.DistrictID != null && temp.DistrictID != 0)
                {
                    strDistrictID = method.GetItemName(temp.DistrictID.Value);
                }

                ViewBag.AddRessName = strCountryID + strProvinceID + strCityID + strDistrictID;
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
                #endregion

                ViewYogaUser userEntity = new ViewYogaUser();
                userEntity = clientUser.GetYogaUserById(temp.UID);
                ViewBag.NickName = userEntity.NickName;//昵称

                //关注 粉丝 
                ViewFollow viewMoel = new ViewFollow();
                ViewBag.iCount = clientFoll.GetFollowByUid(id);
                ViewBag.FCount = clientFoll.GetFollowByCount(id);

                return View(temp);
                #endregion
            }
            else
            {

                #region 导师

                ViewBag.UserType = 1;

                ViewBag.id = id;
                int strUid = user.Uid;
                int iLoginID = user.Uid;//登录用户ID
                ViewBag.iLoginID = user.Uid;

                ViewYogisModels temp = new ViewYogisModels();

                #region 导师专页 基本信息

                temp = modclient.GetYogisModelsById(id);
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
                }
                catch { }
                #endregion

                //关注 粉丝 人气
                ViewFollow viewMoel = new ViewFollow();
                ViewBag.iCount = clientFoll.GetFollowByUid(id);
                ViewBag.FCount = clientFoll.GetFollowByCount(id);

                ViewBag.tzancount = clientMsg.GettMessageUid(id, 1).Count();

                return View(temp);
                #endregion

            }

        }
    }
}
