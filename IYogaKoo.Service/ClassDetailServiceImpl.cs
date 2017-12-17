using IYogaKoo.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Service
{
    public class ClassDetailServiceImpl:IClassDetailService
    {

        IClassDetailRepository _repository;

        public ClassDetailServiceImpl(IClassDetailRepository repository)
        {
            _repository = repository;
        }

        public List<ViewClassDetail> GetClassDetail(string whereStr, int page, int pagesize, out int count)
        {
            List<ClassDetail> list = _repository.GetClassDetail(whereStr, page, pagesize, out count);
            List<ViewClassDetail> model = new List<ViewClassDetail>();

            foreach (var item in list)
            {
                model.Add(ViewClassDetail.ToViewModel(item));
            }
            return model;
        }

        public ViewClassDetail Add(ViewClassDetail entity)
        {
            ClassDetail model = _repository.Add(ViewClassDetail.ToEntity(entity));
            return ViewClassDetail.ToViewModel(model);
        }
        public int updateEntity(ViewClassDetail entity)
        {
            int result = _repository.updateEntity(ViewClassDetail.ToEntity(entity));
            return result;
        }
    }
}
