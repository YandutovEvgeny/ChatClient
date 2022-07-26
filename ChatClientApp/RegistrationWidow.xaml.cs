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

namespace ChatClientApp
{
    /// <summary>
    /// Interaction logic for RegistrationWidow.xaml
    /// </summary>
    public partial class RegistrationWidow : Window
    {
        UserModel context;
        public RegistrationWidow()
        {
            InitializeComponent();
            context = new UserModel();
        }

        private void RegistrationB_Click(object sender, RoutedEventArgs e)
        {

            if (RegistrationNameTB.Text != "" && RegistrationLoginTB.Text != "" &&
                RegistrationPasswordTB.Text != "" && RegistrationEmailTB.Text != "")
            {
                User newUser = new User()
                {
                    Name = RegistrationNameTB.Text,
                    Login = RegistrationLoginTB.Text,
                    Password = RegistrationPasswordTB.Text,
                    Mail = RegistrationEmailTB.Text
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                Close();
            }
            else
                MessageBox.Show("Все поля должны быть заполнены!");
        }
    }
}
