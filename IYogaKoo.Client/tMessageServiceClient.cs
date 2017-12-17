using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class tMessageServiceClient : ItMessageService, IDisposable
    {
        public ItMessageService tMessageServiceImpl { get; set; }


        public tMessageServiceClient()
        {
            tMessageServiceImpl = new tMessageServiceImpl(new tMessageRepository());
        }
        public List<ViewtMessage> GetPageListWhereUidAndloginType(int uid, int loginType, out int count)
        {
            try
            {
                return tMessageServiceImpl.GetPageListWhereUidAndloginType(uid, loginType,out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtMessage> GetPageListWhereFormUidAndloginType(int uid, int loginType, out int count)
        {
            try
            {
                return tMessageServiceImpl.GetPageListWhereFormUidAndloginType(uid, loginType,out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewtMessage> GettMessageList()
        {
            try
            {
                return tMessageServiceImpl.GettMessageList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 我评论了谁
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public  List<ViewtMessage> GetByMessageFromUid(int toType,int id, int ParentID)
        {
            try
            {
                return tMessageServiceImpl.GetByMessageFromUid(toType,id, ParentID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewtMessage> GettMessageUid(int id,int toType)
        {
            try
            {
                return tMessageServiceImpl.GettMessageUid(id,toType);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtMessage> GettMessageParentID(int ParentID)
        {
            try
            {
                return tMessageServiceImpl.GettMessageParentID(ParentID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtMessage> GettMessagePageList(int page, int pagesize, out int count)
        {
            try
            {
                return tMessageServiceImpl.GettMessagePageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 留言列表按id和类型分页
        /// </summary>
        public List<ViewtMessage> GettMessageUidList(int id, int totype, int page, int pagesize, out int count) 
        {
            try
            {
                return tMessageServiceImpl.GettMessageUidList(id,totype,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtMessage model)
        {
            try
            {
                return tMessageServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtMessage GetById(int id)
        {
            try
            {
                return tMessageServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtMessage model)
        {
            try
            {
                return tMessageServiceImpl.Update(model);
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
                return tMessageServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewtMessage> GettMessageList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewtMessage> model = new List<ViewtMessage>();

                List<ViewtMessage> list = GettMessagePageList(page, pagesize, out count);
                 
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

         
        public void Dispose()
        {

        }
        /// <summary>
        /// 谁评论了我
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<ViewtMessage> GetByMessage(int toType,int id, int ParentID)
        {
            try
            {
                return tMessageServiceImpl.GetByMessage(toType,id, ParentID);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewtMessage GettMessageById(int id)
        {
            try
            {
                return tMessageServiceImpl.GettMessageById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid)
        {
            try
            {
                return tMessageServiceImpl.GettMessageDistinct(Touid, strContent, FromUid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid, int ParentID)
        {
            try
            {
                return tMessageServiceImpl.GettMessageDistinct(Touid, strContent, FromUid, ParentID);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public ViewtMessage GettMessageOnly(int Touid, int FromUid, int ParentID)
        {
            try
            {
                return tMessageServiceImpl.GettMessageOnly(Touid, FromUid, ParentID);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
