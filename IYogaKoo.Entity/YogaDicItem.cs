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
    
    public partial class YogaDicItem
    {
        public int ID { get; set; }
        public int DicId { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public int SortId { get; set; }
        public int IsUse { get; set; }
        public int IsDelete { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}