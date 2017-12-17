using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IYogaKoo.Dao
{

    public class YogaDicItemRepository : Repository<YogaDicItem>, IYogaDicItemRepository
    {
        public List<YogaDicItem> GetYogaDicItemPageList(string where, int docid, int page, int pagesize, out int count)
        {
            
            if (string.IsNullOrEmpty(where))
            {
                count = dbSet.Where(x => x.DicId == docid).Count();
                return dbSet.Where(x => x.DicId == docid).OrderByDescending(a => a.ItemName).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                count = dbSet.Where(x => x.DicId == docid && x.ItemName.Contains(where)).Count();
                return dbSet.Where(x => (x.DicId == docid && x.ItemName.Contains(where))).OrderByDescending(a => a.ItemName).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
        }

        /// <summary>
        /// 根据主键ID 获取YogaDicItem 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaDicItem> GetDicById(int id)
        {
            return dbSet.Where(a => a.ID == id).OrderBy(a=>a.ItemName).ToList();
        }

        /// <summary>
        /// 根据主键 DicId 获取YogaDicItem 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaDicItem> GetDicId(int id)
        {
            return dbSet.Where(a => a.DicId == id).OrderBy(a => a.ItemName).ToList();
        }
        /// <summary>
        /// 根据表中所有信息List
        /// </summary>
        /// <returns></returns>
        public List<YogaDicItem> GetYogaDicItemList()
        {
            return dbSet.OrderByDescending(a => a.CreateTime).OrderBy(a => a.ItemName).ToList();
        }
        public List<YogaDicItem> GetYogaDicItemPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<YogaDicItem> linq = dbSet.OrderBy(a => a.ItemName);
             
            count = linq.Count(); 
             
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        ///  根据主健id获取信息 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaDicItem GetYogaDicItemById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }
        public YogaDicItem GetYogaDicItemByItemName(string ItemName)
        {
            return dbSet.Where(a => a.ItemName.Contains( ItemName)).FirstOrDefault();
        }
        public IQueryable<YogaDicItem> Dics()
        {
            return dbSet;
        }

        public int updateEntity(YogaDicItem model)
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

        /// <summary>
        /// 获取字典选择项集合
        /// </summary>
        /// <param name="id">当前编号</param>
        /// <param name="forChild">是否是查找子级</param>
        /// <returns></returns>
        public List<YogaDicItem> GetSelectList(int id, bool? forChild)
        {
            string sqlStr = "";
            if (forChild == true)
                sqlStr = "SELECT * FROM YogaDicItem WHERE DicId=@ID  ORDER BY ItemName";//AND IsUse=1 AND IsDelete=0
            else if (forChild == false)
                sqlStr = "DECLARE @PID INT SELECT @PID = DicId FROM YogaDicItem WHERE ID = @ID SELECT @PID=DicId FROM YogaDicItem WHERE ID=@PID SELECT * FROM YogaDicItem WHERE DicId = @PID  ORDER BY ItemName";// AND IsUse = 1 AND IsDelete = 0
            else if (forChild == null)
                sqlStr = "DECLARE @PID INT SELECT @PID = DicId FROM YogaDicItem WHERE ID = @ID SELECT * FROM YogaDicItem WHERE DicId = @PID  ORDER BY ItemName";//AND IsUse = 1 AND IsDelete = 0
            DataTable dt = SQLHelper.QueryDataSet(sqlStr, new SqlParameter("@ID", id)).Tables[0];
            List<YogaDicItem> list = new List<YogaDicItem>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new YogaDicItem() { ID=Convert.ToInt32(item["id"]), DicId=Convert.ToInt32(item["dicid"]), ItemName=item["itemName"].ToString() });
            }
            return list;
        }
        public List<YogaDicItem> GetSelectList(string idsStr)
        {
            string[] strs = idsStr.Split(',');
            int[] ids = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                ids[i] = int.Parse(strs[i]);
            }
            IQueryable<YogaDicItem> iquery = dbSet.Where(d => ids.Contains(d.ID));
            return iquery.ToList();
        }
        public string GetDicIds(int id)
        {
            string sqlStr = "SELECT DBO.[GetDicIds](" + id + ")";
            string rel = SQLHelper.ExecuteScalar(sqlStr, CommandType.Text, null).ToString();
            return rel;
        }
        public string GetDicNames(int id)
        {
            string sqlStr = "SELECT DBO.[GetDicNames](" + id + ")";
            string rel = SQLHelper.ExecuteScalar(sqlStr, CommandType.Text, null).ToString();
            return rel;
        }
    }
}
