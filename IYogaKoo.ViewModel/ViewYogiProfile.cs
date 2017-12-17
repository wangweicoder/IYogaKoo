using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewYogiProfile
    {
        #region 基本信息

        public int ProfileID { get; set; }
        public int UID { get; set; }
          [DisplayName("身份证类型")]
        public string IDType { get; set; }
          [DisplayName("身份证号")]
        public string IDNum { get; set; }
        [DisplayName("手持身份证照片")]
        public string IDPhoto { get; set; }
         [DisplayName("身份证正面")]
        public string IDScan { get; set; }
         [DisplayName("身份证反面")]
        public string IDScanF { get; set; }
         [DisplayName("瑜伽导师资格证明文件扫描件")]
        public string Diploma { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpgradeDate { get; set; }
         [DisplayName("审核不通过原因")]
        /// <summary>
        /// 审核不通过原因
        /// </summary>
        public string sMemo { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewYogiProfile()
        {
        }

        #endregion

        #region - 方法 -
        public static YogiProfile ToEntity(ViewYogiProfile model)
        {
            YogiProfile item = new YogiProfile();

            item.ProfileID = model.ProfileID;
            item.UID = model.UID;
            item.IDType = model.IDType;
            item.IDNum = model.IDNum;
            item.IDPhoto = model.IDPhoto;
            item.IDScan = model.IDScan;
            item.IDScanF = model.IDScanF;
            item.Diploma = model.Diploma;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.sMemo = model.sMemo;
            return item;
        }

        public static ViewYogiProfile ToViewModel(YogiProfile model)
        {
            if (model == null)
            {

                return null;
            }

            ViewYogiProfile item = new ViewYogiProfile();
            item.ProfileID = model.ProfileID;
            item.UID = model.UID;
            item.IDType = model.IDType;
            item.IDNum = model.IDNum;
            item.IDPhoto = model.IDPhoto;
            item.IDScanF = model.IDScanF;
            item.IDScan = model.IDScan;
            item.Diploma = model.Diploma;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.sMemo = model.sMemo;
            return item;
        }

        #endregion
    }
}
