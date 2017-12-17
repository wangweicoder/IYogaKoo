using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewCentersGroup
    {
        public ViewCenters center { get; set; }
        public int recommond { get; set; }
        public string centertype { get; set; }

        /// <summary>
        /// 详情地址（省城区）
        /// </summary>
        public string Address { get; set; }
    }
}
