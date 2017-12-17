using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo
{
    public class SendEmailModel 
    {
        [Required(ErrorMessage = "请输入邮件地址"), ValidateEmail()]
        public string To { get; set; }

        [Required(ErrorMessage = "请输入邮件标题")]
        public string Title { get; set; }
        [Required(ErrorMessage = "请输入邮件内容")]
        public string Body { get; set; }

        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
