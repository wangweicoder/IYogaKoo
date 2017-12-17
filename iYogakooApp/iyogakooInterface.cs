using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iYogakooApp
{
     
    /// <summary>
    /// 接口类
    /// </summary>
    [ServiceContract]
    public interface iyogakooInterface
    {
        //YogaUser 用户
        [OperationContract]
        int AddUser(string objectId, string salt, string username, string password, string phone);
         
    }
}
