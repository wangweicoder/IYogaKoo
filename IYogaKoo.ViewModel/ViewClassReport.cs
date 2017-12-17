using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewClassReport
    {
        #region 基本信息

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime  CreateTime { get; set; }

        public List<ViewClassFile> Images { get; set; }

        public List<ViewClassFile> Videos { get; set; }

        #endregion

        #region - 构造函数 -

        public ViewClassReport()
        {
        }

        #endregion

        #region - 方法 -
        public static ClassReport ToEntity(ViewClassReport model)
        {
            ClassReport item = new ClassReport();

            item.Id = model.Id;
            item.Title = model.Title;
            item.Content = model.Content;
            item.UserId = model.UserId;
            item.ClassId = model.ClassId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;



            return item;
        }

        public static ViewClassReport ToViewModel(ClassReport model)
        {
            if (model == null)
            {

                return null;
            }

            ViewClassReport item = new ViewClassReport();

            item.Id = model.Id;
            item.Title = model.Title;
            item.Content = model.Content;
            item.UserId = model.UserId;
            item.ClassId = model.ClassId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;
            List<ViewClassFile> files = (from f in model.ClassFile select ViewClassFile.ToViewModel(f)).ToList();
            item.Images = new List<ViewClassFile>();
            item.Videos = new List<ViewClassFile>();
            foreach (var file in files)
            {
                if (file.Type == 11)
                    item.Images.Add(file);
                else if (file.Type == 12)
                    item.Videos.Add(file);
            }
            return item;
        }

        #endregion
    }
}
