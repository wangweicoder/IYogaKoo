using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json.Linq; 
using Commons.Helper.LoginMethod;
using IYogaKoo.ViewModel.Commons.Enums;
using IYogaKoo.ViewModel;
using IYogaKoo.Client;

namespace Commons.Helper
{
    public class Sina : IoAuthLogin
    {
        /// <summary>
        /// 申请应用时分配的AppKey。
        /// </summary>
        public string client_id = "1166584639";//"2778092018";
        /// <summary>
        /// App Secret
        /// </summary>
        public string client_secret = "def61d2ae5b9e1ed210ea28cf48d9a68";//"db4539d8e9d429287a8410c923c5697e";
        /// <summary>
        /// 授权回调地址，站外应用需与设置的回调地址一致，站内应用需填写canvas page的地址。
        /// </summary>
        public string redirect_uri = "http://www.iyogakoo.org/login/OauthBack";
        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        private string state = ((int)LoginType.Sina微博).ToString();

        //页面地址
        //获取code地址
        public string code_url = "https://api.weibo.com/oauth2/authorize";
        //获取用户信息地址
        public string user_url = "https://api.weibo.com/2/users/show.json";
        //获取accesstoken地址
        private string accessToken_url = "https://api.weibo.com/oauth2/access_token";

        private string WechatAuthCode = "";
        //private string openKey = "";
        private string access_token = "";
        private string code = "";
        
        /// <summary>
        /// 获取登陆入口
        /// </summary>
        /// <returns></returns>
        public string GetLoginUrl(HttpCookieCollection cookies)
        {
            return code_url + "?client_id=" + client_id + "&response_type=code&redirect_uri=" + redirect_uri;
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
            YogaUserServiceClient client=new YogaUserServiceClient ();
            lr = client.GetYogaUserByWechatAuthCode(WechatAuthCode);
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
            param = new UrlParameter("redirect_uri", redirect_uri);
            parameters.Add(param);
            param = new UrlParameter("code", code);
            parameters.Add(param);
            string data = OAuthRequest.Request(accessToken_url, parameters, "POST");            
            JObject j = JObject.Parse(data);
            access_token = (string)j["access_token"];
            WechatAuthCode = (string)j["uid"];
            return access_token;
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
            param = new UrlParameter("uid", WechatAuthCode);
            parameters.Add(param);
            string jsonData = OAuthRequest.Request(user_url, parameters, "GET");
            JObject json = JObject.Parse(jsonData);

            OauthInfo oi = new OauthInfo();
            oi.NickName = (string)json["screen_name"];
            oi.Avatar = (string)json["avatar_large"];
            oi.AuthCode = WechatAuthCode;
            oi.ChatBack = code;
            oi.LoginType = LoginType.Sina微博;
            return oi;
        }
    }
}