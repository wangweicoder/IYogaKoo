using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    
    public class ViewtMessageGroup
    {
        public ViewtMessage entity { get; set; }
        /// <summary>
        /// 被留言人
        /// </summary>
        public string  ToUser { get; set; }
        /// <summary>
        /// 留言人
        /// </summary>
        public string FromUser   { get; set; }
        /// <summary>
        /// 留言人类型
        /// </summary>
        public int? UserType { get; set; }
        /// <summary>
        /// 图片 
        /// </summary>
        public string DisplayImg { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string sUrl { get; set; }
        /// <summary>
        /// 回复中的信息列表
        /// </summary>
        public List<ViewtMessageGroup> msgList { get; set; }
    }
}
