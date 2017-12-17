using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel
{
    /// <summary>
    /// 分页返回结果模型
    /// </summary>
    public class PageResult<TEntity>
    {
        public PageResult()
        {
            Code = 0;
            Msg = "";
            Index = 1;
            Objects = null;
            RecordCount = 0;
            _pageSize = 10;
        }

        public PageResult(int code, string msg, int index = 1, int size = 10, int recordCount = 0, List<TEntity> objects = null)
        {
            Code = code;
            msg = Msg;
            Index = index;
            Objects = objects;
            RecordCount = recordCount;
            _pageSize = size;
        }
        public int Index { get; set; }
        public int Code { get; set; }

        public string Msg { get; set; }

        public List<TEntity> Objects { get; set; }

        public int RecordCount { get; set; }

        private int _pageSize;
        public int PageSize 
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling(RecordCount / (double)_pageSize);
            }
        }
    }
}
