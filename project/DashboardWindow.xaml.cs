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

namespace project
{
    /// <summary>
    /// Логика взаимодействия для DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
        }

        public DashboardWindow(string login)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FinanceControl FinanceControl = new FinanceControl();
            FinanceControl.Show();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ClientsControl ClientsControl = new ClientsControl();
            ClientsControl.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            EmployeesControl EmployeesControl = new EmployeesControl();
            EmployeesControl.Show();
        }
    }
}
