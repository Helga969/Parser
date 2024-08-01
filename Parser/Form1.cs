using System.Data;

namespace Parser
{
    public partial class Form1 : Form
    {
        private readonly Parser_xlsx _xlsx_parser;
        private DataGridViewRow selectedRow;
        public Form1()
        {
            InitializeComponent();
            _xlsx_parser = new Parser_xlsx(dataGridView1, label4);
        }

        private void button_select_file_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel *.xlsx|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _xlsx_parser.LoadDataXLSX(ofd.FileName);
                }
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            // Проверка наличия данных в DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.");
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Сохранить файл как";
                saveFileDialog.FileName = "Data.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _xlsx_parser.SaveDataToExcel(saveFileDialog.FileName);
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Получение текущей строки
            DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];

            // Преобразование строки DataGridView в DataRow
            DataRowView dataRowView = dataGridViewRow.DataBoundItem as DataRowView;

            if (dataRowView != null)
            {
                DataRow dataRow = dataRowView.Row;

                // Вызов функции ValidateRow для проверки строки
                string errorMessage;
                if (!_xlsx_parser.ValidateRow(dataRow, out errorMessage))
                {
                    // Если проверка не прошла, показать сообщение об ошибке
                    MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Отключение и включение событий, чтобы предотвратить зацикливание
                    dataGridView1.CellEndEdit -= dataGridView1_CellEndEdit;
                    dataGridView1.CancelEdit();
                    // Установить фокус обратно на текущую ячейку
                    dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                    dataGridView1.BeginEdit(true);
                    // Повторное подключение события
                    dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                bool isChecked = checkBox1.Checked;
                _xlsx_parser.ChangeRowColor(selectedRow, isChecked);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRow = dataGridView1.SelectedRows[0];
                // Устанавливаем цвет фона в зависимости от состояния CheckBox
                _xlsx_parser.ChangeRowColor(selectedRow, checkBox1.Checked);
            }

        }

        private void button_generate_Click(object sender, EventArgs e)
        {
            try 
            {
                // Генерация логина и пароля
                string login = _xlsx_parser.GenerateRandomString(8);
                string password = _xlsx_parser.GenerateRandomString(12);

                // Проверка, выбрана ли строка
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Получаем первую выбранную строку 
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Индексы столбцов
                    int loginColumnIndex = 11; // Индекс столбца логина
                    int passwordColumnIndex = 12; // Индекс столбца пароля

                    // Вставка сгенерированных значений в выбранные ячейки по индексам
                    selectedRow.Cells[loginColumnIndex].Value = login;
                    selectedRow.Cells[passwordColumnIndex].Value = password;

                    // Вставка значений в TextBox
                    textBox2.Text = login;
                    textBox3.Text = password;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите строку для обновления.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                // Обработка исключения, если индекс столбца выходит за границы
                MessageBox.Show($"Ошибка: Некорректный индекс столбца. {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                // Обработка исключения, если один из объектов не инициализирован
                MessageBox.Show($"Ошибка: Неинициализированный объект. {ex.Message}");
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _xlsx_parser.PerformSearch(textBox1.Text);
        }
    }
}
