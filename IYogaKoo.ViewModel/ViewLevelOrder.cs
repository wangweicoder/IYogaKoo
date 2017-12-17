using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
namespace IYogaKoo.ViewModel
{
    public class ViewLevelOrder
    {
        #region  基本信息
        public int ID { get; set; }
        public string LevelOrderID { get; set; }
        public int UID { get; set; }
        public string Name { get; set; }
        public string OrderType { get; set; }
        public string OrderState { get; set; }
        public string OrderScore { get; set; }

        public int OrderDel { get; set; }
        public string OriginalLevel { get; set; }
        public string TargetLevel { get; set; }
        public int? ManageID { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Reason { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; } 

        #endregion

        #region  方法

        public static LevelOrder ToEntity(ViewLevelOrder model)
        {
            LevelOrder item = new LevelOrder();
            item.ID = model.ID;
            item.LevelOrderID = model.LevelOrderID;
            item.UID = model.UID;
            item.Name = model.Name;
            item.OrderType = model.OrderType;
            item.OrderState = model.OrderState;
            item.OrderScore = model.OrderScore;
            item.OrderDel = model.OrderDel;
            item.OriginalLevel = model.OriginalLevel;
            item.TargetLevel = model.TargetLevel;
            item.ManageID = model.ManageID;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;
            item.Reason = model.Reason;
            item.Remark1 = model.Remark1;
            item.Remark2 = model.Remark2;
            return item;
        }
        public static ViewLevelOrder ToViewModel(LevelOrder model)
        {
            if (model == null)
            {
                return null;
            }
            ViewLevelOrder item = new ViewLevelOrder();
            item.ID = model.ID;
            item.LevelOrderID = model.LevelOrderID;
            item.UID = model.UID;
            item.Name = model.Name;
            item.OrderType = model.OrderType;
            item.OrderState = model.OrderState;
            item.OrderScore = model.OrderScore;
            item.OrderDel = model.OrderDel;
            item.OriginalLevel = model.OriginalLevel;
            item.TargetLevel = model.TargetLevel;
            item.ManageID = model.ManageID;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;
            item.Reason = model.Reason;
            item.Remark1 = model.Remark1;
            item.Remark2 = model.Remark2;
            return item;
        }

        #endregion
    }
}
