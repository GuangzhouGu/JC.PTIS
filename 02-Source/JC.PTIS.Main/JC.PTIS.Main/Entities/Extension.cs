using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.MotionInfo;
using StdLib.HTTools;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Entities
{
    public static class Extension
    {
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }
        public static T DeepCopy<T>(this T value)
        {
            return HTDeepCopyHelper.DeepCopyWithBinarySerializer(value);
        }
        public static string GetDescription(this Enum value)
        {
            return HTEnumHelper.GetEnumDesc(value);
        }
        /// <summary>
        /// 把 List<T> 转成 DataSet（表名默认取 T 的类名）
        /// </summary>
        public static DataSet ToDataSet<T>(this IList<T> list, string tableName = null)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var ds = new DataSet();
            var dt = new DataTable(tableName ?? typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead)
                                  .ToArray();

            // 1. 建列（可空类型取底层类型）
            foreach (var prop in props)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dt.Columns.Add(prop.Name, type);
            }

            // 2. 填数据
            foreach (var item in list)
            {
                var row = dt.NewRow();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(item, null);
                    row[prop.Name] = value ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            return ds;
        }
    }
 
    public static class StringExt
    {
        //给 string 增加扩展方法 IsNullOrEmptyEx
        public static bool IsNullOrEmptyEx(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
    public static class CollectionExt
    {
        //判断集合是否为null或者空
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
    public static class DateTimeExt
    {
        public static bool IsToday(this DateTime dt)
        {
            return dt.Date == DateTime.Now.Date;
        }
    }
}
