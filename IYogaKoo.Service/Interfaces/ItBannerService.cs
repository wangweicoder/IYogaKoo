using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItBannerService
    {
        List<ViewtBanner> GettBannerList(int iType);
        List<ViewtBanner> GettBannerPageList(int page, int pagesize, out int count);
        List<ViewtBanner> GettBannerPageListUp(int page, int pagesize, out int count);
       
        List<ViewtBanner> GettBannerPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count); 
        ViewtBanner GettBannerById(int id);
        List<ViewtBanner> GettBannerUid(int id);
        int Add(ViewtBanner model);

        ViewtBanner GetById(int id);

        int Update(ViewtBanner model);

        int Delete(string deletelist);
    }
}
