using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewClassGroup
    {

        public int Id { get; set; }

        public string Banner { get; set; }
        public string Start { get; set; }

        public int AreaID { get; set; }

        public string Name { get; set; }
        public string TopicIds { get; set; }

        [DisplayName("评论人数")]
        public int Nums { get; set; }
        [DisplayName("感兴趣人数")]
        public int InterNums { get; set; }
        [DisplayName("参加人数")]
        public int OrderNums { get; set; }
        [DisplayName("地区")]
        public int DicId { get; set; }
        [DisplayName("地区名称")]
        public string AreaName { get; set; }

         [DisplayName("概述")]
        public string Summary { get; set; }
        [DisplayName("活动详情")]
        public string Content { get; set; }
    }
}
