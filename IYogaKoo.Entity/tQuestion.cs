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
    
    public partial class tQuestion
    {
        public int ID { get; set; }
        public string QuestionContent { get; set; }
        public int TitleID { get; set; }
        public Nullable<System.DateTime> QuestionTime { get; set; }
        public Nullable<int> State { get; set; }
        public int Uid { get; set; }
        public string UserName { get; set; }
        public string ContactInformation { get; set; }
        public Nullable<int> ReplyUid { get; set; }
        public string ReplyContent { get; set; }
        public Nullable<System.DateTime> ReplyTime { get; set; }
        public bool IsFAQ { get; set; }
        public Nullable<int> BeFrom { get; set; }
        public bool IsDelete { get; set; }
        public bool Hot { get; set; }
        public string Mark { get; set; }
        public string Mark2 { get; set; }
    }
}