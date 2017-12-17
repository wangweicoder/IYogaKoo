using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewInterestedClass
    {
        #region 基本信息

        public int Id { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }

        public Class Class { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewInterestedClass()
        {
        }

        #endregion

        #region - 方法 -
        public static InterestedClass ToEntity(ViewInterestedClass model)
        {
            InterestedClass item = new InterestedClass();

            item.Id = model.Id;
            item.ClassId = model.ClassId;
            item.UserId = model.UserId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;

            return item;
        }

        public static ViewInterestedClass ToViewModel(InterestedClass model)
        {
            if (model == null)
            {

                return null;
            }

            ViewInterestedClass item = new ViewInterestedClass();

            item.Id = model.Id;
            item.ClassId = model.ClassId;
            item.UserId = model.UserId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.Class = model.Class;
            item.Class.InterestedClass = null;
            return item;
        }

        #endregion
    }
}
