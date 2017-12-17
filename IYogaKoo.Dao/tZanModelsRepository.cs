using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class tZanModelsRepository : Repository<tZanModels>, ItZanModelsRepository
    {
        public List<tZanModels> GetByFromUidList(int ToUid, int loginType, out int count)
        {
            IQueryable<tZanModels> linq = dbSet.Where(a => a.ToUid == ToUid && a.loginType == loginType);
            count=linq.Count();
            return linq.ToList();
        }

        /// <summary>
        /// 被赞的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tZanModels GetByiToType(int iToType)
        {
            return dbSet.Where(a => a.iToType == iToType).FirstOrDefault();
        }
        /// <summary>
        /// 获取登录用户被赞的数据
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public List<tZanModels> GetToUidList(int Uid)
        {
            return dbSet.Where(a => a.ToUid == Uid).OrderByDescending(a=>a.CreateDate).ToList();
        }
        /// <summary>
        /// 是否已经赞过
        /// </summary>
        /// <param name="iFromUid"></param>
        /// <param name="iToUid"></param>
        /// <returns></returns>
        public tZanModels GetExists(int iFromUid, int iToUid, int iType, int iToType)
        {
            return dbSet.Where(a => a.iFromUid == iFromUid && a.iToUid == iToUid && a.iType == iType && a.iToType == iToType).FirstOrDefault();
        }
        /// <summary>
        /// 赞量
        /// </summary>
        /// <param name="toUid">被赞人</param>
        /// <param name="iToType">被赞类型</param>
        /// <returns></returns>
         public int ZanCount(int toUid, int iToType)
        { 
            return dbSet.Where(a => a.iToUid == toUid && a.iToType==iToType).Count();
        }
        public List<tZanModels> GettZanModelsPageListAll()
        {
            return dbSet.OrderByDescending(a => a.CreateDate).ToList();
        }
        public List<tZanModels> GettZanModelsPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<tZanModels> GettZanModelsPageList(int ParentID)
        {
            return dbSet.Where(x => x.ID == ParentID).ToList();
        }
        /// <summary>
        /// 根据主键获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tZanModels> GettZanModelsUid(int id)
        {
            return dbSet.Where(a => a.ID == id).ToList();
        }
        /// <summary>
        /// 根据导师编号获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tZanModels> GettZanUid(int uid)
        {
            return dbSet.Where(a => a.iToUid == uid).ToList();
        }
        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tZanModels GettZanModelsById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }
          
        /// <summary>
        /// 根据文章名称获取信息
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public tZanModels GettZanModelsByClassName(string ClassName)
        {
            return dbSet.FirstOrDefault();
        }
        public tZanModels GetByFromToUid(int toid, int fromid, int? iToType)
        {
            IQueryable<tZanModels> linq = null;
            if (iToType != 0)
            {
                linq = dbSet.Where(a => a.iToType == iToType);
            }
            return linq.Where(a => a.iToUid == toid && a.iFromUid == fromid).FirstOrDefault();
        }
        public int Count(int toid, int fromid,int? iToType)
        {
            IQueryable<tZanModels> linq = null;
            if (iToType != 0)
            {
                linq = dbSet.Where(a => a.iToType == iToType);
            }
            if (fromid != 0)
            {
                linq = dbSet.Where(a => a.iFromUid == fromid);
            }
            return linq.Where(a => a.iToUid == toid).Count();
        }
        public int updateEntity(tZanModels model)
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
