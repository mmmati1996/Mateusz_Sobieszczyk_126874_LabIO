using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratorium4
{
    class Serwer
    {
        TcpListener server;
        int port;
        IPAddress address;
        bool running = false;
        CancellationTokenSource cts = new CancellationTokenSource();

        Task serverTask;
        public Task ServerTask
        {
            get { return serverTask; }
        }

        public IPAddress Address
        {
            get { return address; }
            set
            {
                if (!running) address = value;
                else;
            }
        }
        public int Port
        {
            get { return port; }
            set
            {
                if (!running)
                    port = value;
                else;
            }
        }

        public Serwer()
        {
            Address = IPAddress.Any;
            port = 2048;
        }
        public Serwer(int port)
        {
            this.port = port;
        }
        public Serwer(IPAddress address)
        {
            this.address = address;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            server = new TcpListener(address, port);

            try
            {
                server.Start();
                running = true;
            }
            catch (SocketException ex)
            {
                throw (ex);
            }
            while (true && !ct.IsCancellationRequested)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                using (ct.Register(() => client.GetStream().Close()))
                {
                    await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                    async (t) =>
                    {
                        int i = t.Result;
                        while (true)
                        {
                            client.GetStream().WriteAsync(buffer, 0, i);
                            try
                            {
                                i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                            }
                            catch
                            {
                                break;
                            }

                        }
                    });
                }

            }

        }
        public void RequestCancellation()
        {
            cts.Cancel();
            server.Stop();
        }
        public void Run()
        {

            serverTask = RunAsync(cts.Token);
        }
        public void StopRunning()
        {
            RequestCancellation();
        }
    }
}
