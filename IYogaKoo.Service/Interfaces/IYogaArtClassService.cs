using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogaArtClassService
    {
        List<ViewYogaArtClass> GetYogaArtClassPageListAll();
        List<ViewYogaArtClass> GetYogaArtClassPageList(int page, int pagesize, out int count);
        List<ViewYogaArtClass> GetYogaArtClassPageList(int ParentID); 
        ViewYogaArtClass GetYogaArtClassById(int id);
        ViewYogaArtClass GetYogaArtClassByClassName(string ClassName);
         
        List<ViewYogaArtClass> GetYogaArtClassUid(int id);
        int Add(ViewYogaArtClass model);

        ViewYogaArtClass GetById(int id);

        int Update(ViewYogaArtClass model);

        int Delete(string deletelist);
    }
}
