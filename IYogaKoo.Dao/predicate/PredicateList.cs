using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace IYogaKoo.Dao
{
    /// <summary>
    /// 功能：条件过滤类
    /// 作者：张占岭
    /// 日期：2012-6-7
    /// </summary>
    public class PredicateList<TEntity> : IEnumerable<Expression<Func<TEntity, bool>>> where TEntity : class
    {
        /// <summary>
        /// 条件过滤表达式
        /// </summary>
        List<Expression<Func<TEntity, bool>>> expressionList;
        public PredicateList()
        {
            expressionList = new List<Expression<Func<TEntity, bool>>>();
        }
        /// <summary>
        /// 将一个表达式树结果装入集合
        /// </summary>
        /// <param name="predicate"></param>
        public void Add(Expression<Func<TEntity, bool>> predicate)
        {
            expressionList.Add(predicate);
        }      
        /// <summary>
        ///将指定多元表达式树的逻辑运算结果装入到集合
        /// </summary>
        /// <param name="exprleft">左条件</param>
        /// <param name="exprright">右条件</param>
        public void Add(Expression<Func<TEntity, bool>> exprleft, Expression<Func<TEntity, bool>> exprright, LogicalOperator logicalOperator)
        {
            switch (logicalOperator)
            {
                case LogicalOperator.And:
                    expressionList.Add(exprleft.And(exprright));
                    break;
                case LogicalOperator.Or:
                    expressionList.Add(exprleft.Or(exprright));
                    break;
                case LogicalOperator.Not:
                    expressionList.Add(exprleft.Not());
                    break;
            }

        }
        /// <summary>
        /// 将指定多元表达式树的逻辑与运算得到的结果装入集合
        /// </summary>
        /// <param name="exprleft">左条件</param>
        /// <param name="exprright">右条件</param>
        public void Add(Expression<Func<TEntity, bool>> exprleft, Expression<Func<TEntity, bool>> exprright)
        {
            Add(exprleft, exprright, LogicalOperator.And);
        }
        /// <summary>
        /// 清空条件集合
        /// </summary>
        /// <returns></returns>
        public PredicateList<TEntity> Clear()
        {
            expressionList.Clear();
            return this;
        }
        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            return expressionList.GetEnumerator();
        }

        #endregion

        #region IEnumerable<Expression<Func<TEntity>>> 成员

        IEnumerator<Expression<Func<TEntity, bool>>> IEnumerable<Expression<Func<TEntity, bool>>>.GetEnumerator()
        {
            return expressionList.GetEnumerator();
        }

        #endregion

    }
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 逻辑与
        /// </summary>
        And,
        /// <summary>
        /// 逻辑或
        /// </summary>
        Or,
        /// <summary>
        /// 逻辑非
        /// </summary>
        Not
    }

    /// <summary>
    /// 序列依据
    /// </summary>
    public enum OrderByOperator
    {
        /// <summary>
        ///递增（正序）
        /// </summary>
        ASC,
        /// <summary>
        /// 递减（倒序）
        /// </summary>
        DESC,
    }


}
