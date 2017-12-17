using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItQuestionRepository : IRepository<tQuestion>
    {

        List<tQuestion> GetList(string whereStr, int page, int pagesize, out int count);
        int Edit(tQuestion model);
        tQuestion GetById(int id);

        int Delete(string deletelist);
    }
}
