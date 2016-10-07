using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfClient
{
    public static class Helpers
    {
        public static void RunAsync(Dispatcher dispatcher,Action loadingAction,Action doneloading)
        {
            Thread CurrentLogWorker = new Thread(delegate ()
            {
                loadingAction.Invoke();
                dispatcher.Invoke(
                       DispatcherPriority.SystemIdle, doneloading);
            });
            CurrentLogWorker.Start();
        }
    }
}
