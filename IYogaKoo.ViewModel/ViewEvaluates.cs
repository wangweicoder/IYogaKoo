using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewEvaluates
    {
        #region 基本信息

        public int Evaluateid { get; set; }
        public DateTime CreateDate { get; set; }
        public string EContent { get; set; }

        public int? ToUid { get; set; }
        public int? FromUid { get; set; }
        public int? iZan { get; set; }
        public int? iShow { get; set; }

        public int? ParentID { get; set; }
        public int? Recommend { get; set; }

        public string Pic { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewEvaluates()
        {
        }
         
        #endregion

        #region - 方法 -
        public static Evaluates ToEntity(ViewEvaluates model)
        {
            Evaluates item = new Evaluates(); 
          
            item.Evaluateid = model.Evaluateid;
            item.CreateDate = model.CreateDate;
            item.EContent = model.EContent;
            item.ToUid = model.ToUid;
            item.FromUid = model.FromUid;
            item.iZan = model.iZan;
            item.iShow = model.iShow;
            item.ParentID = model.ParentID;
            item.Recommend = model.Recommend;
            item.Pic = model.Pic;
                
            return item;
        }

        public static ViewEvaluates ToViewModel(Evaluates model)
        {
            if (model == null) {

                return null;
            }

            ViewEvaluates item = new ViewEvaluates();
            item.Evaluateid = model.Evaluateid;
            item.CreateDate = model.CreateDate;
            item.EContent = model.EContent;
            item.ToUid = model.ToUid;
            item.FromUid = model.FromUid;
            item.iZan = model.iZan;
            item.iShow = model.iShow;
            item.ParentID = model.ParentID;
            item.Recommend = model.Recommend;
            item.Pic = model.Pic;
            return item;
        }

        #endregion
    }
}
