using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewClassTopic
    {
        #region 基本信息

        public int Id { get; set; }
        public int ClassID { get; set; }
        public int TopicID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime  CreateTime { get; set; }


        #endregion

        #region - 构造函数 -

        public ViewClassTopic()
        {
        }

        #endregion

        #region - 方法 -
        public static ClassTopic ToEntity(ViewClassTopic model)
        {
            ClassTopic item = new ClassTopic();

            item.Id = model.Id;
            item.ClassID = model.ClassID;
            item.TopicID = model.TopicID;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;



            return item;
        }

        public static ViewClassTopic ToViewModel(ClassTopic model)
        {
            if (model == null)
            {

                return null;
            }

            ViewClassTopic item = new ViewClassTopic();


            item.Id = model.Id;
            item.ClassID = model.ClassID;
            item.TopicID = model.TopicID;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;


            return item;
        }

        #endregion
    }
}
