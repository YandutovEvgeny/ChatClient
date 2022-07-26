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
using System.IO;

namespace ChatClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserModel context;
        public MainWindow()
        {
            InitializeComponent();
            context = new UserModel();
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            StreamReader streamReader = new StreamReader(fileStream);
            string login = streamReader.ReadLine();
            string pass = streamReader.ReadLine();
            streamReader.Close();
            fileStream.Close();
            if (login != null && pass != null)
            {
                LoginTB.Text = login;
                PassTB.Password = pass;
                RememberCB.IsChecked = true;
                Button_Click_2(null, new RoutedEventArgs());
            }
        }

        void ClearFile()
        {
            FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
            fileStream.SetLength(0);    //Устанавливаем пустой файл
            fileStream.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string pass = PassTB.Password;

            User user = (from _user in context.Users
                         where _user.Login == login && _user.Password == pass
                         select _user).FirstOrDefault();

            if (user != null)
            {
                if (RememberCB.IsChecked == true)
                {
                    FileStream fileStream = new FileStream("config.cht", FileMode.OpenOrCreate);
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(login);
                    streamWriter.WriteLine(pass);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
                if(user.OnLine == false)
                {
                    WindowChat windowChat = new WindowChat(TextBoxIp.Text, user);
                    windowChat.Show();
                    Close();
                }
                else
                    MessageBox.Show("Данный человек уже в чате!");
            }
            else
            {
                MessageBox.Show("Не верное имя пользователя или пароль");
                ClearFile();
            }
        }

        private void RegistrationL_MouseEnter(object sender, MouseEventArgs e)
        {
           RegistrationL.Foreground = new SolidColorBrush(Color.FromRgb(00,51,204));
        }

        private void RegistrationL_MouseLeave(object sender, MouseEventArgs e)
        {
            RegistrationL.Foreground = new SolidColorBrush(Color.FromRgb(00,00,00));
        }

        private void RegistrationL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationWidow registrationWidow = new RegistrationWidow();
            registrationWidow.Show();
        }
    }
}
