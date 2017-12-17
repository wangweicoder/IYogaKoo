using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaPictureRepository : IRepository<YogaPicture>
    {
        YogaPicture ExistsPictureOriginal(int Uid, string PictureOriginal);
        List<YogaPicture> GetPiclist(int id, string FName);
        List<YogaPicture> GetBackPageList(string Uid, DateTime? createTime, out int count);
        List<YogaPicture> GetYogaPicturePageList(int Nums);
        List<YogaPicture> GetUidList(int id);
        List<YogaPicture> GetBackUidList(int id);
        List<YogaPicture> GetBackUidList(int id, string FName);
        List<YogaPicture> GetBackUidList(int id, DateTime create);
        List<YogaPicture> GetBackUidList(int id, string FName, DateTime create);
         
        List<YogaPicture> GetListWhere(int uid, string fileName);
        List<YogaPicture> GetYogaPicturePageList(int page, int pagesize, out int count);
          List<YogaPicture> GetYogaPicturePageList(int uid, int page, int pagesize, out int count);
     
        YogaPicture GetYogaPictureById(int id);
        YogaPicture GetYogaPictureById(int id, string FName);
        YogaPicture GetYogaPictureByCreateTime(int id, DateTime Create);
        int updateEntity(YogaPicture model);

        List<YogaPicture> GetListByType(int uid ,int typeid);

    }
     
}
