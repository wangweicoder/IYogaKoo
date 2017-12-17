using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
   public  class PicListGroup
    {
       public string uid { get; set; }
       /// <summary>
       /// 图片路径
       /// </summary>
       public string picsrc { get; set; }
       /// <summary>
       /// 相册名称
       /// </summary>
       public string pictitle { get; set; }
       /// <summary>
       /// 文件数量
       /// </summary>
       public int picnum{ get; set; }
       /// <summary>
       /// 文件内容
       /// </summary>
       public string piccontent { get; set; }
    }
}
