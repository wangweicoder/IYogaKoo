using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel.Commons.Enums;
using System.ComponentModel.DataAnnotations;
using IYogaKoo.Entity;

namespace IYogaKoo.ViewModel
{
    public  class ViewYogaUserDetail
    {
        #region 基本信息
            /// <summary>
            /// 基本信息流水
            /// </summary>
            [DisplayName("编号")]
            public int ID { get; set; }
            [DisplayName("用户ID")]
            public int UID { get; set; }
            [Required(ErrorMessage = "必须输入真实姓名")]
            [StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{1}个字符")]
            [DisplayName("真实姓名")]
            public string RealName_cn { get; set; }
            [DisplayName("是否公开")]
            public int? IsRealName_cn { get; set; }
            [DisplayName("外文的真实姓名")]         
            [StringLength(10, MinimumLength = 2, ErrorMessage = "{2}到{1}个字符")]
            public string RealName_en { get; set; }
            [DisplayName("是否公开")]
            public int? IsRealName_en { get; set; }
            [DisplayName("性别")]
            [Required(ErrorMessage = "必须选择性别")]
            public int? Gender { get; set; }
            [DisplayName("年")]
            public int? BirthYear { get; set; }
            [DisplayName("是否公开")]
            public int? IsBirthYear { get; set; }
            [DisplayName("月")]
            public int? BirthMonth { get; set; }
            [DisplayName("是否公开")]
            public int? IsBirthMonth { get; set; }
            [DisplayName("日")]
            public int? BirthDay { get; set; }
            [DisplayName("是否公开")]
            public int? IsBirthDay { get; set; }
            [DisplayName("国籍")]
            public int? Nationality { get; set; }
            [DisplayName("是否公开")]
            public int? IsNationality { get; set; }
            [DisplayName("所在街道")]
            public string Street { get; set; }
            [DisplayName("所在商圈")]
            public int? LocationID { get; set; }
            [DisplayName("所在城区")]
            public int? DistrictID { get; set; }
            [DisplayName("所在城市")]
            public int? CityID { get; set; }
            [DisplayName("所在国家")]
            public int? CountryID { get; set; }
        
            [DisplayName("所在省份")]           
            public int? ProvinceID { get; set; }

            public string MSN { get; set; }
            [DisplayName("是否公开")]
            public int? IsMsn { get; set; }

            public string QQ { get; set; }
            [DisplayName("是否公开")]
            public int? IsQQ { get; set; }
            [DisplayName("邮寄地址")]
            //[Required(ErrorMessage = "必须输入邮寄地址")]
            public string Postal { get; set; }
            [DisplayName("是否公开")]
            public int? IsPostal { get; set; }
            [DisplayName("邮政编码")]
            public string Zip { get; set; }
            [DisplayName("是否公开")]
            public int? IsZip { get; set; }
            [DisplayName(" 联系电话（座机）")]
            public string Tel { get; set; }
            [DisplayName("是否公开")]
            public int? IsTel { get; set; }
            [DisplayName("联系手机")]
            public string Mobile { get; set; }
            [DisplayName("是否公开")]
            public int? IsMobile { get; set; }
            [DisplayName("个人简历")] 
            public string Profile { get; set; }
            [DisplayName("微博地址")]
            public string Weibo { get; set; }
            [DisplayName("是否公开")]
            public int? IsWeibo { get; set; }
            [DisplayName("个人网址")]
            public string PersonalURL { get; set; }
            [DisplayName("是否公开")]
            public int? IsPersonalURL { get; set; }
            [DisplayName("开始习练年份")]
            public int? StartYear { get; set; }
            [DisplayName("是否有习练的瑜伽流派")] 
            public string IsUserYogaType { get; set; }
            [DisplayName("是否有习练瑜伽的地方")]  
            public string IsUserYogaPlace { get; set; } 
            [DisplayName("是否有启蒙老师")]  
            public bool? IsYogisMap { get; set; }
 
            [DisplayName("签名")]  
            public string GudWords { get; set; }
            [DisplayName("头像")]    
            public string DisplayImg { get; set; }
            [DisplayName("创建时间")]    
            public DateTime CreateTime { get; set; }
            [DisplayName("每周习练的次数")]    
            public string IsUserWeeknumber{get;set;}
            [DisplayName("习练的内容")]    
            public string sContent { get; set; }
            [DisplayName("理论知识")]    
            public string knowledge { get; set; }
            /// <summary>
            /// 瑜友级别，会员级别
            /// </summary>
            [DisplayName("会员级别")]
            public int Ulevel { get; set; }
            /// <summary>
            /// 瑜友积分，会员积分
            [DisplayName("积分")]
            public int Uscore { get; set; }
            /// <summary>
            /// 支付宝
            /// </summary>
            [DisplayName("支付宝")]
            public string AlipayUser { get; set; }
            /// <summary>
            /// 记录修改时间
            /// </summary>
            [DisplayName("记录修改时间")]
            public DateTime? UpgradeDate { get; set; }
            /// <summary>
            /// 瑜伽流派
            /// </summary> 
            // public List<ViewYogaDicItem> yogatype { get; set; }
            /// <summary>
            /// 习练的瑜伽流派
            /// </summary>
            [DisplayName("习练的瑜伽流派")]
            public string YogaTypeid { get; set; }
            [DisplayName("赞（推荐)")]
            public int? iZan { get; set; }
           
            [DisplayName("封面")]
            public string Covimg { get; set; }
            /// <summary>
            /// 0 没有删除，1 假删除不显示
            /// </summary>
            [DisplayName("删除状态")]
            public int? delState { get; set; }
            #endregion

        #region - 构造函数 -

        public ViewYogaUserDetail()
        {
        }
         
        #endregion

        #region - 方法 -
        public static YogaUserDetail ToEntity(ViewYogaUserDetail model)
        {
            YogaUserDetail item = new YogaUserDetail();

            item.ID = model.ID;
            item.UID = model.UID;
            item.RealName_cn = model.RealName_cn;
            item.IsRealName_cn = model.IsRealName_cn;
            item.RealName_en = model.RealName_en;
            item.IsRealName_en = model.IsRealName_en;
            item.Gender = model.Gender;
            item.BirthYear = model.BirthYear;
            item.IsBirthYear = model.IsBirthYear;
            item.BirthMonth = model.BirthMonth;
            item.IsBirthMonth = model.IsBirthMonth;
            item.BirthDay = model.BirthDay;
            item.IsBirthDay = model.IsBirthDay;
            item.Nationality = model.Nationality;
            item.IsNationality = model.IsNationality;
            item.Street = model.Street;
            item.LocationID = model.LocationID;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.MSN = model.MSN;
            item.IsMsn = model.IsMsn;
            item.QQ = model.QQ;
            item.IsQQ = model.IsQQ;
            item.Postal = model.Postal;
            item.IsPostal = model.IsPostal;
            item.Zip = model.Zip;
            item.IsZip = model.IsZip;
            item.Tel = model.Tel;
            item.IsTel = model.IsTel;
            item.Mobile = model.Mobile;
            item.IsMobile = model.IsMobile;
            item.Profile = model.Profile;
            item.Weibo = model.Weibo;
            item.IsWeibo = model.IsWeibo;
            item.PersonalURL = model.PersonalURL;
            item.IsPersonalURL = model.IsPersonalURL;
            item.StartYear = model.StartYear;
            item.IsUserYogaType = model.IsUserYogaType;
            item.IsUserYogaPlace = model.IsUserYogaPlace;
            item.IsYogisMap = model.IsYogisMap;
            item.GudWords = model.GudWords;
            item.DisplayImg = model.DisplayImg;
            item.CreateTime = model.CreateTime;
            item.Ulevel = model.Ulevel;
            item.Uscore = model.Uscore;
            item.AlipayUser = model.AlipayUser;
            item.UpgradeDate = model.UpgradeDate;
            item.YogaTypeid = model.YogaTypeid;
            item.iZan = model.iZan;
            item.IsUserWeeknumber = model.IsUserWeeknumber;
            item.knowledge = model.knowledge;
            item.sContent = model.sContent;
            item.Covimg = model.Covimg;
            item.delState = model.delState;
            return item;
        }

        public static ViewYogaUserDetail ToViewModel(YogaUserDetail model)
        {
            if (model == null) {

                return null;
            }

            ViewYogaUserDetail item = new ViewYogaUserDetail();
            item.ID = model.ID;
            item.UID = model.UID;
            item.RealName_cn = model.RealName_cn;
            item.IsRealName_cn = model.IsRealName_cn;
            item.RealName_en = model.RealName_en;
            item.IsRealName_en = model.IsRealName_en;
            item.Gender = model.Gender;
            item.BirthYear = model.BirthYear;
            item.IsBirthYear = model.IsBirthYear;
            item.BirthMonth = model.BirthMonth;
            item.IsBirthMonth = model.IsBirthMonth;
            item.BirthDay = model.BirthDay;
            item.IsBirthDay = model.IsBirthDay;
            item.Nationality = model.Nationality;
            item.IsNationality = model.IsNationality;
            item.Street = model.Street;
            item.LocationID = model.LocationID;
            item.DistrictID = model.DistrictID;
            item.CityID = model.CityID;
            item.ProvinceID = model.ProvinceID;
            item.CountryID = model.CountryID;
            item.MSN = model.MSN;
            item.IsMsn = model.IsMsn;
            item.QQ = model.QQ;
            item.IsQQ = model.IsQQ;
            item.Postal = model.Postal;
            item.IsPostal = model.IsPostal;
            item.Zip = model.Zip;
            item.IsZip = model.IsZip;
            item.Tel = model.Tel;
            item.IsTel = model.IsTel;
            item.Mobile = model.Mobile;
            item.IsMobile = model.IsMobile;
            item.Profile = model.Profile;
            item.Weibo = model.Weibo;
            item.IsWeibo = model.IsWeibo;
            item.PersonalURL = model.PersonalURL;
            item.IsPersonalURL = model.IsPersonalURL;
            item.StartYear = model.StartYear;
            item.IsUserYogaType = model.IsUserYogaType;
            item.IsUserYogaPlace = model.IsUserYogaPlace;
            item.IsYogisMap = model.IsYogisMap;
            item.GudWords = model.GudWords;
            item.DisplayImg = model.DisplayImg;
            item.CreateTime = model.CreateTime;
            item.Ulevel = model.Ulevel;
            item.Uscore = model.Uscore;
            item.AlipayUser = model.AlipayUser;
            item.UpgradeDate = model.UpgradeDate;
            item.YogaTypeid = model.YogaTypeid;
            item.iZan = model.iZan;
            item.IsUserWeeknumber = model.IsUserWeeknumber;
            item.knowledge = model.knowledge;
            item.sContent = model.sContent;
            item.Covimg = model.Covimg;
            item.delState = model.delState;
            return item;
        }

        #endregion
    }
}
