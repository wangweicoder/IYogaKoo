using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewYogaArticle
    {
        #region 基本信息
        [DisplayName("编号")]
        public int ID { get; set; }
        [DisplayName("类别编号")]
        public int ClassID { get; set; }
        [DisplayName("标题")]
        public string ArticleTitle { get; set; }
        [DisplayName("内容")]
        public string ArticleCon { get; set; }

        public int CheckID { get; set; }
        public int IsCheck { get; set; }
        public int? Uid { get; set; }
        [DisplayName("作者")]
        public string Author { get; set; }
        [DisplayName("创建人")]
        public string Creator { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("是否删除")]
        public int IsDelete { get; set; }
        public int IsPicture { get; set; }
        [DisplayName("图片")]
        public string PicPath { get; set; }
        
        #endregion

        #region - 构造函数 -

        public ViewYogaArticle()
        {
        }

        #endregion

        #region - 方法 -
        public static YogaArticle ToEntity(ViewYogaArticle model)
        {
            YogaArticle item = new YogaArticle();

            item.ID = model.ID;
            item.ClassID = model.ClassID;
            item.ArticleTitle = model.ArticleTitle;
            item.ArticleCon = model.ArticleCon;
            item.CheckID = model.CheckID;
            item.IsCheck = model.IsCheck;
            item.Uid = model.Uid;
            item.Author = model.Author;
            item.Creator = model.Creator;
            item.CreateTime = model.CreateTime;
            item.IsDelete = model.IsDelete;
            item.IsPicture = model.IsPicture;
            item.PicPath = model.PicPath;


            return item;
        }

        public static ViewYogaArticle ToViewModel(YogaArticle model)
        {
            if (model == null)
            {

                return null;
            }

            ViewYogaArticle item = new ViewYogaArticle();
            item.ID = model.ID;
            item.ClassID = model.ClassID;
            item.ArticleTitle = model.ArticleTitle;
            item.ArticleCon = model.ArticleCon;
            item.CheckID = model.CheckID;
            item.IsCheck = model.IsCheck;
            item.Uid = model.Uid;
            item.Author = model.Author;
            item.Creator = model.Creator;
            item.CreateTime = model.CreateTime;
            item.IsDelete = model.IsDelete;
            item.IsPicture = model.IsPicture;
            item.PicPath = model.PicPath;

            return item;
        }

        #endregion
    }
}
