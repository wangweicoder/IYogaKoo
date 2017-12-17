using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItBannerRepository : IRepository<tBanner>
    {
        List<tBanner> GettBannerList(int iType);
        List<tBanner> GettBannerPageList(int page, int pagesize, out int count);
        List<tBanner> GettBannerPageListUp(int page, int pagesize, out int count);
         
        List<tBanner> GettBannerPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        List<tBanner> GettBannerUid(int id);
        tBanner GettBannerById(int id);
        int updateEntity(tBanner model);
    }
     
}
