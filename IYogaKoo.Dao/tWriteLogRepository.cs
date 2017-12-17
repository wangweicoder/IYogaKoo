using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class tWriteLogRepository : Repository<tWriteLog>, ItWriteLogRepository
    {
        //start 后台调用
        public List<tWriteLog> GettWriteLogPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            IQueryable<tWriteLog> linq = dbSet.OrderByDescending(a => a.CreateDate);
            if (uid != 0)
            {
                linq = linq.Where(a => a.Uid == uid);
            }
            if (!string.IsNullOrEmpty(sTitle))
            {
                linq = linq.Where(a => a.sTitle.Contains(sTitle));
            }
            if (date != null)
            {
                linq = linq.Where(a => a.CreateDate >= date);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 待审核推送日志 没有用到该方法，可以删除/修改
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sTitle"></param>
        /// <param name="date"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tWriteLog> BackGetPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            IQueryable<tWriteLog> linq = dbSet.Where(a => a.ifpush == true).OrderByDescending(a => a.CreateDate);
            if (uid != 0)
            {
                linq = linq.Where(a => a.Uid == uid);
            }
            if (!string.IsNullOrEmpty(sTitle))
            {
                linq = linq.Where(a => a.sTitle.Contains(sTitle));
            }
            if (date != null)
            {
                linq = linq.Where(a => a.CreateDate >= date);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        //end

        /// <summary>
        /// 习练笔记APP/日志图片
        /// </summary>
        /// <param name="ValueType">0 pc ; 1 APP</param>
        /// <returns></returns>
        public List<tWriteLog> GettWriteLogImg(int Uid,int ValueType)
        { 
            return dbSet.Where(a =>a.Uid==Uid && a.ValueType == ValueType).ToList();
        }

        public List<tWriteLog> GettWriteLogPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<tWriteLog> GettWriteLogPageList(int uid, int page, int pagesize, out int count)
        {
            IQueryable<tWriteLog> linq = dbSet.OrderByDescending(a => a.CreateDate);
             
            count = linq.Where(x=>x.Uid==uid).Count();// dbSet.Count();

            return linq.Where(x => x.Uid == uid).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        //前端专页调用
        public List<tWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int? day,int page, int pagesize, out int count)
        {
            IQueryable<tWriteLog> linq = dbSet.OrderBy(a => a.CreateDate);
             
            count = linq.Where(x => x.Uid == uid && (x.CreateDate.Value.Year == Year && x.CreateDate.Value.Month == Month && x.CreateDate.Value.Day == day)).Count();// dbSet.Count();
             
            return linq.Where(x => x.Uid == uid && (x.CreateDate.Value.Year == Year && x.CreateDate.Value.Month == Month && x.CreateDate.Value.Day == day)).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        public List<tWriteLog> GettWriteLogPageList(int uid, int Year, int Month)
        {
            IQueryable<tWriteLog> linq = dbSet.OrderBy(a => a.CreateDate);
              
            return linq.Where(x => x.Uid == uid && (x.CreateDate.Value.Year == Year && x.CreateDate.Value.Month == Month)).ToList();
        }
        public List<tWriteLog> GettWriteLogQuiltUidList(int id)
        {
            return dbSet.Where(a => a.Uid == id ).ToList();
        }
        public tWriteLog GettWriteLogById(int id)
        {
            return dbSet.Where(a => a.Uid == id).FirstOrDefault();
        }
        /// <summary>
        /// 根据uid 及被关注uid 判断是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="QuiltUid"></param>
        /// <returns></returns>
        public tWriteLog GettWriteLogById(int uid, int QuiltUid)
        {
            return dbSet.Where(a => a.Uid == uid).FirstOrDefault();
        }
        public int updateEntity(tWriteLog model)
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


        public List<tWriteLog> GettWriteLogPageList(string urlcontent, DateTime? datetime, int page, int pagesize, out int count)
        {
            IQueryable<tWriteLog> linq = dbSet.OrderBy(a => a.CreateDate);
            if (!string.IsNullOrEmpty(urlcontent))
            {
                linq = linq.Where(a => a.sTitle.Contains(urlcontent));
            }
            if (datetime != null)
            {
                linq = linq.Where(a => a.CreateDate > datetime);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();

        }


        public List<tWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int page, int pagesize, out int count)
        {
            
            IQueryable<tWriteLog> linq = dbSet.OrderByDescending(a => a.CreateDate);
            if (uid != 0)
            {
                linq = linq.Where(a => a.Uid == uid);
            }
            if (Year!=0)
            {
                linq = linq.Where(a => a.CreateDate.Value.Year == Year);
            }
            if (Month != 0)
            {
                linq = linq.Where(a => a.CreateDate.Value.Month == Month);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0、我收到的评论，1、我发出的评论</param>
        /// <param name="uid"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tWriteLog> GettWriteLogPageListByMessage(int type, int uid, int page, int pagesize, out int count)
        {
            List<tWriteLog> logs = new List<tWriteLog>();
            string sqlStr = string.Empty;
            if (type == 0)
            {
                string sqlcount = @"select  Count(id) from
   [tWriteLog] as l   where l.ID=(select distinct ToUid from tMessage as m where l.id=m.ToUid and  m.ToType=4)  
    and  uid=" + uid + " and ifShow=1";
                object o = SQLHelper.ExecuteScalar(sqlcount, CommandType.Text, null);
                count = (int)o;


                sqlStr = @"   select * from
    (select ROW_NUMBER() over(order by id asc) as 'rowNumber', * from [tWriteLog] as l   where l.ID=(select distinct ToUid from tMessage as m where l.id=m.ToUid and  m.ToType=4)) as temp
    where rowNumber between " + ((page - 1) * pagesize + 1) + " AND " + page * pagesize + "and ifShow=1 And uid=" + uid;
            }
            else if (type == 1)
            {
                string sqlcount = @" select  count(id) from [tWriteLog] as l   where l.ID=(select distinct ToUid from tMessage as m where l.id=m.ToUid and  m.ToType=4 and fromuid=" + uid + @") 
    and  ifShow=1 ";
                object o = SQLHelper.ExecuteScalar(sqlcount, CommandType.Text, null);
                count = (int)o;


                sqlStr = @"  select * from
    (select ROW_NUMBER() over(order by id asc) as 'rowNumber', * from [tWriteLog] as l   where l.ID=(select distinct ToUid from tMessage as m where l.id=m.ToUid and  m.ToType=4 and fromuid=" + uid + @")) as temp
    where rowNumber between " + ((page - 1) * pagesize + 1) + " AND " + page * pagesize + "and ifShow=1";
            }
            else
            {
                count = 0;
            }

            if (!string.IsNullOrEmpty(sqlStr))
            {
                SqlDataReader reader = SQLHelper.ExecuteDataReader(sqlStr, CommandType.Text, null);
                tWriteLog log;
                while (reader.Read())
                {
                    log = new tWriteLog();
                    log.ID = (int)reader["ID"];
                    log.Uid = (int?)reader["Uid"];
                    log.sTitle = reader["sTitle"].ToString();
                    log.sContent = reader["sContent"].ToString();
                    log.CreateDate = (DateTime?)reader["CreateDate"];
                    logs.Add(log);
                }
            }

            return logs;
        }
    }
}
