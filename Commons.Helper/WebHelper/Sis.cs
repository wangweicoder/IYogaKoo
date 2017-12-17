
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
namespace Commons.Helper
{
    public class SisHtml :Html
    {
        public SisHtml()
        {

        }

        /// <summary>
        /// 设置主机ip地址
        /// </summary>
        public string Host
        {
            get {
                return strHost;

            }
            set {
                strHost = value;
            }
        }

        private string strHost;

        /// <summary>
        /// 获取目标登录链接的cookies
        /// </summary>
        /// <param name="url">目标的登录链接</param>
        /// <param name="dir">构造头的泛型键值对</param>
        /// <param name="strHtml">登录后返回的页面内容</param>
        /// <returns>登录后的cookies</returns>
        public CookieCollection funGetCookie(string url, Dictionary<string, string> dir, out string strHtml)
        {
            CookieCollection cc = new CookieCollection();
            RequestPPT rppt = new RequestPPT();

            //构建post内容
            string strPost = funMakePost(dir);
            byte[] post = Encoding.Default.GetBytes(strPost);

            //设置标头属性
            rppt.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            rppt.ContentType = "application/x-www-form-urlencoded";
            
            rppt.Method = "Post";
            rppt.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            string server ="http://"+ new Uri(url).Host;
            return cc = base.funGetCookie(url, post, out strHtml, rppt, server);
        }

        /// <summary>
        /// 根据已经获取到cookies来获取目标链接的内容
        /// </summary>
        /// <param name="strUri">目标的url</param>
        /// <param name="ccl">已经获取好的cookies</param>
        /// <returns>目标url的纯文本:"txt/html"</returns>
        public string funGetHtmlByCookies(string strUri,CookieCollection ccl )
        {

            RequestPPT rppt = new RequestPPT();     
            //设置头属性
            rppt.Accept = "txt/html";
            rppt.ContentType = "application/x-www-form-urlencoded";
            rppt.Method = "Post";
            rppt.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            return base.funGetHtmlByCookies(strUri, ccl, rppt);
        }


        /// <summary>
        /// 投票帖子用的方法
        /// </summary>
        /// <param name="strHtml">投票帖子的htmlcode</param>
        /// <param name="ccl">有效的cookies</param>
        /// <returns>投票完成以后的htmlcode</returns>
        public string  funVote(string strHtml,CookieCollection ccl)
        {
           //判断是不是选取投票
            try
            {
                strHtml = strHtml.Substring(strHtml.IndexOf("<form"), strHtml.LastIndexOf("</form>") - strHtml.IndexOf("<form") + 7);
            }
            catch
            {
                return "";
            }
            string strCheck = @"name=""pollanswers[]""";
            //如果代码中包含关键信息说明没有被投票过
            if(strHtml.IndexOf(strCheck)>0)
            {
                //获取post头的需求信息
                string strFormHash = "77b49df4";
                string strPollanswers;
                strPollanswers = strHtml.Substring(strHtml.IndexOf(strCheck)+strCheck.Length, 20).Split('"')[1];
                string strPollansubmit = "提交";
                Dictionary<string,string>dir = new Dictionary<string,string>();
                dir.Add("formhash",strFormHash);
                dir.Add("pollanswers[]",strPollanswers);
                dir.Add("pollsubmit",strPollansubmit);
                string strPost = funMakePost(dir);
                byte[] post = Encoding.Default.GetBytes(strPost);
                //获取请求的路径
                string strUrl= "http://"+Host+"/bbs/";
                string strActionUrl =@"method=""post""";
                strUrl+= strHtml.Substring(strHtml.IndexOf(strActionUrl)+strActionUrl.Length,100).Split('"')[1].Replace("amp;","");
                 //构建头
                RequestPPT rppt = new RequestPPT();
                rppt.Accept = "txt/html";
                rppt.ContentType = "application/x-www-form-urlencoded";
                rppt.Method = "Post";
                rppt.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                strHtml = base.funGetHtmlByCookies(strUrl, post, ccl, rppt);
            }
            return strHtml;
        }

        /// <summary>
        /// 根据泛型来构建字符串用于post
        /// </summary>
        /// <param name="dir">带有键值对的泛型</param>
        /// <returns>构建完毕的字符串</returns>
        private string funMakePost(Dictionary<string,string> dir)
        {
            string strPost="";
            foreach (KeyValuePair<string, string> kvp in dir)
            {
                strPost += kvp.Key + "=";
                if (kvp.Value == "")
                {
                    strPost += "''";
                }
                else
                {
                    strPost += kvp.Value;
                }
                strPost += "&";
            }
            strPost = strPost.Substring(0, strPost.Length - 1);
            return strPost;
        }

        /// <summary>
        /// 获取下一个列表页面的路径
        /// </summary>
        /// <param name="strHtml">当前页面的htmlcode</param>
        /// <returns>下一个列表页面的路径</returns>
        public string funGetNextUrl(string strHtml)
        {
            string strUrl = "";
            //判断是否是列表型页面
            if (strHtml.IndexOf("<form") != -1)
            {
                return strUrl;
            }
            string strKey =@"class=""next""";
            strUrl = "http://"+Host+"/bbs/"+strHtml.Substring(strHtml.IndexOf(strKey) - 100, 100).Split('"')[1].Replace("amp;", "");
            return strUrl;
        }

        public DataTable funGetListTable(string strHtml)
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("Url");
            dt.Columns.Add(dc);
            DataRow dr ;
            string strReg = @"viewthread.php(\S)+highlight=";
            Regex rg = new Regex(strReg);
            MatchCollection mc = rg.Matches(strHtml);

            foreach (Match ms in mc)
            {
                dr = dt.NewRow();
                dr[0] = "http://" + Host + "/bbs/" + ms.ToString().Replace("amp;", "");
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}