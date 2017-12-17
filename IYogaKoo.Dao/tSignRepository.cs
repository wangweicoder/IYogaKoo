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

    public class tSignRepository : Repository<tSign>, ItSignRepository
    {
        /// <summary>
        ///  今天签到数量
        /// </summary>
        /// <param name="dtTimeNow">yyyy-MM-dd</param>
        /// <returns></returns>
        public int GetCount(string dtTimeNow)
        {
            string sqlStr = @"select Count(*) rn from tSign  where CreateDate=getdate()";
            DataTable dt = SQLHelper.ExecuteDataTable(sqlStr, null);

            return Convert.ToInt32(dt.Rows[0][0]);
            
        }
        /// <summary>
        /// 签到排名
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public int RowNums(int Uid)
        {
            string sqlStr = @"select t.rn from (select *,row_number() over(order by ID) rn from tSign where  CONVERT(varchar(10), CreateDate, 120 )  = CONVERT(varchar(10), getdate(), 120 )) t
                where t.Uid=" + Uid;
            DataTable dt = SQLHelper.ExecuteDataTable(sqlStr, null);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0]);
            else return 0;
        }
        /// <summary>
        /// 是否签到
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public bool ExistsSign(int Uid)
        { 
            string sqlStr = @"select * from tSign  where  Uid=" + Uid + " and CONVERT(varchar(10), CreateDate, 120 )    =CONVERT(varchar(10), getdate(), 120 ) ";

            SqlParameter[]  paras = new SqlParameter[]
             { 
                 new SqlParameter("@Uid",Uid) 
             };


            bool bl = SQLHelper.Exists(sqlStr, paras);

            return bl;
        }
        public int updateEntity(tSign model)
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
