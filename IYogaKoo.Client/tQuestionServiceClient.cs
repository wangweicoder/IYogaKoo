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
    public class tQuestionServiceClient : ItQuestionService, IDisposable
    {
        private ItQuestionService Impl { get; set; }

        public tQuestionServiceClient()
        {
            Impl = new tQuestionServiceImpl(new tQuestionRepository());
        }

        public List<ViewtQuestion> GetList(string whereStr, int page, int pagesize, out int count)
        {
            try
            {
                return Impl.GetList(whereStr, page, pagesize, out  count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ViewtQuestion GetById(int id)
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

        public int Add(ViewtQuestion model)
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
        public int Edit(ViewtQuestion model)
        {
            try
            {
                return Impl.Edit(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Delete(string deletelist)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
