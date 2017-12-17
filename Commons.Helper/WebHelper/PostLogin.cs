
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
namespace Commons.Helper
{
   public class Html
    {
        /// 
        /// httpwebrequest类中的一些属性的集合
        /// 
        public struct RequestPPT
        {
            private string strAccept;
            /// 
            /// 获取或设置request类中的Accept属性
            /// 用以设置接受的文件类型
            ///             
            public string Accept
            {
                get
                {
                    return strAccept;
                }
                set
                {
                    strAccept = value;
                }
            }
            private string strContentType;
            /// 
            /// 获取或设置request类中的ContentType属性
            /// 用以设置请求的媒体类型
            ///           
            public string ContentType
            {
                get
                {
                    return strContentType;
                }
                set
                {
                    strContentType = value;
                }
            }
            /// 
            /// 获取或设置request类中的UserAgent属性
            /// 用以设置请求的客户端信息
            /// 
            private string strUserAgent;
            public string UserAgent
            {
                get
                {
                    return strUserAgent;
                }
                set
                {
                    strUserAgent = value;
                }
            }
            private string strMethod;
            /// 
            /// 获取或设置request类中的Method属性
            /// 可以将 Method 属性设置为任何 HTTP 1.1 协议谓词：GET、HEAD、POST、PUT、DELETE、TRACE 或 OPTIONS。
            /// 如果 ContentLength 属性被设置为 -1 以外的任何值，则必须将 Method 属性设置为上载数据的协议属性。
            ///             
            public string Method
            {
                get
                {
                    return strMethod;
                }
                set
                {
                    strMethod = value;
                }
            }
        }
        /// 
        /// 构建一个httt请求以获取目标链接的cookies,需要传入目标的登录地址和相关的post信息,返回完成登录的cookies,以及返回的html内容
        /// 
        /// 登录页面的地址
        /// post信息
        /// 输出的html代码
        /// 请求的标头所需要的相关属性设置
        /// 请求完成后的cookies
        public CookieCollection funGetCookie(string url, byte[] post, out string strHtml, RequestPPT rppt,string server)
        {

            CookieCollection ckclReturn = new CookieCollection();
            CookieContainer cc = new CookieContainer();
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;
            //请求cookies的格式
            //hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            //hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            //string cookie = hwResponse.Headers.Get("Set-Cookie");
            //cookie = cookie.Split(';')[0];
            //hwRequest = null;
            //hwResponse = null;
            //构建即将发送的包头
            //cc.SetCookies(new Uri(server), cookie);           
            hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            hwRequest.CookieContainer = cc;
            hwRequest.Accept = rppt.Accept;
            hwRequest.ContentType = rppt.ContentType;
           
            hwRequest.UserAgent = rppt.UserAgent;
            hwRequest.Method = rppt.Method;
            hwRequest.ContentLength = post.Length;
            //写入标头
            Stream stream;
            stream = hwRequest.GetRequestStream();
            stream.Write(post, 0, post.Length);
            stream.Close();
            //发送请求获取响应内容
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            }
            catch
            {
                strHtml = "";
                return ckclReturn;
            }
            //hwResponse.Headers["location"]
            stream = hwResponse.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
            strHtml = sReader.ReadToEnd();
            sReader.Close();
            stream.Close();
            //获取缓存内容
            ckclReturn = hwResponse.Cookies;
            return ckclReturn;
        }
       /// 
       /// 根据已经获取的有效cookies来获取目标链接的内容
       /// 
       /// 目标链接的url
       /// 已经获取到的有效cookies
       /// 头属性的相关设置
       /// 目标连接的纯文本:"txt/html"
       public string funGetHtmlByCookies(string strUri, CookieCollection ccl, RequestPPT rppt)
       {
           CookieContainer cc = new CookieContainer();
           HttpWebRequest hwRequest;
           HttpWebResponse hwResponse;      

           //构建即将发送的包头
           hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(strUri));
           cc.Add(ccl);
           hwRequest.CookieContainer = cc;
           hwRequest.Accept = rppt.Accept;
           hwRequest.ContentType = rppt.ContentType;
           hwRequest.UserAgent = rppt.UserAgent;
           hwRequest.Method = rppt.Method;
           hwRequest.ContentLength = 0;    

           //发送请求获取响应内容
           try
           {
               hwResponse = (HttpWebResponse)hwRequest.GetResponse();
           }
           catch
           {
               return "";
           }

           Stream stream;
           stream = hwResponse.GetResponseStream();
           StreamReader sReader = new StreamReader(stream, Encoding.Default);
           string strHtml = sReader.ReadToEnd();
           sReader.Close();
           stream.Close();

           //返回值
           return strHtml;
       }
       /// 
       /// 根据已经获取的有效cookies来获取目标链接的内容
       /// 
       /// 目标链接的url
       ///post的byte信息
       /// 已经获取到的有效cookies
       /// 头属性的相关设置
       /// 目标连接的纯文本:"txt/html"
       public string funGetHtmlByCookies(string strUri,byte[] post, CookieCollection ccl, RequestPPT rppt)
       {
           CookieContainer cc = new CookieContainer();
           HttpWebRequest hwRequest;
           HttpWebResponse hwResponse;

           //构建即将发送的包头
           hwRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(strUri));
           cc.Add(ccl);
           hwRequest.CookieContainer = cc;
           hwRequest.Accept = rppt.Accept;
           hwRequest.ContentType = rppt.ContentType;
           hwRequest.UserAgent = rppt.UserAgent;
           hwRequest.Method = rppt.Method;
           hwRequest.ContentLength = post.Length;
           //写入post信息
           Stream stream;
           stream = hwRequest.GetRequestStream();
           stream.Write(post, 0, post.Length);
           stream.Close();
           //发送请求获取响应内容
           try
           {
               hwResponse = (HttpWebResponse)hwRequest.GetResponse();
           }
           catch
           {
               return"" ;
           }

           stream = hwResponse.GetResponseStream();
           StreamReader sReader = new StreamReader(stream, Encoding.Default);
           string strHtml = sReader.ReadToEnd();
           sReader.Close();
           stream.Close();

           //返回值
           return strHtml;
       }
    }
}  


