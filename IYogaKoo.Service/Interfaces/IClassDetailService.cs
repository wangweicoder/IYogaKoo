using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Service.Interfaces
{
    public interface IClassDetailService
    {

        ViewClassDetail Add(ViewClassDetail entity);

        int updateEntity(ViewClassDetail entity);
        List<ViewClassDetail> GetClassDetail(string whereStr, int page, int pagesize, out int count);

    }
}
