using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratorium1
{
    class Program
    {
        static private Object thisLock = new Object();
        static void Main(string[] args)
        {
            Zadanie1();
            Zadanie2();
            Zadanie3i4();
            Zadanie5(100,5);
        }

        //Zadanie 1:
        static void Zadanie1()
        {
            Console.WriteLine("///Zadanie 1/// \n");
            ThreadPool.QueueUserWorkItem(ThreadProc, 500);
            ThreadPool.QueueUserWorkItem(ThreadProc, 600);
            Thread.Sleep(1000);
        }
        static void ThreadProc(Object stateinfo)
        {
            int time = (int)stateinfo;
            Thread.Sleep(time);
            Console.WriteLine("Czekalem " + time + " ms");
        }

        //Zadanie 2:
        static void Zadanie2()
        {
            Console.WriteLine("\n ///Zadanie 2/// \n");
            ThreadPool.QueueUserWorkItem(Serwer);
            ThreadPool.QueueUserWorkItem(Klient);
            ThreadPool.QueueUserWorkItem(Klient);
            Thread.Sleep(7000);
        }
        static void Serwer(Object stateinfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[50];
                client.GetStream().Read(buffer, 0, 50);
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.Close();
            }
        }
        static void Klient(Object stateinfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);
        }

        //Zadanie 3 i 4:
        static void Zadanie3i4()
        {
            Console.WriteLine("\n ///Zadanie 3 i 4/// \n");
            int liczbaklientow = 4;
            ThreadPool.QueueUserWorkItem(Serwer2);
            for (int i = 0; i < liczbaklientow; i++)
                ThreadPool.QueueUserWorkItem(Klient2);
            Thread.Sleep(5000);
        }
        static void Serwer2(Object stateinfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2047);
            server.Start();
            while (true)
            {

                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ObslugaKlienta, client);
            }
        }
        static void ObslugaKlienta(Object stateinfo)
        {
            TcpClient client = (TcpClient)stateinfo;
            byte[] buffer = new byte[50];
            client.GetStream().Read(buffer, 0, 50);
            string tekst = System.Text.Encoding.UTF8.GetString(buffer);
            writeConsoleMessage(tekst, ConsoleColor.Red);
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
        }
        static void Klient2(Object stateinfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2047));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);
            NetworkStream stream = client.GetStream();
            stream.Read(message, 0, message.Length);
            string tekst = System.Text.Encoding.UTF8.GetString(message);
            writeConsoleMessage(tekst, ConsoleColor.Green);
        }
        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock (thisLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
        //Zadanie 5:
        static AutoResetEvent handle;
        static int s = 0;
        static WaitHandle[] waitHandles;
        static int[] tab;
        static void Zadanie5(int rozmiar, int liczbawatkow)
        {
            Console.WriteLine("\n ///Zadanie 5/// \n");
            tab = GenerateRandomTable(rozmiar);
            waitHandles = new WaitHandle[liczbawatkow];
            for (int i = 0; i < liczbawatkow; i++)
            {
                handle = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Suma));
                waitHandles[i] = handle;
            }
            WaitHandle.WaitAll(waitHandles);
            Console.WriteLine("Zakonczono, suma = " + s);
        }
        static void Suma(object stateinfo)
        {
            lock (thisLock)
            {
                for (int i = 0; i < tab.Length; i++)
                {
                    s += tab[i];
                }
            }
        }
        static int[] GenerateRandomTable(int rozmiar)
        {
            int suma = 0;
            Random rnd = new Random();
            int[] tab = new int[rozmiar];
            for (int i = 0; i < rozmiar; i++)
            {
                tab[i] = rnd.Next(0, 500);
                suma += tab[i];
            }
            Console.WriteLine("Suma (liczona bez watków) wyniosła: " + suma);
            return tab;
        }
    }
}
