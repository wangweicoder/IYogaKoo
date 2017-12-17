using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItSignRepository : IRepository<tSign>
    {
        int GetCount(string dtTimeNow); 
        int RowNums(int Uid);
        bool ExistsSign(int Uid);
        int updateEntity(tSign model);
    }
     
}
