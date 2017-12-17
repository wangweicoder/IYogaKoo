using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 枚举转换成SelectListItem集合
        /// </summary>
        /// <param name="value"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToList(this Enum value)
        {
            return (from int val in Enum.GetValues(value.GetType()) select new SelectListItem() { Text = Enum.GetName(value.GetType(), val), Value = val.ToString(), Selected = Enum.GetName(value.GetType(), val) == Enum.GetName(value.GetType(), value) }).ToList();
        }
    }
}