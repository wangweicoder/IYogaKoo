using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel.Commons.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class CentersRepository : Repository<Centers>, ICentersRepository
    {
        public List<Centers> GetCentersPageList(int page, int pagesize, string centertype, out int count)
        {
            if (centertype.Equals("0"))
            {
                count = dbSet.Count();
                return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                count = dbSet.Where(a => a.CenterType == centertype).Count();
                return dbSet.Where(a => a.CenterType == centertype).OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strWhere">搜索条件</param>
        /// <param name="Gender"></param>
        /// <param name="YogisLevel"></param>
        /// <param name="YogaTypeid">机构分类</param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Centers> GetCentersPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<Centers> linq = dbSet.OrderBy(a => a.CreateDate).Where(a => a.CenterName.Contains(strWhere));
            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.CenterName.Contains(strWhere));
            }
            if (!YogaTypeid.Equals("0"))
            {
                linq = linq.Where(a => a.CenterType == YogaTypeid);
            }
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 根据uid 获取 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Centers> GetCentersUid(int id)
        {
            return dbSet.Where(a => a.Uid == id.ToString()).ToList();
        }
        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns></returns>
        public List<Centers> GetCentersUid()
        {
            return dbSet.ToList();
        }
        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Centers GetCentersById(int id)
        {
            return dbSet.Where(a => a.CenterId == id).FirstOrDefault();
        }
        /// <summary>
        /// 根据名称查询该ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Centers GetCentersByCenterName(string CenterName)
        {
            return dbSet.Where(a => a.CenterName.Contains(CenterName)).FirstOrDefault();
        }

        public Centers GetCentersByUid(string Uid)
        {
            return dbSet.Where(a => a.Uid == Uid).FirstOrDefault();
        }
        public int updateEntity(Centers model)
        {

            var entity = dbSet.Find(model.CenterId);

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
        /// 
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="DistrictId"></param>
        /// <param name="CityId"></param>
        /// <param name="PorviceId"></param>
        /// <param name="Countryid"></param>
        /// <param name="lp">流派</param>
        /// <param name="Centertypeid"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Centers> GetCentersPageList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, string Centertypeid, int page, int pagesize, out int count)
        {
            IQueryable<Centers> linq = dbSet.OrderBy(a => a.CreateDate);
            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.CenterName.Contains(strWhere));
            }
            if (!DistrictId.Equals(0))
            {
                linq = linq.Where(a => a.DistrictID == DistrictId);
            }
            if (!CityId.Equals(0))
            {
                linq = linq.Where(a => a.CityID == CityId);
            }
            if (!PorviceId.Equals(0))
            {
                linq = linq.Where(a => a.ProvinceID == PorviceId);
            }
            if (!Countryid.Equals(0))
            {
                linq = linq.Where(a => a.CountryID == Countryid);
            }
            if (!lp.Equals(0))
            {
                string lptemp = "|" + lp + "|";
                //流派
                linq = linq.Where(a => a.YogaTypeid.Contains(lptemp));
            }
            if (!Centertypeid.Equals("0"))
            {
                linq = linq.Where(a => a.CenterType == Centertypeid);
            }
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 根据Class表里面的CenterID查询对应的机构
        /// </summary>
        /// <param name="classCenterID"></param>
        /// <returns></returns>
        public List<Centers> GetCentersListByClassCenterID(string classCenterID)
        {
            List<Centers> list = new List<Centers>();
            string centerID = classCenterID.Replace("|", "").Replace("|", "");
            string sql = "  SELECT *  FROM  Centers where CenterId in (" + centerID + ")";

            SqlDataReader reader = SQLHelper.ExecuteDataReader(sql, CommandType.Text, null);
            while (reader.Read())
            {
                Centers model = new Centers()
                {
                    CenterId = Convert.ToInt32(reader["CenterId"]),
                    CenterName = reader["CenterName"].ToString(),
                    CenterPortrait = reader["CenterPortrait"].ToString()
                };
                list.Add(model);
            }
            return list;
        }
    }
}
