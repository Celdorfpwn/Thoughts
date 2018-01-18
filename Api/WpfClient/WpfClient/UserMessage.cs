using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfClient
{
    public class UserMessage
    {
        public string Sender { get; set; }

        public string Message { get; set; }

        public BitmapImage Image { get; set; }

        public bool IsLocal { get; set; }

        public Visibility Visibility
        {
            get
            {
                if (IsLocal)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
    }
}
