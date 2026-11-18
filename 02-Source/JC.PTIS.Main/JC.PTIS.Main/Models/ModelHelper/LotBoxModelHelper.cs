using JC.PTIS.Main.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models.ModelHelper
{
    public class LotBoxModelHelper : DBHelper
    {
        public static LotBoxModel GetModel(string lotBoxCode)
        {
            LotBoxModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotBoxModel>();
                query = query.Where(it => it.LotBoxCode == lotBoxCode);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<LotBoxModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(LotBoxModel model)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Insertable(model);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static DataSet GetDataSet(DateTime from, DateTime to, string lot = "")
        {
            DataSet ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotBoxModel>().Where(it => it.LOT == lot);
                ret = query.ToDataTable().DataSet;
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static bool Update( LotBoxModel model)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Updateable(model);
                ret = query.ExecuteCommand() > 0;
                if (ret==false)
                {
                    Insert(model);
                }
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static bool Delete(string lotCode)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Deleteable<LotBoxModel>().Where(it => it.LotBoxCode == lotCode);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<LotBoxModelHelper>(ex);
            }
            return ret;
        }
        public static List<LotBoxModel> GetLotList()
        {
            List<LotBoxModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotBoxModel>();
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<LotBoxModelHelper>(ex);
            }
            return ret;
        }
    }
}
