using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewClassDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public System.DateTime EndTime { get; set; }
        public int Level { get; set; }
        public decimal? Price { get; set; }
        public int CenterID { get; set; }
        public string Address { get; set; }
        public int UserID { get; set; }
        public int YID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Mark { get; set; }
        public string Mark2 { get; set; }


        public static ClassDetail ToEntity(ViewClassDetail model)
        {
            ClassDetail item = new ClassDetail();

            item.ID = model.ID;
            item.Name = model.Name;
            item.StartTime = model.StartTime;
            item.Duration = model.Duration;
            item.EndTime = model.EndTime;
            item.Level = model.Level;
            item.Price = model.Price;
            item.CenterID = model.CenterID;
            item.Address = model.Address;
            item.UserID = model.UserID;
            item.YID = model.YID;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;
            item.IsDeleted = model.IsDeleted;
            item.Mark = model.Mark;
            item.Mark2 = model.Mark2;

            return item;
        }

        public static ViewClassDetail ToViewModel(ClassDetail model)
        {
            ViewClassDetail item = new ViewClassDetail();

            item.ID = model.ID;
            item.Name = model.Name;
            item.StartTime = model.StartTime;
            item.Duration = model.Duration;
            item.EndTime = model.EndTime;
            item.Level = model.Level;
            item.Price = model.Price;
            item.CenterID = model.CenterID;
            item.Address = model.Address;
            item.UserID = model.UserID;
            item.YID = model.YID;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;
            item.IsDeleted = model.IsDeleted;
            item.Mark = model.Mark;
            item.Mark2 = model.Mark2;

            return item;
        }

    }
}
