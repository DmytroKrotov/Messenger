using ClientMessenger.CommunicationItems;
using ClientMessenger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using WPFEvents.Infrastructure;

namespace ClientMessenger.ViewModel
{
    internal class MainViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// //////////////////////////////////////////////////////////PROPERTY///////////////////////////////////////////////////////////////////////
        /// </summary>
        /// 
        Client clnt;

        

        public string Login { get; set; }
        public string RegistrationLogin { get; set; }

        public string  Password { get; set; }
        public string  RegistrationPassword { get; set; }
        public string MyColor { get; set; }
        public List<string> Test { get; set; }
        private User selectedUser;

        public User SelectedUser
        {
            get { return selectedUser; }
            set 
            { 
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

       

        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set 
            {
                users = value; 
                OnPropertyChanged(nameof(Users));
            }
        }

        
        //public ObservableCollection<Message> SelectedUserMessages { get; set; }
        
        /// <summary>
        /// //////////////////////////////////////////////////////////CTOR//////////////////////////////////////////////////////////////////////////
        /// </summary>
        public MainViewModel() 
        {
            Users = new ObservableCollection<User>();
            Login = "Dima";
            MyColor = "#F4A460";
            clnt=new Client();
            clnt.MessageReceived += AddMessage;
            
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
        }



        private ICommand closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (closeWindowCommand == null)
                    closeWindowCommand = new RelayCommand(CloseWindowCommandMethod, CanExecuteCloseWindowCommandMethod);
                return closeWindowCommand;
            }

        }
        private bool CanExecuteCloseWindowCommandMethod(object obj)
        {
            return true;
        }
        private void CloseWindowCommandMethod(object obj)
        {
            Application.Current.Shutdown();
        }



        private ICommand minWindowCommand;
        public ICommand MinWindowCommand
        {
            get
            {
                if (minWindowCommand == null)
                    minWindowCommand = new RelayCommand(MinWindowCommandMethod, CanExecuteMinWindowCommandMethod);
                return minWindowCommand;
            }

        }
        private bool CanExecuteMinWindowCommandMethod(object obj)
        {
            return true;
        }
        private void MinWindowCommandMethod(object obj)
        {
            Application.Current.MainWindow.WindowState= WindowState.Minimized;  
        }


        private ICommand maxWindowCommand;
        public ICommand MaxWindowCommand
        {
            get
            {
                if (maxWindowCommand == null)
                    maxWindowCommand = new RelayCommand(MaxWindowCommandMethod, CanExecuteMaxWindowCommandMethod);
                return maxWindowCommand;
            }

        }
        private bool CanExecuteMaxWindowCommandMethod(object obj)
        {
            return true;
        }
        private void MaxWindowCommandMethod(object obj)
        {
            if(Application.Current.MainWindow.WindowState!=WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState=WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }


        private ICommand logInCommand;
        public ICommand LogInCommand
        {
            get
            {
                if (logInCommand == null)
                    logInCommand = new RelayCommand(LogInCommandMethod, CanExecuteLogInCommandMethod);
                return logInCommand;
            }

        }
        private bool CanExecuteLogInCommandMethod(object obj)
        {
            return true;
        }
        private void LogInCommandMethod(object obj)
        {

            _ = clnt.ConnectAsync("127.0.0.1", 12345);
            _ = clnt.SendAsync($"LOGIN|{Login}|Server|{Password}");

        }

        private ICommand registrationCommand;
        public ICommand RegistrationCommand
        {
            get
            {
                if (registrationCommand == null)
                    registrationCommand = new RelayCommand(RegistrationCommandMethod, CanExecuteRegistrationCommandMethod);
                return registrationCommand;
            }

        }
        private bool CanExecuteRegistrationCommandMethod(object obj)
        {
            return true;
        }
        private void RegistrationCommandMethod(object obj)
        {

            _ = clnt.ConnectAsync("127.0.0.1", 12345);
            _ = clnt.SendAsync($"REGISTER|{RegistrationLogin}|Server|{RegistrationPassword}");

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
                if (SelectedUser!= null) 
                {
                    
                    var myNewMessage = new Message()
                    {
                        Sender = Login,
                        UserName = SelectedUser.Name,
                        Color = MyColor,
                        MessageData = textBox.Text,
                        Time = DateTime.Now,
                    };
                    SelectedUser.Messages.Add(myNewMessage);
                    _=clnt.SendAsync($"SEND|{Login}|{SelectedUser.Name}|{myNewMessage.MessageData}");
                    textBox.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("!!!!!!SELECT USER!!!!!!");
                }
                
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


        void AddMessage(string IncomeMessage)
        {
            
            var parsedMessage = MessageHandler.ParseIncomeMessage(IncomeMessage);
            


            var command = parsedMessage.Command;

            switch (command)
            {
                case "ADDUSER":
                    {
                        var userToAdd = new User()
                        {
                            Name = parsedMessage.Message,
                            Color = "#393939"

                        };
                        users.Add(userToAdd);
                        break;
                    }
                case "INCOME":
                    {
                        var messageToAdd = new Message()
                        {
                            Sender = parsedMessage.Sender,
                            UserName = parsedMessage.Recipient,
                            Color = "#131313",
                            MessageData = parsedMessage.Message,
                            Time = DateTime.Now
                        };
                        users.First((x) => x.Name == parsedMessage.Sender).Messages.Add(messageToAdd);
                        break;
                    }

                default:

                    break;
            }
            
        }


        //////////////////////////////////////////////classes////////////////////////////////////////////////////////
        ///

        

    }
}
