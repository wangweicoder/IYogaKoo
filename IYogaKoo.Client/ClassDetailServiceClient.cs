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
    public class ClassDetailServiceClient : IClassDetailService, IDisposable
    {
        private IClassDetailService Impl { get; set; }

        public ClassDetailServiceClient()
        {
            Impl = new ClassDetailServiceImpl(new ClassDetailRepository());
        }
        public List<ViewClassDetail> GetClassDetail(string whereStr, int page, int pagesize, out int count)
        {
            try
            {
                return Impl.GetClassDetail(whereStr, page, pagesize, out  count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewClassDetail Add(ViewClassDetail entity)
        {

            try
            {
                return Impl.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int updateEntity(ViewClassDetail entity)
        {

            try
            {
                return Impl.updateEntity(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
