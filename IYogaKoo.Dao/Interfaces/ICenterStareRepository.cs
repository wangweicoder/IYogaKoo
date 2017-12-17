using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
namespace IYogaKoo.Dao.Interfaces
{
    public interface ICenterStareRepository : IRepository<CenterStare>
    { 
        List<CenterStare> GetCentersPageList(int mid, out int count);

        int IfUidSave(int uid, int mid);
       
    }
}
