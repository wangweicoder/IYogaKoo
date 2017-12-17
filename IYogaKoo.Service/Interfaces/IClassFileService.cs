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
    public interface IClassFileService
    {
        bool AddList(List<ViewClassFile> files);

        List<ViewClassFile> GettReportId(int ReportId);

        int Delete(string deletelist);
    }
}
