using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewtQuestion
    {
        #region 基本信息

        public int ID { get; set; }

        [DisplayName("问题")]
        public string QuestionContent { get; set; }
        public int TitleID { get; set; }
        public Nullable<System.DateTime> QuestionTime { get; set; }
        public Nullable<int> State { get; set; }
        public int Uid { get; set; }
        public string UserName { get; set; }
        [DisplayName("联系方式")]
        public string ContactInformation { get; set; }
        [DisplayName("回答者Uid")]
        public Nullable<int> ReplyUid { get; set; }
        [DisplayName("答案")]
        public string ReplyContent { get; set; }
        public Nullable<System.DateTime> ReplyTime { get; set; }
        public bool IsFAQ { get; set; }
        public Nullable<int> BeFrom { get; set; }
        public bool IsDelete { get; set; }
        [DisplayName("热门")]
        public bool Hot { get; set; }
        public string Mark { get; set; }
        public string Mark2 { get; set; }

        public  YogaUser YogaUser { get; set; }
        public  YogaDicItem YogaDicItem { get; set; }

        #endregion

        public static tQuestion ToEntity(ViewtQuestion model)
        {
            tQuestion item = new tQuestion();
            item.BeFrom = model.BeFrom;
            item.ContactInformation = model.ContactInformation;
            item.ID = model.ID;
            item.IsFAQ = model.IsFAQ;
            item.QuestionContent = model.QuestionContent;
            item.QuestionTime = model.QuestionTime;
            item.ReplyContent = model.ReplyContent;
            item.ReplyTime = model.ReplyTime;
            item.ReplyUid = model.ReplyUid;
            item.State = model.State;
            item.TitleID = model.TitleID;
            item.Uid = model.Uid;
            item.UserName = model.UserName;
            item.IsDelete = model.IsDelete;
            item.Hot = model.Hot;
            item.Mark = model.Mark;
            item.Mark2 = model.Mark2;
            //item.YogaDicItem = model.YogaDicItem;
            //item.YogaUser = model.YogaUser;
            return item;
        }

        public static ViewtQuestion ToViewModel(tQuestion model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtQuestion item = new ViewtQuestion();
            item.BeFrom = model.BeFrom;
            item.ContactInformation = model.ContactInformation;
            item.ID = model.ID;
            item.IsFAQ = model.IsFAQ;
            item.QuestionContent = model.QuestionContent;
            item.QuestionTime = model.QuestionTime;
            item.ReplyContent = model.ReplyContent;
            item.ReplyTime = model.ReplyTime;
            item.ReplyUid = model.ReplyUid;
            item.State = model.State;
            item.TitleID = model.TitleID;
            item.Uid = model.Uid;
            item.UserName = model.UserName;
            item.IsDelete = model.IsDelete;
            item.Hot = model.Hot;
            item.Mark = model.Mark;
            item.Mark2 = model.Mark2;
            //item.YogaDicItem = model.YogaDicItem;
            //item.YogaUser = model.YogaUser;
            return item;
        }
    }
}
