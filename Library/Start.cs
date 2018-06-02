using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
	public partial class Start : Form
	{
		public Start()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				Main form = new Main();
				form.Show();
			}
			catch { }
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				Help.ShowHelp(this, helpProvider1.HelpNamespace);
			}
			catch { }
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Info form = new Info();
			form.Show();
		}

		




	}
}
