using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel.Commons.Enums;
using IYogaKoo.ViewModel.Commons.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogaPictureRepository : Repository<YogaPicture>, IYogaPictureRepository
    {
        /// <summary>
        /// 是否存在该信息(图片）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaPicture ExistsPictureOriginal(int Uid, string PictureOriginal)
        { 
           return dbSet.Where(a => a.Uid == Uid && a.PictureOriginal == PictureOriginal).FirstOrDefault();
             
        }
        /// <summary>
        /// 前台获取相册
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FName">日志相册/活动相册</param>
        /// <returns></returns>
        public List<YogaPicture> GetPiclist(int id, string FName)
        {
            return dbSet.Where(a => a.Uid == id && a.PictureName == FName && a.iAudio==1).ToList();
        }
        /// <summary>
        /// 获取审核图片
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="createTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<YogaPicture> GetBackPageList(string Uid, DateTime? createTime, out int count)
        {
            string dtTime = "";
            if (createTime.ToString().IndexOf('/') != -1)
            {
                //有/
                dtTime = createTime.ToString().Replace('/', '-');
            }
            else dtTime = createTime.ToString();

            string sql = @"SELECT  *  FROM [iyogakoodb].[dbo].[YogaPicture]   where   [Uid]=" + Uid + "   and CreateTime='" + dtTime + "'";
            DataTable dt = SQLHelper.ExecuteDataTable(sql, null);
            List<YogaPicture> list = DataTableHelper.TableToEntity<YogaPicture>(dt);
            count = list.Count();
            return list;
            
        }
        public List<YogaPicture> GetYogaPicturePageList(int Nums)
        {
            if (Nums != 0)
            {
                return dbSet.OrderByDescending(a => a.CreateTime).Take(Nums).ToList();
            }
            else
            {
                 return dbSet.OrderByDescending(a => a.CreateTime).ToList(); 
            }
        }
       
        /// <summary>
        /// 循环删除冗余的图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaPicture> GetListWhere(int uid,string fileName)
        { 
            return dbSet.Where(a => a.Uid == uid && a.PictureName == fileName).ToList();
        }
        /// <summary>
        /// 根据Uid获取YogaPicture 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaPicture> GetUidList(int id)
        {
            return dbSet.Where(a => a.Uid == id && a.iAudio==1).ToList();
        }
        /// <summary>
        /// 后台根据Uid获取list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaPicture> GetBackUidList(int id)
        {
            return dbSet.Where(a => a.Uid == id ).ToList();
        }
        public List<YogaPicture> GetBackUidList(int id,string FName)
        {
            return dbSet.Where(a => a.Uid == id && a.PictureName == FName).ToList();
        }
        public List<YogaPicture> GetBackUidList(int id, DateTime create)
        {
            return dbSet.Where(a => a.Uid == id && a.CreateTime == create).ToList();
        }
        public List<YogaPicture> GetBackUidList(int id, string FName, DateTime create)
        { 
            //return dbSet.Where(a => a.Uid == id && a.PictureName == FName && a.CreateTime == create).ToList();
             
            string sqlStr = @"select *   FROM [iyogakoodb].[dbo].[YogaPicture] where  CreateTime in(
SELECT max(CreateTime)  FROM [iyogakoodb].[dbo].[YogaPicture] where PictureName='" + FName + "' and [Uid]=" + id + "   )";
            DataTable dt = SQLHelper.ExecuteDataTable(sqlStr, null);
            List<YogaPicture> list = DataTableHelper.TableToEntity<YogaPicture>(dt);
            return list;
        }
        public List<YogaPicture> GetYogaPicturePageList(int page, int pagesize, out int count)
        {
            count = dbSet.Where(x=>x.PictureType==2).Count();

            return dbSet.Where(x => x.PictureType == 2).OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogaPicture> GetYogaPicturePageList(int uid,int page, int pagesize, out int count)
        {
            count = dbSet.Where(a => a.Uid == uid && a.iAudio == 1).Count();

            return dbSet.Where(a => a.Uid == uid && a.iAudio == 1).OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
         
        /// <summary>
        /// 根据Uid获取YogaPicture 信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaPicture GetYogaPictureById(int id)
        {
            return dbSet.Where(a => a.Uid == id).FirstOrDefault();
        }
        public YogaPicture GetYogaPictureById(int id,string FName)
        {
            return dbSet.Where(a => a.Uid == id && a.PictureName == FName).FirstOrDefault();
        }
        public YogaPicture GetYogaPictureByCreateTime(int id, DateTime Create)
        {
            return dbSet.Where(a => a.Uid == id && a.CreateTime==Create).FirstOrDefault();
        }
        public int updateEntity(YogaPicture model)
        {

            var entity = dbSet.Find(model.Pid);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        } 

        public List<YogaPicture> GetListByType(int uid, int typeid)
        {
            return dbSet.Where(a => a.Uid == uid && a.PictureType == typeid).ToList();
        }
    }
}
