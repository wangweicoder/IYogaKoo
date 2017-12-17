using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace IYogaKoo.Controllers.ashxFile
{
    /// <summary>
    /// yogakooUserImg 的摘要说明
    /// </summary>
    public class yogakooUserImg : IHttpHandler
    {
        //BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        //[WebMethod(EnableSession = true)]
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            System.IO.Stream stream = null;
            System.Drawing.Image originalImg = null;   //原图
            System.Drawing.Image thumbImg = null;      //缩放图       


            try
            {
                int minWidth = 100;   //最小宽度
                int minHeight = 100;  //最小高度
                int maxWidth = 500;  //最大宽度
                int maxHeight = 500;  //最大高度

                string resultTip = string.Empty;  //返回信息

                HttpPostedFile file = context.Request.Files["Filedata"];      //上传文件    
                string uid = @context.Request.Params["Uid"];
                string iType = @context.Request.Params["UserType"];
                //string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]);  //得到上传路径
                string uploadPath = HttpContext.Current.Server.MapPath("~/Files/avatar/original");
                string uploadPathUid = HttpContext.Current.Server.MapPath("~/Files/avatar/original/" + uid);
                string lastImgUrl = @context.Request.Params["LastImgUrl"];

                if (!string.IsNullOrEmpty(lastImgUrl))
                {
                   // PubClass.FileDel(HttpContext.Current.Server.MapPath(lastImgUrl));
                }

                if (file != null)
                {
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    if (!System.IO.Directory.Exists(uploadPathUid))
                    {
                        System.IO.Directory.CreateDirectory(uploadPathUid);
                    }
                    string ext = System.IO.Path.GetExtension(file.FileName).ToLower();   //上传文件的后缀（小写）

                    if (ext == ".jpg" || ext == ".png")
                    {
                        string flag = "ThumbNail" + DateTime.Now.ToFileTime() + ext;

                        string uploadFilePath = uploadPathUid + "\\" + flag;   //缩放图文件路径

                        stream = file.InputStream;

                        originalImg = System.Drawing.Image.FromStream(stream);

                        if (originalImg.Width > minWidth && originalImg.Height > minHeight)
                        {
                            thumbImg = PubClass.GetThumbNailImage(originalImg, maxWidth, maxHeight);  //按宽、高缩放

                            if (thumbImg.Width > minWidth && thumbImg.Height > minWidth)
                            {
                                thumbImg.Save(uploadFilePath);

                                resultTip = "/Files/avatar/original/"+uid + "/" + flag + "$" + thumbImg.Width + "$" + thumbImg.Height;

                                if (!string.IsNullOrEmpty(iType))
                                {
                                    if (iType == "0")
                                    {
                                        ViewYogaUserDetail listModel = new ViewYogaUserDetail();
                                        using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
                                        {
                                            listModel = clientModels.GetYogaUserDetailById(Convert.ToInt32(uid));
                                            if (!string.IsNullOrEmpty(listModel.DisplayImg))
                                            {
                                                string[] ids = listModel.DisplayImg.Split(';');
                                                if (ids.Count() > 1)
                                                {
                                                    if (!string.IsNullOrEmpty(ids[0]))
                                                    {
                                                        PubClass.FileDel(HttpContext.Current.Server.MapPath("~" + ids[0].Split('$')[0]));

                                                    }
                                                    listModel.DisplayImg = resultTip + ";" + ids[1];
                                                }
                                                else
                                                {
                                                    listModel.DisplayImg = resultTip+";";
                                                }
                                            }
                                            else
                                            {
                                                listModel.DisplayImg = resultTip + ";";
                                            }
                                            clientModels.Update(listModel);

                                        }
                                    }
                                    else if (iType == "1")
                                    {
                                        ViewYogisModels Model = new ViewYogisModels();
                                        using (YogisModelsServiceClient client = new YogisModelsServiceClient())
                                        {
                                            Model = client.GetYogisModelsById(Convert.ToInt32(uid));
                                            if (!string.IsNullOrEmpty(Model.DisplayImg))
                                            {
                                                string[] ids = Model.DisplayImg.Split(';');
                                                if (ids.Count() > 1)
                                                {
                                                    if (!string.IsNullOrEmpty(ids[0]))
                                                    {
                                                        PubClass.FileDel(HttpContext.Current.Server.MapPath("~" + ids[0].Split('$')[0]));

                                                    }
                                                    Model.DisplayImg = resultTip + ";" + ids[1];
                                                }
                                                else
                                                {
                                                    Model.DisplayImg = resultTip + ";";
                                                }
                                            }
                                            else
                                            {
                                                Model.DisplayImg = resultTip + ";";
                                            }

                                            client.Update(Model);

                                        }
                                    }
                                }

                            }
                            else
                            {
                                resultTip = "图片比例不符合要求";
                            }
                        }
                        else
                        {
                            resultTip = "图片尺寸必须大于" + minWidth + "*" + minHeight;
                        }
                    }
                }
                else
                {
                    resultTip = "上传文件为空";
                }

                context.Response.Write(resultTip);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (originalImg != null)
                {
                    originalImg.Dispose();
                }

                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }

                if (thumbImg != null)
                {
                    thumbImg.Dispose();
                }

                GC.Collect();
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