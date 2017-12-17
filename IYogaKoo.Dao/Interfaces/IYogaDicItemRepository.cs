using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaDicItemRepository : IRepository<YogaDicItem>
    {
        List<YogaDicItem> GetYogaDicItemPageList(string where,int dicid,int page, int pagesize, out int count);
        List<YogaDicItem> GetYogaDicItemPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        List<YogaDicItem> GetYogaDicItemList();
        YogaDicItem GetYogaDicItemById(int id);
        YogaDicItem GetYogaDicItemByItemName(string ItemName);
        int updateEntity(YogaDicItem model);
        IQueryable<YogaDicItem> Dics();
        List<YogaDicItem> GetDicById(int id);
        List<YogaDicItem> GetDicId(int id);
        List<YogaDicItem> GetSelectList(int id, bool? forChild);
        List<YogaDicItem> GetSelectList(string ids);
        string GetDicIds(int id);
        string GetDicNames(int id);
    }
     
}
