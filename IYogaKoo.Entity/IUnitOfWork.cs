using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Entity
{
    interface IUnitOfWork
    {
        //事务提交      
        int Save();
    }
}
