using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRException.Hubs
{
    public class MyHub : Hub
    {       

        private static int s_NumberOfConnections = 0;

        public override async Task OnConnectedAsync()
        {
            Interlocked.Increment(ref s_NumberOfConnections);
            Console.WriteLine("New trip plan system connection open. connection id: {0}", Context.ConnectionId);
            Console.WriteLine("Total number of open connections: {0}", s_NumberOfConnections);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Interlocked.Decrement(ref s_NumberOfConnections);
            Console.WriteLine("Trip plan system connection closed. connection id: {0}", Context.ConnectionId);
            Console.WriteLine("Total number of open connections: {0}", s_NumberOfConnections);
            await base.OnDisconnectedAsync(exception);
        }

    }
}
