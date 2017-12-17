using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
namespace IYogaKoo.ViewModel
{
    public class ViewEvaluatesGroup
    {
        public ViewEvaluates entity { get; set; }
      
        /// <summary>
        /// 留言人
        /// </summary>
        public string FromUser { get; set; }

        public string Fromuid { get; set; }

        public string DisplayImg { get; set; }

        public string Url { get; set; }

        public string CetnerName { get; set; }
        /// <summary>
        /// 回复中的信息列表
        /// </summary>
        public List<ViewEvaluatesGroup> msgList { get; set; }
    }
}
