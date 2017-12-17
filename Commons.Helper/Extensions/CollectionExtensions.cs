using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// 把集合中对象的某属性拼接成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">待处理集合</param>
        /// <param name="selector">属性选择委托</param>
        /// <param name="separetor">拼接分隔符</param>
        /// <returns></returns>
        public static string ToString<T>(this ICollection<T> collection, Func<T, string> selector, string separetor = ",")
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (T item in collection)
            {
                if (isFirst)
                {
                    sb.Append(selector(item));
                    isFirst = false;
                }
                else
                {
                    sb.Append(separetor + selector(item));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 把集合中的简单类型拼接成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">待处理集合</param>
        /// <param name="separetor">拼接分隔符</param>
        /// <returns></returns>
        public static string ToString<T>(this ICollection<T> collection, string separetor = ",")
        {
            return collection.ToString<T>(c => c.ToString(), separetor);
        }
    }
}
