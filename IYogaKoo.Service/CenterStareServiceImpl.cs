using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
namespace IYogaKoo.Service
{
    public class CenterStareServiceImpl:ICenterStareService
    {
        ICenterStareRepository Repository;
        public CenterStareServiceImpl(ICenterStareRepository Repository)
        {
            this.Repository = Repository;
        }



        List<ViewCenterStare> ICenterStareService.GetCentersPageList(int mid, out int count)
        {
            List<CenterStare> list = Repository.GetCentersPageList(mid, out count);

            List<ViewCenterStare> model = new List<ViewCenterStare>();

            foreach (var item in list)
            {
                model.Add(ViewCenterStare.ToViewModel(item));
            }
            return model;
        }

        public int IfUidSave(int uid, int mid)
        {
            return Repository.IfUidSave(uid, mid);
        } 

        public int Add(ViewCenterStare model)
        {
            Repository.Add(ViewCenterStare.ToEntity(model));
            return Repository.Save();
        }
    }
}
