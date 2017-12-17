using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
namespace Commons.Helper
{
    public static class CommonInfo
    {
        public static string ExtendName(this string filename)
        {
            return filename.Substring(filename.LastIndexOf('.'));
        }

        /// <summary>
        /// 订单超时时间，分钟
        /// 默认15分钟
        /// </summary>
        public static int Order_Timeout
        {
            get
            {
                string value = AppSetting("Order_Timeout");
                return value == null ? 15 : int.Parse(value);
            }
        }

        /// <summary>
        /// 取消订单时间限制，小时
        /// 默认96小时
        /// </summary>
        public static int Order_Cancel_Timeout
        {
            get
            {
                string value = AppSetting("Order_Cancel_Timeout");
                return value == null ? 96 : int.Parse(value);
            }
        }

        /// <summary>
        /// 国籍字典id
        /// </summary>
        public static int NationID
        {
            get
            {
                string value = AppSetting("nation");
                return value == null ? 0 : int.Parse(value);
            }
        }

        /// <summary>
        /// 瑜伽体系id
        /// </summary>
        public static int YogaSystemID
        {
            get
            {
                string value = AppSetting("yogasystem");
                return value == null ? 0 : int.Parse(value);
            }
        }

        /// <summary>
        /// 国家字典id
        /// </summary>
        public static int CountryID
        {
            get
            {
                string value = AppSetting("country");
                return value == null ? 0 : int.Parse(value);
            }
        }

        /// <summary>
        /// 国家字典id
        /// </summary>
        public static int CountryZHID
        {
            get
            {
                string value = AppSetting("country-zh");
                return value == null ? 0 : int.Parse(value);
            }
        }

        /// <summary>
        /// 课主题
        /// </summary>
        public static int Topic
        {
            get
            {
                string value = AppSetting("topic");
                return value == null ? 0 : int.Parse(value);
            }
        }

        private static string AppSetting(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        #region
        /// <summary>
        /// 格式化人员封面显示
        /// </summary>
        /// <param name="displayimg"></param>
        /// <returns></returns>
        public static string GetDisplayImg(string tempdisplayimg)
        {
            if (!string.IsNullOrEmpty(tempdisplayimg))
            {
                if (tempdisplayimg.IndexOf(';') != -1)
                {
                    var str = tempdisplayimg.Split(';');
                    if (str.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(str[1]))
                        {
                            tempdisplayimg = str[1];
                        }
                        else
                        {
                            tempdisplayimg = str[0];
                        }
                    }
                    else
                    {
                        tempdisplayimg = str[0];
                    }
                }
            }
            return tempdisplayimg;

        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="url">错误地址</param>
        /// <param name="content">错误内容</param>
        /// <param name="filename">文件名称</param>
        public static void WriteLog(string url, string content, string fileName)
        {
            var path = HttpRuntime.AppDomainAppPath + "\\Log\\" + DateTime.Now.ToString("yyyy-MM") + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = path + fileName + ".txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }
            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(url);
                sw.WriteLine("");
                sw.WriteLine(content);
                sw.WriteLine("");
            }
        }

        /// <summary>
        /// 获得当前人的级别
        /// </summary>
        /// <param name="uid">用户ID</param>     
        /// <returns></returns>
        public static string GetCurrentLevel(BasicInfo bas)
        {
            string levelname = string.Empty;
            if (bas != null)
            {
                int type = bas.UserType.Value;

                if (type == 0)
                {
                    //习练者
                    using (YogaUserDetailServiceClient userclient = new YogaUserDetailServiceClient())
                    {
                        ViewYogaUserDetail userdetail = userclient.GetYogaUserDetailById(bas.Uid);
                        if (userdetail != null)
                        {
                            int level = userdetail.Ulevel;
                            switch (level)
                            {
                                case (0):
                                    levelname = MemberLevel.基础习练者.ToString();
                                    break;
                                case (1):
                                    levelname = MemberLevel.初级习练者.ToString();
                                    break;
                                case (2):
                                    levelname = MemberLevel.中级习练者.ToString();
                                    break;
                                case (3):
                                    levelname = MemberLevel.高级习练者.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    //导师
                    using (YogisModelsServiceClient modelclient = new YogisModelsServiceClient())
                    {
                        ViewYogisModels model = modelclient.GetById(bas.Uid);
                        if (model != null)
                        {
                            int level = model.YogisLevel.Value;
                            switch (level)
                            {
                                case (0):
                                    levelname = TeacherLevel.初级老师.ToString();
                                    break;
                                case (1):
                                    levelname = TeacherLevel.中级老师.ToString();
                                    break;
                                case (2):
                                    levelname = TeacherLevel.高级老师.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            return levelname;
        }

        /// <summary>
        /// 目标级别
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="usertype">0习练者，1导师</param>
        /// <returns></returns>
        public static string GetLevelbyScore(int score, int usertype)
        {
            string levelname = string.Empty;
            if (usertype == 0)
            {
                if (score <= 4)
                {
                    levelname = MemberLevel.基础习练者.ToString();
                }
                if (score > 4 && score < 8)
                {
                    levelname = MemberLevel.初级习练者.ToString();
                }
                else if (score > 8 && score <= 12)
                {
                    levelname = MemberLevel.中级习练者.ToString();
                }
                else if (score > 13 && score <= 16)
                {
                    levelname = MemberLevel.高级习练者.ToString();
                }
            }
            else
            {
                if (score > 6 && score < 12)
                {
                    levelname = TeacherLevel.初级老师.ToString();

                }
                else if (score >= 12 && score < 18)
                {
                    levelname = TeacherLevel.中级老师.ToString();

                }
                else if (score >= 18 && score <= 24)
                {
                    levelname = TeacherLevel.高级老师.ToString();
                }
            }
            return levelname;
        }

        #endregion

        #region 格式化图片大小
        //将Image转换为byte[]
        public static byte[] ConvertImage(Image image)
        {

            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(image))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Close();
                }
            }
            return data;

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
        /// <param name="isChange">原图尺寸小于缩略图尺寸是否继续</param>     
        public static byte[] MakeThumbnail(string path, int width, int height, string mode, bool isChange)
        {

            Image originalImage = Image.FromFile(path);
            if (!isChange && (originalImage.Height < height || originalImage.Width < width))
            {
                return ConvertImage(originalImage);
            }

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
                return ConvertImage(bitmap); ;
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
        #endregion
    }
}
