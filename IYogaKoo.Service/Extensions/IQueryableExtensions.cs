using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Service
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="iquery"></param>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PageResult<TEntity> Page<TEntity, TKey>(this IQueryable<TEntity> iquery, Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, bool>> order, int pageIndex, int pageSize) where TEntity : class
        {
            PageResult<TEntity> pr;
            iquery = iquery.Where(predicate).OrderByDescending(order);
            try
            {
                pr = new PageResult<TEntity>(0, "", pageIndex, pageSize, iquery.Where(predicate).Count(), iquery.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
                return pr;
            }
            catch (Exception ex)
            {
                pr = new PageResult<TEntity>(1, "读取出错，" + ex.Message);
            }
            return pr;
        }
    }
}
