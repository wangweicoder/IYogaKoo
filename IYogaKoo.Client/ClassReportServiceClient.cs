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
    public class ClassReportServiceClient : IClassReportService,IDisposable
    {
        ClassReportServiceImpl _impl;
        public ClassReportServiceClient()
        {
            _impl = new ClassReportServiceImpl(new ClassReportRepository());
        }

        public ViewClassReport Add(ViewClassReport report)
        {
            report.CreateTime = DateTime.Now;
            report.IsDeleted = false;
            if (report.Id > 0)
                _impl.Edit(report);
            else
                _impl.Add(report);
            if (report.Id > 0)
            {
                ClassFileServiceClient client = new ClassFileServiceClient();
                foreach (var item in report.Images)
                {
                    item.CreateTime = DateTime.Now;
                    item.ExtendName = item.Url.Substring(item.Url.LastIndexOf('.'));
                    item.Type = 11;//活动图片
                    item.Title = "";
                    item.ReportId = report.Id;
                    item.IsDeleted = false;
                }
                if (report.Images.Count > 0)
                    client.AddList(report.Images);
                foreach (var item in report.Videos)
                {
                    item.CreateTime = DateTime.Now;
                    item.ExtendName = item.Url.Substring(item.Url.LastIndexOf('.'));
                    item.Type = 12;//活动视频
                    item.Title = "";
                    item.ReportId = report.Id;
                    item.IsDeleted = false;
                }
                if (report.Videos.Count > 0)
                    client.AddList(report.Videos);
            }
            return report;
        }

        public bool Edit(ViewClassReport report)
        {
            throw new NotImplementedException();
        }

        public ViewClassReport Get(int id)
        {
            return _impl.Get(id);
        }
        public List<ViewClassReport> GetClassId(int ClassId)
        {
            return _impl.GetClassId(ClassId);
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
