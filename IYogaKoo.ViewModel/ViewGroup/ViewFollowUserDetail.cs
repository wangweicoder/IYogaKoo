using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewFollowUserDetail
    {
        //public ViewFollow VfList{get;set;}
        //public ViewYogaUser VyList { get; set; }
        //public ViewYogaUserDetail VDetailsList { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string spic { get; set; }     
        /// <summary>
        /// 被关注数量
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FollowersCount { get; set; }
        /// <summary>
        /// 自己呢称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string userurl { get; set; }        
        /// <summary>
        /// 级别
        /// </summary>
        public int Leval { get; set; }
        /// <summary>
        /// uid
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 个签
        /// </summary>
        public string Asign { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string ressname { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Profile { get; set; }
        /// <summary>
        /// 粉丝呢称
        /// </summary>
        public string FollowersName { get; set; }
        /// <summary>
        /// 标识导师还是习练者
        /// </summary>
        public int flag { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>

        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最新的信息-与用户信息表的最近登录时间比较=1 最新
        /// </summary>
        public int iNew { get; set; }
        /// <summary>
        /// 日志ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 1 谁 评论了 我 
        /// 2 谁 评论了 我的日志 
        /// 3 谁 评论了 我的活动 
        /// </summary>
        public string messType { get; set; }
    }
}
