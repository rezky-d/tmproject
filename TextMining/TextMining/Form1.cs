using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using jsastrawi.morphology;

namespace TextMining
{
    public partial class Form1 : Form
    {
        PreProcessing preprocess;
        TextBox[] textBox;
        GroupBox[] panel;

        public Form1()
        {
            InitializeComponent();
            textBox = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            panel = new GroupBox[] { groupBox1, groupBox2 };
            comboBox1.SelectedIndex = 0;
            preinput();
        }

        public void preinput()
        {
            String[] input = Properties.Resources.Document.Split('^');
            for(int i = 0; i < textBox.Length; i++)
            {
                textBox[i].Text = input[i].Trim();
            }
        }

        public void updateTable()
        {
            DataTable dataTable = new DataTable();
            int totalDocument = textBox.Length;
            String[] term = preprocess.getTerm();
            int[][] termFrequency = preprocess.getTermFrequency();
            int[] documentFrequency = preprocess.getDocumentFrequency(); 
            
          
            DataColumn column = new DataColumn();
            column.ColumnName = "Term";
            dataTable.Columns.Add(column);
            for (int i = 0; i < totalDocument; i++)
            {    
                column = new DataColumn();
                column.ColumnName = "Doc " + (i + 1);
                dataTable.Columns.Add(column);
            }
            column = new DataColumn();
            column.ColumnName = "DF";
            dataTable.Columns.Add(column);
           
            
            for (int i = 0; i < term.Length; i++)
            {
                int n = 0;
                Object[] row = new Object[totalDocument + 2];
                row[n++] = term[i];
                for(int j = 0; j < totalDocument; j++)
                {
                    row[n++] = termFrequency[i][j];
                }
                row[n] = documentFrequency[i];
                dataTable.Rows.Add(row);
            }
            
            dataGridView1.DataSource = dataTable;
            
            for(int i = 0; i < totalDocument+2; i++)
            {
                dataGridView1.Columns[i].Width = (dataGridView1.Width / (totalDocument + 2))-3;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int counter = 0;
            foreach (TextBox tb in textBox)
            {
                if (!String.IsNullOrEmpty(tb.Text))
                {
                    counter++;
                }
            }
            String[] document = new String[counter];
            for (int i = 0; i < document.Length; i++)
            {
                document[i] = textBox[i].Text;
            }
            preprocess = new PreProcessing(document);
            preprocess.calculate();
            updateTable();
            String[] term = preprocess.getTerm();
            textBox6.Text = String.Join(" ", term);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            for(int i = 0; i < textBox.Length; i++)
            {
                textBox[i].Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = comboBox1.SelectedIndex;
            
            for(int i = 0; i < panel.Length; i++)
            {
                panel[i].Hide();
                
                if (i == selected)
                {
                    panel[i].Show();
                }     
            }
        }
    }
}
