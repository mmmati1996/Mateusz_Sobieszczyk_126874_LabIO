﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratorium4
{
    class Client
    {
        TcpClient client;

        public void Connect()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Loopback, 2048);
        }
        public async Task<string> Ping(string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            buffer = new byte[1024];
            var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, t);
        }
        public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token)
        {
            List<string> messages = new List<string>();
            bool done = false;
            while (!done)
            {
                if (token.IsCancellationRequested)
                    done = true;
                messages.Add(await Ping(message));
                Console.WriteLine(message);
            }

            return messages;
        }
    }
}
