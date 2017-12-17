using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel.Commons.Enums;

namespace IYogaKoo.ViewModel
{
    public class ViewClass
    {
        #region 基本信息

        public int Id { get; set; }
        public int YogaTypeID { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Banner { get; set; }
        public string Start { get; set; }
        public string End
        {
            get
            {
                DateTime start = Convert.ToDateTime(Start);
                if (TimeUnit.天 == (TimeUnit)DurationUnit)
                    start = start.AddDays(Duration);
                else if (TimeUnit.小时 == (TimeUnit)DurationUnit)
                    start = start.AddHours(Duration);
                return start.ToString("yyyy-MM-dd hh:mm");
            }
        }
        public int Duration { get; set; }
        public int DurationUnit { get; set; }
        public string TimeUnitStr
        {
            get
            {
                return ((TimeUnit)DurationUnit).ToString();
            }
        }

        public int AreaID { get; set; }
        public string AreaStr { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsItem { get; set; }
        public int ItemClassID { get; set; }

        public int Max { get; set; }
        public int ClassType { get; set; }
        public int ClassStatus { get; set; }
        public string StatusStr
        {
            get
            {
                return ((ClassStatus)ClassStatus).ToString();
            }
        }
        public string NoPassMsg { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreateTime { get; set; }
        public string Name { get; set; }

        public string TopicIds { get; set; }
        public string Tags { get; set; }
        public string TeacherIds { get; set; }

        public int InterestCount { get; set; }
        public int ReportCount { get; set; }

        public ViewYogaUser User { get; set; }
        public ViewClassTeacher Creater { get; set; }
        public List<ViewClassReport> Reports { get; set; }
        public List<ViewInterestedClass> Interests { get; set; }
        public List<ViewClassTeacher> Teachers { get; set; }

        public UserListItem Poster { get; set; }
        public int OrderCount { get; set; }
        public int Amount { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int? iReadNums { get; set; }

        public int? iShareNums { get; set; }
        public DateTime CloseTime { get; set; }

        public DateTime EndTime { get; set; }

        //是否可编辑
        public bool IfEdit { get; set; }
        //是否可删除
        public bool IfDel { get; set; }
        //是否可撤销
        public bool IfReback { get; set; }
        public string MessageDes { get; set; }

        public string CenterID { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewClass()
        {
        }

        #endregion

        #region - 方法 -
        public static Class ToEntity(ViewClass model)
        {
            Class item = new Class();

            item.Id = model.Id;
            item.YogaTypeID = model.YogaTypeID;
            item.Summary = model.Summary;
            item.Content = model.Content;
            item.Banner = model.Banner;
            item.Start = model.Start;
            item.Duration = model.Duration;
            item.DurationUnit = model.DurationUnit;
            item.AreaID = model.AreaID;
            item.Address = model.Address;
            item.Price = model.Price;
            item.Discount = model.Discount;
            item.IsItem = model.IsItem;
            item.ItemClassID = model.ItemClassID;
            item.Max = model.Max;
            item.ClassType = model.ClassType;
            item.ClassStatus = model.ClassStatus;
            item.NoPassMsg = model.NoPassMsg;
            item.UserId = model.UserId;
            item.UpdateTime = model.UpdateTime;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.Name = model.Name;
            item.TopicIds = model.TopicIds;
            item.Tags = model.Tags;
            item.iReadNums = model.iReadNums;
            item.iShareNums = model.iShareNums;
            item.CloseTime = model.CloseTime;
            item.EndTime = model.EndTime;
            item.MessageDes = model.MessageDes;
            item.CenterID = model.CenterID;
            return item;
        }

        public static ViewClass ToViewModel(Class model)
        {
            if (model == null)
            {

                return null;
            }

            ViewClass item = new ViewClass();


            item.Id = model.Id;
            item.YogaTypeID = model.YogaTypeID;
            item.Summary = model.Summary;
            item.Content = model.Content;
            item.Banner = model.Banner;
            item.Start = model.Start;
            item.Duration = model.Duration;
            item.DurationUnit = model.DurationUnit;
            item.AreaID = model.AreaID;
            item.Address = model.Address;
            item.Price = model.Price;
            item.Discount = model.Discount;
            item.IsItem = model.IsItem;
            item.ItemClassID = model.ItemClassID;
            item.Max = model.Max;
            item.ClassType = model.ClassType;
            item.ClassStatus = model.ClassStatus;
            item.NoPassMsg = model.NoPassMsg;
            item.UserId = model.UserId;
            item.UpdateTime = model.UpdateTime;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.Name = model.Name;
            item.TopicIds = model.TopicIds;
            item.Tags = model.Tags;
            item.InterestCount = model.InterestedClass.Count;
            item.User = ViewYogaUser.ToViewModel(model.User);
            item.ReportCount = model.ClassReport.Count;
            item.Reports = (from r in model.ClassReport select ViewClassReport.ToViewModel(r)).ToList();
            item.Teachers = (from r in model.ClassTeacher select ViewClassTeacher.ToViewModel(r)).ToList();
            item.iShareNums = model.iShareNums;
            item.iReadNums = model.iReadNums;
            item.CloseTime = model.CloseTime;
            item.EndTime = model.EndTime;
            item.MessageDes = model.MessageDes;
            item.CenterID = model.CenterID;
            return item;
        }

        #endregion
    }

    public class UserListItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        private string _avatar;
        public string Avatar
        {
            get
            {
                if (string.IsNullOrEmpty(_avatar))
                    return "/Content/Front/images/default_avatar.png";
                else
                    return _avatar.Split(';')[0].Split('$')[0];
            }
            set { _avatar = value; }
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 导师级别，-99表示不是导师
        /// </summary>
        public int YogisLevel { get; set; }
    }
}
