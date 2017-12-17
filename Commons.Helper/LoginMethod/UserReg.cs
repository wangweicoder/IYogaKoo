using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Commons.Helper
{
   public class UserReg 
   {
       /// <summary>
       /// 添加注册信息
       /// </summary>
       /// <param name="reg"></param>
       /// <returns></returns>
       public static ViewYogaUser RegLearner(YogaUser reg)
        {
            return null;// UserDB.Reg(reg);
        }
       /// <summary>
       /// 后台添加导师的登录表注册信息
       /// </summary>
       /// <param name="reg"></param>
       /// <returns></returns>
       public static ViewYogaUser BRegLearner(YogaUser reg)
       {
           return null;// UserDB.BReg(reg);
       }
    }
}
