using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Service
{
    public  class LevelOrderServiceImpl:ILevelOrderService
    {
        ILevelOrderRepository _repository;
        public LevelOrderServiceImpl(ILevelOrderRepository repository)
        {
            _repository = repository;
        }

        public int Add(ViewLevelOrder model)
        {
            _repository.Add(ViewLevelOrder.ToEntity(model));
            return _repository.Save();
        }

        public ViewLevelOrder GetUid(int UID)
        {
            LevelOrder lo = _repository.GetUid(UID);
            return ViewLevelOrder.ToViewModel(lo); 
        }

        public List<ViewLevelOrder> BackGetOrdersPageList(string Name, string OrderState, string TargetLevel, string OrderType, 
            int page, int pagesize, out int count)
        {
            List<LevelOrder> list = _repository.BackGetOrdersPageList(Name, OrderState, TargetLevel, OrderType, page, pagesize, out count);
            List<ViewLevelOrder> model = new List<ViewLevelOrder>();
            foreach (var item in list)
            {
                model.Add(ViewLevelOrder.ToViewModel(item));
            }
            return model;
        }
        public List<ViewLevelOrder> GetOrdersPageList(int page, int pagesize, string Ordertype, out int count)
        {
            List<LevelOrder> list = _repository.GetOrdersPageList(page, pagesize, Ordertype, out count);
            List<ViewLevelOrder> model = new List<ViewLevelOrder>();
            foreach (var item in list)
            {
                model.Add(ViewLevelOrder.ToViewModel(item));
            }
            return model;
        }


        public ViewLevelOrder GetById(int id)
        {
            LevelOrder lo = _repository.Get(id);
            return ViewLevelOrder.ToViewModel(lo);
        }

        public int Update(ViewLevelOrder model)
        {
            _repository.updateEntity(ViewLevelOrder.ToEntity(model));
            return _repository.Save();
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                _repository.Delete(_repository.Get(int.Parse(item)));
            }
            return _repository.Save();
        }
    }
}
