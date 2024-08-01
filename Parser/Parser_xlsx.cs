using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExcelDataReader;
using ClosedXML.Excel;
using Label = System.Windows.Forms.Label;
using System.Windows.Forms;


namespace Parser
{
    public class Parser_xlsx
    {
        private readonly DataGridView _dataGridView;
        private readonly Label _errorLabel;
        private Random random = new Random();
        public Parser_xlsx(DataGridView dataGridView, Label errorLabel)
        {
            _dataGridView = dataGridView;
            _errorLabel = errorLabel;
            //провайдер кодировок
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public void LoadDataXLSX(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)||!File.Exists(filePath))
            {
                MessageBox.Show("Файл не найден");
                return;
            }
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();
                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        //datatabel с первой сткорой в качестве заголовка
                        var resultTable = new DataTable();
                        if (dataTable.Rows.Count > 1)
                        {
                            //первоя скрока в качестве заголовка
                            foreach (var header in dataTable.Rows[0].ItemArray)
                            {
                                resultTable.Columns.Add(header.ToString());
                            }
                            //добавление оставшихся строк
                            foreach (DataRow row in dataTable.Rows.Cast<DataRow>().Skip(1))
                            {
                                var dataRow = resultTable.NewRow();
                                for (int j = 0; j < resultTable.Columns.Count; j++)
                                {
                                    dataRow[j] = row[j];
                                }
                                    resultTable.Rows.Add(dataRow);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Нет данных");
                        }
                        // Привязка DataTable к DataGridView
                        _dataGridView.DataSource = resultTable;
                        DisplayErr(resultTable);
                        _dataGridView.ReadOnly = false;
                    }
                        else
                        {
                            MessageBox.Show("Нет данных");
                        }
                    }
                } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //для вывода ошибок в лейбл
        public void DisplayErr(DataTable dataTable)
        {
            var error = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                string errorMessage;
                if (!ValidateRow(row, out errorMessage))
                {
                    int rowInd = dataTable.Rows.IndexOf(row) + 1;//индекс где ошибка
                    error.Add($"Ошибка в строке {rowInd}: {errorMessage}");
                }
            }
            if (error.Count > 0)
            {
                _errorLabel.Text = string.Join(Environment.NewLine, error);
            }
            else
            {
                _errorLabel.Text = "OK";
            }
        }
        //валидация данных
        public bool ValidateRow(DataRow row, out string errorMessage)
        {
            int typeColumnIndex = 0;
            int date_recColumnIndex = 1;
            int date_compColumnIndex = 2;
            int fioColumnIndex = 3;
            int innColumnIndex = 4; 
            int kppColumnIndex = 5;
            int nameColumnIndex = 6;
            int cityColumnIndex = 7;
            int depColumnIndex = 8;
            int numberColumnIndex = 9;
            int emailColumnIndex = 10;
            var error = new List<string>();
            string type_req = row[typeColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(type_req))
            {
                error.Add("пустой тип заявки");
            }
            string date_req_str = row[date_recColumnIndex]?.ToString();
            string date_comp_str = row[date_compColumnIndex]?.ToString();
            DateTime date_req;
            DateTime date_comp;
            //строки в DateTime
            bool isDateTime1Valid = DateTime.TryParse(date_req_str, out date_req);
            bool isDateTime2Valid = DateTime.TryParse(date_comp_str, out date_comp);
            if (string.IsNullOrEmpty (date_req_str))
            {
                error.Add("дата не может быть пустой");
            }
            else
            {
                if (!string.IsNullOrEmpty(date_comp_str) && date_req > date_comp)
                {
                    error.Add("Дата приема заявки не может быть раньше ее ответа");
                }
            }
            string fio = row[fioColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(fio))
            {
                error.Add("пустой ФИО");
               // return false;
            }
            else
            {
                if (fio.All(char.IsUpper))
                {
                    error.Add("Фио с больших букв!");
                }
                if (fio.Length < 3)
                {
                    error.Add("ФИО должно быть длинее 3х символов");
                }
            }
            string inn = row[innColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(inn))
            {
                error.Add("пустой ИНН");
            }
            else
            {
                if (!Regex.IsMatch(inn, @"^\d{10}"))
                {
                    error.Add("ИНН 10 символов");
                }
                if (inn.Any(char.IsLetter))
                {
                    error.Add("В ИНН есть буквы! ");
                }
            }
            string kpp = row[kppColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(kpp))
            {
                error.Add("пустой КПП");
            }
            else
            {
                if (!Regex.IsMatch(kpp, @"^\d{9}"))
                {
                    error.Add("КПП должен быть не менее 9 цифр");
                }
                if (kpp.Any(char.IsLetter))
                {
                    error.Add("В КПП есть буквы! ");
                }
            }
            string number = row[numberColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(number))
            {
                error.Add("пустой Номер");
            }
            else
            {
                var numberPattern = @"^\d{5}\s\d{2}-\d{2}-\d{2}$";
                if (!Regex.IsMatch(number, numberPattern))
                {
                    error.Add("Не известный формат номера");
                }
                if (number.Any(char.IsLetter))
                {
                    error.Add("В Номере есть буквы! ");
                }
            }
            string name = row[nameColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(name))
            {
                error.Add("Пустое название организации");
            }
            else
            {
                if (name.Length < 3)
                {
                    error.Add("Название организации должно быть больше 3х символов");
                }
                if (name.Any(char.IsNumber))
                {
                    error.Add("В названии организации есть цифры! ");
                }
            }            
            string city = row[cityColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(city)) 
            {
                error.Add("Пустое название города");
            }
            else
            {
                if (city.Length < 3)
                {
                    error.Add("Название города должно быть больше 3х символов");
                }
                if (city.Any(char.IsNumber))
                {
                    error.Add("В названии города есть цифры! ");
                }
            }
            string department = row[depColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(department))
            {
                error.Add("Пустое название отдела");
            }
            else
            {
                if (department.Length < 3)
                {
                    error.Add("Название отдела должно быть больше 3х символов");
                }
                if (department.Any(char.IsNumber))
                {
                    error.Add("В названии отдела есть цифры! ");
                }
            }
            string email = row[emailColumnIndex]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                error.Add("Пустая почта");
            }
            else
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    error.Add("Не известный формат почты"); 
                }
            }
            errorMessage = string.Join(" ; ", error);
            return error.Count == 0;
        }
        public void SaveDataToExcel(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Укажите путь для сохранения файла");
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");

                    // Записывает заголовки
                    for (int i = 0; i < _dataGridView.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = _dataGridView.Columns[i].HeaderText;
                    }

                    // Записывает данные
                    for (int i = 0; i < _dataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < _dataGridView.Columns.Count; j++)
                        {
                            var cell = worksheet.Cell(i + 2, j + 1); // Получаем ячейку
                            cell.Value = _dataGridView.Rows[i].Cells[j].Value?.ToString();

                            var rowStyle = _dataGridView.Rows[i].DefaultCellStyle;
                            if (rowStyle.BackColor == Color.Red)
                            {
                                cell.Style.Fill.BackgroundColor = XLColor.FromColor(Color.Red); // Устанавливаем цвет фона
                            }
                        }
                    }

                    workbook.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }
        //Генерация случайной строки
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }
        //Поиск
        public void PerformSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (DataGridViewRow row in _dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Visible = true;
                    }
                }
                return;
            }
            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    string fio = row.Cells[3].Value?.ToString() ?? string.Empty;
                    string inn = row.Cells[4].Value?.ToString() ?? string.Empty;
                    string email = row.Cells[10].Value?.ToString() ?? string.Empty;
                    string login = row.Cells[11].Value?.ToString() ?? string.Empty;

                    // Проверяем, соответствует ли значение критериям поиска
                    bool fioMatches = fio.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool innMatches = inn.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool emailMatches = email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool loginMatches = login.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;

                    // Показывает или скрывает строку в зависимости от результатов поиска
                    row.Visible = fioMatches || innMatches || emailMatches || loginMatches;
                }
            }
        }
        //Меняем цвет
        public void ChangeRowColor(DataGridViewRow row, bool isChecked)
        {
            if (row != null)
            {
                if (isChecked)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

    }
}

