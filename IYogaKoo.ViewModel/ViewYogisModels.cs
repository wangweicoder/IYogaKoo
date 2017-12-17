using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewYogisModels
    {
        #region 基本信息
        [DisplayName("编号")]
        public int YID { get; set; }
        public int UID { get; set; }
        [DisplayName("真实姓名")]
        public string RealName { get; set; }
        [DisplayName("性别")]
        public int? Gender { get; set; }
        public int? Headid { get; set; }
        [DisplayName("会馆")]
        public string CenterID { get; set; }
        [DisplayName("流派")]
        public string YogaTypeid { get; set; }
        [DisplayName("所属导师")]
        public string TeachYogis { get; set; }
        [DisplayName("每节课的收费")]   
        public string EachClassCost { get; set; }
        [DisplayName("证件类型")]
        public string IdType { get; set; }
        [DisplayName("证件号码")]
        public string IdCardNum { get; set; }
        [DisplayName("开始年份")]
        public DateTime? StartTeachYear { get; set; }
        [DisplayName("国籍")]
        public string Nationality { get; set; }
        [DisplayName("街道")]
        public string Street { get; set; }
        [DisplayName("所在商圈")]
        public int? LocationID { get; set; }
        [DisplayName("城区")]
        public int? DistrictID { get; set; }
        [DisplayName("城区")]
        public int? CityID { get; set; }
        [DisplayName("省")]
        public int? ProvinceID { get; set; }
        [DisplayName("国家")]
        public int? CountryID { get; set; }
        [DisplayName("导师级别")]
        public int? YogisLevel { get; set; }
        [DisplayName("导师状态")]
        public int? YogiStatus { get; set; }
        [DisplayName("升级为导师的时间")]
        public DateTime? BecomeYogisTime{ get; set; }
        [DisplayName("导师的评分")]
        public int? YogisScore { get; set; }
        [DisplayName("头像")]
        public string DisplayImg { get; set; }
        [DisplayName("是否有付款记录")]
        public bool? IsPayRecord { get; set; }
        [DisplayName("导师认证信息Id")]
        public int? Profileid { get; set; }
        [DisplayName("记录创建时间")]
        public DateTime?  CreateDate{ get; set; }
        [DisplayName("记录更新时间")]
        public DateTime? UpgradeDate { get; set; }
        [DisplayName("是否接收协议")]
        public bool? IsAcceptAgreement{ get; set; }
        [DisplayName("自我介绍")]
        public string YogisDepict{ get; set; }
        [DisplayName("签名")]
        public string GudWords{ get; set; }
        [DisplayName("练习次数")]
        public int? iRate{ get; set; }
        [DisplayName("背景图")]
        public string CoverImg{ get; set; }
        [DisplayName("删除状态")]
        /// <summary>
        /// 删除状态
        /// 1 假删除； 0 真删除
        /// </summary>
        public int? delState { get; set; }
        [DisplayName("级别状态")]
        public string Leveldetails { get; set; }
        [DisplayName("赞")]
        public int? iZan { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewYogisModels()
        {
        }
         
        #endregion

        #region - 方法 -
        public static YogisModels ToEntity(ViewYogisModels model)
        {
            YogisModels item = new YogisModels();
             
            item.YID = model.YID;
            item.UID = model.UID;
            item.RealName = model.RealName;
            item.Gender = model.Gender;
            item.Headid = model.Headid;
            item.CenterID = model.CenterID;
            item.YogaTypeid = model.YogaTypeid;
            item.TeachYogis = model.TeachYogis;
            item.EachClassCost = model.EachClassCost;
            item.IdType = model.IdType;
            item.IdCardNum = model.IdCardNum;
            item.StartTeachYear = model.StartTeachYear;
            item.Nationality = model.Nationality;
            item.Street = model.Street;
            item.LocationID = model.LocationID;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.YogisLevel = model.YogisLevel;
            item.YogiStatus = model.YogiStatus;
            item.BecomeYogisTime = model.BecomeYogisTime;
            item.YogisScore = model.YogisScore;
            item.DisplayImg = model.DisplayImg;
            item.IsPayRecord = model.IsPayRecord;
            item.Profileid = model.Profileid;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.IsAcceptAgreement = model.IsAcceptAgreement;
            item.YogisDepict = model.YogisDepict;
            item.GudWords = model.GudWords;
            item.iRate = model.iRate;
            item.CoverImg = model.CoverImg;
            item.delState = model.delState;
            item.Leveldetails = model.Leveldetails;
            item.iZan = model.iZan;
            return item;
        }

        public static ViewYogisModels ToViewModel(YogisModels model)
        {
            if (model == null) {

                return null;
            }

            ViewYogisModels item = new ViewYogisModels();
            item.YID = model.YID;
            item.UID = model.UID;
            item.RealName = model.RealName;
            item.Gender = model.Gender ;
            item.Headid = model.Headid;
            item.CenterID = model.CenterID;
            item.YogaTypeid = model.YogaTypeid;
            item.TeachYogis = model.TeachYogis;
            item.EachClassCost = model.EachClassCost;
            item.IdType = model.IdType;
            item.IdCardNum = model.IdCardNum;
            item.StartTeachYear = model.StartTeachYear;
            item.Nationality = model.Nationality;
            item.Street = model.Street;
            item.LocationID = model.LocationID;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.YogisLevel = model.YogisLevel;
            item.YogiStatus = model.YogiStatus;
            item.BecomeYogisTime = model.BecomeYogisTime;
            item.YogisScore = model.YogisScore;
            item.DisplayImg = model.DisplayImg;
            item.IsPayRecord = model.IsPayRecord;
            item.Profileid = model.Profileid;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.IsAcceptAgreement = model.IsAcceptAgreement;
            item.YogisDepict = model.YogisDepict;
            item.GudWords = model.GudWords;
            item.iRate = model.iRate;
            item.CoverImg = model.CoverImg;

             item.delState = model.delState;
             item.Leveldetails = model.Leveldetails;
             item.iZan = model.iZan;
            return item;
        }

        #endregion
    }
}
