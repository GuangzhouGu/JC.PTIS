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
    public class TrayModelHelper: DBHelper
    {
        public static TrayModel GetModel(string trayCode)
        {
            TrayModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<TrayModel>();
                query = query.Where(it => it.TrayCode == trayCode);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(TrayModel model)
        {
            bool ret = false;
            try
            {
                Delete(model.TrayCode);
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
        public static bool Update(TrayModel model)
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
        public static DataSet GetDataSet(DateTime from, DateTime to,string code="",string waferID="",string lot="")
        {
            DataSet ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<TrayModel>().Where(it => it.EqpInTime >= from && it.EqpInTime <= to);
                if (!string.IsNullOrEmpty(code))
                {
                    query = query.Where(it => it.TrayCode == code);
                }
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
        public static bool Delete(string trayCode)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Deleteable<TrayModel>().Where(it => it.TrayCode == trayCode);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
        public static List<TrayModel> GetTrayList(DateTime from, DateTime to,string code="", string waferID = "", string lot = "")
        {
            List<TrayModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<TrayModel>().Where(it => it.EqpInTime >= from && it.EqpInTime <= to);
                if (!string.IsNullOrEmpty(code))
                {
                    query = query.Where(it => it.TrayCode == code);
                }
                if (string.IsNullOrEmpty(waferID) == false)
                {
                    query = query.Where(it => it.WAFER == waferID);
                }
                if (string.IsNullOrEmpty(lot) == false)
                {
                    query = query.Where(it => it.LOT == lot);
                }
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<TrayModelHelper>(ex);
            }
            return ret;
        }
    }
}
