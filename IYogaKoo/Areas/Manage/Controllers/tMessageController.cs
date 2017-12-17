using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class tMessageController : Controller
    {
        //
        // GET: /Manage/tMessage/
        tMessageServiceClient msgclient; 
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser; 
        public tMessageController()
        {
            ///认证
            msgclient = new tMessageServiceClient();
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();
        }
        public ActionResult Index(int page=1)
        {
            List<ViewtMessage> list=new List<ViewtMessage> ();
             
            int count = 0;
              
            list = msgclient.GettMessagePageList(page, 10, out count);
           
            PagedList<ViewtMessage> pagelist = new PagedList<ViewtMessage>(list, page, 10, count);

            List<ViewtMessageGroup> listGroup = new List<ViewtMessageGroup>();

            foreach (var item in list)
            {
                ViewtMessageGroup model = new ViewtMessageGroup();

                model.entity = item;
                //被留言人
                //ToType  0 习练者；1 导师
                //if (item.ToType == 0)
                //{
                    ViewYogaUser user = clientUser.GetYogaUserById(item.ToUid.Value);
                    if (user != null)
                        model.ToUser = user.NickName;
                //}
                //else if (item.ToType == 1)
                //{
                //    ViewYogisModels usermodel = client.GetYogisModelsById(item.ToUid.Value);
                //    if (usermodel != null)
                //        model.ToUser = usermodel.RealName;
                //}
                ////留言人
                ////FormType  0 习练者；1 导师
                //if (item.FormType == 0)
                //{
                //    ViewYogaUser user = clientUser.GetYogaUserById(item.FromUid.Value);
                //    if (user != null)
                //        model.FromUser= user.NickName;
                //}
                //else if (item.FormType == 1)
                //{
                  //  ViewYogisModels usermodel = client.GetYogisModelsById(item.FromUid.Value);
                    ViewYogaUser usermodel = clientUser.GetYogaUserById(item.FromUid.Value);
                 
                if (usermodel != null)
                    model.FromUser = usermodel.NickName;
                //}
                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return View(pagelist);
        }

        //
        // GET: /Manage/tMessage/Details/5

        public ActionResult Details(int id)
        {
           ViewtMessage model= msgclient.GetById(id);
           if (model != null)
           {
                   ViewYogaUser user = clientUser.GetYogaUserById(model.ToUid.Value);
                   if (user != null)
                       ViewBag.ToUser = user.NickName;
               
               //留言人
                
                   ViewYogaUser user2 = clientUser.GetYogaUserById(model.FromUid.Value);
                   if (user2 != null)
                       ViewBag.FromUser = user2.NickName;
                
               return View(model);
           }
           else
               return View();
        }

        //
        // GET: /Manage/tMessage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/tMessage/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tMessage/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manage/tMessage/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tMessage/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                msgclient.Delete(id.ToString());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            } 
        }

        //
        // POST: /Manage/tMessage/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
