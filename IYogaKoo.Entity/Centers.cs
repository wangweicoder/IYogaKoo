//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace IYogaKoo.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Centers
    {
        public int CenterId { get; set; }
        public string Uid { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpgradeDate { get; set; }
        public string CenterProfile { get; set; }
        public Nullable<System.DateTime> OpenTime { get; set; }
        public Nullable<System.DateTime> CloseTime { get; set; }
        public string CenterType { get; set; }
        public string CenterBanner { get; set; }
        public string CenterIntroduction { get; set; }
        public string CenterPortrait { get; set; }
        public string YogaTypeid { get; set; }
        public Nullable<int> CenterState { get; set; }
        public Nullable<int> CenterSource { get; set; }
    }
}
