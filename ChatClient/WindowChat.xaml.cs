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
using System.Net;
using System.IO;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WindowChat.xaml
    /// </summary>
    public partial class WindowChat : Window
    {
        string ip;
        TcpClient client;
        ChatUser user;
        public WindowChat()
        {
            InitializeComponent();
        }
        //Конструктор принимает ip адрес сервера на который мы хотим получить доступ
        public WindowChat(string Ip, ChatUser User)
        {
            InitializeComponent();
            ip = Ip;
            user = User;
            client = new TcpClient(ip, 8888);
            //Используем Task, а не thread потому что метод WaitMessage без принимаемых параметров
            Task task = new Task(WaitMessage);
            task.Start();
        }

        void WaitMessage()
        {
            while (true)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    string message = reader.ReadLine();
                    if (message != "")
                    {
                        Dispatcher.Invoke(new Action(() => chatHistory.AppendText(message + "\n")));
                    }
                }
                catch
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Клиент потерял связь с сервером.");
                        Close();
                    }));
                }
            }
        }

        private void MessageTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                NetworkStream networkStream = client.GetStream();   //Соединение с сервером
                StreamWriter streamWriter = new StreamWriter(networkStream); //читаем поток
                streamWriter.WriteLine(MessageTB.Text + "\n");  //Считываем строку в MessageTB
                streamWriter.Flush();   //Отправляем сообщение на сервер
                MessageTB.Text = "";    //Обнуляем строку
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine();
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
