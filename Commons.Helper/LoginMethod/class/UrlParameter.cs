using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helper.LoginMethod 
{
    /// <summary>
    /// url中用到的参数
    /// </summary>
    public class UrlParameter
    {
        public UrlParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
