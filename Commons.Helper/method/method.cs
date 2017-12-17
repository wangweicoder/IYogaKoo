using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Commons.Helper
{
    /// <summary>
    /// 通用：评论/留言
    /// </summary>
    public class method
    {
         
        ClassServiceClient client ;
        YogaDicItemServiceClient dicclient;
        InterestServiceClient interclient;
        tMessageServiceClient msgclient;
        YogaUserServiceClient clientUser;
        tBannerServiceClient clientbanner;
        YogaUserDetailServiceClient udclient;
        YogisModelsServiceClient modelsclient;
        tZanModelsServiceClient zanclient;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        tUserLoginInfoServiceClient userloginInfoclient;

        tInstationInfoServiceClient tinstaclient;
        FollowServiceClient followclient; 
        tMessageServiceClient messageclient;
        CentersServiceClient cenclient;
        public method()
        { 
            client = new ClassServiceClient();
            dicclient = new YogaDicItemServiceClient();
            interclient = new InterestServiceClient();
            msgclient = new tMessageServiceClient();
            clientUser = new YogaUserServiceClient();
            clientbanner = new tBannerServiceClient();
            udclient = new YogaUserDetailServiceClient();
            modelsclient = new YogisModelsServiceClient(); 
            userloginInfoclient = new tUserLoginInfoServiceClient(); 
            tinstaclient = new tInstationInfoServiceClient();
            followclient = new FollowServiceClient();
            zanclient = new tZanModelsServiceClient();
            messageclient = new tMessageServiceClient();
            cenclient = new IYogaKoo.Client.CentersServiceClient(); 
        }
        
        #region  齐奇Add 

        /// <summary>
        /// 会馆名称
        /// </summary>
        /// <param name="CenterId"></param>
        /// <returns></returns>
        public string GetCenterName(string CenterId)
        {
            string ReturnValue = string.Empty;
            if (!string.IsNullOrEmpty(CenterId)) {
                ViewCenters entity = new ViewCenters();
              
                if (CenterId.IndexOf(',') != -1)
                {
                    string[] ids = CenterId.Split(',');
                    foreach (var i in ids)
                    {
                        entity = new ViewCenters();
                        entity = cenclient.GetById(Convert.ToInt32(i));
                        if (entity != null)
                        {
                            ReturnValue += entity.CenterName + ",";
                        } 
                    }
                }
                else
                {
                    entity = cenclient.GetById(Convert.ToInt32(CenterId.Trim()));
                    if (entity != null)
                    {
                        ReturnValue = entity.CenterName;
                    }  
                }
            } 
            return ReturnValue;
        }
        /// <summary>
        /// true 已经关注、false 没有关注
        /// </summary>
        /// <param name="Uid">登录人</param>
        /// <param name="FollowUId">被关注人</param>
        /// <returns></returns>
        public bool iGetFollow(int Uid, int FollowUId)
        {
            ViewFollow followEntity = followclient.GetFollowById(Uid, FollowUId);
            if (followEntity != null)
            {
                return followEntity.isfollow.Value;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取站内信的数量
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tinstatcount">系统信息量</param>
        /// <param name="follcount">关注量</param>
        /// <param name="zancount">赞量</param>
        /// <param name="msgcount">评论量</param>
        /// <returns></returns>
        public int InstationInfo(int uid, out int tinstatcount, out int follcount, out int zancount, out int msgcount)
        {
            List<ViewtInstationInfo> listWhere0 = tinstaclient.GetPageListWhereUidAndloginType(uid, 0, out tinstatcount);

            List<ViewFollow> folllistWhere0 = followclient.GetFollowQuiltUidList(uid, 0, out follcount);

            List<ViewtZanModels> zanlistWhere0 = zanclient.GetByFromUidList(uid, 0, out zancount);

            List<ViewtMessage> msglistWhere0 = messageclient.GetPageListWhereUidAndloginType(uid, 0, out msgcount);
           
            return 0;
        }
        /// <summary>
        /// 评论/留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewtMessageGroup> listMessage(int id,int iType, int page, out int rcount) 
        { 
            #region 留言
            if (page == 0) page = 1;
            int pagesize = 10;
            List<ViewtMessage> msgEntity = new List<ViewtMessage>();
            //msgEntity = msgclient.GettMessageUid(id, 0);
            msgEntity = msgclient.GettMessageUidList(id, iType, page, pagesize, out rcount);
            List<ViewtMessageGroup> listGroupMsg = new List<ViewtMessageGroup>();

            foreach (var item in msgEntity)
            {
                ViewtMessageGroup model = new ViewtMessageGroup();

                model.entity = item;
                model.entity.iZan = zanclient.ZanCount(item.ID,item.FormType.Value);
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
                List<ViewtMessage> listM = msgclient.GettMessageParentID(item.ID);
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

        /// <summary>
        /// 0 首页Banner 1 活动回顾Banner  2 活动预告Banner
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public List<ViewtBanner> listBanner(int iType)
        { 
            List<ViewtBanner> listBanner = new List<ViewtBanner>();
            listBanner = clientbanner.GettBannerList(iType);
            return listBanner;
        }

        /// <summary>
        /// 国籍DicId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetDicId(string ItemName)
        {
            ViewYogaDicItem entity = new ViewYogaDicItem();
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                entity = YogaDicItemServiceClient.GetDicId(103).Where(a=>a.ItemName==ItemName).FirstOrDefault();
            }

            return entity.DicId;
        }
        /// <summary>
        /// 流派DicId
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        public string GetYogaTypeid(string ItemName)
        {
            string Info = "";
            string[] ids;
            if (!string.IsNullOrEmpty(ItemName))
            {
                ids = ItemName.Split(' ');
                using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
                { 
                   List<ViewYogaDicItem> list=new List<ViewYogaDicItem> ();
                   list = YogaDicItemServiceClient.GetDicId(63);

                    for (int i = 0; i < ids.Count(); i++)
                    { 
                        for (int k = 0; k < list.Count(); k++)
                        {
                            if (ids[i] == list[k].ItemName)
                            {
                                Info += list[k].DicId + ",";
                            }
                        }
                    }
                }
            } 

            return Info;
        }

        /// <summary>
        /// 社区=2158
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewYogaDicItem> listDicItem(int id)
        {
            List<ViewYogaDicItem> list=new List<ViewYogaDicItem> (); 
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                list = YogaDicItemServiceClient.GetDicId(id);
            }
            return list;
        }

        /// <summary>
        /// 获取字符串中img的url集合
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public List<string> getImgUrl(string content)
        {
            Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var m = rg.Match(content);
            List<string> imgUrl = new List<string>();
            while (m.Success)
            {
                imgUrl.Add(m.Groups[1].Value); //这里就是图片路径                
                m = m.NextMatch();
            }
            return imgUrl;
        }

        /// <summary>
        /// 地区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetItemName(int id)
        {
            ViewYogaDicItem list = new ViewYogaDicItem();
             
            list = dicclient.GetYogaDicItemById(id);
             
            if (list != null)
                return list.ItemName;
            else
                return "";
        }
        /// <summary>
        /// 返回昵称/管理员
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public string GetNickName(int Uid)
        {
            string NickName = "管理员";
            if (Uid != 0)
            {
                ViewYogaUser userEntity = clientUser.GetYogaUserById(Uid);
                if (userEntity != null)
                    NickName = userEntity.NickName;
            }
            return  NickName;
        }
        /// <summary>
        /// 取他/她人 ：姓别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Gender(int id)
        {
            string strGender = "她";
            ViewYogaUser yogauser = new ViewYogaUser();
            yogauser= clientUser.GetYogaUserById(id);
            if (yogauser.UserType == 0)
            { 
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = udclient.GetYogaUserDetailById(id);
                if(temp!=null)
                {
                    if(temp.Gender!=null)
                    {
                        if(temp.Gender.Value==0)
                        {
                            strGender = "她";
                        }
                        else strGender = "他";
                    }
                }
               
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = modelsclient.GetYogisModelsById(id);
                if (vyogism != null)
                {
                    if (vyogism.Gender != null)
                    {
                        if (vyogism.Gender.Value == 0)
                        {
                            strGender = "她";
                        }
                        else strGender = "他";
                    }
                }
            }
            return strGender; 
        }

        #endregion
        /// <summary>
        /// 返回头像路径
        /// </summary>
        /// <param name="imgpath"></param>
        /// <returns></returns>
        public string DisplayImg(string imgpath)
        {
            string drscr = "";

            if (!string.IsNullOrEmpty(imgpath))
            {
                drscr = imgpath;

                if (drscr.IndexOf(';') != -1)
                {
                    var str = drscr.Split(';');
                    if (str.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(str[1]))
                        {
                            drscr = str[1];
                        }
                        else
                        {
                            drscr = str[0];
                        }
                    }
                    else
                    {
                        drscr = str[0];
                    }
                }
            }
            else
            {
                drscr = "/Content/Front/images/defaultImg.png";
            }
            return drscr;
        }

        /// <summary>
        /// 返回 1 表示最新信息
        /// </summary>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public int iNewInfo(int uid,DateTime CreateTime)
        {
            int iGet = 0;
            ViewtUserLoginInfo model = userloginInfoclient.GetByUid(uid);
            if (model != null)
            {
                if (model.LoginTime < CreateTime)
                {
                    iGet = 1;
                } 
            }
            return iGet;
        }
        /// <summary>
        /// 用户登录/退出信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public ViewtUserLoginInfo GetLoginInfo(int uid,string nickname,string Message,bool flag)
        {
            ViewtUserLoginInfo entity = new ViewtUserLoginInfo();
            entity.sloginInfo = Message;
            entity.Uid = uid;
            entity.NickName = nickname;

            if (!flag)
            {
                entity.QuitTime = DateTime.Now;
            }
            else
            {
                entity.LoginTime = DateTime.Now;

            }
                return entity; 
        }
    }
}