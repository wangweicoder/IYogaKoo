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
    
    public partial class YogaAuthmenu
    {
        public int Id { get; set; }
        public Nullable<int> Mid { get; set; }
        public Nullable<int> Aid { get; set; }
    
        public virtual YogaAuthgroup YogaAuthgroup { get; set; }
        public virtual YogaMenus YogaMenus { get; set; }
    }
}
