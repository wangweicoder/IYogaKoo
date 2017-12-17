using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Commons.Helper.LoginMethod; 

namespace Commons.Helper
{
    public class OAuthRequest
    {
        /// <summary>
        /// 请求地址获取返回值
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">url参数集合</param>
        /// <param name="type">请求类型</param>
        /// <returns></returns>
        public static string Request(string url, List<UrlParameter> parameters, string type)
        {
            WebRequest request = null;
            if (type.ToUpper() == "GET")
            {
                url = GetUrl(url, parameters);//附加参数
                request = HttpWebRequest.Create(url);
                request.Method = type;
            }
            else
            {
                byte[] data = GetPostData(parameters);
                request = HttpWebRequest.Create(url);
                request.Method = type;
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
            }

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("UTF-8"));
            string result = reader.ReadToEnd();

            reader.Close();
            stream.Close();
            response.Close();
            request.Abort();

            return result;
        }
        /// <summary>
        /// 根据url参数获取带参数的url地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetUrl(string url, List<UrlParameter> parameters)
        {
            url += "?";
            foreach (UrlParameter up in parameters)
            {
                url += up.Name + "=" + up.Value + "&";
            }
            url = url.TrimEnd('&').TrimEnd('?');
            return url;
        }
        /// <summary>
        /// 根据参数集合得到表单数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static byte[] GetPostData(List<UrlParameter> parameters)
        {
            string data = "";
            foreach (UrlParameter up in parameters)
            {
                data += up.Name + "=" + up.Value + "&";
            }
            data = data.TrimEnd('&');
            return Encoding.ASCII.GetBytes(data);
        }
    }
}
