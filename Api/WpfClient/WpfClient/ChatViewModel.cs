using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfClient
{
    public class ChatViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<UserMessage> Messages { get; set; }

        private HubConnection _hubConnection { get; set; }

        private IHubProxy _chatProxy { get; set; }

        private string _username { get; set; }

        private Dispatcher _dispatcher { get; set; }

       

        

        public ChatViewModel(string username,Dispatcher dispatcher)
        {
            Messages = new ObservableCollection<UserMessage>();
            _username = username;
            _dispatcher = dispatcher;
        }

        public bool Initialize()
        {
            try
            {
                _hubConnection = new HubConnection("http://thoughtsapi.azurewebsites.net/chat");
                BuildChatProxy();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void BuildChatProxy()
        {
            _chatProxy = _hubConnection.CreateHubProxy("chat");

            _chatProxy.On<UserMessage>("Receive", message =>
             {
                 _dispatcher.Invoke(() => {
                     Messages.Add(message);
   
                 }); 
             });

            _hubConnection.Start().Wait();
        }

        public void Send(string message)
        {
            Messages.Add(new UserMessage
            {
                Message = message,
                Sender = _username,
                IsLocal = true
            });
            _chatProxy.Invoke("Send", _username, message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
