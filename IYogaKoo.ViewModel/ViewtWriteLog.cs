using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    /// <summary>
    /// 我的日志
    /// </summary>
    public class ViewtWriteLog
    {
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("会员编号")]
        public int? Uid { get; set; }
        [DisplayName("标题")]
        public string sTitle { get; set; }
        [DisplayName("内容")]
        public string sContent { get; set; }
         [DisplayName("是否显示")]
        public bool? ifShow { get; set; }
      [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("阅读量")]
      public int? iReadNums { get; set; }
        [DisplayName("是否推送")]
      public bool? ifpush { get; set; }
        /// <summary>
        /// app端 : 1 / pc端 : 0/null
        /// </summary>
        [DisplayName("传值类型")]
        public int? ValueType { get; set; }
        #endregion

        #region - 构造函数 -

      public ViewtWriteLog()
        {
        }

        #endregion

        #region - 方法 -
      public static tWriteLog ToEntity(ViewtWriteLog model)
        {
            tWriteLog item = new tWriteLog();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sTitle = model.sTitle;
            item.sContent = model.sContent;
            item.ifShow = model.ifShow;
            item.CreateDate = model.CreateDate;
            item.iReadNums = model.iReadNums;
            item.ifpush = model.ifpush;
            item.ValueType = model.ValueType;
            return item;
        }

      public static ViewtWriteLog ToViewModel(tWriteLog model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtWriteLog item = new ViewtWriteLog();
           
            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sTitle = model.sTitle;
            item.sContent = model.sContent;
            item.ifShow = model.ifShow;
            item.CreateDate = model.CreateDate;
            item.iReadNums = model.iReadNums;
            item.ifpush = model.ifpush;
            item.ValueType = model.ValueType;
            return item;
        }

        #endregion
    }
}
