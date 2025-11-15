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
    /// Логика взаимодействия для EmployeesControl.xaml
    /// </summary>
    public partial class EmployeesControl : Window
    {
        private user152_dbEntities1 _context = new user152_dbEntities1();
        public EmployeesControl()
        {
            InitializeComponent();
            LoadEmployeesData();
        }
        private void EmployeesControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEmployeesData(); // или другой метод, который ты хочешь вызвать при загрузке
        }

        private void LoadEmployeesData()
        {
            try
            {
                var employeesData = (from e in _context.EMPLOYEES
                                     join p in _context.POST on e.post equals p.post_id
                                     join q in _context.QUALIFICATION on e.qualification equals q.qualification_id
                                     select new
                                     {
                                         last_name = e.last_name,
                                         first_name = e.first_name,
                                         patronymic = e.patronymic,
                                         post = p.post1,
                                         qualification = q.qualification1
                                     }).ToList();

                dgClients.ItemsSource = employeesData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных сотрудников: {ex.Message}");
            }
        }

       
    }
}
