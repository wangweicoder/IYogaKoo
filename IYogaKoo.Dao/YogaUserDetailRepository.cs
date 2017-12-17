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

    public class YogaUserDetailRepository : Repository<YogaUserDetail>, IYogaUserDetailRepository
    {
        #region 后台 习练者
        public List<YogaUserDetail> BackGetPageList(string RealName_cn, int? Ulevel, string YogaTypeid,
            int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID,
            int page, int pagesize, out int count)
        {
            IQueryable<YogaUserDetail> linq = dbSet.OrderBy(a => a.CreateTime);
            if (BackGetBool(RealName_cn))
            {
                int uid = Convert.ToInt32(RealName_cn);
                linq = linq.Where(a => a.UID == uid);
            }
            else if (!string.IsNullOrEmpty(RealName_cn))
            {
                linq = linq.Where(a => a.RealName_cn.Contains(RealName_cn) || a.RealName_en.Contains(RealName_cn));
            }
            if (Ulevel!=null)
            {
                linq = linq.Where(a => a.Ulevel == Ulevel.Value);
            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            }
            //地区
            if (!Nationality.Equals(0))
            {
                linq = linq.Where(a => a.Nationality == Nationality.Value);
            }
            if ( CountryID > 0)
            {
                linq = linq.Where(a => a.CountryID == CountryID.Value);
            } 
            if (!ProvinceID.Equals(0))
            {
                linq = linq.Where(a => a.ProvinceID == ProvinceID.Value);
            }
            if (!CityID.Equals(0))
            {
                linq = linq.Where(a => a.CityID == CityID.Value);
            }
            if (!DistrictID.Equals(0))
            { 
                linq = linq.Where(a => a.DistrictID == DistrictID.Value);
            } 

            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        public bool BackGetBool(string RealName_cn)
        {
            bool bl = false;
            try
            {
                if (Convert.ToInt32(RealName_cn) > 0)
                {
                    bl = true;
                }
                else {
                    bl = false;
                }
            }
            catch (Exception ex)
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        public List<YogaUserDetail> GetYogaUserDetailPageList(int Nums)
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
        /// 根据Uid获取YogaUserDetail 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaUserDetail> GetUidList(int id)
        {
            return dbSet.Where(a => a.UID == id).ToList();
        }
        public List<YogaUserDetail> GetYogaUserDetailPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogaUserDetail> GetYogaUserList(string strWhere, int Gender, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<YogaUserDetail> linq = dbSet.Where(a => a.delState == null).OrderBy(a => a.CreateTime);

            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName_cn.Contains(strWhere) || a.Street.Contains(strWhere) || a.RealName_en.Contains(strWhere));
            }
            if (Gender != 2)
            {
                linq = linq.Where(a => a.Gender == Gender);
            }
            
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            }
            count = linq.Count();// dbSet.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        public YogaUserDetail GetWhere(int id, string DisplayImg)
        {
            return dbSet.Where(a => a.UID == id && a.DisplayImg.Contains(DisplayImg)).FirstOrDefault();
        }
        /// <summary>
        /// 根据名称获取
        /// </summary>
        /// <param name="RealName"></param>
        /// <returns></returns>
        public YogaUserDetail GetByRealName(string RealName)
        {
            return dbSet.Where(a => a.RealName_cn.Contains(RealName) || a.RealName_en.Contains(RealName)).FirstOrDefault();
        }
        public List<YogaUserDetail> GetYogaUserList(string strWhere, int Gender, string YogaTypeid,int Ulevel, int page, int pagesize, out int count)
        {
            IQueryable<YogaUserDetail> linq = dbSet.OrderBy(a => a.CreateTime);

            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName_cn.Contains(strWhere) || a.Street.Contains(strWhere) || a.RealName_en.Contains(strWhere));
            }
            if (Gender != 2)
            {
                linq = linq.Where(a => a.Gender == Gender);
            }

            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            }
            if (Ulevel != 0)
            {
                linq = linq.Where(a => a.Ulevel == Ulevel);
            }
            count = linq.Count();// dbSet.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 权重需要排序， 取全部数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="DistrictId"></param>
        /// <param name="CityId"></param>
        /// <param name="PorviceId"></param>
        /// <param name="Countryid"></param>
        /// <param name="lp"></param>
        /// <param name="level"></param>
        /// <param name="gender"></param> 
        /// <param name="count"></param>
        /// <returns></returns>
        public List<YogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count)
        {
            IYogaUserRepository u = new YogaUserRepository();
            List<YogaUser> us = new List<YogaUser>();
            int[] uids = new int[0];
            if (!string.IsNullOrEmpty(strWhere))
            {
                us = u.GetListByNickName(strWhere);
            }
            if (us != null)
            {
                uids = new int[us.Count];
                for (int i = 0; i < us.Count; i++)
                {
                    uids[i] = us[i].Uid;
                }
            }


            IQueryable<YogaUserDetail> linq = dbSet.Where(a => a.delState == null).OrderBy(a => a.CreateTime);
            if (!string.IsNullOrEmpty(strWhere))
            {
                if (uids.Length > 0)
                {
                    linq = linq.Where(a => uids.Contains(a.UID));
                }
                else
                {
                    count = 0;
                    List<YogaUserDetail> dd = new List<YogaUserDetail>();
                    return dd;
                }
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
                //流派
                string templp = lp.ToString();
                linq = linq.Where(a => a.YogaTypeid.Contains(templp));
            }
            if (!level.Equals(0))
            {
                linq = linq.Where(a => a.Ulevel == level);
            }
            if (!gender.Equals(2))
            {
                linq = linq.Where(a => a.Gender == gender);
            }

            count = linq.Count();
            return linq.ToList();
        }
       

        public List<YogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp,int level,int gender, int page, int pagesize, out int count)
        {
            IYogaUserRepository u = new YogaUserRepository();
            List<YogaUser> us = new List<YogaUser>();
            int[] uids =new int[0];
            if (!string.IsNullOrEmpty(strWhere))
            {
                us = u.GetListByNickName(strWhere);
            }
            if (us != null)
            {
                uids = new int[us.Count];
                for (int i=0;i<us.Count;i++)
                {
                    uids[i] = us[i].Uid;
                } 
            } 
            
              
            IQueryable<YogaUserDetail> linq = dbSet.Where(a => a.delState == null).OrderBy(a => a.CreateTime);
            if (!string.IsNullOrEmpty(strWhere))
            {
                if (uids.Length > 0)
                {
                    linq = linq.Where(a => uids.Contains(a.UID));
                }
                else
                {
                    count = 0;
                    List<YogaUserDetail> dd = new List<YogaUserDetail>();
                    return dd;
                }
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
                //流派
                string templp = lp.ToString();
                linq = linq.Where(a => a.YogaTypeid.Contains(templp));
            }
            if (!level.Equals(0))
            { 
                linq = linq.Where(a => a.Ulevel == level);
            }
            if (!gender.Equals(2))
            {
                linq = linq.Where(a => a.Gender == gender);
            }
            
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 根据Uid获取YogaUserDetail 信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaUserDetail GetYogaUserDetailById(int id)
        {
            return dbSet.Where(a => a.UID == id).FirstOrDefault();
        }
        
        
          
        public int updateEntity(YogaUserDetail model)
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
        /// 随机查询数据
        /// </summary>
        /// <param name="YogisLevel"></param>
        /// <returns></returns>
        public DataTable GetSamelevelSupervisor(int Ulevel)
        {
            string sql = "SELECT TOP 7 *  FROM [iyogakoodb].[dbo].[YogaUserDetail] where Ulevel=" + Ulevel + "  order by NEWID()";

            DataTable dt = SQLHelper.ExecuteDataTable(sql, null);

           // var linq =(from t in dbSet orderby Guid.NewGuid()select t).Take(7);
            return dt;
        }
    }
}
