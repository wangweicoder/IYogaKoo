using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Service
{
    public class OrderServiceImpl : IOrderService
    {
        IOrderRepository _repository;

        public OrderServiceImpl(IOrderRepository repository)
        {
            _repository = repository;
        }

        public PageResult<ViewOrder> GetByUser(int userID, int page, int size)
        {
            IQueryable<Order> orderQuery = _repository.Orders.Where(o => o.UserId == userID).OrderByDescending(o => o.CreateTime);
            PageResult<ViewOrder> pr = new PageResult<ViewOrder>();
            pr.RecordCount = orderQuery.Count();
            orderQuery = orderQuery.Skip((page - 1) * size).Take(size);
            pr.Objects = (from o in orderQuery select new ViewOrder { Id = o.Id, Phone = o.Phone, Name = o.Name, Number = o.Number, UserId = o.UserId }).ToList();
            pr.Code = 0;
            pr.Index = page;
            pr.PageSize = size;
            pr.Msg = "";
            return pr;
        }

        /// <summary>
        /// 只查询出有效的订单
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PageResult<ViewOrder> GetByClass(int classID, int page, int size)
        {
            //需要验证订单有效性
            IQueryable<Order> orderQuery = _repository.Orders.Where(o => o.ClassId == classID).Where(p=>p.Amount==0||p.IsPaid==true).OrderByDescending(o => o.CreateTime);
            PageResult<ViewOrder> pr = new PageResult<ViewOrder>();
            pr.RecordCount = orderQuery.Count();
            orderQuery = orderQuery.Skip((page - 1) * size).Take(size);
            pr.Objects = (from o in orderQuery select new ViewOrder { Id = o.Id, Phone = o.Phone, Name = o.Name, Number = o.Number, UserId = o.UserId }).ToList();
            pr.Code = 0;
            pr.Index = page;
            pr.PageSize = size;
            pr.Msg = "";
            return pr;
        }

        public ViewOrder Get(int classID, int userID)
        {
            //Order order = _repository.Orders.FirstOrDefault(o => o.Id == classID && userID == o.UserId);
            Order order = _repository.Orders.FirstOrDefault(o => o.ClassId == classID && userID == o.UserId);
            return ViewOrder.ToViewModel(order);
        }

        public Result Add(ViewOrder order)
        {
            Order _order = ViewOrder.ToEntity(order);
            _order = _repository.Add(_order);
            return new Result(0, "", ViewOrder.ToViewModel(_order));
        }

        public Result Pay(ViewOrder vo)
        {
            return new Result() { Code = 0, Message = "ok", Obj = vo };
        }


        public int AppliedNumber(int classID, int timeout)
        {
            DateTime expired = DateTime.Now.AddMilliseconds(timeout * -1);
            List<Order> orders = _repository.Orders.Where<Order>(o => o.ClassId == classID && o.CreateTime <= expired).ToList();
            int count = 0;
            foreach (var item in orders)
            {
                count += item.Number;
            }
            return count;
        }

        public Result CanApply(int classID, int count)
        {
            return null;
        }


        public ViewOrder Get(int id)
        {
            //Order order = _repository.Orders.FirstOrDefault(o => o.Id == id);
            Order order = _repository.Get(id);

            return ViewOrder.ToViewModel(order);
        }

        public void Edit(ViewOrder vo)
        { }

        public int updateEntity(ViewOrder vo)
        {
            //var entity = dbSet.Find(vo.ID);

            //if (entity != null)
            //{
            //    Context.Entry(entity).State = System.Data.EntityState.Detached;
            //    //这个是在同一个上下文能修改的关键 
            //}

            //entity = model;

            //Update(entity);
            _repository.updateEntity(ViewOrder.ToEntity(vo));
            return _repository.Save();
        }


        public List<ViewOrder> GetOrdersByuid(int uid)
        {
            List<Order> list = _repository.GetOrdersByuid(uid);
            List<ViewOrder> model = new List<ViewOrder>();

            foreach (var item in list)
            {
                model.Add(ViewOrder.ToViewModel(item));
            }
            return model;
        }

        public List<ViewOrder> GetClassId(int UserId)
        {
            List<Order> list = _repository.GetClassId(UserId);

            List<ViewOrder> model = new List<ViewOrder>();

            foreach (var item in list)
            {
                model.Add(ViewOrder.ToViewModel(item));
            }
            return model;
        }


        public int DeleteNO(int userId, int classId)
        {
            return _repository.DeleteNO(userId, classId);
        }


        public int GetOrdersByclassid(int classid)
        {
            return _repository.GetOrdersByclassid(classid);
        }

        public List<ViewOrder> GetOrder(string whereStr, int page, int pagesize, out int count)
        {
            List<Order> list = _repository.GetOrder(whereStr, page, pagesize, out count);
            List<ViewOrder> model = new List<ViewOrder>();

            foreach (var item in list)
            {
                model.Add(ViewOrder.ToViewModel(item));
            }
            return model;
        }
    }
}
