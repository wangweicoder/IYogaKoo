using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helper
{
    /// <summary>
    /// 获取登录/注册信息
    /// </summary>
    public class BasicInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 用户Email
        /// </summary>
        public string UEmail { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Uphone { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LastIP { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int? LoginTimes { get; set; }
        /// <summary>
        /// 使用类型 0 习练者 1 导师 
        /// </summary>
        public int? UserType { get; set; }
        /// <summary>
        /// 登录类型
        /// </summary>
        public int? LoginType { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ValExpire { get; set; }
        public string ValCode { get; set; }

        public string WechatAuthCode { get; set; }
        public string WechatBack { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime LastDate { get; set; }
        public int UStatus { get; set; }
        /// <summary>
        /// 头像（第三方获取）
        /// </summary>
        public string Avatar { get; set; }

        public string Url { get; set; }
    }
}
