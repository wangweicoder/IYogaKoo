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
    public class LearnController : Controller
    {
        //社区
        // GET: /Learn/
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        tLearingServiceClient client;
        YogaDicItemServiceClient dicclient;
        InterestServiceClient interclient;
        tMessageServiceClient msgclient;
        YogaUserServiceClient clientUser;
        YogaUserDetailServiceClient userDetclient;
        method method;
        tSignServiceClient Signclient;
        tZanModelsServiceClient zanclient;
        public LearnController()
        {
            ViewBag.user = user;
            client = new tLearingServiceClient();
            dicclient = new YogaDicItemServiceClient();
            interclient = new InterestServiceClient();
            msgclient = new tMessageServiceClient();
            clientUser = new YogaUserServiceClient();
            userDetclient = new YogaUserDetailServiceClient();
            method = new method();
            Signclient = new tSignServiceClient();
            zanclient = new tZanModelsServiceClient();

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

        /// <summary>
        /// 社区首页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int page=1)
        {
            ViewBag.iSignCount =  Signclient.GetCount(DateTime.Now.ToString("yyyy-MM-dd"));
            ViewBag.RowNums = Signclient.RowNums(user.Uid);
            int pagesize = 12;
            int count = 0;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            int hidtLearn = 0;
            //类别
            if (!string.IsNullOrEmpty(Request.Form["hidtLearn"]))
            {
                hidtLearn = Convert.ToInt32(Request.Form["hidtLearn"]);
            }

            List<ViewtLearing> list = client.GetPageList(hidtLearn, page, pagesize, out count);
            for (var i = 0; i < list.Count(); i++)
            { 
                list[i].iWritelogNums=msgclient.GettMessageUid(list[i].ID,2).Count();
            }
            PagedList<ViewtLearing> pagelist = new PagedList<ViewtLearing>(list, page, pagesize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("LearingPartial", pagelist);
            }
            return View(pagelist);
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult tSign()
        {
            try
            {
                ViewtSign entity = new ViewtSign();
                if (!Signclient.ExistsSign(user.Uid))
                {
                    entity = new ViewtSign();
                    entity.Uid = user.Uid;
                    entity.CreateDate = DateTime.Now;
                    entity.iType = user.UserType;
                    entity.iIntegral = 0;
                    Signclient.Add(entity);
                    return Json(new { code = 0 });
                }
                else return Json(new { code = 2 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }
        }

        public ActionResult Create()
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            if(user!=null)
                ViewBag.Name = user.NickName;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Create(ViewtLearing model)
        {
            try
            {
                ViewtLearing entity = client.ExistsTitle(user.Uid.ToString(), model.sTitle);
                if (entity == null)
                {
                    model.Uid = user.Uid.ToString();
                    model.iWritelogNums = 0;
                    model.iZanNums = 0;
                    model.iReadNums = 0;
                    model.CreateDate = DateTime.Now;
                    model.ifexamine = false;
                    model.UrlType = 0;
                    client.Add(model);
                    return Json(new { code = 0 });
                }
                return Json(new { code = 2 });//code=2 已经存在该文章
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }
            
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id,int page=1)
        {           
            ViewBag.id = id; 
            ViewtLearing model = client.GetById(id);
            model.iReadNums = model.iReadNums + 1;
            client.Update(model);//阅读量+1

            if (user != null)
            {
                ViewBag.NickName = user.NickName;
            }

            #region 上一篇文章和下一篇文章
             
            //用pre和next变量分别存放上一篇文章和下一篇文章的id号
            int pre = 0, next = 0, i = 0, j;
            int retCount = 0;
            //计算总记录数
            List<ViewtLearing> listEnity = client.GetPageList_All(0,out retCount);
            ViewBag.listEnity = listEnity.Where(x => x.ifexamine == true).Take(15).ToList();
            int num = retCount;
            int[] a = new int[num];
            var query = from c in listEnity select c.ID;
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
                ViewBag.preTitle = listEnity.Where(c => c.ID == pre).Single().sTitle;
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
                ViewBag.nextTitle = listEnity.Where(c => c.ID == next).Single().sTitle;
                ViewBag.next = next;
            }
            
            #endregion
            model.iZanNums = zanclient.Count(id, user.Uid, 2);
            ViewBag.learn = model;
            int rcount = 0;
            int pagesize = 10;
            ViewBag.MsgInfo = method.listMessage(id, 2,page, out rcount);//留言 评论
            ViewBag.rcount = rcount;

            Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup> l = new Webdiyer.WebControls.Mvc.PagedList<ViewtMessageGroup>(ViewBag.MsgInfo, page, pagesize, rcount);
            if (Request.IsAjaxRequest())
                return PartialView("PartialMsg",l);
            return View(l);
            　　
        }
        /// <summary>
        /// 加载时是否赞过
        /// </summary>
        /// <param name="logid"></param>
        /// <returns>0赞过、1未赞</returns>
        public int ifZan(int id)
        {
            int value = 0;
            
            ViewtZanModels zan = zanclient.GetByFromToUid(id, user.Uid, 2);
            if (zan != null)
            {
                value = 1;
            }
             
            return value;
        }
        /// <summary>
        /// 赞(手）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult iZanHand(int id)
        {
            try
            {  
                int iCount = 0;
                ViewtZanModels zanEntity = new ViewtZanModels();
                using (tZanModelsServiceClient zanclient = new tZanModelsServiceClient())
                {
                    zanEntity = zanclient.GetByFromToUid(id, user.Uid, 2);
                    if (zanEntity == null)
                    {
                        zanEntity = new ViewtZanModels();
                        zanEntity.iFromUid = user.Uid;
                        zanEntity.iToUid = id;
                        zanEntity.iType = user.UserType;
                        zanEntity.iToType = 2;
                        zanEntity.CreateDate = DateTime.Now;
                        zanEntity.loginType = 0;
                        zanclient.Add(zanEntity);

                        //查询Count

                        ViewtLearing model = client.GetById(id);
                        iCount = zanclient.Count(id, user.Uid, 2);
                        model.iZanNums = iCount;
                        client.Update(model);//推荐（赞）量+1
                    }
                    else {
                        return Json(new { code = 2 });
                    }
                }

                return Json(new { code = 0, iCount = iCount });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }

        }
        /// <summary>
        /// 评论赞(+1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult iZan()
        {
            try
            {
               
                ViewtZanModels zanEntity = new ViewtZanModels(); 

                int Uid = Convert.ToInt32(Request.Form["uid"]);//主键ID
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
                    zanEntity.loginType = 0;
                    zanclient.Add(zanEntity);
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 2 });//已经赞过
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }

          
        }
    }
}
