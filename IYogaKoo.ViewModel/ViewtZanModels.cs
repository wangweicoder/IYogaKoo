using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewtZanModels
    {
         #region 基本信息
       
        public int ID { get; set; }
        /// <summary>
        /// 登录者id
        /// </summary>
        public int? iFromUid { get; set; }
        /// <summary>
        /// 导师id
        /// </summary>
        public int? iToUid { get; set; }
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 类型1为导师,0为习练者
        /// </summary>
        public int? iType { get; set; }

        /// <summary>
        /// 被赞类型：类型1为导师,0为习练者 ，2 学习互动（社区）3 日志标识
        /// </summary>
        public int? iToType { get; set; }
        /// <summary>
        /// 被赞人ID ，用户ID
        /// </summary>
        public int? ToUid { get; set; }

        public int? loginType { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewtZanModels()
        {
        }

        #endregion
        //, , , , 
        #region - 方法 -
        public static tZanModels ToEntity(ViewtZanModels model)
        {
            tZanModels item = new tZanModels();

            item.ID = model.ID;
            item.iFromUid = model.iFromUid;
            item.iToUid = model.iToUid;
            item.CreateDate = model.CreateDate;
            item.iType = model.iType;
            item.iToType = model.iToType;
            item.ToUid = model.ToUid;
            item.loginType = model.loginType;
            return item;
        }

        public static ViewtZanModels ToViewModel(tZanModels model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtZanModels item = new ViewtZanModels();

            item.ID = model.ID;
            item.iFromUid = model.iFromUid;
            item.iToUid = model.iToUid;
            item.CreateDate = model.CreateDate;
            item.iType = model.iType;
            item.iToType = model.iToType;
            item.ToUid = model.ToUid;
            item.loginType = model.loginType;
            return item;
        }

        #endregion
    }
}
