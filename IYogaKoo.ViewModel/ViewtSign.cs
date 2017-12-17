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
    public class ViewtSign
    {
        #region 基本信息
        [DisplayName("编号")]
        public int  ID {get;set;}
        [DisplayName("签到者")]
        public int?  Uid {get;set;}
        [DisplayName("签到说明")]
        public string  sMemo {get;set;}
        [DisplayName("签到时间")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("签到类型")]
        public int?  iType {get;set;}
        [DisplayName("积分")]
        public int?  iIntegral {get;set;}
  
        #endregion

        #region - 构造函数 -

        public ViewtSign()
        {
        }

        #endregion

        #region - 方法 -
        public static tSign ToEntity(ViewtSign model)
        {
            tSign item = new tSign();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sMemo = model.sMemo;
            item.CreateDate = model.CreateDate;
            item.iType = model.iType;
            item.iIntegral = model.iIntegral; 

            return item;
        }

        public static ViewtSign ToViewModel(tSign model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtSign item = new ViewtSign();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sMemo = model.sMemo;
            item.CreateDate = model.CreateDate;
            item.iType = model.iType;
            item.iIntegral = model.iIntegral; 

            return item;
        }

        #endregion
    }
}
