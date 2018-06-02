using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
	public partial class Insert : Form
	{
		int i = 12;
		OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\KPI\2year\kursach\StriltsovaIS6223\Library\Library\Library.accdb");//Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Library.accdb 
		public Insert()
		{
			InitializeComponent();
		}

		private void buttonUpload_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
			if (open.ShowDialog() == DialogResult.OK)
			{
				img.ImageLocation = open.FileName;
			}
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				i++;
				conn.Open();
				//MessageBox.Show("DB is connected");
				OleDbCommand command = conn.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = "insert into Каталог values ('" + i + "','" + textBoxTitle.Text + "','" + textBoxAuthor.Text + "','" + textBoxYear.Text + "','" + textBoxPublic.Text + "','" + textBoxDescription.Text + "','" + img.ImageLocation + "')";
				command.ExecuteNonQuery();
				conn.Close();
				MessageBox.Show("Додано нові дані.");
				this.Close();
			}
			catch { }
		}
	}
}
