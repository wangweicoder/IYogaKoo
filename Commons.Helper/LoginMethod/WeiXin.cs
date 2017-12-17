using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Commons.Helper
{
    class WeiXin: IoAuthLogin
    {
        /// <summary>
        /// 申请应用时分配的AppKey。
        /// </summary>
        public string appid = "wx199205cc1b843533";
        /// <summary>
        /// App Secret
        /// </summary>
        public string AppSecret = "10b719f2aec46b40dbc128871c679b83";
        /// <summary>
        /// 授权回调地址，站外应用需与设置的回调地址一致，站内应用需填写canvas page的地址。
        /// </summary>
        //public string redirect_uri = "http://www.iyogakoo.org/login/OauthBack";
        public string redirect_uri = System.Web.HttpUtility.UrlEncode(System.Web.HttpUtility.HtmlEncode(@"http://www.iyogakoo.org/login/OauthBack"), Encoding.GetEncoding("UTF-8"));

        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        private string state = ((int)LoginType.微信).ToString();

        //页面地址
        //获取code地址
        public string code_url = "https://open.weixin.qq.com/connect/qrconnect";
        //获取用户信息地址
        public string user_url = "https://api.weixin.qq.com/sns/userinfo";
        //获取accesstoken地址
        private string accessToken_url = "https://api.weixin.qq.com/sns/oauth2/access_token";

        private string openid = "";
        //private string openKey = "";
        private string access_token = "";
        private string code = "";
        //appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect
        /// <summary>
        /// 获取登陆入口
        /// </summary>
        /// <returns></returns>
        public string GetLoginUrl(HttpCookieCollection cookies)
        {
            return code_url + "?appid=" + appid + "&response_type=code&scope=snsapi_login&state="+state+"&redirect_uri=" + redirect_uri;
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
            YogaUserServiceClient client = new YogaUserServiceClient();
            lr = client.GetYogaUserByWechatAuthCode(openid); 
             
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
            UrlParameter param = new UrlParameter("appid", appid);
            parameters.Add(param);
            param = new UrlParameter("secret", AppSecret);
            parameters.Add(param);
            param = new UrlParameter("grant_type", "authorization_code");
            parameters.Add(param);
            //param = new UrlParameter("redirect_uri", redirect_uri);
            //parameters.Add(param);
            param = new UrlParameter("code", code);
            parameters.Add(param);
            string data = OAuthRequest.Request(accessToken_url, parameters, "GET");
            JObject j = JObject.Parse(data);
            access_token = (string)j["access_token"];
            openid = (string)j["openid"];
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
            param = new UrlParameter("openid", openid);
            parameters.Add(param);
            string jsonData = OAuthRequest.Request(user_url, parameters, "GET");
            JObject json = JObject.Parse(jsonData);

            OauthInfo oi = new OauthInfo();
            oi.NickName = (string)json["nickname"];
            oi.Avatar = (string)json["headimgurl"];
            oi.AuthCode = openid;
            oi.ChatBack = code;
            oi.LoginType = LoginType.微信;
            return oi;
        }
    }
    
}
