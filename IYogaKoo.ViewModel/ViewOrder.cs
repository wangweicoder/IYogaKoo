using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewOrder
    {
        #region 基本信息

        public int Id { get; set; }
        public string  Phone { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string PayAccount { get; set; }
        public string Payment { get; set; }
        public bool IsPaid { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime? PayTime { get; set; }
        public string PayNO { get; set; }
        public string PayDomain { get; set; }
        public string OpenID { get; set; }
        public Class Class { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewOrder()
        {
        }

        #endregion

        #region - 方法 -
        public static Order ToEntity(ViewOrder model)
        {
            Order item = new Order();

            item.Id = model.Id;
            item.Phone = model.Phone;
            item.Name = model.Name;
            item.Number = model.Number;
            item.Amount = model.Amount;
            item.Description = model.Description;
            item.PayAccount = model.PayAccount;
            item.Payment = model.Payment;
            item.IsPaid = model.IsPaid;
            item.UserId = model.UserId;
            item.ClassId = model.ClassId;
            item.Status = model.Status;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.PayTime = model.PayTime;
            item.PayNO = model.PayNO;
            item.PayDomain = model.PayDomain;
            item.openid = model.OpenID;
            item.Class = model.Class;
            return item;
        }

        public static ViewOrder ToViewModel(Order model)
        {
            if (model == null)
            {

                return null;
            }

            ViewOrder item = new ViewOrder();

            item.Id = model.Id;
            item.Phone = model.Phone;
            item.Name = model.Name;
            item.Number = model.Number;
            item.Amount = model.Amount;
            item.Description = model.Description;
            item.PayAccount = model.PayAccount;
            item.Payment = model.Payment;
            item.IsPaid = model.IsPaid;
            item.UserId = model.UserId;
            item.ClassId = model.ClassId;
            item.Status = model.Status;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.PayTime = model.PayTime;
            item.PayNO = model.PayNO;
            item.PayDomain = model.PayDomain;
            item.OpenID = model.openid;
            item.Class = model.Class;

            return item;
        }

        #endregion
    }
}
