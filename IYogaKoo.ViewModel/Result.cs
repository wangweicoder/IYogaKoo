using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class Result
    {
        public Result()
        {
            this.Code = 0;
            this.Message = string.Empty;
        }
        public Result(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public Result(int code, string message, object obj)
        {
            this.Code = code;
            this.Message = message;
            this.Obj = obj;
        }
        /// <summary>
        /// code=0 成功，其他数值表示失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// code 对应的错误信息或者成功提示
        /// </summary>
        public string Message { get; set; }
        public object Obj { get; set; }
    }
}
