using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedistInsertTest
{
    class Program
    {
        private static readonly string Coonstr = "192.168.3.16,password=Ji0qneD47Y2Hz7UA";
        private static object _locker = new Object();
        private static ConnectionMultiplexer _instance = _instance = StackExchange.Redis.ConnectionMultiplexer.Connect(Coonstr);
        static Random random = new Random();
        static async Task Main()
        {
            CheckDiscountRate("1.5");
            while (true)
            {
                string sj = DateTime.Now.ToString("yyMMddHHmmssfff") + random.Next(0, 1000);
                //string sj = DateTime.Now.Ticks.ToString();

                var r = await _instance.GetDatabase(14).StringSetAsync("StackRedis:" + sj, "1");

                //_instance.GetDatabase(14).ListLeftPush("StackRedis", sj);
            }
            //Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 检查折让率是否正确
        /// </summary>
        /// <param name="s"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckDiscountRate(string s)
        {
            bool isRight = true;
            if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^\d{1,2}(\.\d)?$") == false)
            {
                //只能是1位小数!
                isRight = false;
            }
            if (Convert.ToDecimal(s) < 0)
            {
                //不能低于0!
                isRight = false;
            }
            return isRight;
        }
    }
}
