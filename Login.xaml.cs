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
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;

namespace Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            errorBox.Visibility = Visibility.Collapsed;
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                
                var user = db.Employees.Where(u => u.Login == loginBox.Text && u.Password == passwordBox.Password).FirstOrDefault();
                    
                if (user != null)
                {
                    new MainWindow(user).Show();
                    this.Close();
                }
                else
                {
                    errorBox.Visibility = Visibility.Visible;
                }
                
            }
            
        }
    }
}
