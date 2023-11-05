using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientMessenger.Model
{
    internal class User:INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Color { get; set; }

        private ObservableCollection<Message> messages;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set 
            { 
                messages = value; 
            }
        }

        

        public User() 
        { 
            Messages= new ObservableCollection<Message>();
        
        }

        void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
