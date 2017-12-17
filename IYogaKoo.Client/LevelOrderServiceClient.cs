using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Client
{
    public  class LevelOrderServiceClient:ILevelOrderService,IDisposable
    {
        public ILevelOrderService Impl { get; set; }
        public LevelOrderServiceClient()
        {
            Impl = new LevelOrderServiceImpl(new LevelOrderRepository());
        }


        //添加
        public int Add(ViewLevelOrder model)
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
        /// <summary>
        /// 根据UID取最近一条数据
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ViewLevelOrder GetUid(int UID) {
            try
            {
                return Impl.GetUid(UID);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
        public List<ViewLevelOrder> BackGetOrdersPageList(string Name, string OrderState, string TargetLevel, string OrderType,
           int page, int pagesize, out int count)
        {
            try
            {
                return Impl.BackGetOrdersPageList(Name, OrderState, TargetLevel, OrderType, page, pagesize, out count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //后台分页
        public List<ViewLevelOrder> GetOrdersPageList(int page, int pagesize, string Ordertype, out int count)
        {
            try
            {
                return Impl.GetOrdersPageList(page, pagesize, Ordertype, out count);
            }
            catch (Exception ex )
            {               
                throw ex;
            }
        }
        public void Dispose()
        {
           //throw new NotImplementedException();
        }



        public ViewLevelOrder GetById(int id)
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

        public int Update(ViewLevelOrder model)
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
         
    }
}
