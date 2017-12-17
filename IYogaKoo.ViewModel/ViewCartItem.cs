using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewCartItem
    {
        #region 基本信息

        public int ID { get; set; }
        public int SellerID { get; set; }
        public int UserID { get; set; }
        public int ObjectID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string ObjectImage { get; set; }
        public decimal  Price { get; set; }
        public decimal Discount { get; set; }
        public DateTime ClassTime { get; set; }
        public int Number { get; set; }
        public string Seller_UID { get; set; }
        public string User_UID { get; set; }


        #endregion

        #region - 构造函数 -

        public ViewCartItem()
        {
        }

        #endregion

        #region - 方法 -
        public static CartItem ToEntity(ViewCartItem model)
        {
            CartItem item = new CartItem();

            item.ID = model.ID;
            item.SellerID = model.SellerID;
            item.UserID = model.UserID;
            item.ObjectID = model.ObjectID;
            item.Type = model.Type;
            item.Name = model.Name;
            item.ObjectImage = model.ObjectImage;
            item.Price = model.Price;
            item.Discount = model.Discount;
            item.ClassTime = model.ClassTime;
            item.Number = model.Number;
            item.Seller_UID = model.Seller_UID;
            item.User_UID = model.User_UID;


            return item;
        }

        public static ViewCartItem ToViewModel(CartItem model)
        {
            if (model == null)
            {

                return null;
            }

            ViewCartItem item = new ViewCartItem();


            item.ID = model.ID;
            item.SellerID = model.SellerID;
            item.UserID = model.UserID;
            item.ObjectID = model.ObjectID;
            item.Type = model.Type;
            item.Name = model.Name;
            item.ObjectImage = model.ObjectImage;
            item.Price = model.Price;
            item.Discount = model.Discount;
            item.ClassTime = model.ClassTime;
            item.Number = model.Number;
            item.Seller_UID = model.Seller_UID;
            item.User_UID = model.User_UID;


            return item;
        }

        #endregion
    }
}
