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
    /// Логика взаимодействия для ScheduleOffice.xaml
    /// </summary>
    public partial class ScheduleOffice : Window
    {
        public ScheduleOffice()
        {
            InitializeComponent();
        }
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var red = "#FFD9534F"; // WPF формат цвета
            var green = "#FF5CB85C";

            var currentColor = button.Background.ToString();

            if (currentColor == red)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(green));
            }
            else
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(red));
            }
        }
    }
}
