using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
namespace IYogaKoo.ViewModel
{
    public class ViewYogaMenus
    {
        #region 基本信息
        public int Id { get; set; }
        public string name { get; set; } 
        public string url { get; set; }

        public bool state { get; set; }
        public int ordervalue { get; set; }
        #endregion

        #region 方法
        public static YogaMenus ToEntity(ViewYogaMenus model)
        {
            YogaMenus item = new YogaMenus();

            item.Id = model.Id;
            item.name = model.name;
            item.url = model.url;
            return item;
        }
        public static ViewYogaMenus ToViewModel(YogaMenus model)
        {
            if (model == null)
            {
                return null;
            }

            ViewYogaMenus item = new ViewYogaMenus();

            item.Id = model.Id;
            item.name = model.name;
            

            return item;
        }
        #endregion
    }
}
