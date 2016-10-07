using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thoughts.Api.Hubs
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void Send(string sender,string message)
        {
            Clients.Others.Receive(new UserMessage
            {
                Sender = sender,
                Message = message
            });
        }
    }

    public class UserMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
    }


}