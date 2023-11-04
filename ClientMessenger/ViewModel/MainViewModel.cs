using ClientMessenger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFEvents.Infrastructure;

namespace ClientMessenger.ViewModel
{
    internal class MainViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// //////////////////////////////////////////////////////////PROPERTY///////////////////////////////////////////////////////////////////////
        /// </summary>
        /// 
        public string Login { get; set; }
        public string MyColor { get; set; }
        public List<string> Test { get; set; }
        public User SelectedUser { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Message> SelectedUserMessages { get; set; }
        
        /// <summary>
        /// //////////////////////////////////////////////////////////CTOR//////////////////////////////////////////////////////////////////////////
        /// </summary>
        public MainViewModel() 
        {
            Users = new ObservableCollection<User>();
            Login = "Dima";
            MyColor = "#F4A460";




            SelectedUserMessages = new ObservableCollection<Message>() { 
                new Message(){MessageData="sfgrtthgtrhtyh"},
                new Message(){MessageData="sfgrtthgtsgsghs rhtyh"},
                new Message(){MessageData="sfg areg rtthgtrhtyh"},
                new Message(){MessageData="sfgrtt  aerf frgr hgtrhtyh"},
                new Message(){MessageData="sfgrtt hgtrht yh"},
                new Message(){MessageData="sfgrtt  hgtrh tyh"},
            };
            for (int i = 0; i < 6; i++)
            {
                              
                Users.Add(new User()
                {
                    Name = $"Friend {i}",
                   
                }) ;
            }

            for (int k =  3; k > 0; k--)
            {
                SelectedUserMessages.Add(new Message()
                {
                    MessageData = "weqrqwretwetegwegtwr wergwet weg wetggggggggggg"
                });
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////EVENTS/////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// ///////////////////////////////////////////////////////COMMANDS///////////////////////////////////////////////////////////////////////////
        /// </summary>
        private ICommand selectedUserCommand;
        public ICommand SelectedUserCommand
        {
            get
            {
                if (selectedUserCommand == null)
                    selectedUserCommand = new RelayCommand(SelectedUserCommandMethod, CanExecuteSelectedUserCommandMethod);
                return selectedUserCommand;
            }

        }
        private bool CanExecuteSelectedUserCommandMethod(object obj)
        {
            return true;
        }
        private void SelectedUserCommandMethod(object obj)
        {            
            ListView lb=(ListView)obj;
            int selectedIndex = lb.SelectedIndex;
            SelectedUser = Users[selectedIndex];
            MessageBox.Show(SelectedUser.Name);
        }

        private ICommand sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null)
                    sendMessageCommand = new RelayCommand(SendMessageCommandMethod, CanExecuteSendMessageCommandMethod);
                return sendMessageCommand;
            }

        }
        private bool CanExecuteSendMessageCommandMethod(object obj)
        {
            return true;
        }
        private void SendMessageCommandMethod(object obj)
        {
            try
            {
                TextBox textBox = (TextBox)obj;
                var myNewMessage = new Message()
                {
                    IsMine = true,
                    UserName = Login,
                    Color = MyColor,
                    MessageData = textBox.Text,
                    Time = DateTime.Now,
                };
                SelectedUserMessages.Add(myNewMessage);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// /////////////////////////////////////////////METHODS/////////////////////////////////////
        /// </summary>
        void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
