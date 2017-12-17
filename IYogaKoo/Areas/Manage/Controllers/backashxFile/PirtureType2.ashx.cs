using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IYogaKoo.Areas.Manage.Controllers.backashxFile
{
    /// <summary>
    /// PirtureType2 的摘要说明
    /// </summary>
    public class PirtureType2 : IHttpHandler
    {

        public string FileServerPath = "";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            try
            {
                HttpPostedFile FilePath = context.Request.Files["Filedata"];
                //if (FilePath != null)
                //{
                //    // 解决uploadify兼容火狐谷歌浏览器上传问题
                //    // 但是，此代码使系统有安全隐患，Flash程序请求该系统不需要验证
                //    // 要解决此安全隐患，需要Flash程序传用户名和密码过来验证，但是该用户名和密码不能写在前端以便被不法用户看到
                //    if (context.Request.UserAgent == "Shockwave Flash" || context.Request.UserAgent == "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                //    { }}
                        string FileServerPath = HttpContext.Current.Server.MapPath("~") + "/Files";
                        string FileServerPathTask = HttpContext.Current.Server.MapPath("~") + "/Files/PirtureType/2";
                        string uid = @context.Request.Params["Uid"];
                        string strPath = @context.Request.Params["fName"];//图片保存路径
                        if (string.IsNullOrEmpty(strPath))
                        {
                            context.Response.Write("还没有创建相册哦！");
                        }
                        else
                        {
                            string spath = uid + "\\" + strPath;
                            if (FilePath != null)
                            {
                                if (!Directory.Exists(FileServerPath))
                                {
                                    Directory.CreateDirectory(FileServerPath);
                                }
                            }
                            if (FileServerPathTask != null)
                            {
                                #region

                                if (!Directory.Exists(FileServerPathTask))
                                {
                                    Directory.CreateDirectory(FileServerPathTask);
                                }

                                if (!Directory.Exists(FileServerPathTask + "\\" + spath))
                                {
                                    Directory.CreateDirectory(FileServerPathTask + "\\" + spath);
                                }
                                int size = FilePath.ContentLength;
                                if (size / (1024 * 1024) >= 4)
                                {
                                    context.Response.Write("<script>alert('文件超大！请精简该文件再执行导入操作！')</script>");
                                }
                                else
                                {
                                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff");// + SysFunction.FsRandomString(10);
                                    string fileExt = FilePath.FileName.Substring(FilePath.FileName.LastIndexOf("."));
                                    string FileServerFullPath = FileServerPathTask + "\\" + spath + "\\" + filename + fileExt;
                                    FilePath.SaveAs(FileServerFullPath);
                                    //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                                    context.Response.Write("Files/PirtureType/2/" + uid + "/" + strPath + "/" + filename + fileExt);
                                }
                                #endregion
                            }
                            else
                            {
                                context.Response.Write("<script type='text/javascript'>alert('上传失败，请重试！');</script>");
                            }
                        }
                 
            }
            catch
            {
                context.Response.Write("<script type='text/javascript'>alert('上传失败，请重试！');</script>");
            }
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