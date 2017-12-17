using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IClassFileRepository : IRepository<ClassFile>
    {
        bool AddList(List<ClassFile> files);
        List<ClassFile> GettReportId(int ReportId);
    }
}
