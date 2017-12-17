using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public  class ViewtLearing
    {
        #region 基本信息
        public int ID { get; set; }
        [DisplayName("登录人")]
        public string Uid { get; set; }
        [DisplayName("昵称")]
        public string NickName { get; set; }
        [DisplayName("图片")]
        public string sPic { get; set; }
        [DisplayName("标题")]
        public string sTitle { get; set; }
        [DisplayName("阅读量")]
        public int? iReadNums { get; set; }
        [DisplayName("评论量")]
        public int? iWritelogNums { get; set; }
        [DisplayName("推荐（赞）")]
        public int? iZanNums { get; set; }
        [DisplayName("时间")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 类型：YogaDicItem ID=2158
        /// </summary>
        [DisplayName("文章类型")]
        public int? iType { get; set; }
        [DisplayName("文章属性")]
        public string TypeValue { get; set; }
        [DisplayName("内容")]
        public string sContent { get; set; }
        /// <summary>
        /// 默认审核通过true , false 等待后台审核
        /// </summary>
        [DisplayName("是否审核")]
        public bool? ifexamine { get; set; }
        /// <summary>
        /// PC端 0 ； APP端 1
        /// </summary>
         [DisplayName("接收类型")]
        public int? UrlType { get; set; }
        #endregion

        public ViewtLearing()
        { }

        #region - 方法 -
        public static tLearing ToEntity(ViewtLearing model)
        {
            tLearing item = new tLearing();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sPic = model.sPic;
            item.CreateDate = model.CreateDate;
            item.sTitle = model.sTitle;
            item.iReadNums = model.iReadNums;
            item.iWritelogNums = model.iWritelogNums;
            item.iZanNums = model.iZanNums;
            item.iType = model.iType;
            item.sContent = model.sContent;
            item.ifexamine = model.ifexamine;
            item.UrlType = model.UrlType;
            return item;
        }

        public static ViewtLearing ToViewModel(tLearing model)
        {
            if (model == null)
            {

                return null;
            }

            ViewtLearing item = new ViewtLearing();

            item.ID = model.ID;
            item.Uid = model.Uid;
            item.sPic = model.sPic;
            item.CreateDate = model.CreateDate;
            item.sTitle = model.sTitle;
            item.iReadNums = model.iReadNums;
            item.iWritelogNums = model.iWritelogNums;
            item.iZanNums = model.iZanNums;
            item.iType = model.iType;
            item.sContent = model.sContent;
            item.ifexamine = model.ifexamine;
            item.UrlType = model.UrlType;

            return item;
        }

        #endregion
    }
}
