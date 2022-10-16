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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        UserContext context;
        public MainWindow()
        {
            InitializeComponent();
            context = new UserContext();
            
            //При инициализации окна читаем потоком файл и если там есть данные присваиваем в TextBox-ы
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            StreamReader streamReader = new StreamReader(fileStream);
            string login = streamReader.ReadLine();
            string password = streamReader.ReadLine();
            streamReader.Close();
            fileStream.Close();
            if (login != null && password != null)
            {
                LoginTB.Text = login;
                PassTB.Password = password;
                RememberCB.IsChecked = true;       
            }
        }

        //Метод очищающий файл
        void ClearFile()
        {
            //Открываем файл на перезапись, если хотим чтобы он дозаписывал нужен FileMode.Append
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine();
            //Flush - очищает поток и отправляет данные
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Сохраняем в переменные то, что написали в TextBox-ах
            string login = LoginTB.Text;
            string password = PassTB.Password;

            //В объект класса Users присваиваем объект из базы данных, который удовлетворяет требованиям
            ChatUser user = (from _user in context.ChatUsers
                          where _user.Login == login && _user.Password == password
                          select _user).FirstOrDefault();
            if (user != null)
            {
                if (RememberCB.IsChecked == true)
                {
                    //Записываем в файл данные с TextBox-ов
                    FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(login);
                    streamWriter.WriteLine(password);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
                WindowChat windowChat = new WindowChat(TextBoxIp.Text, user);
                windowChat.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Не верное имя пользователя или пароль");
                ClearFile();
            }
        }
    }
}
