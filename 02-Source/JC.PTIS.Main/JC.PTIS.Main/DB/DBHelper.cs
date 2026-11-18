using JC.PTIS.Main.Models;
using SqlSugar;
using StdLib.HTAlarm.DB.Model;
using StdLib.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.DB
{
    public class DBHelper
    {
        static string ConnectionString = string.Empty;
        static SqlSugar.DbType DbType = SqlSugar.DbType.SqlServer;
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = DbType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnError = (exp) =>
                    {
                        LogManager.GetLogger("DB").Error(exp.ToString());
                    }
                }
            });
            return db;
        }

        public static DBResult Init(SqlSugar.DbType dbType, string connectionString)
        {
            DbType = dbType;
            ConnectionString = connectionString;
            DBResult result = new DBResult();
            try
            {
                FileInfo info = new FileInfo(ConnectionString);
                SqlSugarClient db = GetInstance();
                if (!info.Exists)
                {
                    db.DbMaintenance.CreateDatabase();
                }
                db.CodeFirst.InitTables<FoupModel>();
                db.CodeFirst.InitTables<TrayModel>();
                db.CodeFirst.InitTables<WaferModel>();
                db.CodeFirst.InitTables<LotModel>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.ToString();
            }
            return result;
        }
        public static void Log<T>(Exception ex)
        {
            LogManager.GetLogger("DB").Error($"{typeof(T).Name}->{ex.ToString()}");
        }
        public static DBResult Insert<T>(T model) where T : class, new()
        {
            DBResult result = new DBResult();
            try
            {
                SqlSugarClient db = GetInstance();
                db.Insertable(model).ExecuteCommand();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 用于数据查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="panelID"></param>
        /// <returns></returns>
        public static DBResult GetModelList<T>(DateTime start, DateTime end, string panelID = "")
        {
            DBResult result = new DBResult();
            try
            {
                SqlSugarClient db = GetInstance();
                var query = db.Queryable<T>();
                var entityType = typeof(T);
                var startTimeProp = entityType.GetProperty("StartTime");
                var endTimeProp = entityType.GetProperty("EndTime");
                //var panelIdProp = entityType.GetProperty("PanelID");

                if (startTimeProp != null && endTimeProp != null)
                {
                    query = query.Where($"StartTime >= @start AND EndTime <= @end", new { start, end });
                }
                //if (!string.IsNullOrEmpty(panelID) && panelIdProp != null)
                //{
                //    query = query.Where("PanelID = @panelID", new { panelID });
                //}
                var s = query.ToDataTable();
                result.Result = s;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.ToString();
            }
            return result;
        }
        public static bool ClearTable<T>() where T : class, new()
        {
            SqlSugarClient db = GetInstance();

            // 泛型参数必须是类型，直接使用 T，而不是 typeof(T) 返回的变量。
            var query = db.Deleteable<T>();
            return query.ExecuteCommand() > 0;
        }
    }
}
