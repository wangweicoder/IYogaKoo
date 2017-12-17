using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewBackUserModelsGroup
    {
        public ViewYogisModels VYogisModels { get; set; }
        /// <summary>
        /// 师从导师
        /// </summary>
        public string  TeachersName { get; set; }
        /// <summary>
        /// 会馆
        /// </summary>
        public string  CentersName { get; set; }
        /// <summary>
        /// 所属流派
        /// </summary>
        public string LiuPai { get; set; }
        /// <summary>
        /// 申请为导师：根据分数判断
        /// </summary>
        public string UpTeacher { get; set; }
    }
}
