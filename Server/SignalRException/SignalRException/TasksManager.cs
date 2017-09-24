using Microsoft.AspNetCore.SignalR;
using SignalRException.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRException
{
    
    public class TasksManager
    {
        private ConcurrentDictionary<int, Task> m_AliveTasks;
        private IHubContext<MyHub>  m_MyHubContext;
        public TasksManager(IHubContext<MyHub> myHubContext)
        {
            m_AliveTasks = new ConcurrentDictionary<int, Task>();
            m_MyHubContext = myHubContext;
        }

        internal void AddTask(Task task, string connectionId)
        {
            Console.WriteLine("Adding new trip plan task with id {0}", task.Id);
            bool isAddSuccseded = m_AliveTasks.TryAdd(task.Id, task);
            if (!isAddSuccseded)
            {
                Console.WriteLine("an error occurred while trying to add a task to the list");
            }
            task.ContinueWith(f => TaskEnded(task,connectionId));
            task.Start();
        }

        private void TaskEnded(Task task, string connectionId)
        {
            try
            {
                Console.WriteLine("Task with id {0} execution ended. status: {1}", task.Id, task.Status);
                MyHubConnector viewConnector = new MyHubConnector(m_MyHubContext, connectionId);
            
                if (task.IsFaulted)
                {
                    Console.WriteLine("Task execution failed.");
                    viewConnector.UpdateTaskStatus("error").Wait();
                }
                else
                {
                    viewConnector.UpdateTaskStatus("succsess").Wait();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been thrown when trying to notify on task end status");
            }
            finally
            {
                Task removedTask;
                bool isRemovedSuccsfully = m_AliveTasks.TryRemove(task.Id, out removedTask);
                if (!isRemovedSuccsfully)
                {
                    Console.WriteLine("an error occurred while trying to remove a task from the list");
                }
            }
        }
        
    }
}
