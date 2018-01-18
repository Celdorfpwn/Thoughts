using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfClient
{
    public class ChatViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<UserMessage> Messages { get; set; }

        public string Message { get; set; }

        public ICommand SendMessage { get; private set; }

        private HubConnection _hubConnection { get; set; }

        private IHubProxy _chatProxy { get; set; }

        private string _username { get; set; }

        private Dispatcher _dispatcher { get; set; }              

        public ChatViewModel(string username,Dispatcher dispatcher)
        {
            Messages = new ObservableCollection<UserMessage>();
            _username = username;
            _dispatcher = dispatcher;
            SendMessage = new CommandHandler(SendMessageAction);

        }

        public bool Initialize()
        {
            try
            {
                _hubConnection = new HubConnection/*("http://localhost:5700/chat");*/("http://thoughtsapi.azurewebsites.net/chat");
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
                     try
                     {

                         if(_username.Equals(message.Sender))
                         {
                             message.IsLocal = true;
                         }

                         var c = new WebClient();
                         var bytes = c.DownloadData(new Uri(message.Message));
                         var ms = new MemoryStream(bytes);
                         message.Image = new BitmapImage();

                         message.Image.BeginInit();
                         message.Image.StreamSource = ms;
                         message.Image.EndInit();

                         Messages.Add(message);
                     }
                     catch(Exception)
                     {

                     }
   
                 }); 
             });

            _hubConnection.Start().Wait();
        }

        private void SendMessageAction()
        {
            _chatProxy.Invoke("Send", _username, Message);
            Message = String.Empty;
            OnPropertyChanged("Message");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
