using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Data.SqlClient;
using IYogaKoo.ViewModel.Commons.Helper;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Dao
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public IQueryable<Class> Classes
        {
            get
            {
                return dbSet.Include("User");
            }
        }

        public IQueryable<Class> GetIncludes(params string[] paths)
        {
            throw new NotImplementedException();
        }
        public Class Get(int id)
        {
            return dbSet.Where(c => c.Id == id).FirstOrDefault();
        }
        public int Count(int id)
        {
            return dbSet.Where(c => c.ItemClassID == id).Count();
        }
        public int Edit(Class entity)
        {
            var tempEntity = dbSet.Find(entity.Id);

            if (tempEntity != null)
            {
                Context.Entry(tempEntity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            tempEntity = entity;

            Update(tempEntity);

            return Save();
        }

        public int SetClassMany()
        {
            string sqlStr = "update Class set ClassStatus=3 where ClassStatus=2 and EndTime<=getdate();";
            int result = SQLHelper.ExecuteNonQuery(sqlStr, CommandType.Text, null);
            return result;
        }
        /// <summary>
        /// 获取头像列表
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="classId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<YogisModels> GetAvatars(string uids, int classId, int page, int size)
        {
            List<YogisModels> users = new List<YogisModels>();
            string sqlStr = @"SELECT * FROM (
SELECT ROW_NUMBER() OVER(ORDER BY I.CreateTime DESC) AS ROWID, U.UID,CASE WHEN Y.REALNAME IS NULL THEN U.RealName_cn ELSE Y.RealName END AS NAME,  CASE WHEN Y.DisplayImg IS NULL THEN U.DisplayImg ELSE Y.DisplayImg END AS AVATAR
 FROM YogaUserDetail U LEFT JOIN YogisModels Y ON U.UID = Y.UID JOIN InterestedClass I ON I.UserId=U.UID
WHERE '," + uids + ",' LIKE '%,'+CAST(U.UID AS VARCHAR(50))+',%' AND I.ClassId=" + classId + @"
) T WHERE T.ROWID BETWEEN " + ((page - 1) * size + 1) + " AND " + page * size;
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sqlStr, CommandType.Text, null);
            while (reader.Read())
            {
                YogisModels user = new YogisModels() { UID = Convert.ToInt32(reader["uid"]), RealName = reader["name"].ToString(), DisplayImg = reader["avatar"].ToString() };
                users.Add(user);
            }
            return users;
        }

        public List<UserListItem> GetAvatars(string uids)
        {
            List<UserListItem> users = new List<UserListItem>();
            string sqlStr = "SELECT YU.Uid,CASE YU.USERTYPE WHEN 1 THEN YG.RealName ELSE YU.NickName END AS Name,CASE YU.USERTYPE WHEN 1 THEN YG.DisplayImg ELSE YD.DisplayImg END AS AVATAR ,YU.UserType , ISNULL( YG.YogisLevel,-99) as YogisLevel FROM YogaUser YU LEFT JOIN YogaUserDetail YD ON YU.UID=YD.UID LEFT JOIN YogisModels YG ON YU.UID=YG.UID WHERE YU.UID IN (" + uids + ")";
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sqlStr, CommandType.Text, null);
            while (reader.Read())
            {
                UserListItem user = new UserListItem()
                {
                    ID = Convert.ToInt32(reader["uid"]),
                    Name = reader["name"].ToString(),
                    Avatar = reader["avatar"].ToString(),
                    UserType = Convert.ToInt32(reader["UserType"]),
                    YogisLevel = Convert.ToInt32(reader["YogisLevel"])
                };
                users.Add(user);
            }
            return users;
        }

        public List<Class> GetClasses(int code, int page, int size, out int count, string[] args)
        {
            #region sql
            string sqlWithWhere = " CLASSSTATUS = 2  ";
            //<<<<<<< .mine
            string sqlWith = "WITH CL AS(SELECT cc.id,cc.name,AreaID,DBO.GetDicNames(AreaID) AS areanames,DBO.GetDicIds(AreaID) AS areaids,Start,EndTime,Banner,Price,TopicIds,cc.IsDeleted,cc.CreateTime,CLASSSTATUS,SUMMARY,DBO.GETUSERNAME(cc.USERID) AS TNAME,DBO.GETUSERAVATAR(cc.USERID) AS AVATAR,DBO.COUNTINTEREST(cc.ID) AS INTEREST   FROM Class as  cc WHERE 1=1 AND {0}) ";
            //=======
            //            string sqlWith = "WITH CL AS(SELECT id,name,AreaID,DBO.GetDicNames(AreaID) AS areanames,DBO.GetDicIds(AreaID) AS areaids,Start,EndTime,Banner,Price,TopicIds,IsDeleted,CreateTime,CLASSSTATUS,SUMMARY,DBO.GETUSERNAME(USERID) AS TNAME,DBO.GETUSERAVATAR(USERID) AS AVATAR,DBO.COUNTINTEREST(ID) AS INTEREST FROM Class WHERE 1=1 AND {0}) ";
            //>>>>>>> .r2936
            string sqlSelectOrder = " ORDER BY START ";
            string sqlSelect = " SELECT ROW_NUMBER() OVER({0}) AS ROWID ";
            //最新报道,报道查询,      每个活动的最新报道
            string sqlReport = "SELECT ClassId,Title AS RTITLE,CONTENT AS RCONTENT,URL AS RURL FROM ClassReport CR LEFT JOIN (SELECT ReportId,URL FROM ClassFile WHERE Id IN(SELECT MAX(ID) FROM ClassFile GROUP BY ReportId) AND Type=11) CF ON CR.Id=CF.ReportId WHERE CR.Id IN (SELECT MAX(ID) FROM ClassReport GROUP BY ClassId) AND IsDeleted=0";
            switch (code)
            {
                //按照时间倒序,sqlWithWhere不做限制，用于活动首页默认加载
                case -3:
                    {
                        sqlWithWhere = " IsDeleted=0 ";
                        if (args[0] != "")//主题
                            sqlWithWhere += " and TopicIds =" + args[0];
                        if (args[1] != "")//地区(区域名，不是区域id)
                            sqlSelect += " ,* FROM CL WHERE  CL.areanames like '%" + args[1] + "%'";
                        else
                            sqlSelect += ",* FROM CL where 1=1  ";
                        if (args[2] == "")
                            sqlWithWhere += "and (CLASSSTATUS = 2 or CLASSSTATUS = 3)";
                        else if (args[2] == "2")//对应页面【全部、活动预告、活动回顾】
                            sqlWithWhere += " and  CLASSSTATUS = 2 and  GETDATE()< CONVERT(datetime, cc.Start) ";
                        else if (args[2] == "3")
                            sqlWith = "WITH CL AS(SELECT cc.id,cc.name,AreaID,DBO.GetDicNames(AreaID) AS areanames,DBO.GetDicIds(AreaID) AS areaids,Start,EndTime,Banner,Price,TopicIds,cc.CreateTime,CLASSSTATUS,SUMMARY,DBO.GETUSERNAME(cc.USERID) AS TNAME,DBO.GETUSERAVATAR(cc.USERID) AS AVATAR,DBO.COUNTINTEREST(cc.ID) AS INTEREST  "
                                           + " FROM Class as  cc inner join (select ClassId from  ClassReport group by ClassId) d on cc.Id=d.ClassId WHERE 1=1 AND {0}) ";
                        //if (args[3] != "")单独的日历，只能选择一个时间。现修改为两个时间，即开始时间、结束时间
                        //    sqlSelect += " AND  CAST(START AS DATETIME) = '" + args[3] + "'  ";
                        if (args[6] != "")//表示开始时间
                            sqlSelect += " AND  CAST(START AS DATETIME) >= '" + args[6] + "'  ";
                        if (args[7] != "")//表示结束时间
                            sqlSelect += " AND  CAST(START AS DATETIME) <= '" + args[7] + "'  ";
                        //只有是“全部”的时候 才有 状态,活动预告和活动回顾页面没有这些状态
                        if (args[2] == "")
                        {
                            //对应页面【全部状态、报名中、进行中、已结束】
                            if (args[4] == "1")//报名中
                                sqlWithWhere += " and ClassStatus=2 and GETDATE()<cc.CloseTime ";
                            if (args[4] == "2")//进行中
                                sqlWithWhere += "  and  GETDATE()> CONVERT(datetime, cc.Start) and  GETDATE()<cc.EndTime ";
                            if (args[4] == "3")//已结束
                            {
                                sqlWithWhere += " and  GETDATE()>cc.EndTime ";
                            }
                        }
                        //args[2]等于3的时候是回顾，时间是往前推，需要查历史，不等于3的时候是全部或预告，时间是往后推,查将来
                        //args[4] 等于3的时候查询的是结束的活动，比较字段用EndTime且时间同样往前推，查历史
                        if (args[2] != "3" && args[4] != "3")
                        {

                            if (args[5] == "1")
                                sqlWithWhere += " AND CONVERT(datetime,cc.Start) > GETDATE() and CONVERT(datetime,cc.Start) < dateadd(dd,1,GETDATE()) ";
                            else if (args[5] == "2")
                                sqlWithWhere += " AND CONVERT(datetime,cc.Start) > GETDATE() and CONVERT(datetime,cc.Start) < dateadd(dd,3,GETDATE()) ";
                            else if (args[5] == "3")
                                sqlWithWhere += " AND CONVERT(datetime,cc.Start) > GETDATE() and CONVERT(datetime,cc.Start) < dateadd(dd,7,GETDATE()) ";
                            else if (args[5] == "4")
                                sqlWithWhere += " AND CONVERT(datetime,cc.Start) > GETDATE() and CONVERT(datetime,cc.Start) < dateadd(dd,30,GETDATE()) ";
                        }
                        else
                        {
                            if (args[5] == "1")
                                sqlWithWhere += " AND cc.EndTime > dateadd(dd,-1,GETDATE()) ";
                            else if (args[5] == "2")
                                sqlWithWhere += " AND cc.EndTime >  dateadd(dd,-3,GETDATE()) ";
                            else if (args[5] == "3")
                                sqlWithWhere += " AND cc.EndTime > dateadd(dd,-7,GETDATE()) ";
                            else if (args[5] == "4")
                                sqlWithWhere += " AND cc.EndTime > dateadd(dd,-30,GETDATE()) ";
                        }
                        sqlSelectOrder = " ORDER BY START DESC";
                    } break;
                //按照时间倒序
                case -2:
                    {
                        sqlSelectOrder = " ORDER BY START DESC";
                        if (args.Length > 1 && !string.IsNullOrEmpty(args[1]) && args[1] != "0")
                            sqlSelect += " ,* FROM CL WHERE  ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[1]) + "'+'%'";
                        else
                            sqlSelect += ",* FROM CL ";
                    } break;
                //首页活动搜索
                case -1:
                    {
                        //2015/10/28李贺加【if (args[0] != "-1")】表示没有附带条件
                        if (args[0] != "-1")
                            sqlWith = sqlWith.TrimEnd(' ').TrimEnd(')') + " AND (TAGS LIKE '%" + args[0] + "%' OR NAME LIKE '%" + args[0] + "%') ) ";
                        if (args.Length > 1 && !string.IsNullOrEmpty(args[1]) && args[1] != "0")
                            sqlSelect += " ,* FROM CL WHERE  ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[1]) + "'+'%'";
                        else
                            sqlSelect += ",* FROM CL ";
                    } break;
                //0-3，对应报道页面的四列查询
                case 0:
                    {
                        if (args[0] == "intrest")
                            sqlSelect += " ,* FROM CL C JOIN (SELECT CLASSID,COUNT(*) AS COUNT FROM InterestedClass GROUP BY ClassId) AS I ON C.Id=I.ClassId JOIN (" + sqlReport + ") REP ON CL.ID=REP.CLASSID ORDER BY I.COUNT DESC ";
                    } break;
                case 1:
                    {
                        sqlSelect += ",* FROM CL JOIN (" + sqlReport + ") REP ON CL.ID=REP.CLASSID WHERE ','+areaids+',' LIKE '%'+'" + int.Parse(args[0]) + "'+'%' ";
                    } break;
                case 2:
                    {
                        sqlSelect += " ,* FROM CL JOIN (" + sqlReport + ") REP ON CL.ID=REP.CLASSID WHERE  ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[0]) + "'+'%'";
                    } break;
                case 3:
                    {
                        sqlSelect += " ,* FROM CL JOIN (" + sqlReport + ") REP ON CL.ID=REP.CLASSID WHERE  CAST(START AS DATETIME)>='" + args[0] + "' AND CAST(START AS DATETIME) <= '" + args[1] + "'";
                    } break;
                case 4:
                    {
                        sqlSelect += " ,* FROM CL WHERE  1=1";
                        if (args[0] != "")
                            sqlSelect += " AND ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[0]) + "'+'%' ";
                        if (args[1] != "")
                            sqlSelect += " AND ','+areaids+',' LIKE '%'+'" + int.Parse(args[1]) + "'+'%'  ";
                        DateTime timenow = DateTime.Now;
                        if (args[2] != "")
                        {
                            string classtype = args[2].ToString();
                            if (classtype.Equals("2"))//活动预告
                                sqlSelect += " AND  CAST(START AS DATETIME)> '" + timenow + "'  ";
                            if (classtype.Equals("3"))//活动回顾
                                sqlSelect += " AND EndTime <= '" + timenow + "'  ";
                        }
                        if (args[3] != "")
                            sqlSelect += " AND  CAST(START AS DATETIME) = '" + args[3] + "'  ";
                    } break;
                //活动报道页面的搜索条件的活动查询，备用
                case 5:
                    {
                        if (args[0] == "intrest")
                            sqlSelect += " ,* FROM CL C JOIN (SELECT CLASSID,COUNT(*) AS COUNT FROM InterestedClass GROUP BY ClassId) AS I ON C.Id=I.ClassId ORDER BY I.COUNT DESC ";
                    } break;
                case 6:
                    {
                        sqlSelect += ",* FROM CL WHERE ','+areaids+',' LIKE '%'+'" + int.Parse(args[0]) + "'+'%' ";
                    } break;
                case 7:
                    {
                        sqlSelect += " ,* FROM CL WHERE  ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[0]) + "'+'%'";
                    } break;
                //时间参数
                case 8:
                    {
                        sqlWithWhere = " IsDeleted=0 and  CLASSSTATUS =  " + args[0];
                        sqlSelect += " ,* FROM CL WHERE  CAST(START AS DATETIME)>='" + args[1] + "' AND CAST(START AS DATETIME) <= '" + args[2] + "'";
                        if (args.Length > 3)
                            sqlSelectOrder = " ORDER BY START DESC ";
                    } break;
                //查询登陆用户活动
                case 9:
                    {
                        sqlSelect += " ,* FROM CL WHERE  CAST(START AS DATETIME)>='" + args[0] + "' AND CAST(START AS DATETIME) <= '" + args[1] + "'";
                    } break;
                case 10:
                    {
                        sqlSelect += " ,* FROM CL WHERE  1=1 AND IsDeleted =0 ";
                        if (args[0] != "")
                            sqlSelect += " AND ','+Name+',' LIKE '%'+'" + args[0] + "'+'%' ";
                        if (args[1] != "" && args[1] != "0")
                            sqlSelect += " AND ','+TopicIds+',' LIKE '%'+'" + int.Parse(args[1]) + "'+'%'  ";
                        DateTime timenow = DateTime.Now;
                        if (args[2] != "")
                            sqlSelect += " AND  CAST(START AS DATETIME) = '" + args[2] + "'  ";
                    } break;
            }
            //string sqlStr = string.Format(sqlWith, sqlWithWhere) + " SELECT * FROM (" + string.Format(sqlSelect, sqlSelectOrder) + ") T";  下句替换此句配合查出多少人参加活动使用
            string sqlStr = string.Format(sqlWith, sqlWithWhere) + " SELECT T.*,o.ClassId, ISNULL( o.Amount,0) as Amount FROM (" + string.Format(sqlSelect, sqlSelectOrder) + ") T";
            //增加此句查出有多少人参加活动
            sqlStr += " left join (select ClassId,COUNT(ClassId)as Amount from [order] group by ClassId) o on o.ClassId=T.id";
            sqlStr += " WHERE T.ROWID BETWEEN " + ((page - 1) * size + 1) + " AND " + page * size;
            string sqlCount = string.Format(sqlWith, sqlWithWhere) + " SELECT COUNT(*) FROM (" + string.Format(sqlSelect, sqlSelectOrder) + ") T ";
            #endregion
            List<Class> list = new List<Class>();
            count = Convert.ToInt32(SQLHelper.ExecuteScalar(sqlCount, CommandType.Text, null));
            if (count > 0)
            {
                SqlDataReader reader = SQLHelper.ExecuteDataReader(sqlStr, CommandType.Text, null);
                while (reader.Read())
                {
                    Class c = new Class();
                    c.Id = Convert.ToInt32(reader["id"]);
                    c.Name = reader["name"].ToString();
                    c.Banner = reader["banner"].ToString();
                    c.Start = Convert.ToDateTime(reader["start"]).ToString("yyyy-MM-dd");
                    c.CreateTime = Convert.ToDateTime(reader["createtime"]);
                    c.ClassStatus = Convert.ToInt32(reader["CLASSSTATUS"]);//不等于空的话，就选择倒数第二个，倒数第二个代表城市
                    c.Content = reader["areanames"].ToString() == "" ? "" : reader["areanames"].ToString().Split('-')[reader["areanames"].ToString().Split('-')[3].Length - 2]; //reader["areanames"].ToString().Split('-')[3];
                    c.User = new YogaUser() { NickName = reader["TNAME"].ToString(), IsAssessor = false, IsWebworkers = false, LastDate = DateTime.Now };
                    c.Price = decimal.Parse(reader["PRICE"].ToString());
                    c.InterestedClass = (new InterestedClass[int.Parse(reader["INTEREST"].ToString())]).ToList();
                    c.NoPassMsg = reader["AREANAMES"].ToString();
                    c.Summary = reader["summary"].ToString();
                    //此句配合查出多少人参加活动使用
                    //c.Order = (new Order[int.Parse(reader["Amount"].ToString())]).ToList();
                    c.Max = int.Parse(reader["Amount"].ToString());

                    if (code >= 0 && code <= 3)
                    {
                        c.Name = reader["RTITLE"].ToString();
                        if (reader["rurl"].ToString() != "")
                            c.Banner = reader["rurl"].ToString();
                    }
                    list.Add(c);
                }
                reader.Close();
            }
            return list;
        }


        /// <summary>
        /// 逐层获取地理位置区域
        /// </summary>
        /// <returns></returns>
        public List<DistrictModel> GetDistrictModel(int areaID)
        {
            string sqlStr = "select DistrictTable.ID as DistrictID,DistrictTable.DicID as DistrictDicID,"
            + " DistrictTable.ItemName as DistrictItemName ,CityTable.ID as CityID,"
            + " CityTable.DicId as CityDicID,CityTable.ItemName as CityItemName,"
            + " ProvinceTable.ID as ProvinceID,ProvinceTable.DicId as ProvinceDicID,"
            + " ProvinceTable.ItemName as ProvinceItemName,LocationTable.ID as LocationID,"
            + "  LocationTable.DicID as LocationDicID,LocationTable.ItemName as LocationItemName"
            + " from("
            + " SELECT ID ,DicID ,ItemName  FROM  YogaDicItem  where id=" + areaID + " ) DistrictTable "
            + " join  YogaDicItem CityTable on DistrictTable.DicId=CityTable.ID "
            + " join  YogaDicItem ProvinceTable on CityTable.DicId=ProvinceTable.ID"
            + " join  YogaDicItem LocationTable on ProvinceTable.DicId=LocationTable.ID";
            DataTable dt = SQLHelper.ExecuteDataTable(sqlStr, null);
            List<DistrictModel> districtModelList = DataTableHelper.TableToEntity<DistrictModel>(dt);
            return districtModelList;
        }

        /// <summary>
        /// 我发起的活动
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Class> GetClassesByUid(int uid)
        {
            return dbSet.Where(c => c.UserId == uid && c.IsDeleted == false).OrderByDescending(c => c.CreateTime).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="teacherId"></param>
        /// <param name="type">1是老师，2是习练者，3是机构</param>
        /// <returns></returns>
        public List<Class> GetClassesByZhuanYe(int uid, int teacherId, int centerId, int type)
        {
            string sql = "";
            if (type == 1)
                sql = "SELECT top 8  c.Id,c.YogaTypeID,c.Summary,ISNULL(c.Content,'') Content,c.Banner,c.Start"
                 + " ,c.Duration,c.DurationUnit,c.AreaID,c.[Address] ,c.Price,c.Discount,c.IsItem"
                 + "  ,c.ItemClassID,c.[Max],c.ClassType,c.ClassStatus,ISNULL(c.NoPassMsg,'') NoPassMsg"
                 + " ,c.UserId,c.UpdateTime,c.IsDeleted,c.CreateTime,c.Name,c.TopicIds,ISNULL(c.Tags,'') Tags"
                 + " ,ISNULL(c.iReadNums,0) iReadNums,c.CloseTime,c.EndTime,ISNULL(c.MessageDes,'') MessageDes,ISNULL(c.CenterID,'') CenterID"
                 + ",ISNULL( c.FieldID,'') FieldID,r.ClassId FROM ClassTeacher t  join Class c on t.Class_Id=c.Id left join ClassReport r on c.Id=r.ClassId where (t.TeacherId="
                 + teacherId + " or c.UserId=" + uid + ") and c.IsDeleted=0 and (ClassStatus=2 or ClassStatus=3) order by convert(datetime,c.Start) desc";
            if (type == 2)
                sql = "SELECT top 8  c.Id,c.YogaTypeID,c.Summary,ISNULL(c.Content,'') Content,c.Banner,c.Start"
                 + " ,c.Duration,c.DurationUnit,c.AreaID,c.[Address] ,c.Price,c.Discount,c.IsItem"
                 + "  ,c.ItemClassID,c.[Max],c.ClassType,c.ClassStatus,ISNULL(c.NoPassMsg,'') NoPassMsg"
                 + " ,c.UserId,c.UpdateTime,c.IsDeleted,c.CreateTime,c.Name,c.TopicIds,ISNULL(c.Tags,'') Tags"
                 + " ,ISNULL(c.iReadNums,0) iReadNums,c.CloseTime,c.EndTime,ISNULL(c.MessageDes,'') MessageDes,ISNULL(c.CenterID,'') CenterID"
                 + ",ISNULL( c.FieldID,'') FieldID,o.ClassId FROM [Class] c join ( select distinct ClassId,UserId from [Order]) o on c.Id=o.ClassId where ( c.UserId=" + uid
                 + " or o.UserId=" + uid + ") and c.IsDeleted=0 and (ClassStatus=2 or ClassStatus=3) order by convert(datetime,c.Start) desc";
            if (type == 3)//teacherId在此处用来存放pageindex
                sql = "select c.*, r.classid from ( select top 5 * from [Class]   where CenterID like '%'+'|" + centerId
                    + "|'+'%' and IsDeleted=0 and (ClassStatus=2 or ClassStatus=3) "
                 + " and id not in ( select top (5*" + (teacherId-1) + ") id  FROM [Class]  where CenterID like '%'+'|" + centerId
                 + "|'+'%' and IsDeleted=0 and (ClassStatus=2 or ClassStatus=3) "
                 + " order by convert(datetime,Start) desc) ) c  left join ClassReport  r on c.id=r.classid order by convert(datetime,Start) desc";

            List<Class> list = new List<Class>();
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sql, CommandType.Text, null);
            while (reader.Read())
            {
                Class c = new Class();
                c.Id = Convert.ToInt32(reader["id"]);
                c.Name = reader["name"].ToString();
                c.Banner = reader["banner"].ToString();
                c.Start = Convert.ToDateTime(reader["start"]).ToString("yyyy-MM-dd");
                c.CreateTime = Convert.ToDateTime(reader["createtime"]);
                c.EndTime = Convert.ToDateTime(reader["EndTime"]);
                c.Price = decimal.Parse(reader["PRICE"].ToString());
                c.Summary = reader["summary"].ToString();
                c.MessageDes = reader["ClassId"].ToString();
                list.Add(c);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 根据多个classID及userID获取活动列表
        /// </summary>
        /// <param name="strId">多个classID</param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Class> GetClassesByUid(string strId, int uid)
        {
            string[] ids = strId.TrimEnd(',').Split(',');
            List<Class> users = new List<Class>();
            string whereOr = "";
            for (int i = 0; i < ids.Count(); i++)
            {
                if (ids[i] != "")
                {
                    whereOr += "  or  Id =" + Convert.ToInt32(ids[i]);
                }
            }
            string sqlStr = "SELECT *  FROM [iyogakoodb].[dbo].[Class]  where   userId=" + uid + " " + whereOr + "   order by CreateTime desc ";
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sqlStr, CommandType.Text, null);
            while (reader.Read())
            {
                Class user = new Class()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Banner = reader["Banner"].ToString()
                };

                users.Add(user);
            }
            return users;
        }

        /// <summary>
        /// 回顾首页
        /// </summary>
        /// <param name="Orderby">四个排序</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ViewClassGroup> GetClassHuiGuList(string Orderby, string where, int ClassStatus)
        {
            List<ViewClassGroup> list = new List<ViewClassGroup>();
            string OrderByValue = "EndTime";
            if (!string.IsNullOrEmpty(Orderby))
            {
                OrderByValue = Orderby;
            }

            string orderby = "   order by " + OrderByValue + " desc ";

            string sql = GetSql(ClassStatus) + where + orderby;
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sql, CommandType.Text, null);
            while (reader.Read())
            {
                ViewClassGroup user = new ViewClassGroup()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Banner = reader["Banner"].ToString(),
                    Start = reader["Start"].ToString(),
                    TopicIds = reader["TopicIds"].ToString(),
                    AreaID = Convert.ToInt32(reader["AreaID"]),
                    Nums = Convert.ToInt32(reader["Nums"]),
                    InterNums = Convert.ToInt32(reader["InterNums"]),
                    OrderNums = Convert.ToInt32(reader["OrderNums"]),
                    DicId = Convert.ToInt32(reader["DicId"]),
                    AreaName = reader["AreaName"].ToString()

                };

                list.Add(user);
            }
            return list;
        }

        public string GetSql(int ClassStatus)
        {
            #region

            string sql = @"select 
		[Id] 
      ,[Banner]
      ,[Start] 
      ,[AreaID] 
      ,[Name] 
	  ,[TopicIds]  
	  ,EndTime 
	  ,CASE WHEN Nums=null THEN '0'  WHEN Nums is null   THEN '0'    ELSE Nums END Nums  --评论人数 

	  ,CASE WHEN InterNums=null THEN '0'   WHEN InterNums is null   THEN '0'   ELSE InterNums END InterNums  --感兴趣人数 

	  ,CASE WHEN OrderNums=null THEN '0'   WHEN OrderNums is null   THEN '0'   ELSE OrderNums END OrderNums  --参加人数  
	 
	 ,ISNULL( DistrictID ,0) as DistrictID  --地区
	 ,ISNULL( DistrictDicID ,0) as DicId  
	 ,ISNULL( AreaName ,'') as AreaName  
 from (
select a.*, 
	b.Nums ,  
	c.InterNums,
	d.OrderNums, 
	e.DistrictID ,
	e.DistrictDicID,
	e.LocationItemName+'-'+e.ProvinceItemName +'-'+e.CityItemName +'-'+e.DistrictItemName as AreaName
	from 
	(
SELECT   [Id] 
      ,[Banner]
      ,[Start] 
      ,[AreaID] 
      ,[Name] 
	  ,[TopicIds] 
	  ,EndTime 
  FROM [iyogakoodb].[dbo].[Class]  where  IsDeleted=0 and ClassStatus=" + ClassStatus + @"  )  a 
   
  left join ( 
  SELECT    [ToUid] ,count(ToUid) Nums  --评论人数
  FROM [iyogakoodb].[dbo].[tMessage] group by ToUid ) b on a.Id=b.ToUid
left join (
SELECT  [ClassId]  ,count([ClassId]) InterNums  --感兴趣人数
  FROM [iyogakoodb].[dbo].[InterestedClass] group by [ClassId] 
) c on a.Id=c.[ClassId]

left join (
SELECT  [ClassId]  ,count([ClassId]) OrderNums  --参加人数 
  FROM [iyogakoodb].[dbo].[Order] group by [ClassId] 
) d on a.Id=d.ClassId

left  join (
  
  select	DistrictTable.ID as DistrictID,
			DistrictTable.DicID as DistrictDicID, 
            DistrictTable.ItemName as DistrictItemName ,
			CityTable.ID as CityID, 
            CityTable.DicId as CityDicID,
			CityTable.ItemName as CityItemName, 
            ProvinceTable.ID as ProvinceID,
			ProvinceTable.DicId as ProvinceDicID, 
            ProvinceTable.ItemName as ProvinceItemName,
			LocationTable.ID as LocationID, 
            LocationTable.DicID as LocationDicID,
			LocationTable.ItemName as LocationItemName 
            from( 
            SELECT ID ,DicID ,ItemName  FROM  YogaDicItem  ) DistrictTable  
            join  YogaDicItem CityTable on DistrictTable.DicId=CityTable.ID  
            join  YogaDicItem ProvinceTable on CityTable.DicId=ProvinceTable.ID 
            join  YogaDicItem LocationTable on ProvinceTable.DicId=LocationTable.ID

) e on a.AreaID=e.DistrictID  ) t  ";

            #endregion
            return sql;
        }

        public int GetShoudCloseActivityCount()
        {
            return dbSet.Where(p => p.ClassStatus == 2 && p.EndTime <= DateTime.Now).Count();
        }

        /// <summary>
        /// 首页回顾（展示有报道的活动）
        /// </summary>
        /// <returns></returns>
        public List<ViewClassGroup> GetClassesHaveReport()
        {
            List<ViewClassGroup> list = new List<ViewClassGroup>();
            string sql = "select c.* from Class c join("
            + " SELECT top 8 ClassId,max(CreateTime)as CreateTime  FROM ClassReport where IsDeleted=0 group by  ClassId"
            + " order by CreateTime desc) r on c.Id=r.ClassId";
            SqlDataReader reader = SQLHelper.ExecuteDataReader(sql, CommandType.Text, null);
            while (reader.Read())
            {
                ViewClassGroup user = new ViewClassGroup()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Banner = reader["Banner"].ToString(),
                    Start = reader["Start"].ToString(),
                    TopicIds = reader["TopicIds"].ToString(),
                    AreaID = Convert.ToInt32(reader["AreaID"]),
                    //Nums = Convert.ToInt32(reader["Nums"]),
                    //InterNums = Convert.ToInt32(reader["InterNums"]),
                    //OrderNums = Convert.ToInt32(reader["OrderNums"]),
                    //DicId = Convert.ToInt32(reader["DicId"]),
                    //AreaName = reader["AreaName"].ToString()

                };

                list.Add(user);
            }
            return list;
        }

        /// <summary>
        /// 首页展示的活动预告
        /// </summary>
        /// <returns></returns>
        public List<ViewClass> GetClassesAdvance()
        {
            var ls = dbSet.Where(p => p.ClassStatus == 2).GroupBy(a => a.TopicIds).Select(g => (new ViewClass
            {
                TopicIds = g.Key,
                Id = g.Max(item => item.Id),
                Banner = g.Max(item => item.Banner),
                Name = g.Max(item => item.Name)
            }));
            return ls.ToList();
        }

    }
}
