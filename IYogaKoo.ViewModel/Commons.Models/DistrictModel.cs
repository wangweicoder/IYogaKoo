using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    public class DistrictModel
    {
        public int DistrictID { get; set; }
        public int DistrictDicID { get; set; }
        public string DistrictItemName { get; set; }
        public int CityID { get; set; }
        public int CityDicID { get; set; }
        public string CityItemName { get; set; }
        public int ProvinceID { get; set; }
        public int ProvinceDicID { get; set; }
        public string ProvinceItemName { get; set; }
        public int LocationID { get; set; }
        public int LocationDicID { get; set; }
        public string LocationItemName { get; set; }
    }
}
