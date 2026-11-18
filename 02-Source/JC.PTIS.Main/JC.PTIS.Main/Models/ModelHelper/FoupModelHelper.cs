using JC.PTIS.Common;
using JC.PTIS.Main.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models.ModelHelper
{
    public class FoupModelHelper: DBHelper
    {
        public static FoupModel GetFoup(int foupNo)
        {
            FoupModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<FoupModel>();
                query = query.Where(p=>p.FoupNo==foupNo).OrderByDescending(p=>p.InEqpTime);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<FoupModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(FoupModel foup)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Insertable(foup);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<FoupModelHelper>(ex);
            }
            return ret;
        }
        public static bool Update(int foupNo,FoupModel foup)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Updateable(foup).Where(it => it.FoupNo == foupNo);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<FoupModelHelper>(ex);
            }
            return ret;
        }   
        public static bool Delete(int foupNo)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Deleteable<FoupModel>().Where(it => it.FoupNo == foupNo);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<FoupModelHelper>(ex);
            }
            return ret;
        }
        public static List<FoupModel> GetFoupList()
        {
            List<FoupModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<FoupModel>();
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<FoupModelHelper>(ex);
            }
            return ret;
        }
    }
}
