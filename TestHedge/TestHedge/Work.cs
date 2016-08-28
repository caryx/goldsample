using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHedge
{
    class Work
    {
        //static string basePath = @"C:\new_gtja_v6\vipdoc\sh\lday\";

        delegate bool CompareItem(StockDayData data1, StockDayData data2);

        bool closeDec(StockDayData data1, StockDayData data2)
        {
            return data1.Close > data2.Close;
        }

        bool dayDec(StockDayData data1, StockDayData data2)
        {
            //return data1.Open > data1.Close && data2.Open > data2.Close;
            return data2.Open > data2.Close && data2.Vol > data1.Vol*0.8;
        }

        bool dayInc(StockDayData data1, StockDayData data2)
        {
            //return data1.Open > data1.Close && data2.Open > data2.Close;
            return data2.Open < data2.Close && data2.Vol > data1.Vol;
        }

        bool bigInc(StockDayData data1, StockDayData data2)
        {
            //return data1.Open > data1.Close && data2.Open > data2.Close;
            return data2.Open > data1.Close * 1.08m;
        }

        /// <summary>
        /// 1 fall and 2 raise
        /// </summary>
        /// <param name="dataList">data</param>
        /// <param name="current">day</param>
        /// <returns></returns>
        bool filter1(List<StockDayData> dataList, int current)
        {
            if (dataList[current].Vol > dataList[current-2].Vol *2 &&
                dataList[current].Open < dataList[current].Close &&
                dataList[current-1].Open < dataList[current-1].Close &&
                dataList[current-2].Open > dataList[current-2].Close &&
                dataList[current].Open > dataList[current - 1].Open && dataList[current].Close > dataList[current - 1].Close &&
                dataList[current-1].Open > dataList[current - 2].Close && dataList[current-1].Close > dataList[current - 2].Open &&
                dataList[current].Vol > dataList[current - 1].Vol && dataList[current - 1].Vol > dataList[current - 2].Vol )
            {
                return true;
            }

            return false;
        }

        bool filter(List<StockDayData> dataList, int current, int count, CompareItem compare)
        {
            if (current - count - 1 < 0)
            {
                return false;
            }

            for (int i = 0; i < count; ++i)
            {
                if (!compare(dataList[current-i-1], dataList[current - i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void work3()
        {
            List<string> codes = Code.getCodes();

            int goodCount = 0;
            int badCount = 0;
            int testItem = 1;
            //for (int k = testItem; k <= testItem; ++k)
            for (int k = 0; k < codes.Count; ++k)
            {
                var item = codes[k];

                int winCount = 0;
                int loseCount = 0;
                List<StockDayData> dataList = DataSource.getData(item);
                if (dataList == null)
                {
                    continue;
                }
                bool output = false;
                int dayCount = 5;
                for (int i = dayCount; i < dataList.Count-1; ++i)
                {
                    if (filter1(dataList, i))
                    {
                        //if (dataList[i].Close < dataList[i+1].Open)
                        if (dataList[i].Close*1.005m < dataList[i + 1].High)
                        //if (dataList[i].Close < dataList[i + 1].Amount / dataList[i + 1].Vol)
                        {
                            if (output)
                            Log.e(dataList[i].ToString() + "\tResult WWW");
                            winCount++;
                        }
                        else
                        {
                            if (output)
                                Log.w(dataList[i].ToString() + "\tResult LLL");
                            loseCount++;
                        }
                    }
                    else
                    {
                        if (output)
                            Log.i(dataList[i].ToString());
                    }
                }

                //string outputLine = Utils.Int2Str(item, 6) + "\tWin: " + Utils.Int2Str(winCount, 3) + "\tLose: " + Utils.Int2Str(loseCount, 3) + "\tWin ratio:\t";
                string outputLine = (item) + "\tWin: " + Utils.Int2Str(winCount, 3) + "\tLose: " + Utils.Int2Str(loseCount, 3) + "\tWin ratio:\t";
                string count = "\t" + Utils.Int2Str(k + 1, 4) + "/" + codes.Count;
                string ratioStr = "0.00%";
                decimal ratio = 0.00m;
                if (winCount + loseCount != 0)
                {
                    ratio = winCount * 100.0m / (winCount + loseCount);
                }

                ratioStr = ratio.ToString("0.00") + "%";

                string vol = Utils.Int2Str(dataList[dataList.Count - 1].Vol, 10);
                string amount = Utils.Int2Str(dataList[dataList.Count - 1].Amount, 10);

                Log.i(outputLine + ratioStr + count + "\t" + vol + "\t" + amount);
                if (ratio > 65)
                {
                    goodCount++;
                }
                else
                {
                    badCount++;
                }
            }

            if (goodCount + badCount > 0)
            { 
                Log.e("good = " + goodCount + " badCount = " + badCount + " ratio = " +  goodCount*1.0m / (goodCount + badCount));
            }
        }

        public void work2()
        {
            List<string> codes = Code.getCodes();

            //for (int k = 0; k < codes.Count; ++k)
            for (int k = 0; k < 1; ++k)
            {
                var item = codes[k];

                int winCount = 0;
                int loseCount = 0;
                List<StockDayData> dataList = DataSource.getData(item);
                if (dataList == null)
                {
                    continue;
                }

                int dayCount = 1;
                for (int i = dayCount; i < dataList.Count; ++i)
                {
                    if (filter(dataList, i, dayCount, bigInc))
                    {
                        Log.e(dataList[i].ToString());
                        if (dataList[i - 1].Close < dataList[i].Open)
                        {
                            winCount++;
                        }
                        else
                        {
                            loseCount++;
                        }
                    }
                    else
                    {
                        Log.i(dataList[i].ToString());
                    }
                }

                //Log.i(item + "\tWin: " + winCount + "\tLose: " + loseCount + "\tWin ratio:\t" + (winCount * 100.0 / (winCount + loseCount)).ToString("0.00") + "%\t" + k + "/" + codes.Count);

            }
        }

        public void work()
        {
            List<string> codes = Code.getCodes();

            for (int k = 0; k < codes.Count; ++k)
            {
                var item = codes[k];

                int winCount = 0;
                int loseCount = 0;
                List<StockDayData> dataList = DataSource.getData(item);
                if (dataList == null)
                {
                    continue;
                }

                for (int i = 6; i < dataList.Count; ++i)
                {
                    if ((dataList[i - 1].Close < dataList[i - 2].Close)
                        && (dataList[i - 2].Close < dataList[i - 3].Close)
                        && (dataList[i - 3].Close < dataList[i - 4].Close)
                        && (dataList[i - 4].Close < dataList[i - 5].Close)
                        && (dataList[i - 5].Close < dataList[i - 6].Close))
                    {
                        //Log.e(dataList[i].ToString());
                        if (dataList[i - 1].Close < dataList[i].Open)
                        {
                            winCount++;
                        }
                        else
                        {
                            loseCount++;
                        }
                    }
                    else
                    {
                        //Log.i(dataList[i].ToString());
                    }
                }

                Log.i(item + "\tWin: " + winCount + "\tLose: " + loseCount + "\tWin ratio:\t" + (winCount * 100.0 / (winCount + loseCount)).ToString("0.00") + "%\t" + k + "/" + codes.Count);

            }
        }
    }
}
