using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    /// <summary>
    /// 关健字搜索
    /// </summary>
    public class ViewtKeyWord
    {
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("登录者ID")]
        public int?  Uid { get; set; }
        [DisplayName("关健字")] 
        public string sWord { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewtKeyWord()
        {
        }

        #endregion

        #region - 方法 -
        public static tKeyWord ToEntity(ViewtKeyWord model)
        {
            tKeyWord item = new tKeyWord();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sWord = model.sWord;
            item.CreateTime = model.CreateTime;
            return item;
        }

        public static ViewtKeyWord ToViewModel(tKeyWord model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtKeyWord item = new ViewtKeyWord();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sWord = model.sWord;
            item.CreateTime = model.CreateTime;

            return item;
        }

        #endregion
    }
}
