using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    /// <summary>
    /// 搜索：分组查询
    /// </summary>
    public class tKeyWordGroup
    {
        /// <summary>
        /// 编号
        /// </summary>
         [DisplayName("编号")]
        public int rownum { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
         [DisplayName("关键字")]
        public string sWord { get; set; }
       /// <summary>
       /// 搜索次数
       /// </summary>
       [DisplayName("搜索次数")]
        public int iNums { get; set; }
       
    }
}
