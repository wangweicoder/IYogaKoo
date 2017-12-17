using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class ViewYogaPicture
    {
        #region 基本信息

        public int Pid { get; set; }
        /// <summary>
        /// 2 相册，5 会馆 3 日志PC端 ,4 日志APP端,6 活动报道图片
        /// </summary>
        public int? PictureType { get; set; }
        [DisplayName("用户名")]
        public int Uid { get; set; }
        public int? AlbumId { get; set; }
        public int? EvaluateId { get; set; }
        public int? Comid { get; set; }
        public string PictureName { get; set; }
         [DisplayName("描述")]
        public string PictureContent { get; set; }
        [DisplayName("图片")]
        public string PictureOriginal { get; set; }
        public string PictureLarge { get; set; }
        public string PictureMiddle { get; set; }
        public string PictureSmall { get; set; }
        public string PircureSize { get; set; }
        public int? CommentCount { get; set; }

        public int? LikeCount { get; set; }
        public int? NotLikeCount { get; set; }
        public int? CommentLimite { get; set; }
        public DateTime? CreateTime { get; set; }
         [DisplayName("创建人")]
        public int? CreateUser { get; set; }
        public DateTime? LastChangeTime { get; set; }
        public int? HitNum { get; set; }
        [DisplayName("审核通过")]
        public int? iAudio { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewYogaPicture()
        {
        }

        #endregion

        #region - 方法 -
        public static YogaPicture ToEntity(ViewYogaPicture model)
        {
            YogaPicture item = new YogaPicture();

            item.Pid = model.Pid;
            item.PictureType = model.PictureType;
            item.Uid = model.Uid;
            item.AlbumId = model.AlbumId;
            item.EvaluateId = model.EvaluateId;
            item.Comid = model.Comid;
            item.PictureName = model.PictureName;
            item.PictureContent = model.PictureContent;
            item.PictureOriginal = model.PictureOriginal;
            item.PictureLarge = model.PictureLarge;
            item.PictureMiddle = model.PictureMiddle;
            item.PictureSmall = model.PictureSmall;
            item.PircureSize = model.PircureSize;
            item.CommentCount = model.CommentCount;
            item.LikeCount = model.LikeCount;
            item.NotLikeCount = model.NotLikeCount;
            item.CommentLimite = model.CommentLimite;
            item.CreateTime = model.CreateTime;
            item.CreateUser = model.CreateUser;
            item.LastChangeTime = model.LastChangeTime;
            item.HitNum = model.HitNum;
            item.iAudio = model.iAudio;
            return item;
        }

        public static ViewYogaPicture ToViewModel(YogaPicture model)
        {
            if (model == null)
            {

                return null;
            }

            ViewYogaPicture item = new ViewYogaPicture();
            item.Pid = model.Pid;
            item.PictureType = model.PictureType;
            item.Uid = model.Uid;
            item.AlbumId = model.AlbumId;
            item.EvaluateId = model.EvaluateId;
            item.Comid = model.Comid;
            item.PictureName = model.PictureName;
            item.PictureContent = model.PictureContent;
            item.PictureOriginal = model.PictureOriginal;
            item.PictureLarge = model.PictureLarge;
            item.PictureMiddle = model.PictureMiddle;
            item.PictureSmall = model.PictureSmall;
            item.PircureSize = model.PircureSize;
            item.CommentCount = model.CommentCount;
            item.LikeCount = model.LikeCount;
            item.NotLikeCount = model.NotLikeCount;
            item.CommentLimite = model.CommentLimite;
            item.CreateTime = model.CreateTime;
            item.CreateUser = model.CreateUser;
            item.LastChangeTime = model.LastChangeTime;
            item.HitNum = model.HitNum;
            item.iAudio = model.iAudio;

            return item;
        }

        #endregion
    }
}
