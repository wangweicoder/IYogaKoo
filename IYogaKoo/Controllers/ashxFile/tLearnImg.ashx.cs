using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IYogaKoo.Controllers.ashxFile
{
    /// <summary>
    /// 文章图片
    /// tLearnImg 的摘要说明
    /// </summary>
    public class tLearnImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            try
            {
                HttpPostedFile FilePath = context.Request.Files["Filedata"];
                string uid = @context.Request.Params["Uid"];
                string FileServerPath = HttpContext.Current.Server.MapPath("~/Files/tLearing");
                string FileServerPathTask = HttpContext.Current.Server.MapPath("~/Files/tLearing/" + uid);
               
                if (FilePath != null)
                {
                    if (!Directory.Exists(FileServerPath))
                    {
                        Directory.CreateDirectory(FileServerPath);
                    }
                }
                if (FileServerPathTask != null)
                {
                    if (!Directory.Exists(FileServerPathTask))
                    {
                        Directory.CreateDirectory(FileServerPathTask);
                    }
                     
                    int size = FilePath.ContentLength;
                    if (size / (1024 * 1024) >= 4)
                    {
                        context.Response.Write("文件太大！请上传小于4M的图片！");
                    }
                    else
                    {
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff");// + SysFunction.FsRandomString(10);
                        string fileExt = FilePath.FileName.Substring(FilePath.FileName.LastIndexOf("."));
                        string FileServerFullPath = FileServerPathTask + "\\" + filename + fileExt;
                        FilePath.SaveAs(FileServerFullPath);
                        //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                        context.Response.Write("Files/tLearing/"  +uid+"/"+filename + fileExt);
                    }
                }
                else
                {
                    context.Response.Write("<script type='text/javascript'>alert('上传失败，请重试！');</script>");
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