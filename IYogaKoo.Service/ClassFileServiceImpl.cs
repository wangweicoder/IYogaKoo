using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Service
{
    public class ClassFileServiceImpl : IClassFileService
    {
        IClassFileRepository _repository;
        public ClassFileServiceImpl(IClassFileRepository Repository)
        {
            this._repository = Repository;
        }
        public bool AddList(List<ViewClassFile> files)
        {
            return _repository.AddList((from f in files select ViewClassFile.ToEntity(f)).ToList());
        }
        public List<ViewClassFile> GettReportId(int ReportId)
        {
            List<ClassFile> list = _repository.GettReportId(ReportId);

            List<ViewClassFile> model = new List<ViewClassFile>();

            foreach (var item in list)
            {
                model.Add(ViewClassFile.ToViewModel(item));
            }
            return model;
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
