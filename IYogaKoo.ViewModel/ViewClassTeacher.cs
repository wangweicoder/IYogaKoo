using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewClassTeacher
    {
        #region 基本信息

        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string YogaSystem { get; set; }
        public string Info { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime  CreateTime { get; set; }
        public int? Class_Id { get; set; }
        public int UserId { get; set; }
        public int? TeacherId { get; set; }
        public string Avatar { get; set; }


        #endregion

        #region - 构造函数 -

        public ViewClassTeacher()
        {
        }

        #endregion

        #region - 方法 -
        public static ClassTeacher ToEntity(ViewClassTeacher model)
        {
            ClassTeacher item = new ClassTeacher();

            item.Id = model.Id;
            item.Name = model.Name;
            item.Gender = model.Gender;
            item.Country = model.Country;
            item.YogaSystem = model.YogaSystem;
            item.Info = model.Info;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.Class_Id = model.Class_Id;
            item.UserId = model.UserId;
            item.TeacherId = model.TeacherId;
            item.Avatar = model.Avatar;

            return item;
        }

        public static ViewClassTeacher ToViewModel(ClassTeacher model)
        {
            if (model == null)
            {

                return null;
            }

            ViewClassTeacher item = new ViewClassTeacher();

            item.Id = model.Id;
            item.Name = model.Name;
            item.Gender = model.Gender;
            item.Country = model.Country;
            item.YogaSystem = model.YogaSystem;
            item.Info = model.Info;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            item.Class_Id = model.Class_Id;
            item.UserId = model.UserId;
            item.TeacherId = model.TeacherId;
            item.Avatar = model.Avatar;

            return item;
        }

        #endregion
    }
}
