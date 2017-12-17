using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Client
{

    public class YogaDicItemServiceClient : IYogaDicItemService, IDisposable
    {
        public IYogaDicItemService Impl { get; set; }


        public YogaDicItemServiceClient()
        {
            Impl = new YogaDicItemServiceImpl(new YogaDicItemRepository());
        }
        public List<ViewYogaDicItem> GetYogaDicItemList()
        {
            try
            {
                return Impl.GetYogaDicItemList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaDicItem> GetYogaDicItemPageList(string where,int docid,int page, int pagesize, out int count)
        {
            try
            {
                return Impl.GetYogaDicItemPageList(where,docid,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaDicItem> GetYogaDicItemPageList(string where,int Gender,int YogisLevel,string YogaTypeid,int page, int pagesize, out int count)
        {
            try
            {
                return Impl.GetYogaDicItemPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogaDicItem model)
        {
            try
            {
                return Impl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogaDicItem GetById(int id)
        {
            try
            {
                return Impl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ViewYogaDicItem> GetDicId(int id)
        {
            try
            {
                return Impl.GetDicId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ViewYogaDicItem> GetDicById(int id)
        {
            try
            {
                return Impl.GetDicById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogaDicItem model)
        {
            try
            {
                return Impl.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string deletelist)
        {
            try
            {
                return Impl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewYogaDicItem> GetYogaDicItemList(string where,int docid,int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

                List<ViewYogaDicItem> list = GetYogaDicItemPageList(where,docid,page, pagesize, out count);
                 
                //var questionList = from u in list
                //                   where u.ID == null
                //                   select u;

                //var answerList = from u in list
                //                 where u.ID != null
                //                 select u;

                //foreach (var item in questionList)
                //{
                //    tEducationModel temp = new tEducationModel();

                //    //temp.question = item;

                //    //temp.answer = (from u in answerList
                //    //               where u.ID == item.ID
                //    //               select u).FirstOrDefault();

                //    //ViewModelProduct product = new ViewModelProduct();

                //    //using (ProductServiceClient client = new ProductServiceClient())
                //    //{
                //    //    product = client.GetById((int)item.ProductId);
                //    //}
                //    //temp.product = product;
                //    model.Add(temp);
                //}
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ViewYogaDicItem> GetYogaDicItemList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

                List<ViewYogaDicItem> list = GetYogaDicItemPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
 
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Dispose()
        {

        }


        public ViewYogaDicItem GetYogaDicItemById(int id)
        {
            try
            {
                return Impl.GetYogaDicItemById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public ViewYogaDicItem GetYogaDicItemByItemName(string ItemName)
        {
            try
            {
                return Impl.GetYogaDicItemByItemName(ItemName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ViewYogaDicItem> Dics(Expression<Func<ViewYogaDicItem, bool>> predicate)
        {
            return Impl.Dics(predicate);
        }

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="forChild">true：查子级，false：查父级，null，查本身级别</param>
        /// <returns></returns>
        public List<ViewYogaDicItem> GetSelectList(int id, bool? forChild)
        {
            return Impl.GetSelectList(id, forChild);
        }

        public List<ViewYogaDicItem> GetSelectList(string ids)
        {
            return Impl.GetSelectList(ids);
        }


        public string GetDicIds(int id)
        {
            return Impl.GetDicIds(id);
        }

        public string GetDicNames(int id)
        {
            return Impl.GetDicNames(id);
        }
    }
}
