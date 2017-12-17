using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IYogaKoo.Controllers.ashxFile
{
    /// <summary>
    /// CoverImg 的摘要说明
    /// </summary>
    public class GetMyselfImg : IHttpHandler
    {

        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            context.Response.CacheControl = "no-cache";
            string imgJson = string.Empty;

            if (context.Request.QueryString["id"] != null)
            {
                int uid = Convert.ToInt32(context.Request.QueryString["id"]);
                List<ViewYogaPicture> imgs = new List<ViewYogaPicture>();
                bool part = false;
                string PictureName = "";//相册名称
                if (context.Request.QueryString["part"] != null)
                {
                    part = true;
                    if (context.Request.QueryString["PictureName"] != null)
                    {
                        PictureName = context.Request.QueryString["PictureName"].ToString();
                        imgs = GetImgs(uid, PictureName, part);
                    }
                    else
                    {
                        imgs = GetImgs(uid, "", part);
                    }
                }  
                else
                {
                    imgs = GetImgs(uid,"", part);
                }
               
                int imgid = Convert.ToInt32(context.Request.QueryString["imgid"]);
                imgJson = FormateJson(imgs, uid, imgid);
            }

            context.Response.Write(imgJson);
            context.Response.End();
        }

        /// <summary>
        /// 格式化json
        /// </summary>
        /// <param name="imgs"></param>
        /// <returns></returns>
        private string FormateJson(List<ViewYogaPicture> imgs, int uid, int imgid)
        {
            //获得当前相册人
            ViewYogisModels model = null;
            ViewYogaUserDetail udetail = null;
            ViewYogaUser u = null;
            string defaultimg = string.Empty;
            string defaultname = string.Empty;

            YogaUserDetailServiceClient udetailClient = new YogaUserDetailServiceClient();
            YogisModelsServiceClient modelClient = new YogisModelsServiceClient();
            YogaUserServiceClient userClient = new YogaUserServiceClient();
            u = userClient.GetYogaUserById(uid);

            if (u != null)
            {
                //习练者
                if (u.UserType == 0)
                {
                    udetail = udetailClient.GetYogaUserDetailById(uid);
                    if (udetail != null)
                    {
                        defaultimg = CommonInfo.GetDisplayImg(udetail.DisplayImg); 
                        defaultname = u.NickName;
                    }
                }
                else
                {
                    //导师
                    model = modelClient.GetYogisModelsById(uid);
                    if (model != null)
                    {
                        defaultimg =CommonInfo.GetDisplayImg(model.DisplayImg);
                        //大师
                        //if (model.YogisLevel == 4)
                            defaultname = model.RealName;
                        //else
                        //    defaultname = u.NickName;
                    }
                }
            }

            tMessageServiceClient mesClient = new tMessageServiceClient();

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"code\":1,");
            sb.Append("\"album\":\"默认相册\",");
            sb.Append(String.Format("\"showimages\":\"{0}\",", imgid));
            sb.Append("\"thumbList\":[");

            using (tMessageServiceClient msgClient = new tMessageServiceClient())
            {
                foreach (ViewYogaPicture img in imgs)
                {
                    sb.Append("{");
                    sb.Append("\"id\":");
                    sb.Append(String.Format("\"{0}\",", img.Pid));
                    sb.Append("\"title\":");
                    sb.Append("\"相册\",");
                    sb.Append("\"user\":");
                    sb.Append(String.Format("\"{0}\",", defaultname));
                    sb.Append("\"avatar\":");
                    sb.Append(String.Format("\"{0}\",", defaultimg));
                    sb.Append("\"time\":");
                    sb.Append(string.Format("\"{0}\",", img.CreateTime==null?"":Convert.ToDateTime(img.CreateTime).ToString("yyyy-MM-dd HH:mm")));
                    sb.Append("\"desc\":");
                    sb.Append(string.Format("\"{0}\",", img.PictureContent));
                    sb.Append("\"thumb\":");
                    sb.Append(string.Format("\"{0}\",", img.PictureOriginal));
                    sb.Append("\"large\":");
                    sb.Append(string.Format("\"{0}\",", img.PictureOriginal));
                    sb.Append("\"comment\":");
                    sb.Append("[");
                    List<ViewtMessage> messages = mesClient.GettMessageUid(img.Pid, 5);
                    foreach (ViewtMessage v in messages)
                    {
                        string defcovimg = string.Empty;
                        string defname = string.Empty;
                        if (v.FormType == 0)
                        {
                            udetail = udetailClient.GetYogaUserDetailById((int)v.FromUid);
                            if (udetail != null)
                                defcovimg = CommonInfo.GetDisplayImg(udetail.DisplayImg);
                            u = userClient.GetYogaUserById((int)v.FromUid);
                            if (u != null)
                                defname = u.NickName;
                        }
                        else
                        {
                            model = modelClient.GetYogisModelsById((int)v.FromUid);
                            if (model != null)
                            {
                                defcovimg = CommonInfo.GetDisplayImg(model.DisplayImg);
                                defname = model.RealName;
                            }
                        }

                        sb.Append("{");
                        sb.Append("\"avatar\":");
                        sb.Append(String.Format("\"{0}\",", defcovimg));
                        sb.Append("\"user\":");
                        sb.Append(String.Format("\"{0}\",", defname));
                        sb.Append("\"msg\":");
                        sb.Append(String.Format("\"{0}\"", v.sContent));
                        sb.Append("},");
                    }
                    if (messages != null && messages.Count > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    sb.Append("]");
                    sb.Append("},");
                }
            }
            if (imgs != null && imgs.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }


            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 根据uid得到图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="part">记录是否显示部分相册</param>
        /// <returns></returns>
        private List<ViewYogaPicture> GetImgs(int id, string PictureName, bool part)
        {
            List<ViewYogaPicture> listPic = new List<ViewYogaPicture>();
            using (YogaPictureServiceClient picclient = new YogaPictureServiceClient())
            {
                if (part)
                {
                    int piccount = 0;
                    if (!string.IsNullOrEmpty(PictureName))
                    {
                        listPic = picclient.GetBackUidList(id, PictureName);
                        piccount = listPic.Count();
                    }
                    else
                    {
                        listPic = picclient.GetYogaPicturePageList(id, 1, 19, out piccount);
                    }
                }
                else
                {
                    listPic = picclient.GetUidList(id);
                }
            }

            return listPic;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}