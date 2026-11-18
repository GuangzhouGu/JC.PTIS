using JC.PTIS.Common;
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
    public class DieModelHelper : DBHelper
    {
        public static event Action<DieModel> UpdateTotalCount;
        public static DieModel GetModel(string dieCode)
        {
            DieModel ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<DieModel>();
                query = query.Where(it => it.DieCode == dieCode);
                ret = query.First();
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static bool Insert(DieModel model)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Insertable(model);
                ret = query.ExecuteCommand() > 0;
                UpdateTotalCount?.BeginInvoke(model, null, null);
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static bool Update(DieModel model)
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
                if (model.TotalResult!=Result.None)
                {
                    UpdateTotalCount?.BeginInvoke(model, null, null);
                }
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static bool Delete(string dieCode)
        {
            bool ret = false;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Deleteable<DieModel>().Where(it => it.DieCode == dieCode);
                ret = query.ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static List<DieModel> GetByTimeRange(DateTime startTime, DateTime endTime,string waferID="", string lot = "")
        {
            List<DieModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<DieModel>();
                query = query.Where(it => it.StartTime >= startTime && it.StartTime <= endTime);
                if (string.IsNullOrEmpty(waferID)==false)
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
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static DataSet GetDataSet(DateTime startTime, DateTime endTime,string result="", string waferID = "", string lot = "")
        {
            DataSet ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<DieModel>();
                query = query.Where(it => it.StartTime >= startTime && it.StartTime <= endTime);
                if (string.IsNullOrEmpty(result)==false)
                {
                    query = query.Where(it => it.DieStatus.ToString() == result);
                }
                var table = query.ToDataTable();
                ret=table.DataSet;
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
        public static List<DieModel> GetDieList()
        {
            List<DieModel> ret = null;
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<DieModel>();
                ret = query.ToList();
            }
            catch (Exception ex)
            {
                Log<DieModelHelper>(ex);
            }
            return ret;
        }
    }

}
