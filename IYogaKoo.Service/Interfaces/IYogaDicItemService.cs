using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogaDicItemService
    {
        List<ViewYogaDicItem> GetYogaDicItemPageList(string where,int docid,int page, int pagesize, out int count);
        List<ViewYogaDicItem> GetYogaDicItemPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        ViewYogaDicItem GetYogaDicItemById(int id);
        List<ViewYogaDicItem> GetYogaDicItemList();
        int Add(ViewYogaDicItem model);
        List<ViewYogaDicItem> GetDicById(int id);
        List<ViewYogaDicItem> GetDicId(int id);
        ViewYogaDicItem GetById(int id);
        ViewYogaDicItem GetYogaDicItemByItemName(string ItemName);
        int Update(ViewYogaDicItem model);

        List<ViewYogaDicItem> Dics(Expression<Func<ViewYogaDicItem, bool>> predicate);

        int Delete(string deletelist);

        List<ViewYogaDicItem> GetSelectList(int id, bool? forChild);
        List<ViewYogaDicItem> GetSelectList(string ids);

        string GetDicIds(int id);
        string GetDicNames(int id);
    }
}
