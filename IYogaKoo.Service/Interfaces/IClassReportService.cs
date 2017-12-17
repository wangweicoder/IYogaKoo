using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.Dao;

namespace IYogaKoo.Service.Interfaces
{
    public interface IClassReportService 
    {

        ViewClassReport Add(ViewClassReport report);

        bool Edit(ViewClassReport report);

        ViewClassReport Get(int id);
        List<ViewClassReport> GetClassId(int ClassId);
        int Delete(string deletelist);

    }
}
