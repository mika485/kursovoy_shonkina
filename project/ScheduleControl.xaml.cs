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
    /// Логика взаимодействия для ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : Window
    {
        private user152_dbEntities1 _context = new user152_dbEntities1();
        public ScheduleControl(string login)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClientsData();
        }
        private void LoadClientsData()
        {
            try
            {
                var clientsData = (from c in _context.CLIENTS
                                   select new
                                   {
                                       client_id = c.client_id,
                                       last_name = c.last_name,
                                       first_name = c.first_name,
                                       patronymic = c.patronymic,
                                       phone = c.phone,
                                       email = c.email,
                                       discount = c.discount
                                   }).ToList();

                dgClients.ItemsSource = clientsData;
                dgClients.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = dgClients.SelectedItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Выберите клиента для удаления.");
                    return;
                }

                // Спрашиваем подтверждение
                var result = MessageBox.Show("Вы действительно хотите удалить выбранного клиента?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Получаем client_id из выбранного объекта
                    var idProp = selectedItem.GetType().GetProperty("client_id");
                    if (idProp == null)
                    {
                        MessageBox.Show("В источнике данных нет client_id. Убедитесь, что LoadClientsData выбирает client_id.");
                        return;
                    }

                    int clientId = (int)idProp.GetValue(selectedItem);

                    // Находим клиента в контексте по id
                    var client = _context.CLIENTS.FirstOrDefault(c => c.client_id == clientId);
                    if (client == null)
                    {
                        MessageBox.Show("Клиент не найден в базе.");
                        return;
                    }

                    // Удаляем
                    _context.CLIENTS.Remove(client);
                    _context.SaveChanges();

                    // Обновляем DataGrid
                    LoadClientsData();

                    MessageBox.Show("Клиент успешно удалён.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}");

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = dgClients.SelectedItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Выберите клиента для изменения.");
                    return;
                }

                // Получаем client_id из выбранного объекта
                var idProp = selectedItem.GetType().GetProperty("client_id");
                if (idProp == null)
                {
                    MessageBox.Show("В источнике данных нет client_id. Убедитесь, что LoadClientsData выбирает client_id.");
                    return;
                }

                int clientId = (int)idProp.GetValue(selectedItem);

                // Находим клиента в контексте
                var client = _context.CLIENTS.FirstOrDefault(c => c.client_id == clientId);
                if (client == null)
                {
                    MessageBox.Show("Клиент не найден в базе.");
                    return;
                }

                // Обновляем данные из текстбоксов
                client.last_name = txtLastName.Text.Trim();
                client.first_name = txtFirstName.Text.Trim();
                client.patronymic = string.IsNullOrWhiteSpace(txtPatronymic.Text) ? null : txtPatronymic.Text.Trim();
                client.phone = txtPhone.Text.Trim();
                client.email = txtEmail.Text.Trim();
                client.discount = txtDiscount.Text.Trim();

                // Сохраняем изменения
                _context.SaveChanges();

                // Обновляем DataGrid
                LoadClientsData();

                MessageBox.Show("Информация о клиенте успешно изменена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении клиента: {ex.Message}");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Введите текст для поиска.");
                    return;
                }

                // Поиск по фамилии, имени и отчеству
                var results = _context.CLIENTS
                    .Where(c =>
                        c.last_name.Contains(searchText) ||
                        c.first_name.Contains(searchText) ||
                        c.patronymic.Contains(searchText))
                    .Select(c => new
                    {
                        client_id = c.client_id,
                        last_name = c.last_name,
                        first_name = c.first_name,
                        patronymic = c.patronymic,
                        phone = c.phone,
                        email = c.email,
                        discount = c.discount
                    })
                    .ToList();

                // Выводим результат в DataGrid
                dgClients.ItemsSource = results;
                dgClients.Items.Refresh();

                MessageBox.Show($"Найдено клиентов: {results.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ScheduleOffice ScheduleOffice = new ScheduleOffice();
            ScheduleOffice.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                string lastName = txtLastName.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string patronymic = txtPatronymic.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string email = txtEmail.Text.Trim();
                string discount = txtDiscount.Text.Trim();

                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля: Фамилия, Имя, Телефон, Email.");
                    return;
                }

                // Проверка скидки (например, должна быть числом)
                if (!string.IsNullOrWhiteSpace(discount) &&
                    !int.TryParse(discount, out int discountValue))
                {
                    MessageBox.Show("Скидка должна быть числом.");
                    return;
                }

                // Создаём новый объект клиента
                var newClient = new CLIENTS
                {
                    last_name = lastName,
                    first_name = firstName,
                    patronymic = string.IsNullOrWhiteSpace(patronymic) ? null : patronymic,
                    phone = phone,
                    email = email,
                    discount = discount
                };

                // Добавляем в контекст и сохраняем
                _context.CLIENTS.Add(newClient);
                _context.SaveChanges();

                // Обновляем DataGrid
                LoadClientsData();

                // Очищаем поля ввода
                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtPatronymic.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
                txtDiscount.Text = "";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        MessageBox.Show($"Ошибка в поле {ve.PropertyName}: {ve.ErrorMessage}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}");
            }
        }
    }
}
