using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Curse
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form1 childForm = new Form1();
            childForm.MdiParent = this;
            childForm.Text = "Окно " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "All files (*.*)|*.*|csv files (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Form1 childForm = new Form1();
                childForm.MdiParent = this;
                childForm.Show();

                String filePath = openFileDialog1.FileName;
                var reader = new StreamReader(File.OpenRead(filePath));
                var line = reader.ReadLine();
                var values = line.Split(';');
                childForm.dataGridView1.RowHeadersWidth = 50;
                childForm.Text = filePath;

                childForm.dataGridView1.ColumnCount = 0;
                foreach (string i in values)
                {
                    childForm.dataGridView1.ColumnCount++;
                    childForm.dataGridView1.Columns[childForm.dataGridView1.ColumnCount - 1].HeaderCell.Value = i;
                }

                try
                {
                    int index = 1;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        values = line.Split(';');

                        childForm.dataGridView1.RowCount++;
                        childForm.dataGridView1.Rows[index-1].HeaderCell.Value = Convert.ToString(index);

                        int colIndex = 0;
                        foreach (string i in values)
                        {
                            childForm.dataGridView1[colIndex, index-1].Value = i;
                            colIndex++;
                        }
                        index++;
                    }
                    childForm.startIndex = childForm.dataGridView1.ColumnCount;
                }
                catch
                {
                    MessageBox.Show("Неверный формат входных данных!", "Ошибка чтения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            Form1 curForm = (Form1)this.ActiveMdiChild;

            if (curForm.dataGridView1.RowCount == 0)
            {
                MessageBox.Show("Таблица не заполнена!", "Ошибка сохранения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileDialog1.Filter = "All files (*.*)|*.*|csv files (*.csv)|*.csv";
                saveFileDialog1.FilterIndex = 2;

                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    String filePath = saveFileDialog1.FileName;
                    string strSeperator = ";";
                    StringBuilder sbOutput = new StringBuilder();
                    string ss = "";

                    for (int i = 0; i < curForm.dataGridView1.ColumnCount; i++)
                    {
                        ss += curForm.dataGridView1.Columns[i].HeaderCell.Value + strSeperator;
                    }
                    sbOutput.AppendLine(ss.Remove(ss.Length - 1));

                    for (int j = 0; j < curForm.dataGridView1.RowCount; j++)
                    {
                        ss = "";
                        for (int i = 0; i < curForm.dataGridView1.ColumnCount; i++)
                        {
                            ss += curForm.dataGridView1[i, j].Value.ToString() + strSeperator;
                        }
                        sbOutput.AppendLine(ss.Remove(ss.Length - 1));
                    }

                    File.WriteAllText(filePath, sbOutput.ToString());
                }
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveAsToolStripMenuItem_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программу разработал студент ЭПИ-21 Кузьменко Илья Максимович", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {

        }
    }
}
