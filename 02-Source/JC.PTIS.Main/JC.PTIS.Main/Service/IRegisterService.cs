using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Service
{
     public interface IRegisterService
    {
        /// <summary>
        /// 设置bool寄存器，置位true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        void SetBoolReg<T>(T index);
        /// <summary>
        /// 设置bool寄存器，复位False
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        void ResetBoolReg<T>(T index, [CallerFilePath] string filePath = "", [CallerMemberName] string name = "", [CallerLineNumber] int num = 0);
        /// <summary>
        /// 复位所有Bool寄存器
        /// </summary>
        void ResetAllBoolReg();
        /// <summary>
        /// 获取指定Bool寄存器的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        bool GetBoolReg<T>(T index);
        /// <summary>
        /// 设置Double寄存器的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="val"></param>
        void SetDoubleReg<T>(T index, double val);
        /// <summary>
        /// 清除Double寄存器的值（0）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        void ResetDoubleReg<T>(T index);
        /// <summary>
        /// 获取指定Double寄存器的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        double GetDoubleReg<T>(T index);
        /// <summary>
        /// 清除所有Double寄存器的值
        /// </summary>
        void ResetAllDoubleReg();
    }
}
