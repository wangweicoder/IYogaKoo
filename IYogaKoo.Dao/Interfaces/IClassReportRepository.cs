using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IClassReportRepository : IRepository<ClassReport>
    {
        IQueryable<ClassReport> Reports { get; }
        List<ClassReport> GetClassId(int ClassId);
    }
}
