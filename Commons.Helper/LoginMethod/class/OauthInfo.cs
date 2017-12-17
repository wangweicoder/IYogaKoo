using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helper.LoginMethod 
{   
    /// <summary>
    /// 第三方登录时，从第三方读取的信息
    /// </summary>
   public class OauthInfo
    {
        public string NickName { get; set; }
        public string Place { get; set; }
        public string Avatar { get; set; }
        public string AuthCode { get; set; }
        public string Email { get; set; }
        public string ChatBack { get; set; }
        public LoginType LoginType { get; set; }
    }
}
