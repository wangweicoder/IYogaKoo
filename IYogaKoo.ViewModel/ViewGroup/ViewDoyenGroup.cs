using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewDoyenGroup
    {
        //public ViewYogaUser VyList { get; set; }
        //public ViewYogaUserDetail VydList { get; set; }
        //public ViewYogisModels VmList { get; set; }

        /// <summary>
        /// 被关注数量
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 呢称
        /// </summary>
        public string  nickname { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string userurl { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string imgurl { get; set; }
        /// <summary>
        /// 标识导师还是习练者
        /// </summary>
        public int flag { get; set; }
        /// <summary>
        /// uid
        /// </summary>
        public int uid { get; set; }

    }
}
