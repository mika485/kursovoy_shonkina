using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }
            LoginProgressBar.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;

            // Имитация задержки для лучшего UX (в реальном приложении убрать)
            Task.Delay(800).ContinueWith(_ =>
            {
                // Возвращаемся в UI поток
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        // Проверка учетных данных
                        string userRole = CheckCredentials(login, password);

                        if (userRole != null)
                        {
                            // Успешная авторизация
                            OpenUserPage(userRole, login);
                        }
                        else
                        {
                            ShowError("Неверный логин или пароль");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError($"Ошибка авторизации: {ex.Message}");
                    }
                    finally
                    {
                        // Скрыть индикатор загрузки
                        LoginProgressBar.Visibility = Visibility.Collapsed;
                        LoginButton.IsEnabled = true;
                    }
                });
            });
        }

        private string CheckCredentials(string login, string password)
        {
            // Проверка по вашим данным
            if (login == "manager.anna" && password == "An2024!p")
                return "Менеджер";

            if (login == "stylist.elena" && password == "El2024!v")
                return "Сотрудник";
            
            return null; // Неверные credentials
        }

        private void OpenUserPage(string userRole, string login)
        {
            Window userWindow = null;

            switch (userRole)
            {
                case "Менеджер":
                    userWindow = new DashboardWindow(login);
                    break;
                case "Сотрудник":
                    userWindow = new ScheduleControl(login);
                    break;
                
                default:
                    ShowError("Неизвестная роль пользователя");
                    return;
            }
            if (userWindow != null)
            {
                userWindow.Show();
                this.Close(); // Закрыть окно авторизации
            }
        }

        private void ShowError(string errorMessage)
        {
            ErrorTextBlock.Text = errorMessage;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

       
    }
}

