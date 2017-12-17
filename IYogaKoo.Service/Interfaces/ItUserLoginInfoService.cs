using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{ 
    public interface ItUserLoginInfoService
    { 
        List<ViewtUserLoginInfo> GetPageList(int page, int pagesize, out int count);
   
        int Add(ViewtUserLoginInfo model); 
        ViewtUserLoginInfo GetById(int id);
        ViewtUserLoginInfo GetByUid(int Uid);
        int Update(ViewtUserLoginInfo model);

        int Delete(string deletelist);
    }
}
