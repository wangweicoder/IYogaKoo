using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewUserModelsGroup
    {
        public ViewYogaUser VyList { get; set; }
        public ViewYogisModels VmList { get; set; }
        /// <summary>
        /// 被关注数量
        /// </summary>
        public int FollowCount { get; set; }
        public int iZan { get; set; }

        //派别
        public string usertype { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 关注 false否，true 是
        /// </summary>
        public bool iFollow { get; set; }
    }
}
