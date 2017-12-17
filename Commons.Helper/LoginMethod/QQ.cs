using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web; 
using Newtonsoft.Json.Linq; 
using System.Text;
using IYogaKoo.ViewModel.Commons.Enums;
using Commons.Helper.LoginMethod;
using IYogaKoo.ViewModel;
using IYogaKoo.Client;

namespace Commons.Helper
{
    /// <summary>
    ///QQLogin 的摘要说明
    /// </summary>
    public class QQ : IoAuthLogin
    {
        /// <summary>
        /// 申请应用时分配的AppKey。
        /// </summary>
        private string client_id = "101238544";
        /// <summary>
        /// App Secret
        /// </summary>
        private string client_secret = "092be7d7d5fea9beca024da6af50dfd6";
        /// <summary>
        /// 授权回调地址，站外应用需与设置的回调地址一致，站内应用需填写canvas page的地址。
        /// </summary>
        //private string redirect_uri = "http://www.iyogakoo.org/login/OauthBack";
        public string redirect_uri = System.Web.HttpUtility.UrlEncode(System.Web.HttpUtility.HtmlEncode(@"http://www.iyogakoo.org/login/OauthBack"), Encoding.GetEncoding("UTF-8"));
        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        private string state = ((int)LoginType.QQ).ToString();

        //页面地址
        //获取code地址
        private string code_url = "https://graph.qq.com/oauth2.0/authorize";
        //获取用户信息地址
        private string user_url = "https://graph.qq.com/user/get_user_info";
        //获取accesstoken地址
        private string accessToken_url = "https://graph.qq.com/oauth2.0/token";

        private string openID = "";
        //private string openKey = "";
        private string access_token = "";
        private string code = "";

        /// <summary>
        /// 获取登陆入口
        /// </summary>
        /// <returns></returns>
        public string GetLoginUrl(HttpCookieCollection cookies)
        {
            return code_url + "?response_type=code&client_id=" + client_id + "&redirect_uri=" + redirect_uri + "&state="+state;
        }

        /// <summary>
        /// 是否已经注册
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ViewYogaUser IsRegister(NameValueCollection collection, HttpCookieCollection cookies)
        {
            ViewYogaUser lr = new ViewYogaUser();
            code = collection["code"];
            access_token = GetAccessToken();
            openID = GetUID();
            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                lr = client.GetYogaUserByWechatAuthCode(openID);  
            } 
            return lr;
        }
        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetAccessToken()
        {
            List<UrlParameter> parameters = new List<UrlParameter>();
            UrlParameter param = new UrlParameter("client_id", client_id);
            parameters.Add(param);
            param = new UrlParameter("client_secret", client_secret);
            parameters.Add(param);
            param = new UrlParameter("grant_type", "authorization_code");
            parameters.Add(param);
            param = new UrlParameter("code", code);
            parameters.Add(param);
            param = new UrlParameter("state", "qq");
            parameters.Add(param);
            param = new UrlParameter("redirect_uri", redirect_uri);
            parameters.Add(param);
            string data = OAuthRequest.Request(accessToken_url, parameters, "GET");
            access_token = Login.GetValueFromUrlParameter(data, "access_token");
            return access_token;
        }
        
        /// <summary>
        /// 获取openid
        /// </summary>
        /// <returns></returns>
        private string GetUID()
        {
            List<UrlParameter> parameters = new List<UrlParameter>();
            UrlParameter param = new UrlParameter("access_token", access_token);
            parameters.Add(param);
            string data = OAuthRequest.Request("https://graph.qq.com/oauth2.0/me", parameters, "GET");
            //callback( {"client_id":"101238544","openid":"9990DBF4348CA41F6606530FC20648F2"} ); 
            int st = data.IndexOf('(');
            data = data.Substring(st);
            data = data.Substring(1, data.Length - 4);
            JObject j = JObject.Parse(data);
            data = (string)j["openid"]; 
            return data;
        }
        /// <summary>
        /// 获取openid这种样式的字符串中每个值
        /// </summary>
        /// <param name="urlParameter"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public static string GetValueFromUrlParameter(string urlParameter, string pName)
        {
            string[] strs = urlParameter.Split(',');
            string result = null;
            foreach (string str in strs)
            {
                string[] p = str.Split(':');
                if (p[0].ToLower() == pName.ToLower())
                    result = p[1];
            }
            return result;
        }
        /// <summary>
        /// 获取第三方的用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public OauthInfo GetUser()
        {
           //string accessToken = GetAccessToken();

            List<UrlParameter> parameters = new List<UrlParameter>();
            UrlParameter param = new UrlParameter("access_token", access_token);
            parameters.Add(param);
            param = new UrlParameter("oauth_consumer_key", client_id);
            parameters.Add(param);
            param = new UrlParameter("openid", openID);
            parameters.Add(param);
            param = new UrlParameter("format", "json");
            parameters.Add(param);
            string jsonData = OAuthRequest.Request(user_url, parameters, "GET");
            JObject json = JObject.Parse(jsonData);

            OauthInfo oi = new OauthInfo();
            oi.NickName = (string)json["nickname"];
            oi.Avatar = (string)json["figureurl_2"];
            oi.AuthCode = openID;
            oi.ChatBack = code;
            oi.LoginType = LoginType.QQ;
            return oi;
        }
 
    }
}