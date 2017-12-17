using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;

namespace iYogakooApp
{ 
    /// <summary>
    /// 接口类实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class yogakooInterface : iyogakooInterface
    {
        #region iyogakooInterface 成员
        
        /// <summary>
        /// 是否存在该信息
        /// </summary>
        /// <param name="objectId">App主键</param>
        /// <returns>0-表示不存在；否则存在</returns>
        //public int iYogakooExists(string objectId)
        //{
        //    string sqlExists = "select count(*) from YogaUser where objectId='" + objectId + "' and InputType=1";
        //    int iCountExists = Convert.ToInt32(SQLHelper.ExecuteDataTable(sqlExists).Rows[0][0]);
        //    return iCountExists;
        //}

        /// <summary>
        /// App端用户表--添加方法
        /// </summary>
        /// <param name="objectId">主键objectId（*必传项：根据objectId判断是否存在该用户*）</param>
        /// <param name="salt">自定义字符串salt</param>
        /// <param name="username">用户昵称</param>
        /// <param name="password">88位密码（*不带转义符*)</param>
        /// <param name="phone">联系电话</param>
        /// <returns>返回值：0 添加失败，1 添加成功,2 存在该用户（objectId）不能添加</returns>
        public int AddUser(string objectId, string salt, string username,string password, string phone)
        {
            //InputType=1 表示 App端转入PC端
            string sqlExists = "select count(*) from YogaUser where objectId='" + objectId + "' and InputType=1";
            int iCountExists = Convert.ToInt32(SQLHelper.ExecuteDataTable(sqlExists).Rows[0][0]);
            int iCount = 0;
            if (iCountExists == 0)
            {
                #region

                string sql = @"INSERT INTO [dbo].[YogaUser]
                            ([UEmail]
                            ,[Uphone]
                            ,[Pwd]
                            ,[NickName]
                            ,[RegDate]
                            ,[UStatus]
                            ,[IsAssessor]
                            ,[IsWebworkers] 
                            ,[LoginTimes]
                            ,[UserType]
                            ,[LoginType] 
                            ,[salt]
                            ,[objectId]
                            ,[InputType])
                         VALUES
                               ('','" + phone + "','" + password + "','" + username + "','" + DateTime.Now + "',0,0,0,0,0,1,'" + salt + "','" + objectId + "',1)";

                #endregion

                iCount = SQLHelper.ExecuteNonquery(sql);
                if (iCount == 1)
                {
                    //添加到详情表
                    string sql2 = "select * from YogaUser where objectId='" + objectId + "' and InputType=1";
                    DataTable dt=  SQLHelper.ExecuteDataTable(sql2);
                    int Uid = Convert.ToInt32(dt.Rows[0]["Uid"]);
                    
                    string sqlDetails = "insert into YogaUserDetail(UID,CreateTime,Ulevel,Uscore) values(" + Uid + ",'"+DateTime.Now+"',0,0)";
                    iCount = SQLHelper.ExecuteNonquery(sqlDetails);
                }
            }
            else {
                iCount = 2;
            }
            return iCount;
        }
         
        #endregion

       
    }
}
