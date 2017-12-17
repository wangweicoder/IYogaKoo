using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace IYogaKoo.ViewModel
{
    public class ViewYogaUser
    {
        /// <summary>
        /// 获取或设置 登录成功后返回地址
        /// </summary>
       // public string ReturnUrl { get; set; }

        #region 基本信息
        /// <summary>
        /// 登录次数 cookie 记录
        /// </summary> 
        public int iChi { get; set; }
        [DisplayName("会员编号")]        
        public int Uid { get; set; }
        [DisplayName("邮箱")]        
        public string UEmail { get; set; }       
        [DisplayName("手机号")]
        public string Uphone { get; set; }       
        [DisplayName("密码")]        
        public string Pwd { get; set; }
        [DisplayName("呢称")]       
        public string NickName { get; set; }
        [DisplayName("注册时间")]       
        public DateTime? RegDate { get; set; }
         [DisplayName("瑜友状态")]      
        public int? UStatus { get; set; }
         [DisplayName("是否是审核员")]      
        public bool IsAssessor { get; set; }
          [DisplayName("是否网站工作人员")]      
        public bool IsWebworkers { get; set; }
          [DisplayName("最近登录时间")]      
        public DateTime   LastDate { get; set; }
         [DisplayName("最近登录IP")]       
        public string LastIP { get; set; }
         [DisplayName("登录次数")]       
        public int? LoginTimes { get; set; }
        [DisplayName("瑜友类别")]       
        public int? UserType { get; set; }
          [DisplayName("登录类型")]    
        public int? LoginType { get; set; }
        [DisplayName("sina登录编号")]    
        public string SinaAuthCode { get; set; }
        [DisplayName("sina授权编号")]    
        public string SinaBack { get; set; }
        [DisplayName("qq登录编号")]    
        public string QQAuthCode { get; set; }
        [DisplayName("qq授权编号")]    
        public string QQBack { get; set; }
        [DisplayName("微信登录编号")]    
        public string WechatAuthCode { get; set; }
        [DisplayName("微信授权编号")]    
        public string WechatBack { get; set; }
        [DisplayName("验证码，邮箱验证和找回密码的时候使用")] 
        public string ValCode { get; set; }
        [DisplayName("验证码的过期时间")]    
        public DateTime? ValExpire { get; set; }

        public ICollection<YogisModels> YogisModels { get; set; }
        /// <summary>
        /// 1 是 0 否
        /// </summary>
         [DisplayName("删除状态")]    
        public int? delState { get; set; }
        /// <summary>
         /// App端：自定义
        /// </summary>
         public string salt { get; set; }
         /// <summary>
         /// App端用户表主键
         /// </summary>
         public string objectId { get; set; }
        /// <summary>
         /// 录入类型0 :pc; 1:app
        /// </summary>
         [DisplayName("录入类型")]    
         public int? InputType { get; set; }
        #endregion

        #region - 构造函数 -

        public ViewYogaUser()
        {
        }
         
        #endregion

        #region - 方法 -
        public static YogaUser ToEntity(ViewYogaUser model)
        { 
            YogaUser item = new YogaUser();
            item.Uid = model.Uid;
            item.UEmail = model.UEmail;
            item.Uphone = model.Uphone;
            item.Pwd = model.Pwd;
            item.NickName = model.NickName;
            item.RegDate = model.RegDate;
            item.UStatus = model.UStatus;
            item.IsAssessor = model.IsAssessor;

            item.IsWebworkers = model.IsWebworkers;
            item.LastDate = model.LastDate;
            item.LastIP = model.LastIP;
            item.LoginTimes = model.LoginTimes;
            item.UserType = model.UserType;
            item.LoginType = model.LoginType;
            item.SinaAuthCode = model.SinaAuthCode;
            item.SinaBack = model.SinaBack;

            item.QQAuthCode = model.QQAuthCode;
            item.QQBack = model.QQBack;
            item.WechatAuthCode = model.WechatAuthCode;
            item.WechatBack = model.WechatBack;
            item.ValCode = model.ValCode;
            item.ValExpire = model.ValExpire;
            item.YogisModels = model.YogisModels;
            item.delState = model.delState;
            item.objectId = model.objectId;
            item.salt = model.salt;
            item.InputType = model.InputType;
            return item;
        }

        public static ViewYogaUser ToViewModel(YogaUser model)
        {
            if (model == null) {

                return null;
            }

            ViewYogaUser item = new ViewYogaUser();
            item.Uid = model.Uid;
            item.UEmail = model.UEmail;
            item.Uphone = model.Uphone;
            item.Pwd = model.Pwd;
            item.NickName = model.NickName;
            item.RegDate = model.RegDate;
            item.UStatus = model.UStatus;
            item.IsAssessor = model.IsAssessor.Value;

            item.IsWebworkers = model.IsWebworkers.Value;
            item.LastDate = model.LastDate.Value;
            item.LastIP = model.LastIP;
            item.LoginTimes = model.LoginTimes;
            item.UserType = model.UserType;
            item.LoginType = model.LoginType;
            item.SinaAuthCode = model.SinaAuthCode == null ? "" : model.SinaAuthCode;
            item.SinaBack = model.SinaBack== null ? "" : model.SinaBack;

            item.QQAuthCode = model.QQAuthCode == null ? "" : model.QQAuthCode;
            item.QQBack = model.QQBack == null ? "" : model.QQBack;
            item.WechatAuthCode = model.WechatAuthCode == null ? "" : model.WechatAuthCode;
            item.WechatBack = model.WechatBack == null ? "" : model.WechatBack;
            item.ValCode = model.ValCode == null ? "" : model.ValCode;
            item.ValExpire = model.ValExpire;
            item.YogisModels = model.YogisModels;
            item.delState = model.delState;
            item.objectId = model.objectId;
            item.salt = model.salt;
            item.InputType = model.InputType;
            return item;
        }

        #endregion
    }
}
