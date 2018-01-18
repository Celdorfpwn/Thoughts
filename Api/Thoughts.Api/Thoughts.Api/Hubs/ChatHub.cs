using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Thoughts.Api.Hubs
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void Send(string sender,string message)
        {

            var html = GetHtmlCode(message);

            var urls = GetUrls(html);

            var random = new Random();

            var result = urls.ElementAt(random.Next(0, urls.Count));

            Clients.All.Receive(new UserMessage
            {
                Sender = sender,
                Message = result
            });
        }

        private string GetHtmlCode(string topic)
        {
            string url = "https://www.google.com/search?q=" + topic + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }
    }

    public class UserMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
    }



}