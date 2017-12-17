using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogiProfileService
    {
        List<ViewYogiProfile> GetYogiProfileList();
        List<ViewYogiProfile> GetYogiProfilePageList(int page, int pagesize, out int count);
        List<ViewYogiProfile> GetYogiProfilePageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count); 
        ViewYogiProfile GetYogiProfileById(int id);
        List<ViewYogiProfile> GetYogiProfileUid(int id);
        int Add(ViewYogiProfile model);

        ViewYogiProfile GetById(int id);

        int Update(ViewYogiProfile model);

        int Delete(string deletelist);
    }
}
