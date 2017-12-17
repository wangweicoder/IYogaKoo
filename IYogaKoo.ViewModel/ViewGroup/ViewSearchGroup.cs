using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewSearchGroup
    {
        //标题
        public string Title { get; set; }
        //图片
        public string Displayimg { get; set; }
        //正文
        public string Content { get; set; }
        //时间
        public DateTime? Date { get; set; }
        //编号
        public int ID { get; set; }
        //类别
        public string SearchType { get; set; }
        //url
        public string Url { get; set; }
    }

    public class ViewSearchTypeGroup
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string type { get; set; }

        public int isKep { get; set; }

        public string hidname { get; set; }
        public List<ViewTypes> ts { get; set; }

        public string hidValue { get; set; }
    }
    public class ViewTypes
    {
      
        public string name { get; set; }
        public string vlaue { get; set; }
        public string on { get; set; }
    }
}
