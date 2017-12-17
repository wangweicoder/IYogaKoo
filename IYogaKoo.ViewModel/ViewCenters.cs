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
    public class ViewCenters
    {
        #region 基本信息
        [DisplayName("会馆编号")]
        public int CenterId { get; set; }
        [DisplayName("会员编号")]
        public string  Uid { get; set; }
        [DisplayName("会馆名称")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "会馆名称不能为空")]
        public string CenterName { get; set; }
        [DisplayName("会馆地址")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "会馆地址不能为空")]
        public string  CenterAddress { get; set; }
        [DisplayName("城区")]
        public int? DistrictID { get; set; }
        [DisplayName("城市")]
        public int? CityID { get; set; }
        [DisplayName("省")]
        public int? ProvinceID { get; set; }
        [DisplayName("国家")]
        public int? CountryID { get; set; }
        [DisplayName("信息创建时间")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("信息修改时间")]
        public DateTime?  UpgradeDate { get; set; }
        [DisplayName("会馆简介")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "会馆简介不能为空")]
        public string CenterProfile { get; set; }

        [DisplayName("开馆时间")]
        public string OpenTime { get; set; }

        [DisplayName("闭馆时间")]        
        public string CloseTime { get; set; }

        [DisplayName("会馆类型")]
        public string CenterType { get; set; }
        [DisplayName("Banner")]
        public string CenterBanner { get; set; }
        [DisplayName("功能介绍")]
        public string CenterIntrodition { get; set; }

        [DisplayName("封面")]
        public string CenterPortraint
        {
            get;
            set;
        }
        [DisplayName("流派")]
        public string YogaTypeid { get; set; }

        [DisplayName("会馆状态")]
        public int? CenterState { get; set; }

        [DisplayName("添加会馆来源")]
        public int? CenterSource { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewCenters()
        {
        }

        #endregion

        #region - 方法 -
        public static Centers ToEntity(ViewCenters model)
        {
            Centers item = new Centers();

            item.CenterId = model.CenterId;
            item.Uid = model.Uid;
            item.CenterAddress = model.CenterAddress;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.CenterProfile = model.CenterProfile;
            item.OpenTime  =Convert.ToDateTime(model.OpenTime);
            item.CloseTime =Convert.ToDateTime(model.CloseTime);
            item.CenterName = model.CenterName;
            item.CenterType = model.CenterType;
            item.CenterBanner = model.CenterBanner;
            item.CenterIntroduction = model.CenterIntrodition;
            item.CenterPortrait = model.CenterPortraint;
            item.YogaTypeid = model.YogaTypeid;
            item.CenterState = model.CenterState;
            item.CenterSource = model.CenterSource;

            return item;
        }

        public static ViewCenters ToViewModel(Centers model)
        {
            if (model == null)
            {

                return null;
            }

            ViewCenters item = new ViewCenters();

            item.CenterId = model.CenterId;
            item.Uid = model.Uid;
            item.CenterAddress = model.CenterAddress;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.CreateDate = model.CreateDate;
            item.UpgradeDate = model.UpgradeDate;
            item.CenterProfile = model.CenterProfile;
            item.OpenTime = model.OpenTime == null ? "" : DateTime.Parse(model.OpenTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            item.CloseTime = model.CloseTime == null ? "" : DateTime.Parse(model.CloseTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"); 
            item.CenterName = model.CenterName;
            item.CenterType = model.CenterType;
            item.CenterBanner = model.CenterBanner;
            item.CenterIntrodition = model.CenterIntroduction;
            item.CenterPortraint = model.CenterPortrait;
            item.YogaTypeid = model.YogaTypeid;
            item.CenterSource = model.CenterSource;
            item.CenterState = model.CenterState;
            return item;
        }

        #endregion
    }
}
