using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
using Word = Microsoft.Office.Interop.Word;
namespace project
{
    /// <summary>
    /// Логика взаимодействия для FinanceControl.xaml
    /// </summary>
    public partial class FinanceControl : Window
    {
       

        private user152_dbEntities1 _context = new user152_dbEntities1();
        public FinanceControl()
        {
            InitializeComponent();
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFinanceData();
            LoadServicesData();
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void LoadFinanceData()
        {
            try
            {
                // Правильный JOIN через services_id
                var financeData = (from r in _context.REGISTRATION
                                   join s in _context.SERVICES
                                   on r.services_id equals s.services_id
                                   select new
                                   {
                                       REGISTRATION = r,
                                       services_name = s.services_name,
                                       price = s.price
                                   }).ToList();

                // ДИАГНОСТИКА: Проверим сколько данных получили
                MessageBox.Show($"Загружено записей: {financeData.Count}");

                // ДИАГНОСТИКА: Выведем первые несколько записей в консоль
                foreach (var item in financeData.Take(5))
                {
                    System.Diagnostics.Debug.WriteLine($"Дата: {item.REGISTRATION.registration_date}, " +
                                                     $"Услуга: {item.services_name}, " +
                                                     $"Цена: {item.price}");
                }

                FinanceDataGrid.ItemsSource = financeData;

                // Принудительное обновление DataGrid
                FinanceDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string procedureName = ProcedureInput.Text.Trim();
                string priceText = PriceInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(procedureName) || string.IsNullOrWhiteSpace(priceText))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Если в вашей локали десятичный разделитель — запятая, используйте CurrentCulture
                if (!decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal priceValue))
                {
                    MessageBox.Show("Цена должна быть числом.");
                    return;
                }

                var newService = new SERVICES
                {
                    services_name = procedureName,
                    price = priceValue // ← присваиваем decimal, без ToString
                                       // time можно не задавать (NULL допускается)
                };

                _context.SERVICES.Add(newService);
                _context.SaveChanges();

                LoadServicesData();

                ProcedureInput.Text = "";
                PriceInput.Text = "0";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        MessageBox.Show($"Ошибка в поле {ve.PropertyName}: {ve.ErrorMessage}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
        private void LoadServicesData()
        {
            var list = _context.SERVICES
         .Select(s => new
         {
             s.services_name,
             price = s.price // decimal остаётся decimal
         })
         .ToList();

            FinanceDataGrid.ItemsSource = list;
            FinanceDataGrid.ItemsSource = _context.SERVICES.ToList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаём приложение Word
                var wordApp = new Word.Application();
                wordApp.Visible = false;

                // Новый документ
                var doc = wordApp.Documents.Add();

                // Заголовок
                Word.Paragraph title = doc.Content.Paragraphs.Add();
                title.Range.Text = "Отчет";
                title.Range.Font.Bold = 1;
                title.Range.Font.Size = 16;
                title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();

                // Таблица: строки = количество элементов + заголовок, столбцы = 2
                int rows = FinanceDataGrid.Items.Count + 1;
                int cols = 2;
                Word.Table table = doc.Tables.Add(title.Range, rows, cols);
                table.Borders.Enable = 1;

                // Заголовки таблицы
                table.Cell(1, 1).Range.Text = "Процедура";
                table.Cell(1, 2).Range.Text = "Цена";

                // Заполнение данными из DataGrid
                int rowIndex = 2;
                foreach (var item in FinanceDataGrid.Items)
                {
                    if (item != null)
                    {
                        dynamic row = item;
                        table.Cell(rowIndex, 1).Range.Text = row.services_name?.ToString();
                        table.Cell(rowIndex, 2).Range.Text = row.price?.ToString();
                        rowIndex++;
                    }
                }

                // Сохраняем документ
                string filePath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Отчет.docx");

                doc.SaveAs2(filePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Отчет успешно сформирован: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выбранный элемент из DataGrid
                var selectedItem = FinanceDataGrid.SelectedItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Выберите запись для удаления.");
                    return;
                }

                // Спрашиваем подтверждение
                var result = MessageBox.Show("Вы действительно хотите удалить выбранную запись?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Преобразуем выбранный объект к типу SERVICES
                    var service = selectedItem as SERVICES;
                    if (service == null)
                    {
                        // Если ItemsSource — анонимный объект, нужно получить id
                        // Например, если вы загружаете через Select, то лучше загружать напрямую SERVICES
                        MessageBox.Show("Не удалось определить запись для удаления. Убедитесь, что DataGrid привязан к SERVICES.");
                        return;
                    }

                    // Удаляем из контекста
                    _context.SERVICES.Remove(service);
                    _context.SaveChanges();

                    // Обновляем DataGrid
                    LoadServicesData();

                    MessageBox.Show("Запись успешно удалена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        //price = priceValue.ToString(CultureInfo.InvariantCulture) // преобразование decimal → string
    }
}
