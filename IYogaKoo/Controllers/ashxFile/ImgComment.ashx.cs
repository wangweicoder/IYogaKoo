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
    public class ImgComment : IHttpHandler
    {
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            context.Response.CacheControl = "no-cache";
            string commentJson = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (context.Request.Params["msg"] != null)
            {
                using (tMessageServiceClient msgClient = new tMessageServiceClient())
                {
                    ViewtMessage msgmodel = new ViewtMessage();
                    msgmodel.ToUid = Convert.ToInt32(context.Request.Params["pictureid"]);
                    msgmodel.ToType = 5;
                    msgmodel.sContent = context.Request.Params["msg"];
                    msgmodel.FromUid = user.Uid;
                    msgmodel.FormType = user.UserType;
                    msgmodel.CreateDate = DateTime.Now;
                    msgmodel.iZan = 0;
                    msgmodel.iAudio = 0;
                    msgmodel.ParentID = 0;
                    msgmodel.photo = " ";
                    if (user != null)
                    {
                        msgClient.Add(msgmodel);
                    }
                }

                sb.Append("{");
                sb.Append("\"code\":1,");
                sb.Append("\"msg\":\"成功\",");
                sb.Append("\"comment\":");
                sb.Append("[");
                sb.Append("{");

                sb.Append(string.Format("\"avatar\":\"{0}\",", user.Avatar));
                sb.Append(string.Format("\"user\":\"{0}\",", user.NickName));
                sb.Append(String.Format("\"msg\":\"{0}\"", context.Request.Params["msg"]));

                sb.Append("}");
                sb.Append("]");
                sb.Append("}");
            }
            context.Response.Write(sb.ToString());
            context.Response.End();
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