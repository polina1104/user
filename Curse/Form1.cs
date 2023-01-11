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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "0";
            textBox2.Text = "10";
            textBox3.Text = "10";
            comboBox1.Text = "Line";
        }

        public List<Function> listFun = new List<Function>();
        Function[] functions;
        public int startIndex = 1;
        public int currentIndex = 1;
        int arbitIndex = 1;
        public bool arbit = false;

        private void button4_Click(object sender, EventArgs e) // Добавить функцию
        {
            Form2 form2 = new Form2();
            form2.Owner = this;

            form2.ShowDialog();
            
            
            functions = listFun.ToArray();
            dataGridView1.ColumnCount++;
            String ss = dataGridView1.ColumnCount.ToString();
            double a = functions[currentIndex - 1].ValueA;
            double b = functions[currentIndex - 1].ValueB;

            switch (functions[currentIndex - 1].Formula)
            {
                case 0:
                    ss = "y = " + a.ToString() + "*sin(x/" + b.ToString() + ")";
                    break;
                case 1:
                    ss = "y = |x+" + a.ToString() + "| - |x+" + b.ToString() + "|";
                    break;
                case 2:
                    ss = "y = sin(x+" + a.ToString() + ") - cos(x+" + b.ToString() + ")";
                    break;
            }
            dataGridView1.Columns[dataGridView1.ColumnCount-1].HeaderCell.Value = ss;
            currentIndex++;
        }

        private void button1_Click(object sender, EventArgs e) // Рассчёт
        {
            bool error = false;
            double lowlim = 0;
            double toplim = 0;
            double count_step = 0;

            if(listFun.Count == 0)
            {
                MessageBox.Show("Пожалуйста, добавьте функции", "Нет функций", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (arbit) // Если есть произвольная функция
            {
                if(MessageBox.Show("При автоматическом рассчёте все Ваши произвольные функции сотрутся", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    // Очистка всего кроме listFun
                    arbit = false;
                    startIndex = 1;
                    dataGridView1.RowCount = 1;
                    dataGridView1.ColumnCount = 1;
                    dataGridView1.Columns[0].HeaderCell.Value = "X";

                    functions = listFun.ToArray();
                    String ss = "";
                    foreach (Function i in functions)
                    {
                        dataGridView1.ColumnCount++;
                        double a = i.ValueA;
                        double b = i.ValueB;

                        switch (i.Formula)
                        {
                            case 0:
                                ss = "y = " + a.ToString() + "*sin(x/" + b.ToString() + ")";
                                break;
                            case 1:
                                ss = "y = |x+" + a.ToString() + "| - |x+" + b.ToString() + "|";
                                break;
                            case 2:
                                ss = "y = sin(x+" + a.ToString() + ") - cos(x+" + b.ToString() + ")";
                                break;
                        }
                        dataGridView1.Columns[dataGridView1.ColumnCount - 1].HeaderCell.Value = ss;
                    }
                }
            }

            // Рассчёт
            try
            {
                lowlim = double.Parse(textBox1.Text);
                toplim = double.Parse(textBox2.Text);
                count_step = double.Parse(textBox3.Text);

                if (lowlim > toplim)
                {
                    error = true;
                    MessageBox.Show("Нижняя граница интервала больше верхней!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                error = true;
                MessageBox.Show("Неправильный формат данных!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!error)
            {
                dataGridView1.RowCount = Convert.ToInt32(count_step);
                dataGridView1.RowHeadersWidth = 50;

                for (int k = 0; k < dataGridView1.RowCount; k++)
                {
                    dataGridView1.Rows[k].HeaderCell.Value = Convert.ToString(k + 1);
                }

                double step = (toplim - lowlim) / count_step;
                double table_value = lowlim;
                for (int j = 0; j < count_step; j++)
                {
                    dataGridView1[0, j].Value = table_value;
                    table_value += step;
                }

                int tempinex = 1;
                foreach (Function i in functions)
                {
                    if (tempinex < startIndex)
                    {
                        tempinex++;
                        continue;
                    }
                    int count = dataGridView1.ColumnCount;

                    for (int j = 0; j < count_step; j++)
                    {
                        table_value = Convert.ToDouble(dataGridView1[0, j].Value);
                        double value = 0;
                        double a = i.ValueA;
                        double b = i.ValueB;
                        switch (i.Formula)
                        {
                            case 0:
                                value = a * Math.Sin(table_value / b);
                                break;
                            case 1:
                                value = Math.Abs(table_value + a) - Math.Abs(table_value + b);
                                break;
                            case 2:
                                value = Math.Sin(table_value + a) - Math.Cos(table_value + b);
                                break;
                        }
                        dataGridView1[startIndex, j].Value = value.ToString();
                    }
                    startIndex++;
                    tempinex++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // График
        {
            chart1.Series.Clear();
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                chart1.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series()); 
                chart1.Series[i - 1].Points.Clear();
                chart1.Series[i - 1].LegendText = dataGridView1.Columns[i].HeaderCell.Value.ToString();
                chart1.Series[i - 1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i - 1].BorderWidth = 4;

                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    chart1.Series[i-1].Points.AddXY(Convert.ToDouble(dataGridView1[0, j].Value), Math.Round(Convert.ToDouble(dataGridView1[i, j].Value), 2));
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // Добавить строку
        {
            dataGridView1.RowCount++;
        }

        private void button6_Click(object sender, EventArgs e) // Удалить строку
        {
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.RowCount--;
            }
        }

        private void button7_Click(object sender, EventArgs e) // Добавить столбец
        {
            arbit = true;
            dataGridView1.ColumnCount++;
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].HeaderCell.Value = "Произвольная функция " + arbitIndex.ToString();
        }

        private void button8_Click(object sender, EventArgs e) // Удалить столбец
        {
            if (dataGridView1.ColumnCount > 0)
            {
                dataGridView1.ColumnCount--;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Очистка
        {
            arbit = false;
            listFun = new List<Function>();
            startIndex = 1;
            currentIndex = 1;
            dataGridView1.RowCount = 0;
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].HeaderCell.Value = "X";

            //dataGridView1[0, 0].Value = "";
            //dataGridView1.Rows[0].HeaderCell.Value = "";

            //chart1.Series[0].Points.Clear();
            chart1.Series.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Изменение линии графика
        {
            string selectedState = comboBox1.SelectedItem.ToString();
            System.Windows.Forms.DataVisualization.Charting.SeriesChartType type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line; ;
            switch (selectedState)
            {
                case "Line":
                    type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    break;
                case "Point":
                    type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    break;
                case "Spline":
                    type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    break;
                case "Step Line":
                    type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
                    break;
            }
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].ChartType = type;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
