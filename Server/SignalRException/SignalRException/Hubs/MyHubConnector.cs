using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRException.Hubs
{
    public class MyHubConnector
    {     
        private IClientProxy m_Client;

        public MyHubConnector(IHubContext<MyHub> myHubContext, string connectionId)
        {          
            m_Client = myHubContext.Clients.Client(connectionId);
        }

        internal async Task SendGeneralData(GeneralData generalData)
        {
            Console.WriteLine("Sending trip general data");            
            await m_Client.InvokeAsync("setGeneralData", generalData);
        }

        internal async Task UpdateTaskStatus(string status)
        {
            Console.WriteLine("Updating task status {0}",status);            
            await m_Client.InvokeAsync("updateTaskStatus", status);
        }

    }
}
