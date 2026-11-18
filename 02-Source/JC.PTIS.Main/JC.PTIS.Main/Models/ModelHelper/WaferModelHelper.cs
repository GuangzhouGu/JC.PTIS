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
    public class WaferModelHelper : DBHelper
    {
        public static WaferModel GetModel(string waferCode)
        {
            WaferModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<WaferModel>();
                query = query.Where(it => it.WAFER == waferCode);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<WaferModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(WaferModel model)
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
        public static DataSet GetDataSet(DateTime from, DateTime to, string waferCode = "", string lot = "")
        {
            DataSet ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<WaferModel>().Where(it => it.LOT == lot);
                if (string.IsNullOrEmpty(waferCode) == false)
                {
                    query = query.Where(it => it.WAFER == waferCode);
                }
                if (string.IsNullOrEmpty(lot) == false)
                {
                    query = query.Where(it => it.LOT == lot);
                }
                ret = query.ToDataTable().DataSet;
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static bool Update(WaferModel model)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Updateable(model);
                ret = query.ExecuteCommand() > 0;
                if (ret == false)
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
        public static bool Delete(string waferCode)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Deleteable<WaferModel>().Where(it => it.WAFER == waferCode);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<WaferModelHelper>(ex);
            }
            return ret;
        }
        public static List<WaferModel> GetLotList()
        {
            List<WaferModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<WaferModel>();
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<WaferModelHelper>(ex);
            }
            return ret;
        }
    }
}
