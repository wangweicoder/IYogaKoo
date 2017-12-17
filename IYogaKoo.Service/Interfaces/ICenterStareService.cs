using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
namespace IYogaKoo.Service.Interfaces
{
     public  interface ICenterStareService
    {
         List<ViewCenterStare> GetCentersPageList(int mid, out int count);

         int IfUidSave(int uid, int mid);
         int Add(ViewCenterStare model);
    }
}
