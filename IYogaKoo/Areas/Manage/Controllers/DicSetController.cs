using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text.RegularExpressions;
using IYogaKoo.ViewModel;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.Client;
using Webdiyer.WebControls.Mvc;
namespace IYogaKoo.Areas.Manage.Controllers
{
    public class DicSetController : BaseController
    { 
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDic()
        {
            return View();
        }

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string where="")
        {
            List<ViewYogaDicItem> list = new List<ViewYogaDicItem>();
            int count = 0;
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                list = client.GetYogaDicItemPageList(where,0, page, 15, out count);
            }
            PagedList<ViewYogaDicItem> pagelist = new PagedList<ViewYogaDicItem>(list, page, 15, count);

            ViewBag.listDic = pagelist;
            return View(pagelist);
        }

        /// <summary>
        /// 修改字典信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EditDic(int id)
        {
            ViewYogaDicItem dic = new ViewYogaDicItem();
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                dic = client.GetYogaDicItemById(id);
            }
            return View(dic);
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <returns></returns>
        public ActionResult AddItem(int id)
        {
            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 字典项列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AllItem(int id, int page = 1, string where = "")
        {
            List<ViewYogaDicItem> list = new List<ViewYogaDicItem>();
            int count = 0;
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                list = client.GetYogaDicItemPageList(where, id, page, 15, out count);
            }
            PagedList<ViewYogaDicItem> pagelist = new PagedList<ViewYogaDicItem>(list, page, 15, count);
            ViewBag.id = id;

            return View(pagelist);
        }

        /// <summary>
        /// 编辑字典项
        /// </summary>
        /// <returns></returns>
        public ActionResult EditItem(int id)
        {
            ViewYogaDicItem dic = new ViewYogaDicItem();
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                dic = client.GetYogaDicItemById(id);
            }
            return View(dic); 
        }
        
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="DM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDic(ViewYogaDicItem DM)
        {
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                DM.CreateTime = DateTime.Now;
                client.Add(DM);
            }
            ViewData["message"] = "添加成功！";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑字典信息
        /// </summary>
        /// <param name="DM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDic(ViewYogaDicItem DM)
        {
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                client.Update(DM);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="DIM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItem(ViewYogaDicItem DIM)
        {
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                DIM.CreateTime = DateTime.Now;
                client.Add(DIM);
            }
            ViewData["info"] = "添加成功！";
            string dif = Request.Form["hiddif"];
            if (dif.Equals("0"))
            {
                return RedirectToAction("AllItem", new { id = DIM.DicId });
            }
            else
            {
                return RedirectToAction("AddItem", new { id = DIM.DicId });
            }
        }

        /// <summary>
        /// 编辑字典项
        /// </summary>
        /// <param name="DIM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(ViewYogaDicItem DIM)
        {
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                client.Update(DIM);
            }
            return RedirectToAction("AllItem", new { id = DIM.DicId });
        }

        /// <summary>
        /// 删除字典项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index()
        {
            string myValue = Request.Form["checkVal"];

            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                client.Delete(myValue);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AllItem()
        {
            string myValue = Request.Form["checkVal"];
            string parid = Request.Form["parid"];
            using (YogaDicItemServiceClient client = new YogaDicItemServiceClient())
            {
                client.Delete(myValue);

            }
            return RedirectToAction("AllItem/"+parid);
        }

    }
}