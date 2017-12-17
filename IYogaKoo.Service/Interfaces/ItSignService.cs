using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItSignService
    {
        int GetCount(string dtTimeNow);
        int RowNums(int Uid);
        bool ExistsSign(int Uid);
        int Add(ViewtSign model); 
        ViewtSign GetById(int id);

        int Update(ViewtSign model);

        int Delete(string deletelist);
    }
}
