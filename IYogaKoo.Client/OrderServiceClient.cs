using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Client
{
    public class OrderServiceClient : IOrderService, IDisposable
    {

        private IOrderService Impl { get; set; }
        //订单有效时间
        private const int ORDER_TIME_OUT = 15 * 60 * 1000;

        public OrderServiceClient()
        {
            Impl = new OrderServiceImpl(new OrderRepository());
        }

        public ViewOrder Get(int classID, int userID)
        {
            return Impl.Get(classID, userID);
        }

        public Result Add(ViewOrder order)
        {
            Result result = CanApply(order.ClassId, order.Number);
            if (result.Code == 0)
            {
                ClassServiceClient cc = new ClassServiceClient();
                ViewClass vc = cc.Get(order.ClassId);
                order.Amount = (double)vc.Price * order.Number;
                return Impl.Add(order);
            }
            else
                return result;
        }

        public PageResult<ViewOrder> GetByUser(int userID, int page, int size)
        {
            return Impl.GetByUser(userID, page, size);
        }

        public PageResult<ViewOrder> GetByClass(int classID, int page, int size)
        {
            return Impl.GetByClass(classID, page, size);
        }

        public Result Pay(ViewOrder vo)
        {
            return Impl.Pay(vo);
        }

        public void Dispose()
        {

        }


        public int AppliedNumber(int classID, int timeout)
        {
            return Impl.AppliedNumber(classID, timeout);
        }


        public Result CanApply(int classID, int count)
        {
            ClassServiceClient cc = new ClassServiceClient();
            ViewClass vc = cc.Get(classID);

            if (vc.ClassStatus == 2)
            {
                int hasAppliedNumber = AppliedNumber(classID, ORDER_TIME_OUT);
                if (hasAppliedNumber < vc.Max)
                {
                    if (hasAppliedNumber + count <= vc.Max)
                    {
                        if (DateTime.Parse(vc.Start) > DateTime.Now)
                        {
                            return new Result(0, "OK");
                        }
                        else
                            return new Result(1, "活动已经开始了");
                    }
                    else
                    {
                        return new Result(1, string.Format("名额不足，还剩 {0} 个名额", (vc.Max - hasAppliedNumber - count)));
                    }
                }
                else
                    return new Result(1, "报名已满");
            }
            else
                return new Result(1, "活动已不能报名！");
        }



        public ViewOrder Get(int id)
        {
            return Impl.Get(id);
        }


        public void Edit(ViewOrder vo)
        {
            Impl.Edit(vo);
        }
        public int updateEntity(ViewOrder vo)
        {
            return Impl.updateEntity(vo);
        }

        /// <summary>
        /// 获取我参加的活动
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<ViewOrder> GetOrdersByuid(int uid)
        {
            return Impl.GetOrdersByuid(uid);
        }

        public List<ViewOrder> GetClassId(int UserId)
        {
            return Impl.GetClassId(UserId);
        }


        public int DeleteNO(int userId, int classId)
        {
            try
            {
                return Impl.DeleteNO(userId, classId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetOrdersByclassid(int classid)
        {
            try
            {
                return Impl.GetOrdersByclassid(classid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ViewOrder> GetOrder(string whereStr, int page, int pagesize, out int count)
        {
            try
            {
                return Impl.GetOrder(whereStr, page, pagesize, out  count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
