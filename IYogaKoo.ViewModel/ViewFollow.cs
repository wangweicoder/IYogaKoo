using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewFollow
    {
        #region 基本信息

        public int Followid { get; set; }
        public int Uid { get; set; }
        public int QuiltUid { get; set; }
        public DateTime FollowDate { get; set; }
        public int? QuiltCenterID { get; set; }
        public bool? isfollow { get; set; }

        /// <summary>
        /// 0 习练者，1 导师 = UserType
        /// </summary>
        public int? iType { get; set; }

        /// <summary>
        /// 类型与站内信一样
        /// </summary>
        public int? loginType { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewFollow()
        {
        }

        #endregion

        #region - 方法 -
        public static Follow ToEntity(ViewFollow model)
        {
            Follow item = new Follow();

            item.Followid = model.Followid;
            item.Uid = model.Uid;
            item.QuiltUid = model.QuiltUid;
            item.FollowDate = model.FollowDate;
            item.QuiltCenterID = model.QuiltCenterID;
            item.isfollow = model.isfollow;
            item.iType = model.iType;
            item.loginType = model.loginType;
            return item;
        }

        public static ViewFollow ToViewModel(Follow model)
        {
            if (model == null)
            {

                return null;
            }

            ViewFollow item = new ViewFollow();

            item.Followid = model.Followid;
            item.Uid = model.Uid;
            item.QuiltUid = model.QuiltUid;
            item.FollowDate = model.FollowDate;
            item.QuiltCenterID = model.QuiltCenterID;
            item.isfollow = model.isfollow;
            item.iType = model.iType;
            item.loginType = model.loginType;

            return item;
        }

        #endregion
    }
}
