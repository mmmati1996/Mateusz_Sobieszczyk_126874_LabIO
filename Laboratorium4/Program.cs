using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratorium4
{
    class Program
    {
        static void Main(string[] args)
        {
            Serwer server = new Serwer();
            server.Run();


            Client client = new Client();
            Client client2 = new Client();
            client.Connect();
            client2.Connect();

            CancellationTokenSource token1 = new CancellationTokenSource();
            CancellationTokenSource token2 = new CancellationTokenSource();

            var c1 = client.keepPinging("Tokenik 1", token1.Token);
            var c2 = client2.keepPinging("Tokenik 2", token2.Token);


            token1.CancelAfter(2000);
            token2.CancelAfter(3000);

            Task.WaitAll(new Task[] { c1, c2 });

            try
            {
                server.StopRunning();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
