using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace iyogakooWebService
{
    /// <summary>
    /// iyogakooService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class iyogakooService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        /// <summary>
        /// App端用户表--添加方法
        /// </summary>
        /// <param name="objectId">主键objectId（*必传项：根据objectId判断是否存在该用户*）</param>
        /// <param name="salt">自定义字符串salt</param>
        /// <param name="username">用户昵称</param>
        /// <param name="password">88位密码（*不带转义符*)</param>
        /// <param name="phone">联系电话</param>
        /// <param name="sex">姓别(男/女)</param>
        /// <returns>返回值：-1 :值的传不能为空：-88 :密码长度!=88位 ;0 报错（没有执行添加/更新)，11 添加失败;22 更新失败；1 添加成功,2 更新成功</returns>
        [WebMethod]
        public int AddUser(string objectId, string salt, string username, string password, string phone, string sex)
        {
            DateTime dtTime = DateTime.Now;
            ///错误提示
            int ierror = 0;
            ///成功提示
            int iSuccess = 0;
            int iCount = 0;
            if (string.IsNullOrEmpty(objectId) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(sex))
            {
                iSuccess = -1;
                return iSuccess;
            }
            else if (password.Length!=88)
            {
                iSuccess = -88;
                return iSuccess;
            }
            else
            {
                try
                {
                    int Gender = 0;//女
                    if (sex.Trim() == "男")
                    {
                        Gender = 1;
                    } 
                    //InputType=1 表示 App端转入PC端
                    string sqlExists = "select count(*) from YogaUser where objectId='" + objectId + "' and InputType=1";
                    int iCountExists = Convert.ToInt32(SQLHelper.ExecuteDataTable(sqlExists).Rows[0][0]);

                    if (iCountExists == 0)
                    {
                        //添加
                        ierror = 1;
                        #region

                        string sql = @"INSERT INTO [dbo].[YogaUser]
                            ([UEmail]
                            ,[Uphone]
                            ,[Pwd]
                            ,[NickName]
                            ,[RegDate]
                            ,[LastDate]
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
                               ('','" + phone + "','" + password + "','" + username + "',getdate(),getdate(),1,0,0,0,0,1,'" + salt + "','" + objectId + "',1)";

                        #endregion

                        iCount = SQLHelper.ExecuteNonquery(sql);
                        if (iCount == 1)
                        {
                            iSuccess = 1;
                            SQLHelper.WriteTextLogSuccess("添加用户", "成功" + " " + dtTime);

                            #region 添加到详情表
                            string sql2 = "select * from YogaUser where objectId='" + objectId + "' and InputType=1";
                            DataTable dt = SQLHelper.ExecuteDataTable(sql2);
                            int Uid = Convert.ToInt32(dt.Rows[0]["Uid"]);

                            string sqlDetails = "insert into YogaUserDetail(UID,Gender,CreateTime,Ulevel,Uscore) values(" + Uid + "," + Gender + ",getdate(),0,0)";
                            iCount = SQLHelper.ExecuteNonquery(sqlDetails);
                            if (iCount > 0)
                            {
                               // iSuccess = 3;
                                SQLHelper.WriteTextLogSuccess("添加用户详情", "成功" + " " + dtTime);
                            }
                            else
                            {
                                //iSuccess = 33;
                                SQLHelper.WriteTextLogFail("添加用户详情", "失败" + " " + dtTime);
                            }
                            #endregion

                        }
                        else {
                            iSuccess = 11;
                            SQLHelper.WriteTextLogFail("添加用户", "失败 objectId=" + objectId + " " + dtTime);
                        }
                    }
                    else
                    {
                        #region 更新

                        ierror = 2;
                        string sqlupdate = @"UPDATE [dbo].[YogaUser]
                                           SET [Uphone] ='" + phone 
                                           + "',[Pwd] ='" + password 
                                           + "',[NickName] ='" + username
                                           + "',[salt] ='" + salt
                                           + "'  where objectId='" + objectId + "' and InputType=1";

                        iCount = SQLHelper.ExecuteNonquery(sqlupdate);
                        if (iCount > 0)
                        {
                            iSuccess = 2;
                            SQLHelper.WriteTextLogSuccess("更新用户", "成功" + " " + dtTime);
                        }
                        else
                        {
                            iSuccess = 22;
                            SQLHelper.WriteTextLogFail("更新用户", "失败 objectId=" + objectId + " " + dtTime);
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (ierror == 1)
                    {
                        SQLHelper.WriteTextLogFail("添加用户", "失败 ：objectId=" + objectId + " 原因：" + ex.Message + " " + dtTime);
                    }
                    else if (ierror == 2)
                    {
                        SQLHelper.WriteTextLogFail("更新用户", "失败 ：objectId=" + objectId + " 原因：" + ex.Message + " " + dtTime);
                    }
                    else
                    {
                        SQLHelper.WriteTextLogFail("用户添加/更新报错：", " objectId=" + objectId + " " + ex.Message + " " + dtTime);
                    }
                }
            }
            return iSuccess;
        }
         
        /// <summary>
        /// 添加日志 要求每天分两次定时转数据到PC端数据库
        /// </summary>
        /// <param name="userId">App用户表主键(objectId)</param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <returns>返回值：0：报错（没有执行添加/更新)；11 添加失败;22 更新失败；1 添加成功,2 更新成功 </returns>
        [WebMethod]
        public int AddWriteLog(string userId,string text,string title)
        { 
            int iCount = 0;
            ///错误提示
            int ierror = 0;
            ///成功提示
            int iSuccess = 0;
            DateTime dtTime = DateTime.Now;
            try
            {
                string sqluser = "select * from YogaUser where objectId='" + userId + "' and InputType=1";
                DataTable dt = SQLHelper.ExecuteDataTable(sqluser);
                int Uid = Convert.ToInt32(dt.Rows[0]["Uid"]);

                string sqlExitis = "select count(*) from tWriteLog where Uid=" + Uid + " and sTitle='" + title + "' and ValueType=1";
                DataTable dtExitis = SQLHelper.ExecuteDataTable(sqlExitis);
                if (Convert.ToInt32(dtExitis.Rows[0][0]) == 0)
                {
                    #region 添加到日志表 ValueType=1  app端

                    ierror = 1; 

                    string sql = @"INSERT INTO [dbo].[tWriteLog]
                           ([Uid]
                           ,[sTitle]
                           ,[sContent]
                           ,[ifShow]
                           ,[iReadNums]
                           ,[ifpush]
                           ,[CreateDate]
                           ,[ValueType])
                        VALUES
                             (" + Uid + ",'" + title + "','" + text + "',1,0,0,getdate(),1)";

                    iCount = SQLHelper.ExecuteNonquery(sql);
                    if (iCount > 0)
                    {
                        iSuccess = 1;
                        SQLHelper.WriteTextLogSuccess("添加日志", "成功" + " " + dtTime);
                    }
                    else {
                        iSuccess = 11;
                        SQLHelper.WriteTextLogFail("添加日志", "失败 ：userId=" + userId +  " " + dtTime);
                    }

                    #endregion
                }
                else
                {
                    #region 更新

                    ierror = 2;
                    string sql = "UPDATE [dbo].[tWriteLog] SET  [sContent] = '" + text + "' where Uid=" + Uid + " and sTitle='" + title + "' and ValueType=1";
                    iCount = SQLHelper.ExecuteNonquery(sql);
                    if (iCount > 0)
                    {
                        iSuccess = 2;
                        SQLHelper.WriteTextLogSuccess("更新日志", "成功" + " " + dtTime);
                    }
                    else
                    {
                        iSuccess = 22;
                        SQLHelper.WriteTextLogFail("更新日志", "失败 ：userId=" + userId +  " " + dtTime);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            { 
                if (ierror == 1) {
                    SQLHelper.WriteTextLogFail("添加日志", "失败 ：userId=" + userId + " 原因：" + ex.Message + " " + dtTime);
                }
                else if (ierror == 2)
                {
                    SQLHelper.WriteTextLogFail("更新日志", "失败 ：userId=" + userId + " 原因：" + ex.Message + " " + dtTime);
                }
                else {
                    SQLHelper.WriteTextLogFail("日志添加/更新报错：", "失败 ：userId=" + userId + " " +ex.Message + " " + dtTime);
                }
            }
            return iSuccess;
        }
    }
}
