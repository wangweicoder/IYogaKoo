using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel.Commons.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogisModelsRepository : Repository<YogisModels>, IYogisModelsRepository
    {
        /// <summary>
        /// 专页：相似导师-根据流派查询
        /// </summary>
        /// <param name="uid">不包括本人(有的重复数据，去重）</param>
        /// <param name="YogaTypeid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<YogisModels> GetYogisModelsByYogaTypeid(int uid,string YogaTypeid, int count)
        {
            string[] ids = null;
            if (YogaTypeid.IndexOf(',') != -1)
            {
                //存在
                ids = YogaTypeid.Split(',');
              
                List<YogisModels> list = new List<YogisModels>();
                List<YogisModels> list2 = new List<YogisModels>();
                foreach (var i in ids)
                {
                    list2 = dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.YogaTypeid.Contains(i) && a.UID!=uid && a.delState == 0 && a.YogiStatus == 1).ToList();
                    if (list2.Count() != 0)
                    {
                        if (list2.Count() > 1)
                        {
                            foreach (YogisModels entity in list2)
                            {
                                list.Add(entity);
                            }
                        }
                        else
                        {
                            list.Add(list2[0]);
                        }
                    }
                }
                //list = list.Distinct().ToList();
                if (count > 0)
                    return list.OrderBy(n => Guid.NewGuid()).Take(count).Distinct().ToList();
                else
                    return list.Distinct().ToList();
            }
            else
            {
                IQueryable<YogisModels> linq = dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.YogaTypeid == YogaTypeid && a.UID != uid  && a.delState == 0 && a.YogiStatus == 1);
                if (count > 0)
                    return linq.Take(count).ToList();
                else
                    return linq.ToList();
            }

        }


        #region 后台按条件分页查询导师
        //
        public List<YogisModels> BackPageList(string RealName, string CenterID, string YogaTypeid, int YogiStatus, int? YogisLevel, int page, int pagesize, out int count)
        {
            IQueryable<YogisModels> linq = dbSet.OrderBy(a => a.StartTeachYear);
            if (!string.IsNullOrEmpty(RealName))
            { 
                linq = linq.Where(a => a.RealName.Contains(RealName)); 
            }
            if (!string.IsNullOrEmpty(CenterID))
            {
                linq = linq.Where(a => a.CenterID.Contains(CenterID));
            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            } 
            if (YogisLevel != 0)
            {
                linq = linq.Where(a => a.YogisLevel==YogisLevel);
            } if (YogiStatus == 1)
            {
                linq = linq.Where(a => a.YogiStatus == YogiStatus && a.delState == 0);
            }
            else {
                linq = linq.Where(a => a.YogiStatus == YogiStatus );
            }
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        #endregion

        /// <summary>
        /// 导师
        /// </summary>
        /// <returns>0928修改显示未删除的和有头像的</returns>
        public List<YogisModels> GetYogisModelsList()
        {
            return dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.delState == 0 && a.YogiStatus ==1).ToList();
        }
        public YogisModels GetByRealName(string RealName)
        {
            return dbSet.Where(a => a.RealName.Contains(RealName)).FirstOrDefault();
        }

        //end

        /// <summary>
        /// 导师列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns>0928修改</returns>
        public List<YogisModels> GetYogisModelsPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Where(x => x.YogiStatus == 1 && x.delState== 0).Count();

            return dbSet.Where(x => x.YogiStatus == 1 && x.delState == 0).OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 升级导师列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<YogisModels> GetYogisModelsPageListUp(int page, int pagesize, out int count)
        {
            count = dbSet.Where(x => x.YogiStatus == 0).Count();

            return dbSet.Where(x => x.YogiStatus == 0).OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogisModels> GetYogisModelsList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<YogisModels> linq = dbSet.OrderBy(a => a.StartTeachYear);

            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName.Contains(strWhere) || a.Street.Contains(strWhere) || a.YogisDepict.Contains(strWhere));
            }
            if (Gender != 2)
            {
                linq = linq.Where(a => a.Gender == Gender);
            }
            if (YogisLevel != 0)
            {
                linq = linq.Where(a => a.YogisLevel == YogisLevel);
            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            }
            count = linq.Count();// dbSet.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        public List<YogisModels> GetYogisModelsList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int Ulevel, int page, int pagesize, out int count)
        {
            IQueryable<YogisModels> linq = dbSet.OrderBy(a => a.StartTeachYear);

            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName.Contains(strWhere) || a.Street.Contains(strWhere) || a.YogisDepict.Contains(strWhere));
            }
            if (Gender != 2)
            {
                linq = linq.Where(a => a.Gender == Gender);
            }
            if (YogisLevel != 0)
            {
                linq = linq.Where(a => a.YogisLevel == YogisLevel);
            }
            if (!string.IsNullOrEmpty(YogaTypeid))
            {
                linq = linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            }
            if (Ulevel != null)
            {
                linq = linq.Where(a => a.YogisLevel == Ulevel);
            }
            count = linq.Count();// dbSet.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 根据Uid 获取list列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogisModels> GetYogisModelsUid(int id)
        {
            return dbSet.Where(a => a.UID == id).ToList();
        }
        /// <summary>
        /// 根据Uid 获取Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogisModels GetYogisModelsById(int id)
        {
            return dbSet.Where(a => a.UID == id).FirstOrDefault();
        }

        public YogisModels GetWhere(int id, string DisplayImg)
        {
            return dbSet.Where(a => a.UID == id && a.DisplayImg.Contains(DisplayImg)).FirstOrDefault();
        }
        public int updateEntity(YogisModels model)
        {
            var entity = dbSet.Find(model.UID);
            if (entity == null)
            {
                entity = dbSet.Find(model.YID);
            }
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
        /// 根据机构id，返回导师列表
        /// </summary>
        /// <param name="centerid">机构id</param>
        /// <param name="count">返回数量</param>
        /// <returns></returns>
        public List<YogisModels> GetYogisModelsByCenterId(int tempcenterids, int count)
        {
            string centerid = tempcenterids.ToString();
            //  return dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.YogiStatus == 1 && a.CenterID.Split(',').Contains(centerid)).Take(count).ToList();
            //List<YogisModels> linq = dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.CenterID.Contains(centerid)).ToList();//原来 qiqi 注释 2015-12-21

            List<YogisModels> linq = dbSet.OrderByDescending(a => a.StartTeachYear).Where(a => a.CenterID.Contains(centerid) && a.delState == 0 && a.YogiStatus == 1).ToList();


            List<YogisModels> list = new List<YogisModels>();
            string[] arrcenterid;
            foreach (var item in linq)
            {
                arrcenterid = item.CenterID.Split(',');
                for (int i = 0; i < arrcenterid.Length; i++)
                {
                    if (arrcenterid[i] == tempcenterids.ToString())
                    {
                        list.Add(item);
                        break;
                    }
                }
            }
            //qiqi 2015-12-21
            //if (count > 0)
            //{
            //    return list.Take(count).ToList();
            //}
            //else
            //{
            //    //返回所有
            //    return list;
            //}
            return list;
        }
         
        /// <summary>
        /// 检查身份证号码
        /// </summary>        
        public YogisModels ExistIdCard(string idcardnum)
        {
            return dbSet.Where(a => a.IdCardNum==idcardnum).FirstOrDefault();
        }
        /// <summary>
        /// 权重，查全部数据排序
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
        public List<YogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count)
        {
            IQueryable<YogisModels> linq = dbSet.Where(a => a.delState == 0 && a.YogiStatus==1).OrderBy(a => a.CreateDate);
            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName.Contains(strWhere));
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
                linq = linq.Where(a => a.YogisLevel == level);
            }
            if (!gender.Equals(2))
            {
                linq = linq.Where(a => a.Gender == gender);
            }

            count = linq.Count();
            return linq.ToList();
        }

        public List<YogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender,int page,  int pagesize, out int count)
        {
            IQueryable<YogisModels> linq = dbSet.Where(a => a.delState == 0 && a.YogiStatus == 1).OrderBy(a => a.CreateDate);
            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.RealName.Contains(strWhere));
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
                linq = linq.Where(a => a.YogisLevel == level);
            }
            if (!gender.Equals(2))
            {
                linq = linq.Where(a => a.Gender == gender);
            }

            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 随机查询数据
        /// </summary>
        /// <param name="YogisLevel"></param>
        /// <returns></returns>
        public DataTable GetSamelevelSupervisor(int YogisLevel)
        {
            string sql = "SELECT TOP 7 *  FROM [iyogakoodb].[dbo].[YogisModels] where YogisLevel=" + YogisLevel + " and YogiStatus=1  order by NEWID()";

            DataTable dt = SQLHelper.ExecuteDataTable(sql, null);
            
            return dt;
        }
    }
}
