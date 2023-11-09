using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessenger.CommunicationItems
{
    public class ServerUser : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public TcpClient Client { get; set; }
        public string Color { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public ServerUser()
        {

        }

        void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    
    }
}
