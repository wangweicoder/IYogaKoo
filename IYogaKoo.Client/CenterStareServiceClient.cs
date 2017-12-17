using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Client
{
    public class CenterStareServiceClient : ICenterStareService, IDisposable
    {
        public ICenterStareService CentersstareServiceImpl { get; set; }

        public CenterStareServiceClient()
        {
            CentersstareServiceImpl = new CenterStareServiceImpl(new CenterStareRepository());
        }

        public List<ViewCenterStare> GetCentersPageList(int mid, out int count)
        {
            try
            {
                return CentersstareServiceImpl.GetCentersPageList(mid, out count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int IfUidSave(int uid, int mid)
        {
            try
            {
                return CentersstareServiceImpl.IfUidSave(uid, mid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      

        public int Add(ViewCenterStare model)
        {
            try
            {
                return CentersstareServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Dispose()
        {

        }
    }
}
