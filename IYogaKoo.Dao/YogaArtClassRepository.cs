using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogaArtClassRepository : Repository<YogaArtClass>, IYogaArtClassRepository
    {
        public List<YogaArtClass> GetYogaArtClassPageListAll()
        { 
            return dbSet.OrderByDescending(a => a.CreateTime).ToList();
        }
        public List<YogaArtClass> GetYogaArtClassPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();
             
            return dbSet.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogaArtClass> GetYogaArtClassPageList(int ParentID)
        { 
            return dbSet.Where(x => x.ParentID == ParentID).ToList();
        }
        /// <summary>
        /// 根据主键获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaArtClass> GetYogaArtClassUid(int id)
        {
            return dbSet.Where(a => a.ID == id).ToList();
        }
        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaArtClass GetYogaArtClassById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }
        /// <summary>
        /// 根据文章名称获取信息
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public YogaArtClass GetYogaArtClassByClassName(string ClassName)
        {
            return dbSet.Where(a => a.ClassName == ClassName).FirstOrDefault();
        }
        public int updateEntity(YogaArtClass model)
        {

            var entity = dbSet.Find(model.ID);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }
    }
}
