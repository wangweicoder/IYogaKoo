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
    /// 站内信
    /// </summary>
    public class ViewtInstationInfo
    { 
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("会员编号")]
        public int?  Uid { get; set; }
       
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        
        [DisplayName("内容")]
        public string sContent { get; set; }
        /// <summary>
        /// 取字典表：站内信
        /// </summary>
        [DisplayName("类型")] 
        public  int?  iType { get; set; }
        [DisplayName("创建人")]
        public string CreateUser { get; set; }

        [DisplayName("是否删除")]
        public bool? ifDel { get; set; }
        [DisplayName("登录状态")]
        /// <summary>
        /// 登录状态：默认为 0； 1 第一次登录查看最新数据； 2 已经查看过数据
        /// </summary>
        public int? loginType { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewtInstationInfo()
        {
        }

        #endregion

        #region - 方法 -
        public static tInstationInfo ToEntity(ViewtInstationInfo model)
        {
            tInstationInfo item = new tInstationInfo();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.CreateTime = model.CreateTime;
            item.CreateUser = model.CreateUser;
            item.iType = model.iType;
            item.sContent = model.sContent;
            item.ifDel = model.ifDel;
            item.loginType = model.loginType;
            return item;
        }

        public static ViewtInstationInfo ToViewModel(tInstationInfo model)
        {
            if (model == null)
            { 
                return null;
            }

            ViewtInstationInfo item = new ViewtInstationInfo();
             
            item.ID = model.ID;
            item.Uid = model.Uid;
            item.CreateTime = model.CreateTime;
            item.CreateUser = model.CreateUser;
            item.iType = model.iType;
            item.sContent = model.sContent;
            item.ifDel = model.ifDel;
            item.loginType = model.loginType;
            return item;
        }

        #endregion
    }
}
