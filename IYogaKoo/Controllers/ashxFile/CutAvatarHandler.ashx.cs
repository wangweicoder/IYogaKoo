using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IYogaKoo.Controllers.ashxFile
{
    /// <summary>
    /// CutAvatarHandler 的摘要说明
    /// </summary>
    public class CutAvatarHandler : IHttpHandler
    {
        //BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
           
            System.Drawing.Bitmap bitmap = null;   //按截图区域生成Bitmap
            System.Drawing.Image thumbImg = null;      //被截图 
            System.Drawing.Graphics gps = null;    //存绘图对象   
            System.Drawing.Image finalImg = null;  //最终图片
             
            try
            {
                string pointX = context.Request.Params["pointX"];   //X坐标
                string pointY = context.Request.Params["pointY"];   //Y坐标
                string imgUrl = context.Request.Params["imgUrl"];   //被截图图片地址
                string rlSize = context.Request.Params["maxVal"];        //截图矩形的大小
                string iType = context.Request.Params["iType"];     //0 习练者； 1 导师 
                string Uid = context.Request.Params["Uid"]; //用户Uid

                int finalWidth = 300;
                int finalHeight = 300;

                if (!string.IsNullOrEmpty(pointX) && !string.IsNullOrEmpty(pointY) && !string.IsNullOrEmpty(imgUrl))
                {

                    string ext = System.IO.Path.GetExtension(imgUrl).ToLower();   //上传文件的后缀（小写）

                    bitmap = new System.Drawing.Bitmap(Convert.ToInt32(rlSize), Convert.ToInt32(rlSize));

                    thumbImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));

                    System.Drawing.Rectangle rl = new System.Drawing.Rectangle(Convert.ToInt32(pointX), Convert.ToInt32(pointY), Convert.ToInt32(rlSize), Convert.ToInt32(rlSize));   //得到截图矩形

                    gps = System.Drawing.Graphics.FromImage(bitmap);      //读到绘图对象

                    gps.DrawImage(thumbImg, 0, 0, rl, System.Drawing.GraphicsUnit.Pixel);

                    finalImg = PubClass.GetThumbNailImage(bitmap, finalWidth, finalHeight);

                    string finalPath = "/Files/avatar/original/"+Uid+"/" + DateTime.Now.ToFileTime() + ext;

                    finalImg.Save(HttpContext.Current.Server.MapPath(finalPath));
                    if (!string.IsNullOrEmpty(iType))
                    {
                        if (iType == "0")
                        {
                            ViewYogaUserDetail listModel = new ViewYogaUserDetail();
                            using (YogaUserDetailServiceClient clientModels = new YogaUserDetailServiceClient())
                            {
                                listModel = clientModels.GetYogaUserDetailById(Convert.ToInt32(Uid));
                               
                                if (!string.IsNullOrEmpty(listModel.DisplayImg))
                                {
                                    string[] ids = listModel.DisplayImg.Split(';');
                                    if (ids.Count() > 1)
                                    {
                                        if(!string.IsNullOrEmpty(ids[1]))
                                        { 
                                            PubClass.FileDel(HttpContext.Current.Server.MapPath("~" + ids[1]));
                                            
                                        }
                                        listModel.DisplayImg = ids[0] +";"+ finalPath;
                                    }
                                    else
                                    {
                                        listModel.DisplayImg = finalPath + ";";
                                    }
                                }
                                else
                                {
                                    listModel.DisplayImg = finalPath + ";";
                                }
                                clientModels.Update(listModel);

                            }
                        }
                        else if (iType == "1")
                        {
                            ViewYogisModels Model = new ViewYogisModels();
                            using (YogisModelsServiceClient client  = new YogisModelsServiceClient())
                            {
                                Model = client.GetYogisModelsById(Convert.ToInt32(Uid));
                               
                                if (!string.IsNullOrEmpty(Model.DisplayImg))
                                {
                                    string[] ids = Model.DisplayImg.Split(';');
                                    if (ids.Count() > 1)
                                    {
                                        if (!string.IsNullOrEmpty(ids[1]))
                                        {
                                            PubClass.FileDel(HttpContext.Current.Server.MapPath("~" + ids[1]));
                                            
                                        }
                                        Model.DisplayImg = ids[0] + ";" + finalPath;
                                    }
                                    else
                                    {
                                        Model.DisplayImg = finalPath + ";";
                                    }
                                }
                                else
                                {
                                    Model.DisplayImg = finalPath + ";";
                                }
                                client.Update(Model);

                            }
                        }
                    }
                    bitmap.Dispose();
                    thumbImg.Dispose();
                    gps.Dispose();
                    finalImg.Dispose();
                    GC.Collect();

                    //PubClass.FileDel(HttpContext.Current.Server.MapPath(imgUrl));

                    context.Response.Write(finalPath);
                }
            }
            catch (Exception)
            {
                throw;
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