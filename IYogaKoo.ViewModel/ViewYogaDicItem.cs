using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewYogaDicItem
    {
        #region 基本信息
        /// <summary>
        /// 字典项编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 字典编号
        /// </summary>
        public int DicId { get; set; }
 
        /// <summary>
        /// 字典项名称
        /// </summary>
        [DisplayName("名称")]
        public string ItemName { get; set; }

        /// <summary>
        /// 字典项值
        /// </summary>
        public string ItemValue { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortId { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        #endregion

         #region - 构造函数 -

        public ViewYogaDicItem()
        {
        }
         
        #endregion

        #region - 方法 -
        public static YogaDicItem ToEntity(ViewYogaDicItem model)
        {
            YogaDicItem item = new YogaDicItem();

            item.ID = model.ID;
            item.DicId = model.DicId;
            item.ItemName = model.ItemName;
            item.ItemValue = model.ItemValue;
            item.SortId = model.SortId;
            item.IsUse = model.IsUse==true?1:0;
            item.IsDelete = model.IsDelete==true?1:0;
            item.CreateTime = model.CreateTime;
              
            return item;
        }

        public static ViewYogaDicItem ToViewModel(YogaDicItem model)
        {
            if (model == null) {

                return null;
            }

            ViewYogaDicItem item = new ViewYogaDicItem();
            item.ID = model.ID;
            item.DicId = model.DicId;
            item.ItemName = model.ItemName;
            item.ItemValue = model.ItemValue;
            item.SortId = model.SortId;
            item.IsUse = model.IsUse == 1 ? true : false; ;
            item.IsDelete = model.IsDelete == 1 ? true : false ;
            item.CreateTime = model.CreateTime; 

            return item;
        }

        #endregion
    }
}
