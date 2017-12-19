using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Laboratorium3
{
    class Program
    {

        //ZADANIA TAP
        static void Main(string[] args)
        {
            Zadanie1();
            Zadanie2();
            Zadanie3();
            Zadanie4();
        }
        //Zadanie 1:
        static void Zadanie1()
        {
            Console.WriteLine("\n ///Zadanie 1/// \n");
            var zad1 = Dozadanie1();
            Console.WriteLine("I1 = " + zad1.I1 + " I2 = " + zad1.I2);
        }
        public struct TResultDataStructure
        {
            private int i1, i2;
            public int I1 { get => i1; set => i1 = value; }
            public int I2 { get => i2; set => i2 = value; }

            public TResultDataStructure(int x1, int x2)
            {
                i1 = x1;
                i2 = x2;
            }
        }
        public static Task<TResultDataStructure> AsyncMethod1(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure(3, 5));
            });
            return tcs.Task;
        }
        public static TResultDataStructure Dozadanie1()
        {
            var task = AsyncMethod1(null);
            task.Wait();
            return task.GetAwaiter().GetResult();
        }
        //Zadanie 2
        static private bool zadanie2 = false;
        static public bool Z2
        {
            get { return zadanie2; }
            set { zadanie2 = value; }
        }
        static public void Zadanie2()
        {
            Console.WriteLine("\n ///Zadanie 2/// \n");
            Task.Run(
                () =>
                {
                    Z2 = true;
                    Console.WriteLine("<Zadanie2> Dzialanie async");
                }).Wait();
            Console.WriteLine(zadanie2);
        }
        //Zadanie 3
        static void Zadanie3()
        {
            Console.WriteLine("\n ///Zadanie 3/// \n");
            var task = TaskZad3("http://www.feedforall.com/sample.xml");
            var xml = task.Result;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xml.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                Console.WriteLine(stringWriter.GetStringBuilder().ToString());
            }
        }
        public static async Task<XmlDocument> TaskZad3(string address)
        {
            WebClient wc = new WebClient();
            string s = await wc.DownloadStringTaskAsync(new Uri(address));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            return doc;
        }

        //Zadanie 4:
        static void Zadanie4()
        {
            Task server = serverTask();
            Task klient1 = clientTask();
            Task klient2 = clientTask();
            Task klient3 = clientTask();
            Task klient4 = clientTask();
            Task klient5 = clientTask();
            Task.WaitAll(new Task[] { server, klient1, klient2, klient3, klient4, klient5 });
        }
        static async Task serverTask()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(async (t) =>
                {
                    int i = t.Result;
                    while (true)
                    {
                        //await client.GetStream().WriteAsync(buffer, 0, i);
                        i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    }
                });
            }
        }
        static async Task clientTask()
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            await client.GetStream().WriteAsync(message, 0, message.Length);
        }

    }
}
