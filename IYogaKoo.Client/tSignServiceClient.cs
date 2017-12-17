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

    public class tSignServiceClient : ItSignService, IDisposable
    {
        public ItSignService tSignServiceImpl { get; set; }
         
        public tSignServiceClient()
        {
            tSignServiceImpl = new tSignServiceImpl(new tSignRepository());
        }

        public int GetCount(string dtTimeNow)
        {
            try
            {
                return tSignServiceImpl.GetCount(dtTimeNow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        /// <summary>
        /// 签到排名
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public int RowNums(int Uid)
        {
            try
            {
                return tSignServiceImpl.RowNums(Uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 是否签到
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public bool ExistsSign(int Uid)
        {
            try
            {
                return tSignServiceImpl.ExistsSign(Uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtSign model)
        {
            try
            {
                return tSignServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtSign GetById(int id)
        {
            try
            {
                return tSignServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtSign model)
        {
            try
            {
                return tSignServiceImpl.Update(model);
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
                return tSignServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
          
        public void Dispose()
        {

        }
         
    }
}
