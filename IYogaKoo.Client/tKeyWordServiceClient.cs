using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class tKeyWordServiceClient : ItKeyWordService, IDisposable
    {
        public ItKeyWordService tKeyWordServiceImpl { get; set; }


        public tKeyWordServiceClient()
        {
            tKeyWordServiceImpl = new tKeyWordServiceImpl(new tKeyWordRepository());
        }
        public DataTable GetPageListdt(int iType, string where, int page, int pagesize, out int count)
        {
            try
            {
                return tKeyWordServiceImpl.GetPageListdt(  iType,   where,   page,   pagesize, out   count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtKeyWord> GetPageList(string sWord, int page, int pagesize, out int count)
        {
            try
            {
                return tKeyWordServiceImpl.GetPageList( sWord,  page,  pagesize, out  count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public List<ViewtKeyWord> SearchKeyWordList(string sWord)
        {
            try
            {
                return tKeyWordServiceImpl.SearchKeyWordList(sWord);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public int Add(ViewtKeyWord model)
        {
            try
            {
                return tKeyWordServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtKeyWord GetById(int id)
        {
            try
            {
                return tKeyWordServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int Update(ViewtKeyWord model)
        {
            try
            {
                return tKeyWordServiceImpl.Update(model);
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
                return tKeyWordServiceImpl.Delete(deletelist);
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
