namespace SBD.AMS.MYOB
{
	partial class MYOBLogin
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			textPassword = new DevExpress.XtraEditors.TextEdit();
			labelControl1 = new DevExpress.XtraEditors.LabelControl();
			textUser = new DevExpress.XtraEditors.TextEdit();
			labelControl2 = new DevExpress.XtraEditors.LabelControl();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(textPassword.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(textUser.Properties)).BeginInit();
			SuspendLayout();
			// 
			// textPassword
			// 
			textPassword.EnterMoveNextControl = true;
			textPassword.Location = new System.Drawing.Point(161, 89);
			textPassword.Name = "textPassword";
			textPassword.Properties.PasswordChar = '*';
			textPassword.Size = new System.Drawing.Size(162, 20);
			textPassword.TabIndex = 1;
			// 
			// labelControl1
			// 
			labelControl1.Location = new System.Drawing.Point(56, 92);
			labelControl1.Name = "labelControl1";
			labelControl1.Size = new System.Drawing.Size(46, 13);
			labelControl1.TabIndex = 1;
			labelControl1.Text = "Password";
			// 
			// textUser
			// 
			textUser.EnterMoveNextControl = true;
			textUser.Location = new System.Drawing.Point(161, 37);
			textUser.Name = "textUser";
			textUser.Size = new System.Drawing.Size(162, 20);
			textUser.TabIndex = 0;
			// 
			// labelControl2
			// 
			labelControl2.Location = new System.Drawing.Point(56, 40);
			labelControl2.Name = "labelControl2";
			labelControl2.Size = new System.Drawing.Size(25, 13);
			labelControl2.TabIndex = 3;
			labelControl2.Text = "User ";
			// 
			// button1
			// 
			button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			button1.Location = new System.Drawing.Point(313, 185);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 4;
			button1.Text = "Cancel";
			button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			button2.DialogResult = System.Windows.Forms.DialogResult.OK;
			button2.Location = new System.Drawing.Point(211, 185);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(75, 23);
			button2.TabIndex = 2;
			button2.Text = "OK";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(button2_Click);
			// 
			// MYOBLogin
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(400, 220);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(labelControl2);
			Controls.Add(textUser);
			Controls.Add(labelControl1);
			Controls.Add(textPassword);
			Name = "MYOBLogin";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "MYOBLogin";
			Load += new System.EventHandler(MYOBLogin_Load);
			((System.ComponentModel.ISupportInitialize)(textPassword.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(textUser.Properties)).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.TextEdit textPassword;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.TextEdit textUser;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}