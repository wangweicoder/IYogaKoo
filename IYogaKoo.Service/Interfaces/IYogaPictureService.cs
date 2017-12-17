using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{
    public interface IYogaPictureService
    {
        ViewYogaPicture ExistsPictureOriginal(int Uid, string PictureOriginal);
        List<ViewYogaPicture> GetPiclist(int id, string FName);
        List<ViewYogaPicture> GetBackPageList(string Uid, DateTime? createTime, out int count);
        List<ViewYogaPicture> GetYogaPicturePageList(int page, int pagesize, out int count);
        List<ViewYogaPicture> GetYogaPicturePageList(int uid, int page, int pagesize, out int count);
        /// <summary>
        /// 瑜伽达人/导师
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        List<ViewYogaPicture> GetYogaPicturePageList(int Nums);

        List<ViewYogaPicture> GetListWhere(int uid, string fileName);

        List<ViewYogaPicture> GetUidList(int id);
        List<ViewYogaPicture> GetBackUidList(int id);
        List<ViewYogaPicture> GetBackUidList(int id,string FName);
        List<ViewYogaPicture> GetBackUidList(int id, DateTime create);
        List<ViewYogaPicture> GetBackUidList(int id, string FName, DateTime create);
        ViewYogaPicture GetYogaPictureById(int id);
        ViewYogaPicture GetYogaPictureById(int id, string FName);
        ViewYogaPicture GetYogaPictureByCreateTime(int id, DateTime Create);
        int Add(ViewYogaPicture model);

        ViewYogaPicture GetById(int id);

        int Update(ViewYogaPicture model);

        int Delete(string deletelist);
        List<ViewYogaPicture> GetListByType(int uid, int typeid);

    }
}
