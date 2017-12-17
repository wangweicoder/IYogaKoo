using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IYogaKoo
{
    //验证邮箱
    public class ValidateEmail : ValidationAttribute
    {
        public ValidateEmail()
        {
            ErrorMessage = "{0}必须是有效的邮件地址";
        }
        public override bool IsValid(object value)
        {
            string stringValue = value as string;

            if (stringValue != null)

                return stringValue.Contains("@");

            return true;

        }
    }
}
