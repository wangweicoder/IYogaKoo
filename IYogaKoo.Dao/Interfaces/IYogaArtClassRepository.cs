using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaArtClassRepository : IRepository<YogaArtClass>
    {
        List<YogaArtClass> GetYogaArtClassPageListAll();
        List<YogaArtClass> GetYogaArtClassPageList(int page, int pagesize, out int count);
        List<YogaArtClass> GetYogaArtClassPageList(int ParentID);
        List<YogaArtClass> GetYogaArtClassUid(int id);
        YogaArtClass GetYogaArtClassByClassName(string ClassName);
         
        YogaArtClass GetYogaArtClassById(int id);
        int updateEntity(YogaArtClass model);
    }
     
}
