using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using WhatsDown.Models;

namespace WhatsDown.Hubs
{
    public class MessageHub : Hub
    {
        public static void SendMessage(string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            //hubContext.Clients.All.foo(msg);
            hubContext.Clients.All.sendMessage(msg);


            // var hubContext = GlobalHost.ConnectionManager.GetHubContext<AdminHub>();
            // hubContext.Clients.All.foo(msg);
        }
        
        public static void SendNotification(NotificationNode notificationNode)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            
            // Add Users Filter here.
            hubContext.Clients.All.sendMessage(notificationNode);
        }

        public int recordsToBeProcessed = 100;
        
        public void DoLongOperation()
        {
            for (int record = 0; record <= recordsToBeProcessed; record++)
            {
                if (ShouldNotifyClient(record))
                {
                    Clients.Caller.sendMessage(string.Format("Processing item {0} of {1}", record, recordsToBeProcessed));
                    Thread.Sleep(100);
                }
            }
        }

        private static bool ShouldNotifyClient(int record)
        {
            return record % 10 == 0;
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void Hello()
        {
            Clients.All.hello();
        }


    }
}