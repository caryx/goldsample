using System;
using GMSDK;

namespace test_strategy
{
    class Program
    {
        static void Main(string[] args)
        {
            StrategySimple s = new StrategySimple();
            int ret = s.InitWithConfig("Test1.ini");
            System.Console.WriteLine("init: {0}", ret);

            ret = s.Run();

            if(ret != 0)
            {
                System.Console.WriteLine("run error: {0}", ret);
            }

            Console.Read();
        }
    }

    public class StrategySimple : Strategy
    {
    
        public override void OnLogin()
        {
        }

        public override void OnError(int error_code, string error_msg)
        {
        }

        /// <summary>
        /// 收到bar事件。
        /// </summary>
        /// <param name="bar"></param>
        public override void OnBar(Bar bar)
        {
        }

        /// <summary>
        /// 委托执行回报，订单的任何执行回报都会触发本事件，通过rpt可访问回报信息。
        /// </summary>
        /// <param name="rpt"></param>
        public override void OnExecRpt(ExecRpt rpt)
        {
        }

        /// <summary>
        /// 当订单已被交易所接受时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderNew(Order order)
        {
        }

        /// <summary>
        /// 订单全部成交时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderFilled(Order order)
        {
        }

        /// <summary>
        /// 订单部分成交时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderPartiallyFilled(Order order)
        {
        }

        /// <summary>
        /// 订单被停止执行时，触发本事件, 比如限价单到收市仍未成交，作订单过期处理。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderStopExecuted(Order order)
        {
        }

        /// <summary>
        /// 撤单成功时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderCancelled(Order order)
        {
        }

        /// <summary>
        /// 撤单请求被拒绝时，触发本事件
        /// </summary>
        /// <param name="rpt"></param>
        public override void OnOrderCancelRejected(ExecRpt rpt)
        {
        }
    }
}