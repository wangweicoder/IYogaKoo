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
    /// 留言
    /// </summary>
    public class ViewtMessage
    {
       
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("被留言人")]
        /// <summary>
        /// 被留言人
        /// </summary>
        public int? ToUid { get; set; }
        [DisplayName("外健")]
        /// <summary>
        /// 外健（日志/活动的ID）
        /// </summary>
        public int? Toid { get; set; }
        [DisplayName("内容")]
        /// <summary>
        /// 内容
        /// </summary>
        public string sContent { get; set; }
         [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
          [DisplayName("留言人")]
        /// <summary>
        /// 留言人
        /// </summary>
        public int? FromUid { get; set; }
            [DisplayName("赞")]
        /// <summary>
        /// 赞
        /// </summary>
        public int? iZan { get; set; }
         [DisplayName("审核状态")]
        /// <summary>
        /// 审核状态0 审核中 1 通过 2 不通过
        /// </summary>
        public int? iAudio { get; set; }
        /// <summary>
        /// 被留言人类型： 0 习练者/ 1 导师 
        /// </summary>
        public int? ToType { get; set; }
        /// <summary>
        /// 留言人类型： 0 习练者/ 1 导师   
        /// </summary>
        public int? FormType { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int? ParentID { get; set; }
         [DisplayName("图片")]
        public string photo { get; set; }
         public int? loginType { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewtMessage()
        {
        }

        #endregion

        #region - 方法 -
        public static tMessage ToEntity(ViewtMessage model)
        {
            tMessage item = new tMessage();

            item.ID = model.ID;
            item.ToUid = model.ToUid;
            item.sContent = model.sContent;
            item.CreateDate = model.CreateDate;
            item.FromUid = model.FromUid;
            item.iZan = model.iZan;
            item.iAudio = model.iAudio;
            item.ToType = model.ToType;
            item.FormType = model.FormType;
            item.ParentID = model.ParentID;
            item.photo = model.photo;
            item.Toid = model.Toid;
            item.loginType = model.loginType;
            return item;
        }

        public static ViewtMessage ToViewModel(tMessage model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtMessage item = new ViewtMessage();

            item.ID = model.ID;
            item.ToUid = model.ToUid;
            item.sContent = model.sContent;
            item.CreateDate = model.CreateDate;
            item.FromUid = model.FromUid;
            item.iZan = model.iZan;
            item.iAudio = model.iAudio;
            item.ToType = model.ToType;
            item.FormType = model.FormType;
            item.ParentID = model.ParentID;
            item.photo = model.photo;
            item.Toid = model.Toid;
            item.loginType = model.loginType;
            return item;
        }

        #endregion
    }
}
