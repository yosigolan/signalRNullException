using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRException.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace SignalRException.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private TasksManager m_TasksManager;
        private IHubContext<MyHub> m_MyHubContext;

        public ValuesController(TasksManager tasksManager, IHubContext<MyHub> myHubContext)
        {
            m_TasksManager = tasksManager;
            m_MyHubContext = myHubContext;
        }

        // POST api/values
        [HttpPost]
        public int SendDataToClient([FromQuery]string connectionId)
        {
            Console.WriteLine("Sending data to connection id: {0}", connectionId);
            MyHubConnector viewConnector = new MyHubConnector(m_MyHubContext, connectionId);
            Task task = new Task(() =>
            {
                Thread.Sleep(10000);
                GeneralData generalData = new GeneralData
                {
                    SomeId = Guid.NewGuid(),
                    SomeId2 = Guid.NewGuid(),
                    SomeId3 = Guid.NewGuid(),
                    SomeString = "Hello world"
                };
                Task sendGeneralDataTask = viewConnector.SendGeneralData(generalData);
                sendGeneralDataTask.Wait();
                Console.WriteLine("Done sending the current loaded trip plan");
            });
            m_TasksManager.AddTask(task,connectionId);
            return 0;
        }
        
    }
}
