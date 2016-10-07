using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {

        private ChatViewModel _chatViewModel { get; set; }

        private bool ChatRunning { get; set; }

        public Chat(string username)
        {
            InitializeComponent();


            Helpers.RunAsync(this.Dispatcher, () => Load(username), () =>
            {
                this.ProgressRing.IsActive = false;
                if (this.ChatRunning)
                {
                    this.DataContext = _chatViewModel;
                    this.MainGrid.Visibility = Visibility.Visible;
                }
            });

        }

        private void Load(string username)
        {
            _chatViewModel = new ChatViewModel(username,Dispatcher);

            ChatRunning = _chatViewModel.Initialize();
        }

        private void SendMessage(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && !String.IsNullOrEmpty(this.MessageTextBox.Text))
            {
                _chatViewModel.Send(this.MessageTextBox.Text);
                this.MessageTextBox.Clear();
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            _chatViewModel.Messages.Clear();
        }
    }
}
