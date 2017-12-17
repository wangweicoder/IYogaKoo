using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串数组转成成int数组
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string[] strs)
        {
            int[] result = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                result[i] = int.Parse(strs[i]);
            }
            return result;
        }
    }
}
