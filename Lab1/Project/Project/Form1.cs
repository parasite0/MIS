using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Project
{
    public partial class Form1 : Form
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cs = "Host=localhost;Username=postgres;Password=Spawn777;Database=MIS";

            var con = new NpgsqlConnection(cs);
            con.Open();

            var text = textBox1.Text.Split(' ');

            bool isNum(string s)
            {
                foreach (char c in s)
                {
                    if (!Char.IsDigit(c)) return false;
                }
                return true;
            }

            if (textBox1.Text == "")
                return;

            string sql = $"SELECT name as \"Название фильма\", year as \"Год\" FROM movies WHERE ";
            bool f = isNum(textBox1.Text) && textBox1.Text.Length == 4;
            if (text.Length == 2 && isNum(text[0]) && isNum(text[1]) && text[0].Length == 4 && text[1].Length == 4)
                sql += $"(year = '{text[0]}' or name ilike '%{text[0]}%') and (year = '{text[1]}' or name ilike '%{text[1]}%')";
            else if (f == false)
            {
                int k = 0;
                int m = text.Length;

                if (isNum(text[0]) && text[0].Length == 4)
                {
                    k += 1;
                    sql += $"year = '{text[0]}' and ";
                }
                if (isNum(text[text.Length - 1]) && text[text.Length - 1].Length == 4)
                {
                    m -= 1;
                    sql += $"year = '{text[text.Length - 1]}' and ";
                }

                if (text.Length > 1)
                {
                    for (int i = k; i < m; i++)
                    {
                        sql += $"name ~~* '%{text[i]}%' ";
                        if (i < m - 1)
                            sql += $"and ";
                    }
                }
                else
                    sql += $"name ~~* '%{text[0]}%' ";
            }
            else
            {
                sql += $"year = '{textBox1.Text}' or name ilike '%{textBox1.Text}%'";
            }
            sql += $"LIMIT 10";

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}