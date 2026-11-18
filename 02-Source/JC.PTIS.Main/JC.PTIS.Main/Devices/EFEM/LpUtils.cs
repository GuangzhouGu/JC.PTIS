using JC.PTIS.LoadPort.Devices.EFEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Map.Devices.EFEM
{
    public static class LpUtils
    {
        public static string GetErrorMessage(LPCodes lpCode)
        {
            switch (lpCode)
            {
                case LPCodes.SUCCESS: return "操作成功";
                case LPCodes.EXCEPTION: return "运行异常";
                case LPCodes.CommunicationError: return "通信错误";
                case LPCodes.InvalidLpCode: return "未知错误码";
                case LPCodes.InvalidMessageFormat: return "消息格式错误";
                case LPCodes.NotConnected: return "LoadPort未连接";
                case LPCodes.CannotSwitchFoupType: return "当前Lp上有Foup，不允许切换Foup类型";
                case LPCodes.LpIsRunning: return "LoadPort不是空闲状态";
                case LPCodes.LpLoadNoFoup: return "该端口没有Foup，不允许Load";
                case LPCodes.LpUnloadNoFoup: return "该端口没有Foup，不允许Unload";
                case LPCodes.OHTLoadHasFoup: return "该端口有Foup，不允许Load";
                case LPCodes.OHTUnloadNoFoup: return "该端口没有Foup，不允许Unload";
                case LPCodes.IO_VALID_ERROR: return "等待VALID信号失败";
                case LPCodes.IO_CSX_ERROR: return "读取CS_0或者CS_1信号失败";
                case LPCodes.IO_CSX_INVALID: return "CS0和CS1信号同时触发, 不支持的LoadPort上下料方式";
                case LPCodes.IO_CSX_INVALID2: return "CS0和CS1信号都没触发, 不支持的LoadPort上下料方式";
                case LPCodes.IO_L_REQ_ERROR: return "上料写入L_REQ信号失败";
                case LPCodes.IO_U_REQ_ERROR: return "下料写入U_REQ信号失败";
                case LPCodes.IO_TR_REQ_ERROR: return "等待TR_REQ信号失败";
                case LPCodes.IO_READY_ERROR: return "写入READY信号失败";
                case LPCodes.IO_BUSY_ERROR: return "等待BUSY信号失败";
                case LPCodes.FOUP_PLACE_TIMEOUT: return "等待天车放Foup超时";
                case LPCodes.FOUP_TAKE_TIMEOUT: return "等待天车取Foup超时";
                case LPCodes.IO_COMPT_ERROR: return "等待COMPT完成信号失败";
                case LPCodes.IO_CS0_RESET_ERROR: return "等待CS_0信号重置失败";
                case LPCodes.IO_CS1_RESET_ERROR: return "等待CS_1信号重置失败";
                case LPCodes.ORGYT: return "未复位, 执行复位";
                case LPCodes.ERROR: return "系统异常, 重新复位";
                case LPCodes.CBUSY: return "系统忙碌中, 待当前动作执行完在操作";
                case LPCodes.EMO: return "急停, 1.检查急停是否拍下并解除 2.安全信号是否接入";
                case LPCodes.MENTE: return "未切换到手动模式, 单一操作需要在手动模式下执行";
                case LPCodes.ONMGV: return "未切换到自动模式, 复杂操作需要在手动模式下执行";
                case LPCodes.AIRSN: return "气压异常, 检查正压或者负压是否开启";
                case LPCodes.PROTS: return "凸片,  1.整理凸片wafer 2.凸片传感器故障";
                case LPCodes.CIOPS: return "底座解锁异常, 1.检查正压源 2.磁力传感器位置调整 3.机构是否卡顿";
                case LPCodes.CICLS: return "底座上锁异常, 1.检查正压源 2.磁力传感器位置调整 3.机构是否卡顿";
                case LPCodes.YLMIT: return "平台移动异常, 1.检查气源 2.平台前后移动障碍 3.检知传感器安装位置不佳";
                case LPCodes.DLMIT: return "门移动异常(门前倾/后仰), 1.气源供应确认 2.调整压力源至额定压力 3.检知传感器安装位置不佳";
                case LPCodes.DROPS: return "钥匙开门异常, 1.气源供应确认 2.调整压力源至额定压力 3.检知传感器安装位置不佳 4.机构动作失败";
                case LPCodes.DRCLS: return "钥匙关门异常, 1.气源供应确认 2.调整压力源至额定压力 3.检知传感器安装位置不佳 4.机构动作失败";
                case LPCodes.MPBAR: return "MAPARM动作异常, 1.气源供应确认 2.调整压力源至额定压力 3.检知传感器安装位置不佳 4.机构动作失败";
                case LPCodes.DRDKE: return "FOUP加载错误, 1.检知传感器安装位置不佳 2.FOUP或治具移动到位检知失败 3.检知传感器位置调整";
                case LPCodes.VACCS: return "真空开关异常, 1.气源是否开启";
                case LPCodes.LATCH: return "门钥匙状态异常";
                case LPCodes.FPILG: return "料盒放置异常, 检查料盒是否正确放置";
                case LPCodes.DPOSI: return "门位置异常, 前置条件不满足无法完成当前动作";
                case LPCodes.YDOOR: return "平台位置异常, 前置条件不满足无法完成当前动作";
                case LPCodes.ZPOSI: return "Z轴位置异常, 前置条件不满足无法完成当前动作";
                case LPCodes.ZLIMIT: return "Z轴移动异常, 前置条件不满足无法完成当前动作";
                case LPCodes.SAFTY: return "障碍物异常, 1.移开障碍物 2.误检知调整 3.传感器故障更换";
                case LPCodes.SafeInterLock: return "互锁信号, 恢复互锁信号";
                case LPCodes.PNBAR: return "顶PIN气缸异常, 检查顶PIN是否到位及是否有信号";
                default:
                    return "未知错误";
            }
        }
    }
}
