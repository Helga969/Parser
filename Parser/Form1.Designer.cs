namespace Parser
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGridView1 = new DataGridView();
            button_select_file = new Button();
            bindingSource1 = new BindingSource(components);
            button_save = new Button();
            checkBox1 = new CheckBox();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            button_generate = new Button();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(2, 65);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(795, 180);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // button_select_file
            // 
            button_select_file.Location = new Point(12, 5);
            button_select_file.Name = "button_select_file";
            button_select_file.Size = new Size(75, 23);
            button_select_file.TabIndex = 1;
            button_select_file.Text = "Обзор";
            button_select_file.UseVisualStyleBackColor = true;
            button_select_file.Click += button_select_file_Click;
            // 
            // button_save
            // 
            button_save.Location = new Point(435, 33);
            button_save.Name = "button_save";
            button_save.Size = new Size(75, 23);
            button_save.TabIndex = 2;
            button_save.Text = "Сохранить";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += button_save_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 36);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(94, 19);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "Обработано";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(435, 8);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Введите текст для поиска.....";
            textBox1.Size = new Size(362, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(116, 8);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 5;
            label1.Text = "Логин";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(238, 8);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 6;
            label2.Text = "Пароль";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(116, 32);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(238, 32);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 8;
            // 
            // button_generate
            // 
            button_generate.Location = new Point(344, 33);
            button_generate.Name = "button_generate";
            button_generate.Size = new Size(75, 23);
            button_generate.TabIndex = 9;
            button_generate.Text = "Генерация";
            button_generate.UseVisualStyleBackColor = true;
            button_generate.Click += button_generate_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(2, 248);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 10;
            label3.Text = "Ошибки";
            // 
            // label4
            // 
            label4.Location = new Point(4, 265);
            label4.MaximumSize = new Size(795, 180);
            label4.Name = "label4";
            label4.Size = new Size(795, 180);
            label4.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button_generate);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(checkBox1);
            Controls.Add(button_save);
            Controls.Add(button_select_file);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button_select_file;
        private BindingSource bindingSource1;
        private Button button_save;
        private CheckBox checkBox1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button_generate;
        private Label label3;
        private Label label4;
    }
}
