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
    public class LotModelHelper : DBHelper
    {
        public static LotModel GetModel(string lotCode)
        {
            LotModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotModel>();
                query = query.Where(it => it.LotCode == lotCode);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<LotModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(LotModel model)
        {
            bool ret = false;
            try
            {
                Delete(model.LotCode);
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
        public static DataSet GetDataSet(DateTime from, DateTime to,string waferID="", string lot = "")
        {
            DataSet ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotModel>().Where(it => it.LOT == lot);
                if (string.IsNullOrEmpty(waferID) == false)
                {
                    query = query.Where(it => it.WAFER == waferID);
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
        public static bool Update( LotModel model)
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
                var query = db.Deleteable<LotModel>().Where(it => it.LotCode == lotCode);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<LotModelHelper>(ex);
            }
            return ret;
        }
        public static List<LotModel> GetLotList()
        {
            List<LotModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<LotModel>();
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<LotModelHelper>(ex);
            }
            return ret;
        }
    }
}
