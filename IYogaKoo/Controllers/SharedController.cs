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
using System.Drawing;

namespace IYogaKoo.Controllers
{
    public class SharedController : Controller
    {
        /// <summary>
        /// 获取登录/注册信息
        /// </summary>
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        method method;
        int tinstatcount = 0;
        int follcount = 0;
        int zancount = 0;
        int msgcount = 0;
        public SharedController()
        {
            ViewBag.user = user;
            ViewBag.id = user.Uid;
            method =new method();
            #region  站内信-信息数量
             
            method.InstationInfo(user.Uid, out   tinstatcount, out   follcount, out   zancount, out   msgcount);

            ViewBag.tinstatcount = tinstatcount;
            ViewBag.follcount = follcount;
            ViewBag.zancount = zancount;
            ViewBag.msgcount = msgcount;
            ViewBag.AllCount = tinstatcount + follcount + zancount + msgcount;
            #endregion
        }
        //
        // GET: /Shared/
        public ActionResult Index()
        {
            return View();
        }
        // GET: /Shared/
        public ActionResult Register()
        {
            return View();
        }
        // GET: /Shared/
        public ActionResult YogaError()
        {
            return View();
        }
        // GET: /Shared/
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// 站内信母版页
        /// </summary>
        /// <returns></returns>
        public ActionResult _LayoutInstationInfo()
        {
            #region  站内信-信息数量 
            ViewBag.tinstatcount = tinstatcount;
            ViewBag.follcount = follcount;
            ViewBag.zancount = zancount;
            ViewBag.msgcount = msgcount;
            ViewBag.AllCount = tinstatcount + follcount + zancount + msgcount;
            #endregion
            return View(); 
        }
        
        /// <summary>
        /// 返回底部部分视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _ParialFooter()
        {
            return PartialView("_PartialFooter");
        }

        #region 验证码

        public ActionResult check()
        { 
            //调用自定义方法绘制验证码
            CreateCheckCodeImage(GenerateCheckCode());
            return View();
        }
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
            //使用For循环生成4个数字
            for (int i = 0; i < 4; i++)
            {
                //生成一个随机数
                number = random.Next();
                //将数字转换成为字符型
                code = (char)('0' + (char)(number % 10));

                checkCode += code.ToString();
            }
            //将生成的随机数添加到Cookies中
            Response.Cookies.Add(new HttpCookie("emailCheckCode", checkCode));
            //返回字符串
            return checkCode;
        }

        private void CreateCheckCodeImage(string checkCode)
        {
            //判断字符串不等于空和null
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            //创建一个位图对象
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(65, 34);
            //创建Graphics对象
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景干扰线
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                Font font = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Black, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //将图片输出到页面上
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion

        public JsonResult GetArea(int id = 0, bool? forChild=null)
        {
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            List<ViewYogaDicItem> list = dicClient.GetSelectList(id==0?CommonInfo.CountryZHID:id, forChild);
            List<SelectListItem> items = (from s in list select new SelectListItem() { Text=s.ItemName, Value=s.ID.ToString() }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        
    }
}