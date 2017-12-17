using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        T Get(object id);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>ID</returns>
        T Add(T entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">ID</param>
        void Delete(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">ID</param>
        void Delete(object id);

        IEnumerable<T> Get(
           Func<T, bool> exp);

        int Save();

    }
}
