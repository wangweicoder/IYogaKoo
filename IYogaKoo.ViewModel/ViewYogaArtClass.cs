using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewYogaArtClass
    {
        #region 基本信息
          [DisplayName("编号")]
        public int ID { get; set; }
          [DisplayName("名称")]
        public string ClassName { get; set; }
          [DisplayName("是否删除")]
        public int IsDelete { get; set; }
          [DisplayName("创建人")]
        public string Creator { get; set; }
         [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("级别")]
        public int? ParentID { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewYogaArtClass()
        {
        }

        #endregion

        #region - 方法 -
        public static YogaArtClass ToEntity(ViewYogaArtClass model)
        {
            YogaArtClass item = new YogaArtClass();

            item.ID = model.ID;
            item.ClassName = model.ClassName;
            item.IsDelete = model.IsDelete;
            item.Creator = model.Creator;
            item.CreateTime = model.CreateTime;
            item.ParentID = model.ParentID;
            return item;
        }

        public static ViewYogaArtClass ToViewModel(YogaArtClass model)
        {
            if (model == null)
            {

                return null;
            }

            ViewYogaArtClass item = new ViewYogaArtClass();
            item.ID = model.ID;
            item.ClassName = model.ClassName;
            item.IsDelete = model.IsDelete;
            item.Creator = model.Creator;
            item.CreateTime = model.CreateTime;
            item.ParentID = model.ParentID;

            return item;
        }

        #endregion
    }
}
