using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace Library
{
	public partial class Main : Form
	{

		OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Library.accdb ");//Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Library.accdb 
		DataTable dt;
		DataView BooksDataView;
		public Main()
		{
			InitializeComponent();
		}



		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			//conn.Open();
			//string command = "select * from Каталог";
			//dt = new DataTable();
			//OleDbDataAdapter adapter = new OleDbDataAdapter(command, conn);
			//adapter.Fill(dt);
			//dataGridView.DataSource = dt;
			//dataGridView.Sort(dataGridView.Columns[0], ListSortDirection.Ascending);
			//conn.Close();
			this.каталогTableAdapter1.Fill(this.libraryDataSet2.Каталог);
			BooksDataView = new DataView(libraryDataSet2.Каталог);
			dataGridView.DataSource = BooksDataView;
		}


		private void buttonClose_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Ви дійсно бажаєте вийти?", "Увага!", MessageBoxButtons.YesNo);
			Application.Exit();
		}

		private void Form1_Load_1(object sender, EventArgs e)
		{
			this.каталогTableAdapter1.Fill(this.libraryDataSet2.Каталог);
			BooksDataView = new DataView(libraryDataSet2.Каталог);
			dataGridView.DataSource = BooksDataView;
			dataGridView.Rows[0].Selected = true;
		}

		private void каталогBindingSource_CurrentChanged(object sender, EventArgs e)
		{
			textBoxTitle.DataBindings.Add("Text", каталогBindingSource, "Название книги");
			textBoxAuthor.DataBindings.Add("Text", каталогBindingSource, "Автор");
			textBoxYear.DataBindings.Add("Text", каталогBindingSource, "Издание(год)");
			textBoxPublic.DataBindings.Add("Text", каталогBindingSource, "Название издания");
			textBoxDescription.DataBindings.Add("Text", каталогBindingSource, "Аннотации");
		}
		private int prevPosition = -1;
		private void каталогBindingSource_PositionChanged(object sender, EventArgs e)
		{
			if (prevPosition != -1)
				dataGridView.Rows[prevPosition].Selected = false;
			int position = каталогBindingSource.Position;
			if (libraryDataSet2.Tables[0].Rows[position].ItemArray[6].ToString().Length > 6)
				img.ImageLocation = "" + libraryDataSet2.Tables[0].Rows[position].ItemArray[6].ToString();
			else
				img.ImageLocation = "" + Application.StartupPath + "\\images\\" + libraryDataSet2.Tables[0].Rows[position].ItemArray[6]; // ItemArray[6] - костыль!
			dataGridView.Rows[position].Selected = true;
			prevPosition = position;
			ForbidEdit();
		}

		private void buttonEdit_Click(object sender, EventArgs e)
		{
			AllowEdit();
		}

		private void AllowEdit()
		{
			textBoxTitle.ReadOnly = false;
			textBoxAuthor.ReadOnly = false;
			textBoxYear.ReadOnly = false;
			textBoxPublic.ReadOnly = false;
			textBoxDescription.ReadOnly = false;
			System.Drawing.Color color = System.Drawing.Color.White;
			textBoxTitle.BackColor = color;
			textBoxAuthor.BackColor = color;
			textBoxYear.BackColor = color;
			textBoxPublic.BackColor = color;
			textBoxDescription.BackColor = color;
		}

		private void ForbidEdit()
		{
			textBoxTitle.ReadOnly = true;
			textBoxAuthor.ReadOnly = true;
			textBoxYear.ReadOnly = true;
			textBoxPublic.ReadOnly = true;
			textBoxDescription.ReadOnly = true;
			System.Drawing.Color color = System.Drawing.Color.WhiteSmoke;
			textBoxTitle.BackColor = color;
			textBoxAuthor.BackColor = color;
			textBoxYear.BackColor = color;
			textBoxPublic.BackColor = color;
			textBoxDescription.BackColor = color;
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show("Ви впевнені, що хочете видалити запис?", "Увага!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
				{
					int rowIndex = dataGridView.CurrentCell.RowIndex;
					conn.Open();
					//MessageBox.Show(rowIndex.ToString());
					OleDbCommand com = conn.CreateCommand();
					com.CommandType = CommandType.Text;
					com.CommandText = "delete from Каталог where Код=" + dataGridView.SelectedRows[0].Cells[0].Value;
					com.ExecuteNonQuery();
					this.каталогTableAdapter1.Fill(this.libraryDataSet2.Каталог);
					BooksDataView = new DataView(libraryDataSet2.Каталог);
					dataGridView.DataSource = BooksDataView;
					//string command = "select * from Каталог";
					//dt = new DataTable();
					//OleDbDataAdapter adapter = new OleDbDataAdapter(command, conn);
					//adapter.Fill(dt);
					//dataGridView.DataSource = dt;
					MessageBox.Show("Дані були видалені!");
					conn.Close();
				}
			}
			catch
			{
				MessageBox.Show("Не вдалося видалити запис.", "Попередження!", MessageBoxButtons.OK);
			}

		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			Insert form = new Insert();
			form.Show();
		}

		private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
		{

		}

		private void textBoxFilter_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				string columnName = comboBox1.SelectedItem.ToString();
				BooksDataView.RowFilter = string.Format("[{0}] = '{1}'", columnName, textBoxFilter.Text);
				dataGridView.Update();
				textBoxSearch.Text = "" + string.Format("[{0}] = '{1}'", columnName, textBoxFilter.Text);
			}
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			каталогBindingSource.EndEdit();
			каталогTableAdapter1.Update(libraryDataSet2.Каталог);
			MessageBox.Show("Дані збережено!","", MessageBoxButtons.OK);
		}

		private void buttonFilter_Click(object sender, EventArgs e)
		{
			string columnName = comboBox1.SelectedItem.ToString();
			BooksDataView.RowFilter = string.Format("[{0}] = '{1}'", columnName, textBoxFilter.Text);
			dataGridView.Update();

		}

		private void buttonSearch_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridView.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (row.Selected)
					{
						row.Selected = false;
					}
				}

			}
			string searchValue = textBoxSearch.Text;

			dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			bool isFound = false;
			try
			{
				foreach (DataGridViewRow row in dataGridView.Rows)
				{
					foreach (DataGridViewCell cell in row.Cells)
					{
						if (cell.Value.ToString().Equals(searchValue))
						{
							row.Selected = true;
							isFound = true;
							break;
						}
					}

				}
				if (!isFound)
				{
					MessageBox.Show("За даним запитом нічого не було знайдено.");
				}
			}
			catch { }
		}
	}
}

