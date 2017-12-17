using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IClassDetailRepository : IRepository<ClassDetail>
    {
        List<ClassDetail> GetClassDetail(string whereStr, int page, int pagesize, out int count);
        int updateEntity(ClassDetail model);

    }
}
