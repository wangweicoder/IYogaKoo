using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewClassFile
    {
        #region 基本信息

        public int Id { get; set; }
        public string  Title { get; set; }
        public string Url { get; set; }
        public string  ExtendName { get; set; }
        public int Type { get; set; }
        public int ReportId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }


        #endregion

        #region - 构造函数 -

        public ViewClassFile()
        {
        }

        #endregion

        #region - 方法 -
        public static ClassFile ToEntity(ViewClassFile model)
        {
            ClassFile item = new ClassFile();

            item.Id = model.Id;
            item.Title = model.Title;
            item.Url = model.Url;
            item.Type = model.Type;
            item.ExtendName = model.ExtendName;
            item.ReportId = model.ReportId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;



            return item;
        }

        public static ViewClassFile ToViewModel(ClassFile model)
        {
            if (model == null)
            {

                return null;
            }

            ViewClassFile item = new ViewClassFile();

            item.Id = model.Id;
            item.Title = model.Title;
            item.Url = model.Url;
            item.Type = model.Type;
            item.ExtendName = model.ExtendName;
            item.ReportId = model.ReportId;
            item.IsDeleted = model.IsDeleted;
            item.CreateTime = model.CreateTime;



            return item;
        }

        #endregion
    }
}
