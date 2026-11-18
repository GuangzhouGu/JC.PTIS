using JC.PTIS.Common;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using Newtonsoft.Json.Linq;
using StdLib.LogHelper;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JC.PTIS.Main.Service
{
    /// <summary>
    /// 系统寄存器操作,目前支持bool和double两种；
    /// </summary>
    public class RegisterService : Singleton<RegisterService>, IRegisterService
    {
        internal readonly int[] SysBoolReg = new int[1000];
        internal readonly double[] SysDoubleReg = new double[100];
        internal readonly int[] SysIntReg = new int[100];
        internal readonly object[] SysObjReg = new object[100];
        public  bool IsManualMode { get; set; } = false;
   
        #region BoolReg
        public void SetBoolReg<T>(T index)
        {
            if (IsManualMode)
            {
                return;
            }
            var ret = GetIndex(index);
            if (ret == -1||ret>=SysBoolReg.Length)
            {
                return;
            }
            Interlocked.Exchange(ref SysBoolReg[ret], 1);
            LogManager.GetLogger("RegisterService").Info($"信号：{index} 置位");
        }
        public bool GetBoolReg<T>(T index)
        {
            var ret = GetIndex(index);
            if (ret == -1 || ret >= SysBoolReg.Length)
            {
                return false;
            }
            var value= Interlocked.CompareExchange(ref SysBoolReg[ret], 0, 0) == 1;
            return value;
        }

        public void ResetBoolReg<T>(T index, [CallerFilePath] string filePath = "", [CallerMemberName] string name = "", [CallerLineNumber] int num = 0)
        {
            var ret = GetIndex(index);
            if (ret == -1 || ret >= SysBoolReg.Length)
            {
                return ;
            }
            Interlocked.Exchange(ref SysBoolReg[ret], 0);
            LogManager.GetLogger("RegisterService").Info($"信号：{index} 复位。文件：{filePath}，方法：{name}，行号：{num}");
        }
        public void ResetAllBoolReg()
        {
            for (int i = 0; i < SysBoolReg.Count(); i++)
            {
                SysBoolReg[i] = 0;
            }
            //for (int i = 0; i < RegisterEnum.None.ToInt(); i++)
            //{
            //    var regs = RedisHelper.Instance.StringSet($"{(RegisterEnum)i}","0");
            //}
        }

        #endregion

        public void SetIntValue<T>(T index, int value)
        {
            if (IsManualMode)
            {
                return;
            }
            var ret = GetIndex(index);
            if (ret == -1 || ret >= SysIntReg.Length)
            {
                return;
            }
            Interlocked.Exchange(ref SysIntReg[ret], value);
        }
        public int GetIntValue<T>(T index)
        {
            var ret = GetIndex(index);
            if (ret == -1 || ret >= SysIntReg.Length)
            {
                return 0;
            }
            var value= Interlocked.CompareExchange(ref SysIntReg[ret], 0, 0);
            return value;
        }
        public void ResetIntValue<T>(T index)
        {
            var ret = GetIndex(index);
            if (ret == -1 || ret >= SysIntReg.Length)
            {
                return;
            }
            Interlocked.Exchange(ref SysIntReg[ret], 0);
        }
        #region DoubleReg
        public void SetDoubleReg<T>(T index, double val)
        {
            int k = 0;
            if (index.GetType() == typeof(int) || index.GetType() == typeof(Enum))
            {
                k = Convert.ToInt32(index);
                Interlocked.Exchange(ref SysDoubleReg[k], val);
            }
        }
        public double GetDoubleReg<T>(T index)
        {
            int k = 0;
            if (index.GetType() == typeof(int) || index.GetType() == typeof(Enum))
            {
                k = Convert.ToInt32(index);
                return Interlocked.CompareExchange(ref SysDoubleReg[k], 0, 0);
            }
            return 0;
        }
        public void ResetDoubleReg<T>(T index)
        {
            int k = 0;
            if (index.GetType() == typeof(int) || index.GetType() == typeof(Enum))
            {
                k = Convert.ToInt32(index);
                Interlocked.Exchange(ref SysDoubleReg[k], 0);
            }
            return;
        }
        public void ResetAllDoubleReg()
        {
            for (int i = 0; i < SysDoubleReg.Count(); i++)
            {
                SysDoubleReg[i] = 0;
            }
        }
        #endregion
        private int GetIndex<T>(T index)
        {
            if (index == null)
            {
                return -1;
            } 
            var runtimeType = index.GetType();
            if (runtimeType == typeof(int) || runtimeType.IsEnum)
            {
                int k = Convert.ToInt32(index);             
                return k;
            }
            return -1;
        }

        private void RestoreReg()
        {
            try
            {
                for (int i = 0; i < RegisterEnum.None.ToInt(); i++)
                {
                    var regs = RedisHelper.Instance.StringGet($"{(RegisterEnum)i}");
                    SysBoolReg[i] = regs == "1" ? 1 : 0;
                }
                for (int i = 0; i < IntValueEnum.None.ToInt(); i++)
                {
                    var regs = RedisHelper.Instance.StringGet($"{(IntValueEnum)i}");
                    SysIntReg[i] = int.Parse(regs);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("RegisterService").Info($"{ex}");
            }
        }
    }
}
