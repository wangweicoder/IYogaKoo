using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Service
{
    public class ClassReportServiceImpl : IClassReportService
    {
        IClassReportRepository _repository;
        public ClassReportServiceImpl(IClassReportRepository Repository)
        {
            this._repository = Repository;
        }
        public ViewModel.ViewClassReport Add(ViewModel.ViewClassReport report)
        {
            ClassReport cr = _repository.Add(ViewClassReport.ToEntity(report));
            report.Id = cr.Id;
            return report;
        }

        public bool Edit(ViewModel.ViewClassReport report)
        {
            _repository.Update(ViewClassReport.ToEntity(report));
            _repository.Save();
            return true;
        }

        public ViewModel.ViewClassReport Get(int id)
        {
            ClassReport report = _repository.Get(id);
            return ViewClassReport.ToViewModel(report);
        }

        public List<ViewClassReport> GetClassId(int ClassId)
        {
            List<ViewClassReport> list = new List<ViewClassReport>();
            var report = _repository.GetClassId(ClassId);
            foreach (var item in report)
            {
                list.Add(ViewClassReport.ToViewModel(item));
            }
            return list;
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                _repository.Delete(_repository.Get(int.Parse(item)));
            }
            return _repository.Save();
        }

    }
}
