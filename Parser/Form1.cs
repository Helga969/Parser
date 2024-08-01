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
            // �������� ������� ������ � DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("��� ������ ��� ����������.");
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "��������� ���� ���";
                saveFileDialog.FileName = "Data.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _xlsx_parser.SaveDataToExcel(saveFileDialog.FileName);
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // ��������� ������� ������
            DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];

            // �������������� ������ DataGridView � DataRow
            DataRowView dataRowView = dataGridViewRow.DataBoundItem as DataRowView;

            if (dataRowView != null)
            {
                DataRow dataRow = dataRowView.Row;

                // ����� ������� ValidateRow ��� �������� ������
                string errorMessage;
                if (!_xlsx_parser.ValidateRow(dataRow, out errorMessage))
                {
                    // ���� �������� �� ������, �������� ��������� �� ������
                    MessageBox.Show(errorMessage, "������ ���������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // ���������� � ��������� �������, ����� ������������� ������������
                    dataGridView1.CellEndEdit -= dataGridView1_CellEndEdit;
                    dataGridView1.CancelEdit();
                    // ���������� ����� ������� �� ������� ������
                    dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                    dataGridView1.BeginEdit(true);
                    // ��������� ����������� �������
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
                // ������������� ���� ���� � ����������� �� ��������� CheckBox
                _xlsx_parser.ChangeRowColor(selectedRow, checkBox1.Checked);
            }

        }

        private void button_generate_Click(object sender, EventArgs e)
        {
            try 
            {
                // ��������� ������ � ������
                string login = _xlsx_parser.GenerateRandomString(8);
                string password = _xlsx_parser.GenerateRandomString(12);

                // ��������, ������� �� ������
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // �������� ������ ��������� ������ 
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // ������� ��������
                    int loginColumnIndex = 11; // ������ ������� ������
                    int passwordColumnIndex = 12; // ������ ������� ������

                    // ������� ��������������� �������� � ��������� ������ �� ��������
                    selectedRow.Cells[loginColumnIndex].Value = login;
                    selectedRow.Cells[passwordColumnIndex].Value = password;

                    // ������� �������� � TextBox
                    textBox2.Text = login;
                    textBox3.Text = password;
                }
                else
                {
                    MessageBox.Show("����������, �������� ������ ��� ����������.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                // ��������� ����������, ���� ������ ������� ������� �� �������
                MessageBox.Show($"������: ������������ ������ �������. {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                // ��������� ����������, ���� ���� �� �������� �� ���������������
                MessageBox.Show($"������: �������������������� ������. {ex.Message}");
            }
            catch (Exception ex)
            {
                // ��������� ������ ����������
                MessageBox.Show($"��������� ������: {ex.Message}");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _xlsx_parser.PerformSearch(textBox1.Text);
        }
    }
}
