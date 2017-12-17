using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace IYogaKoo.Areas.Manage.Controllers.backashxFile
{
    /// <summary>
    /// PirtureType2 的摘要说明
    /// </summary>
    public class PirtureThumbnail : IHttpHandler
    {

        public string FileServerPath = "";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            try
            {
                HttpPostedFile FilePath = context.Request.Files["Filedata"];
                string FileServerPath = HttpContext.Current.Server.MapPath("~") + "/Files";
                string FileServerPathTask = HttpContext.Current.Server.MapPath("~") + "/Files/Evaluate/2";
                string FileServerPathTaskSmall = HttpContext.Current.Server.MapPath("~") + "/Files/Evaluate/2/Small";
                string FileServerPathTaskMiddle = HttpContext.Current.Server.MapPath("~") + "/Files/Evaluate/2/Middle";
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
                    if (!Directory.Exists(FileServerPathTaskSmall))
                    {
                        Directory.CreateDirectory(FileServerPathTaskSmall);
                    }
                    if (!Directory.Exists(FileServerPathTaskMiddle))
                    {
                        Directory.CreateDirectory(FileServerPathTaskMiddle);
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
                        string FileServerFullPath = FileServerPathTask + "\\" + filename + fileExt;
                        FilePath.SaveAs(FileServerFullPath);

                        //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                        string newimg = "Files/Evaluate/2/" + filename + fileExt;
                        context.Response.Write(newimg);
                        //生成缩略图,中图
                        MakeThumbnail(FilePath.InputStream, FileServerPathTaskMiddle + "\\" + filename + fileExt, 367, 400, "W"); 
                        //生成缩略图,小图
                        MakeThumbnail(FilePath.InputStream, FileServerPathTaskSmall + "\\" + filename + fileExt, 50, 50, "Cut"); 
                    }
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            catch
            {
                context.Response.Write("0");
            }
        }

        /**/
        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public static void MakeThumbnail(Stream me  , string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage ;//= Image.FromFile(originalImagePath);
            //MemoryStream me = new MemoryStream(mybyte, 0, mybyte.Length);
            originalImage = Image.FromStream(me);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
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