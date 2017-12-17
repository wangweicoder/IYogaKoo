using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewCenterStare
    {
        #region 基本信息
        [DisplayName("编号")]
        public int Sid { get; set; }
        [DisplayName("价格")]
        public decimal  Price { get; set; }
      
        [DisplayName("课程")]
        public int Centerclass { get; set; }
        [DisplayName("服务")]
        public int Service { get; set; }
        [DisplayName("环境")]
        public int Env { get; set; }
        [DisplayName("创建人")]
        public int Uid { get; set; }
         [DisplayName("会馆编号")]
        public int Mid { get; set; }
         [DisplayName("状态")]
        public int Satate { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateDate { get; set; }
        
        #endregion

        #region - 构造函数 -

        

        #endregion

        #region - 方法 -
        public static CenterStare ToEntity(ViewCenterStare model)
        {
            CenterStare item = new CenterStare();
            item.Sid=model.Sid;
            item.Price=model.Price;
            item.Centerclass=model.Centerclass;
            item.Service=model.Service;
            item.Env=model.Env;
            item.Uid=model.Uid;
            item.Mid=model.Mid;
            item.Satate=model.Satate;
            item.CreateDate=model.CreateDate;
            return item;
        }

        public static ViewCenterStare ToViewModel(CenterStare model)
        {
            if (model == null)
            {
                return null;
            }

            ViewCenterStare item = new ViewCenterStare();
            item.Sid = model.Sid;
            item.Price = model.Price;
            item.Centerclass = model.Centerclass;
            item.Service = model.Service;
            item.Env = model.Env;
            item.Uid = model.Uid;
            item.Mid = model.Mid;
            item.Satate = model.Satate;
            item.CreateDate = model.CreateDate;
            return item;
        }

        #endregion
    }
}
