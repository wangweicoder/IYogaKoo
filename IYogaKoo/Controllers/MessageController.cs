using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using zzfIBM.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/
        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        tMessageServiceClient client;
        ViewtMessage model;
        List<ViewtMessage> list;
        YogaUserDetailServiceClient udclient;
        YogisModelsServiceClient mclient;
        YogaUserServiceClient clientUser;
        tWriteLogServiceClient logclient;
        method method;
        public MessageController()
        {
            ViewBag.user = user;
            client = new tMessageServiceClient();
            model = new ViewtMessage();
            list = new List<ViewtMessage>();
            clientUser = new YogaUserServiceClient();
            udclient = new YogaUserDetailServiceClient();
            mclient = new YogisModelsServiceClient();
            logclient = new tWriteLogServiceClient();
            method = new method();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = udclient.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else//导师级别
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = mclient.GetYogisModelsById(user.Uid);
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
        /// 留言|推荐 一览列表
        /// </summary>
        /// <param name="iType">0 留言 1 推荐 </param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int iType, int page = 1)
        {
            ViewBag.iType = iType;
            int count = 0;
            list = client.GettMessageUidList(user.Uid, iType, page, 10, out count);
            PagedList<ViewtMessage> pagelist = new PagedList<ViewtMessage>(list, page, 10, count);

            List<ViewtMessageGroup> listGroup = new List<ViewtMessageGroup>();
            foreach (var item in list)
            {
                ViewtMessageGroup model = new ViewtMessageGroup();
                model.entity = item;

                ViewYogaUser userEntity = clientUser.GetYogaUserById(item.FromUid.Value);
                if (userEntity != null)
                {
                    model.FromUser = userEntity.NickName;
                }
                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            return View(pagelist);

        }

        //
        // GET: /Message/Details/5

        public ActionResult MyMessage(int iType, int page = 1)
        {
            ViewBag.delif = iType;
            ViewBag.iType = 2;
            int count = 0;
            ViewBag.uid = user.Uid;
           
            int pagesize =10;
            List<ViewLogsMessages> logsmsgs = new List<ViewLogsMessages>();
            ViewLogsMessages lm;
            List<ViewtWriteLog> ls = logclient.GettWriteLogPageListByMessage(iType, user.Uid, page, pagesize, out count);
            foreach (ViewtWriteLog log in ls)
            {
                lm = new ViewLogsMessages();
                //留言人
                ViewYogaUser usermodel = clientUser.GetYogaUserById(log.Uid.Value);
                if (usermodel != null)
                {
                    ViewBag.currentname = lm.name = usermodel.NickName;
                    if (usermodel.UserType == 0)
                    {
                        ViewBag.url = lm.userurl = "/YogaUserDetail/Details/" + log.Uid;
                        ViewYogaUserDetail ViewDet = udclient.GetYogaUserDetailById((int)log.Uid);
                        if (ViewDet != null)
                        {
                            ViewBag.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
                        }

                    }
                    else
                    {
                        ViewBag.url = lm.userurl = "/YogisModels/Details/" + log.Uid;

                        ViewYogisModels ViewDet = mclient.GetYogisModelsById((int)log.Uid);
                        if (ViewDet != null)
                        {
                            ViewBag.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
                        }
                    }
                }
             
                lm.id = log.ID.ToString();
                lm.title = log.sTitle;
                lm.imgurl = GetFirstImages(log.sContent);
                string noContent=NoHTML(log.sContent);
                if(noContent.Length>120)
                    noContent=noContent.Substring(0,120)+"...";
                lm.content = noContent;
                int mescount = 0;
                lm.messages = listMessage(log.ID, 1, 50, out mescount);
                lm.messagescount = mescount;
                logsmsgs.Add(lm);
            }

            Webdiyer.WebControls.Mvc.PagedList<ViewLogsMessages> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewLogsMessages>(logsmsgs, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialMsg", pagelist);
            }else
            return View(pagelist);

        }

        /// <summary>
        /// 获取第一张图片
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public  string GetFirstImages(string htmlText)
        {
            const string pattern = "<img [^~]*?>";
            const string pattern1 = "src\\s*=\\s*((\"|\')?)(?<url>\\S+)(\"|\')?[^>]*";
            string s = null;
            Match match = Regex.Match(htmlText, pattern, RegexOptions.IgnoreCase);  //找到img标记
            if (match.Success)
            {
                string img = match.Value;
                string imgsrc = Regex.Match(img, pattern1, RegexOptions.IgnoreCase).Result("${url}");
                imgsrc = Regex.Replace(imgsrc, "\"|\'|\\>", "", RegexOptions.IgnoreCase);
                s = imgsrc;
            }
            return s;
        }

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelMes(int id)
        {
            int val = client.Delete(id.ToString());
            return val;
        }

        /// <summary>
        /// 评论/留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewtMessageGroup> listMessage(int id, int page, int pagesize,out int rcount)
        {
            #region 留言
            if (page == 0) page = 1;            
            List<ViewtMessage> msgEntity = new List<ViewtMessage>();            
            msgEntity = client.GettMessageUidList(id, 4, page, pagesize, out rcount);
            List<ViewtMessageGroup> listGroupMsg = new List<ViewtMessageGroup>();

            foreach (var item in msgEntity)
            {
                ViewtMessageGroup model = new ViewtMessageGroup();

                model.entity = item;
                //被留言人

                ViewYogaUser yuser = clientUser.GetYogaUserById(item.ToUid.Value);
                if (yuser != null)
                    model.ToUser = yuser.NickName;
                //留言人
                ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                if (usermodel != null)
                    model.FromUser = usermodel.NickName;

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
                                model.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
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
                                model.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
                            }
                            model.sUrl = "/YogisModels/Details/" + item.FromUid.Value;
                        }

                    }
                }

                #endregion

                //strPic
                //回复
                List<ViewtMessage> listM = client.GettMessageParentID(item.ID);
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

                    #region 头像

                    if (it.FormType == 0)
                    {
                        //习练者头像
                        using (YogaUserDetailServiceClient clientDet = new YogaUserDetailServiceClient())
                        {
                            ViewYogaUserDetail ViewDet = new ViewYogaUserDetail();
                            if (it.FromUid != 0)
                            {
                                ViewDet = clientDet.GetYogaUserDetailById(it.FromUid.Value);
                                if (ViewDet != null)
                                {
                                    entityMsg.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
                                }
                                entityMsg.sUrl = "/YogaUserDetail/Details/" + it.FromUid.Value;
                            }
                        }
                    }
                    else
                    {
                        //导师头像
                        using (YogisModelsServiceClient clientDet = new YogisModelsServiceClient())
                        {
                            ViewYogisModels ViewDet = new ViewYogisModels();
                            if (it.FromUid != 0)
                            {
                                ViewDet = clientDet.GetYogisModelsById(it.FromUid.Value);
                                if (ViewDet != null)
                                {
                                    entityMsg.DisplayImg = CommonInfo.GetDisplayImg(ViewDet.DisplayImg);
                                }
                                entityMsg.sUrl = "/YogisModels/Details/" + it.FromUid.Value;
                            }
                        }
                    }

                    #endregion
                    entitylist.Add(entityMsg);

                }
                model.msgList = entitylist;
                listGroupMsg.Add(model);
            }
            #endregion

            return listGroupMsg;
        }
        
        /// <summary>
        /// 将html文本转化为 文本内容方法NoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("/r/n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }

    }
}
