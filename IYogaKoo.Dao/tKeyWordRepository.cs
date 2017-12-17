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

    public class tKeyWordRepository : Repository<tKeyWord>, ItKeyWordRepository
    { 
        public List<tKeyWord> GetPageList(string sWord,int page, int pagesize, out int count)
        {
            IQueryable<tKeyWord> linq = dbSet.OrderByDescending(a=>a.sWord);
            if (!string.IsNullOrEmpty(sWord))
            {
                linq = linq.Where(x => x.sWord == sWord);
            } 
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 后台分页
        /// </summary>
        /// <param name="iType">0,：游客；1 习练者 ；2 导师</param>
        /// <param name="where"></param>
        /// <param name="sWord"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetPageListdt(int  iType, string where, int page, int pagesize, out int count)
        {
            DataTable dt = new DataTable();
            //游客查询的关键字
            string sqlCount = string.Empty;

            string sqltop = string.Empty;
            count = 0;

            if (iType == 0 || iType == 3)
            {
                sqlCount = @"SELECT sWord,count(sWord) iNums   FROM [iyogakoodb].[dbo].[tKeyWord] 
                            where 1=1 " + where + "   group by  sWord  order by iNums desc";
                count = SQLHelper.QueryDataSet(sqlCount).Tables[0].Rows.Count;
                 
                sqltop = @"SELECT TOP (10) ROW_NUMBER() OVER ( ORDER BY  sWord DESC ) AS rownum ,
                            sWord,count(sWord) iNums   FROM [iyogakoodb].[dbo].[tKeyWord] 
                            where 1=1 " + where + "   group by  sWord  order by iNums desc"; 
            }
            else if (iType == 1)//习练者: 初级习练者...查询的
            {
                sqlCount = @" SELECT  a.sWord, count(a.sWord)iNums    FROM [iyogakoodb].[dbo].[tKeyWord] a 
                           inner join [dbo].[YogaUser] b on a.uid=b.[Uid]
                           inner join [dbo].[YogaUserDetail] c on a.uid=c.[UID]
                           where b.UserType=0  " + where + "   group by  sWord  order by iNums desc";
                count = SQLHelper.QueryDataSet(sqlCount).Tables[0].Rows.Count;

                sqltop = @"SELECT TOP (10) ROW_NUMBER() OVER ( ORDER BY  sWord DESC ) AS rownum ,
                           sWord,count(sWord) iNums   FROM [iyogakoodb].[dbo].[tKeyWord]  a 
                           inner join [dbo].[YogaUser] b on a.uid=b.[Uid]
                           inner join [dbo].[YogaUserDetail] c on a.uid=c.[UID]
                           where b.UserType=0  " + where + "   group by  a.sWord  order by iNums desc"; 

            }
            else if (iType == 2)  // 导师搜索的，初级导师...查询的
            {
                sqlCount = @" SELECT  a.sWord, count(a.sWord)iNums    FROM [iyogakoodb].[dbo].[tKeyWord] a 
                           inner join [dbo].[YogaUser] b on a.uid=b.[Uid]
                           inner join [dbo].[YogisModels] c on a.uid=c.[UID]
                           where b.UserType=1   " + where + "   group by  sWord  order by iNums desc";
                count = SQLHelper.QueryDataSet(sqlCount).Tables[0].Rows.Count;

                sqltop = @"SELECT TOP (10) ROW_NUMBER() OVER ( ORDER BY  sWord DESC ) AS rownum ,
                           sWord,count(sWord) iNums   FROM [iyogakoodb].[dbo].[tKeyWord]  a 
                           inner join [dbo].[YogaUser] b on a.uid=b.[Uid]
                           inner join [dbo].[YogisModels] c on a.uid=c.[UID]
                           where b.UserType=1  " + where + "   group by  a.sWord  order by iNums desc";
            }
             
            string sql = @" SELECT  *  FROM    (  " + sqltop + "  ) AS temp WHERE   temp.rownum > ( " + pagesize * (page - 1) + " )  ORDER BY iNums desc";

            dt = SQLHelper.QueryDataSet(sql).Tables[0];
             
            return dt;
        }
         

        public List<tKeyWord> SearchKeyWordList(string sWord)
        {
            return dbSet.Where(a => a.sWord == sWord).ToList();
        }
        
        public int updateEntity(tKeyWord model)
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
