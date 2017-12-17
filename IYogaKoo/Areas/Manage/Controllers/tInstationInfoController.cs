using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class tInstationInfoController : Controller
    {
        //后台站内信管理
        // GET: /Manage/tInstationInfo/
        tInstationInfoServiceClient client;
        List<ViewtInstationInfo> list;
        YogaUserServiceClient userclient;
        YogisModelsServiceClient modelsclient;
        int count = 0;
        public tInstationInfoController()
        {
            client = new tInstationInfoServiceClient();
            list = new List<ViewtInstationInfo>();
            userclient = new YogaUserServiceClient();
            modelsclient = new YogisModelsServiceClient();
        }
        public ActionResult Index(DateTime? CreateTime,int page=1)
        {
            //按时间/内容/类型查询
            int iType = 0;
            if (!string.IsNullOrEmpty(Request.Form["iType"]))
            {
                iType = Convert.ToInt32(Request.Form["iType"]);
            }
            string content = "";
            if (!string.IsNullOrEmpty(Request.Form["content"])) { 
                content=Request.Form["content"].ToString();
            }
             
            list = client.GetPageList(content, iType, CreateTime, page, 10, out count);
            PagedList<ViewtInstationInfo> pagelist = new PagedList<ViewtInstationInfo>(list, page, 10, count);

            return View(pagelist);
        }

        //
        // GET: /Manage/tInstationInfo/Details/5

        public ActionResult Details(int id)
        {
            ViewtInstationInfo model = client.GetById(id);
            if (model != null)
            { 
                return View(model);
            }
            else
            {
                return View();
            }
        }
        public JsonResult Detailslistuser(string sContent)
        {
            try
            {
                list = client.GetByContent(sContent);
                SearchInstationInfo entity = new SearchInstationInfo();
                List<SearchInstationInfo> listinfo = new List<SearchInstationInfo>();
                string strUserType = "";
                foreach (var item in list)
                {
                    entity = new SearchInstationInfo();
                    ViewYogaUser userEntity = userclient.GetById(item.Uid.Value);
                    strUserType += userEntity.UserType + ",";
                    if (userEntity != null)
                    {
                        if (string.IsNullOrEmpty(userEntity.NickName))
                        {
                            entity.NickName = modelsclient.GetYogisModelsById(item.Uid.Value).RealName;
                        }
                        else
                        {
                            entity.NickName = userEntity.NickName;
                        }
                        entity.UId = item.Uid.Value;
                    }
                    listinfo.Add(entity);
                }
               // ViewBag.sType = strUserType.Distinct();//判断是习练者/导师/全部

                return Json(new {code=strUserType.Distinct(),listinfo= listinfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { code = 1 });
            }
        }
        //
        // GET: /Manage/tInstationInfo/Create

        public ActionResult Create()
        { 
            return View();
        }
        public class SearchInstationInfo
        {
            public int UId { get; set; }
            public string NickName { get; set; }
        }
        public JsonResult listuser(int userType)
        {
            List<ViewYogaUser> listuser = userclient.BackGetPageList(userType);
            SearchInstationInfo entity = new SearchInstationInfo();
            List<SearchInstationInfo> list = new List<SearchInstationInfo>();
            if (userType == 1)
            {
                foreach (var item in listuser)
                {
                    entity = new SearchInstationInfo();
                    if (string.IsNullOrEmpty(item.NickName))
                    {
                        entity.NickName = modelsclient.GetYogisModelsById(item.Uid).RealName;
                    }
                    else
                    {
                        entity.NickName = item.NickName; 
                    }
                    entity.UId = item.Uid;

                    list.Add(entity);
                }
            }
            else
            {
                foreach (var item in listuser)
                {
                    entity = new SearchInstationInfo();
                    entity.UId = item.Uid;
                    entity.NickName = item.NickName;
                    list.Add(entity);
                }
            }
            JsonResult js = new JsonResult();
            js.Data = list;
            js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return js;
        } 
         
        //
        // POST: /Manage/tInstationInfo/Create

        [HttpPost]
        public JsonResult Create(ViewtInstationInfo model)
        {
            try
            {
                // TODO: Add insert logic here
 
                string strUidInfo = "";
                if (!string.IsNullOrEmpty(Request.Form["hidselectType"])) {
                    if (Request.Form["hidselectType"].ToString() == "2")
                    { 
                        List<ViewYogaUser> listuser = userclient.GetYogaUserPageList(100000).Where(x=>x.delState==0).ToList();
                        strUidInfo = string.Join(",", listuser.Select(a => a.Uid)); 
                    }
                    else {
                        strUidInfo = Request.Form["hiduserInfo"].ToString().TrimEnd(',');//去掉最后一个字符,
                    }
                }
                if (!string.IsNullOrEmpty(strUidInfo))
                {
                    foreach (var i in strUidInfo.Split(','))
                    {
                        if (!string.IsNullOrEmpty(i))
                        {
                            if (Convert.ToInt32(i) != 10 && Convert.ToInt32(i)!=11)
                            {
                                model.Uid = Convert.ToInt32(i);
                                model.CreateTime = DateTime.Now;
                                model.ifDel = false;
                                model.iType = 2644;
                                model.CreateUser = ConfigurationManager.AppSettings["BACK_POSTER"].ToString();//100316
                                model.loginType = 0;
                                client.Add(model);
                            }
                           
                        }
                    }
                } 
              
                //return RedirectToAction("Index");
                return Json(new { code=0});
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //
        // GET: /Manage/tInstationInfo/Edit/5

        public ActionResult Edit(int id)
        {
            ViewtInstationInfo model = client.GetById(id);
            return View(model);
        }

        //
        // POST: /Manage/tInstationInfo/Edit/5

        [HttpPost]
        public JsonResult Edit(string hidsContent)
        {
            try
            {
                list = client.GetByContent(hidsContent);

                ViewtInstationInfo model = new ViewtInstationInfo();
                // TODO: Add update logic here
                
                foreach (ViewtInstationInfo item in list)
                { 
                    item.sContent = Request.Form["sContent"].ToString(); 
                    client.Update(item);
                }

                return Json(new { code=0});
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //
        // GET: /Manage/tInstationInfo/Delete/5

        public ActionResult Delete(int id)
        {
            ViewtInstationInfo model = client.GetById(id); 
            model.ifDel = true;
            model.CreateTime = DateTime.Now;
            client.Update(model);
            return RedirectToAction("Index");
        }

        //
        // POST: /Manage/tInstationInfo/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                ViewtInstationInfo model = client.GetById(id);
                model.ifDel = true;
                model.CreateTime = DateTime.Now;
                client.Update(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
