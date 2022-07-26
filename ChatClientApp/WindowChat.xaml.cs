using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.IO;

namespace ChatClientApp
{
    /// <summary>
    /// Interaction logic for WindowChat.xaml
    /// </summary>
    public partial class WindowChat : Window
    {
        string ip;
        TcpClient client;
        User user;
        UserModel context;
        bool online;
        public WindowChat()
        {
            InitializeComponent();
        }
        public WindowChat(string ip, User user)
        {
            InitializeComponent();
            context = new UserModel();
            this.ip = ip;
            this.user = (from _user in context.Users 
                        where _user.Id == user.Id select _user).FirstOrDefault();
            client = new TcpClient(this.ip, 8888);
            online = true;
            Task task = new Task(WaitMessages);
            task.Start();
            SendMessage("<ID>" + user.Id.ToString());
        }
        
        void SendMessage(string message)
        {
            //networkStream нужен для отправки сообщений
            NetworkStream networkStream = client.GetStream();
            //streamWriter нужен для принятия и прочитки сообщения
            StreamWriter streamWriter = new StreamWriter(networkStream);
            streamWriter.WriteLine(message + "\n");
            streamWriter.Flush();
        }

        void WaitMessages()
        {
            while (online)
            {
                try
                {
                    NetworkStream networkStream = client.GetStream();
                    StreamReader streamReader = new StreamReader(networkStream);
                    string message = streamReader.ReadLine();
                    if (message != "")
                    {
                        if(message.IndexOf("<ONLINE>") == 0)
                        {
                            Dispatcher.Invoke(new Action(() => OnlineList.Items.Clear()));
                            string[] names = message.Split(' ');
                            for(int i = 1; i<names.Length - 1; i++)
                            {
                                int id = Convert.ToInt32(names[i]);
                                string name = (from x in context.Users where x.Id == id
                                               select x.Name).FirstOrDefault();
                                Dispatcher.Invoke(new Action(() => OnlineList.Items.Add(name)));
                            }
                        }
                        else
                        Dispatcher.Invoke(new Action(() => ChatHistory.AppendText(message + "\n")));
                    }
                }
                catch
                {
                    Dispatcher.Invoke(new Action(() => MessageBox.Show("Клиент потерял связь с сервером.")));
                    Dispatcher.Invoke(new Action(() => Close()));
                } 
            }
        }

        private void MessageTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessage(user.Name + ": " + MessageTB.Text);
                MessageTB.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            fileStream.SetLength(0);
            fileStream.Close();
            online = false;
            client.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();  //открываем окно входа
            Close();    //закрываем текущее окно(окно чата)
        }
    }
}
