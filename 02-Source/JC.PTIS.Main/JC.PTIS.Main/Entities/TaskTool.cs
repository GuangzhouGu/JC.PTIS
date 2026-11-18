using JC.PTIS.Main.MotionInfo;
using StdLib.PCBaseFrame.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Entities
{
    public class TaskTool
    {
        const int FlowOkStep = 0;
        const int FlowNgStep = -1;
        public static bool IsPause = false;
        static Delay Delay = new Delay();
        /// <summary>
        /// 执行状态机
        /// </summary>
        /// <param name="func"></param>
        /// <param name="step"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public async static Task<bool> RunStateMachine(Func<bool> func, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool res = false;
            var task = await Task.Factory.StartNew(() =>
            {
                while (res == false)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = func.Invoke();
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<int> RunStateMachine(Func<int> func, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int res = -1;
            var task = await Task.Factory.StartNew(() =>
            {
                while (res != FlowOkStep)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || res < 0 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = func.Invoke();
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<int> RunStateMachine(Func<int, int> func, int step, CancellationToken token, double waitTime = 20)
        {
            Delay.Start(waitTime);
            var task = await Task.Factory.StartNew(() =>
            {
                while (step != FlowOkStep)
                {
                    if (!IsPause)
                    {
                        if (Delay.TimeUp() || step <= 0 || token.IsCancellationRequested)
                        {
                            break;
                        }
                        step = func.Invoke(step);
                    }
                    Thread.Sleep(20);
                }
                IsPause = false;
                return step;
            }, token);
            return task;
        }
        public static int RunStateMachineSync(Func<int, int> func, int step, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (step != FlowOkStep)
            {
                if (sw.ElapsedMilliseconds > waitTime * 1000 || step < 0 || token.IsCancellationRequested)
                {
                    break;
                }
                step = func.Invoke(step);
                Thread.Sleep(20);
            }
            return step;
        }
        public async static Task<T> RunStateMachine<T>(Func<T> func, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            T res = default(T);
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = func.Invoke();
                    if (Convert.ToInt32(res) <= 0)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<int> RunStateMachine<T>(Func<T, int> func, T value, CancellationToken token, double waitTime = 20)
        {
            Delay.Start(waitTime);
            int res = -1;
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!IsPause)
                    {
                        if (Delay.TimeUp()|| token.IsCancellationRequested)
                        {
                            break;
                        }
                        res = func.Invoke(value);
                        if (res <= 0)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<T> RunStateMachine<T, T1>(Func<T, T1, T> func, T step, T1 t1Value, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    step = func.Invoke(step, t1Value);
                    if (Convert.ToInt32(step) <= 0)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                return step;
            });
            return task;
        }
        public async static Task<bool> RunStateMachine(List<Func<bool>> taskList, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool res = true;
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = true;
                    taskList.ForEach(p => res &= p.Invoke());
                    if (res)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<int> RunStateMachine(List<Func<int>> taskList, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int res = -1;
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = 0;
                    taskList.ForEach(p => res += p.Invoke());
                    if (res == FlowOkStep)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<bool> RunStateMachine(List<Func<PosName, bool>> funcList, PosName posName, CancellationToken token, double waitTime = 20)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool res = true;
            var task = await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = true;
                    funcList.ForEach(p => res &= p.Invoke(posName));
                    if (res)
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public async static Task<bool> RunStateMachine(Func<PosName, bool> func, PosName posName, CancellationToken token, double waitTime = 60)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool res = false;
            var task = await Task.Factory.StartNew(() =>
            {
                while (res == false)
                {
                    if (sw.ElapsedMilliseconds > waitTime * 1000 || token.IsCancellationRequested)
                    {
                        break;
                    }
                    res = func.Invoke(posName);
                    Thread.Sleep(20);
                }
                return res;
            });
            return task;
        }
        public static void Pause()
        {
            IsPause = true;
        }
        public static void Resume()
        {
            IsPause = false;
        }
    }
}
