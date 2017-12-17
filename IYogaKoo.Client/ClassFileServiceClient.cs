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
    public class ClassFileServiceClient : IClassFileService,IDisposable
    {
        IClassFileService _impl;
        public ClassFileServiceClient()
        {
            _impl = new ClassFileServiceImpl(new ClassFileRepository());
        }

        public bool AddList(List<ViewClassFile> files)
        {
            return _impl.AddList(files);
        }
        public List<ViewClassFile> GettReportId(int ReportId)
        {
            try
            {
                return _impl.GettReportId(ReportId);
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
                return _impl.Delete(deletelist);
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
