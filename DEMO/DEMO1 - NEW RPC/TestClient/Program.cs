﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYNet.CloudSystem;
using ZYNet.CloudSystem.Client;
using ZYNet.CloudSystem.Frame;
using ZYNet.CloudSystem.SocketClient;

namespace TestClient
{
    class Program
    {
        /// <summary>
        /// 其他不懂看DEMO 1
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LogAction.LogOut += LogAction_LogOut;          
            CloudClient client = new CloudClient(new SocketClient(), 500000, 1024 * 1024); //最大数据包能够接收 1M
            PackHander tmp = new PackHander();
            client.Install(tmp);
            client.Disconnect += Client_Disconnect;           

            if (client.Connect("127.0.0.1", 2285))
            {
            
                var ServerPacker = client.Sync.Get<IPacker>(); //获取一个 IPACKER 实例 用来调用服务器
                


                var isSuccess = ServerPacker.IsLogOn("123123", "3212312")?.First?.Value<bool>(); //调用服务器的isLOGON函数

                var html = ServerPacker.StartDown("http://www.baidu.com").First?.Value<string>(); //调用服务器的StartDown 函数
                Console.WriteLine("BaiduHtml:" + html.Length);

                var time = ServerPacker.GetTime();//调用服务器的GetTime 函数

                Console.WriteLine("ServerTime:" + time);

                ServerPacker.SetPassWord("3123123"); //调用服务器的SetPassWord 函数

                System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
                stop.Start();
                int c = ServerPacker.TestRec2(10000);
                stop.Stop();
                Console.WriteLine("Rec:{0} time:{1} MS", c, stop.ElapsedMilliseconds);

                RunTest(client);

                Console.ReadLine();

            }

        }


        public static async void RunTest(CloudClient client)
        {
            var Server = client.NewAsync().Get<IPacker>();
            
            int? v = (await Server.TestRecAsync(100))?[0]?.Value<int>();

            System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
            stop.Start();
            int? c = (await Server.TestRecAsync(10000))?.First?.Value<int>();
            stop.Stop();

            if (c != null)
                Console.WriteLine("Sync Rec:{0} time:{1} MS", c, stop.ElapsedMilliseconds);


        }


        private static void LogAction_LogOut(string msg, LogType type)
        {
            Console.WriteLine(msg);
        }

        private static void Client_Disconnect(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
