using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewSpace
    {
        #region 基本信息

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PersonNum { get; set; }
        public string Content { get; set; }
        public bool IsDefault { get; set; }
        public string Point { get; set; }
        public int OwnerID { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }


        public decimal PriceOfDay { get; set; }
        public decimal PriceOfHour { get; set; }
        public decimal PriceOfMonth { get; set; }


        public bool IsRenting { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }


        #endregion

        #region - 构造函数 -

        public ViewSpace()
        {
        }

        #endregion

        #region - 方法 -
        public static Space ToEntity(ViewSpace model)
        {
            Space item = new Space();

            item.ID = model.ID;
            item.Name = model.Name;
            item.Address = model.Address;
            item.PersonNum = model.PersonNum;
            item.Content = model.Content;
            item.IsDefault = model.IsDefault;
            item.Point = model.Point;
            item.OwnerID = model.OwnerID;
            item.OpenTime = model.OpenTime;
            item.CloseTime = model.CloseTime;
            item.PriceOfDay = model.PriceOfDay;
            item.PriceOfHour = model.PriceOfHour;
            item.PriceOfMonth = model.PriceOfMonth;
            item.IsRenting = model.IsRenting;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;



            return item;
        }

        public static ViewSpace ToViewModel(Space model)
        {
            if (model == null)
            {

                return null;
            }

            ViewSpace item = new ViewSpace();

            item.ID = model.ID;
            item.Name = model.Name;
            item.Address = model.Address;
            item.PersonNum = model.PersonNum;
            item.Content = model.Content;
            item.IsDefault = model.IsDefault;
            item.Point = model.Point;
            item.OwnerID = model.OwnerID;
            item.OpenTime = model.OpenTime;
            item.CloseTime = model.CloseTime;
            item.PriceOfDay = model.PriceOfDay;
            item.PriceOfHour = model.PriceOfHour;
            item.PriceOfMonth = model.PriceOfMonth;
            item.IsRenting = model.IsRenting;
            item.CreateTime = model.CreateTime;
            item.UpdateTime = model.UpdateTime;



            return item;
        }

        #endregion
    }
}
