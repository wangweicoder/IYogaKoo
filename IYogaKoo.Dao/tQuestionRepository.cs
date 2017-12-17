using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{
    public class tQuestionRepository : Repository<tQuestion>, ItQuestionRepository
    {
        public tQuestion GetById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }

        public int Edit(tQuestion entity)
        {
            var tempEntity = dbSet.Find(entity.ID);

            if (tempEntity != null)
            {
                Context.Entry(tempEntity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            tempEntity = entity;

            Update(tempEntity);

            return Save();
        }

        public List<tQuestion> GetList(string whereStr, int page, int pagesize, out int count)
        {

            whereStr = whereStr.TrimEnd(',');
            string[] whereArray = whereStr.Split(',');
            IQueryable<tQuestion> linq = dbSet.OrderByDescending(a => a.TitleID);

            if (!string.IsNullOrWhiteSpace(whereStr))
            {
                foreach (string item in whereArray)
                {
                    if (item.Contains("IsFAQ"))
                    {
                        bool value = bool.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.IsFAQ == value);
                    }
                    if (item.Contains("IsDelete"))
                    {
                        bool value = bool.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.IsDelete == value);
                    }
                    if (item.Contains("Hot"))
                    {
                        bool value = bool.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.Hot == value);
                    }
                    if (item.Contains("BeFrom"))
                    {
                        var value = int.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.BeFrom == value);
                    }
                    if (item.Contains("TitleID"))
                    {
                        var value = int.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.TitleID == value);
                    }
                    if (item.Contains("Uid"))
                    {
                        var value = int.Parse(item.Split('!').Last());
                        linq = linq.Where(p => p.Uid == value);
                    }
                    if (item.Contains("QuestionContent"))
                    {
                        var value = item.Split('!').Last();
                        linq = linq.Where(p => p.QuestionContent.Contains(value));
                    }
                }
            }
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }


        public int Delete(string deletelist)
        {
            throw new NotImplementedException();
        }
    }
}
