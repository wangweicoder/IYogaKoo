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
    /// 登录信息
    /// </summary>
    public class ViewtUserLoginInfo
    { 
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("会员编号")]
        public int? Uid { get; set; } 
        /// <summary>
        /// 登录/退出信息
        /// </summary>
        public string sloginInfo { get; set; }
       
        [DisplayName("信息创建时间")]
        public DateTime? LoginTime { get; set; }
        [DisplayName("信息修改时间")]
        public DateTime?  QuitTime { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewtUserLoginInfo()
        {
        }

        #endregion

        #region - 方法 -
        public static tUserLoginInfo ToEntity(ViewtUserLoginInfo model)
        {
            tUserLoginInfo item = new tUserLoginInfo();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sloginInfo = model.sloginInfo;

            item.LoginTime = model.LoginTime;
            item.QuitTime = model.QuitTime;
            item.NickName = model.NickName;
            return item;
        }

        public static ViewtUserLoginInfo ToViewModel(tUserLoginInfo model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtUserLoginInfo item = new ViewtUserLoginInfo();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sloginInfo = model.sloginInfo; 
            item.LoginTime = model.LoginTime;
            item.QuitTime = model.QuitTime;
            item.NickName = model.NickName;
            return item;
        }

        #endregion
    }
}
