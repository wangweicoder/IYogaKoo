using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewUserDetailGroup
    {
        public ViewYogaUser VyList { get; set; }
        public ViewYogaUserDetail VDetailsList { get; set; }

        /// <summary>
        /// 被关注数量
        /// </summary>
        public int FollowCount { get; set; }

        //派别
        public string usertype { get; set; }
        /// <summary>
        /// 所属地区
        /// </summary>
        public string AddRessName { get; set; }
    }
}
