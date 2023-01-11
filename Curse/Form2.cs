using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Curse
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fr1 = new Form1();
            Function function = new Function();
            try
            {
                function.Formula = comboBox1.SelectedIndex;
                function.ValueA = double.Parse(textBox1.Text);
                function.ValueB = double.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Заполните, пожалуйста, все данные", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox1.Text != null && textBox2.Text != null)
            {
                Form1 main = this.Owner as Form1;
                if (main != null)
                {
                    main.listFun.Add(function);
                }
                Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
