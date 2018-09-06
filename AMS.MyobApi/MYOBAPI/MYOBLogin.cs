using System;
using System.Windows.Forms;

namespace SBD.AMS.MYOB
{
	public partial class MYOBLogin : Form
	{
		public MYOBLogin()
		{
			InitializeComponent();
		}

		public string Login;
		private void MYOBLogin_Load(object sender, EventArgs e)
		{
			textUser.Text = Login;
		}

		private string _password;
		public string Password()
		{
			return _password;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			_password = textPassword.Text;
			Login = textUser.Text;
		}
	}
}
