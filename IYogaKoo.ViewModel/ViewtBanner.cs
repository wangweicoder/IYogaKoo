using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewtBanner
    {
        #region 基本信息

        public int ID { get; set; }
        [DisplayName("图片")]
        public string spic { get; set; }

        [DisplayName("链接网址")]
        public string sUrl { get; set; }
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 0 首页Banner 1 活动回顾Banner 2 活动预告Banner
        /// </summary>
         [DisplayName("所在位置")]
        public int? iType { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewtBanner()
        {
        }

        #endregion

        #region - 方法 -
        public static tBanner ToEntity(ViewtBanner model)
        {
            tBanner item = new tBanner();

            item.ID = model.ID;
            item.CreateDate = model.CreateDate;
            item.spic = model.spic;
            item.sUrl = model.sUrl;
            item.iType = model.iType;
            return item;
        }

        public static ViewtBanner ToViewModel(tBanner model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtBanner item = new ViewtBanner();

            item.ID = model.ID;
            item.CreateDate = model.CreateDate;
            item.spic = model.spic;
            item.sUrl = model.sUrl;
            item.iType = model.iType;
            return item;
        }

        #endregion
    }
}
