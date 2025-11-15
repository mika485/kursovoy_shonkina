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
    /// Логика взаимодействия для ClientsControl.xaml
    /// </summary>
    public partial class ClientsControl : Window
    {
        private user152_dbEntities1 _context = new user152_dbEntities1();
        public ClientsControl()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRegistrationData();
        }
        private void LoadRegistrationData()
        {
            try
            {
                // Сначала загружаем данные из базы без преобразования цвета
                var rawData = (from r in _context.REGISTRATION
                               join c in _context.CLIENTS on r.client_id equals c.client_id
                               join e in _context.EMPLOYEES on r.employee_id equals e.employee_id
                               join s in _context.SERVICES on r.services_id equals s.services_id
                               join st in _context.STATUS on r.status equals st.status_id
                               select new
                               {
                                   c.first_name,
                                   e.last_name,
                                   s.services_name,
                                   r.registration_date,
                                   st.status_name,
                                   st.color
                               }).ToList();

                // Затем преобразуем цвет в памяти
                var registrationData = rawData.Select(item => new
                {
                    ClientFirstName = item.first_name,
                    EmployeeLastName = item.last_name,
                    ServiceName = item.services_name,
                    RegistrationDate = item.registration_date,
                    StatusName = item.status_name,
                    StatusColor = TryConvertColor(item.color)
                }).ToList();

                dgRegistration.ItemsSource = registrationData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        // Метод безопасного преобразования строки в цвет
        private SolidColorBrush TryConvertColor(string colorString)
        {
            try
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString(colorString);
            }
            catch
            {
                return Brushes.Gray; // если цвет некорректный
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
