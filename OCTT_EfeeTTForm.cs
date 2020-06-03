#region Open Course Timetabler - An application for school and university course timetabling
//
// Author:
//   Ivan Ćurak (mailto:Ivan.Curak@fesb.hr)
//
// Copyright (c) 2007 Ivan Ćurak, Split, Croatia
//
// http://www.openctt.org
//
//This file is part of Open Course Timetabler.
//
//Open Course Timetabler is free software;
//you can redistribute it and/or modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2 of the License,
//or (at your option) any later version.
//
//Open Course Timetabler is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
//or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//You should have received a copy of the GNU General Public License along with
//Open Course Timetabler; if not, write to the Free Software Foundation, Inc., 51 Franklin St,
//Fifth Floor, Boston, MA  02110-1301  USA

#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using OCTT_Efee_Plugin;

namespace OCTT_Efee_Plugin
{
	/// <summary>
	/// Summary description for OCTT_EfeeTTForm.
	/// </summary>
	public class OCTT_EfeeTTForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
        /// 

        private BackgroundWorker _backWorker;
		public static OCTT_EfeeTTForm OCTT_MYSQL_EDBF;
		public static OCTT_EfeePlugin OCTT_EFEE_EXPLG;
		public System.Windows.Forms.Button _postOnServerButton;
		public System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.TextBox _userNameTextBox;
		public System.Windows.Forms.TextBox _passwordTextBox;
		public System.Windows.Forms.ProgressBar progressBar1;

		private System.Windows.Forms.Label _statusLabel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label _descriptionLabel;
		private System.Windows.Forms.Label _authorLabel;
		private System.Windows.Forms.Label _versionLabel;
		private System.Windows.Forms.GroupBox groupBox3;
        private Label label1;
        public Button _exportJSON;
        public TextBox _serverURLBox;
        public Button _exportXML;
        private Label _cityText;
        private Label _FacultyText;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public OCTT_EfeeTTForm(OCTT_EfeePlugin explg)
		{
			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();

			OCTT_MYSQL_EDBF=this;
			OCTT_EFEE_EXPLG=explg;

			_statusLabel.Text="";
			this.Text=OCTT_EFEE_EXPLG.Name;

			_descriptionLabel.Text=_descriptionLabel.Text+" "+OCTT_EFEE_EXPLG.Description;
			_authorLabel.Text=_authorLabel.Text+" "+OCTT_EFEE_EXPLG.Author;
			_FacultyText.Text = _FacultyText.Text + " " + OCTT_EFEE_EXPLG.Faculty;
            _cityText.Text = _cityText.Text + " " + OCTT_EFEE_EXPLG.City;
            _versionLabel.Text=_versionLabel.Text+" "+OCTT_EFEE_EXPLG.Version;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._postOnServerButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._userNameTextBox = new System.Windows.Forms.TextBox();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this._statusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._cityText = new System.Windows.Forms.Label();
            this._FacultyText = new System.Windows.Forms.Label();
            this._versionLabel = new System.Windows.Forms.Label();
            this._authorLabel = new System.Windows.Forms.Label();
            this._descriptionLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._serverURLBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._exportJSON = new System.Windows.Forms.Button();
            this._exportXML = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // _postOnServerButton
            // 
            this._postOnServerButton.BackColor = System.Drawing.SystemColors.Control;
            this._postOnServerButton.Enabled = false;
            this._postOnServerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._postOnServerButton.Location = new System.Drawing.Point(38, 435);
            this._postOnServerButton.Name = "_postOnServerButton";
            this._postOnServerButton.Size = new System.Drawing.Size(525, 38);
            this._postOnServerButton.TabIndex = 5;
            this._postOnServerButton.Text = "Post on server";
            this._postOnServerButton.UseVisualStyleBackColor = false;
            this._postOnServerButton.Click += new System.EventHandler(this._exportButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(250, 660);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(107, 38);
            this._cancelButton.TabIndex = 6;
            this._cancelButton.Text = "Close";
            // 
            // _userNameTextBox
            // 
            this._userNameTextBox.Location = new System.Drawing.Point(212, 93);
            this._userNameTextBox.Name = "_userNameTextBox";
            this._userNameTextBox.Size = new System.Drawing.Size(256, 26);
            this._userNameTextBox.TabIndex = 3;
            this._userNameTextBox.TextChanged += new System.EventHandler(this.checkInput);
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Location = new System.Drawing.Point(212, 128);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '●';
            this._passwordTextBox.Size = new System.Drawing.Size(256, 26);
            this._passwordTextBox.TabIndex = 4;
            this._passwordTextBox.TextChanged += new System.EventHandler(this.checkInput);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(46, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 29);
            this.label3.TabIndex = 11;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(51, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 29);
            this.label5.TabIndex = 12;
            this.label5.Text = "Username:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(38, 540);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(525, 29);
            this.progressBar1.TabIndex = 13;
            // 
            // _statusLabel
            // 
            this._statusLabel.Location = new System.Drawing.Point(51, 584);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(512, 58);
            this._statusLabel.TabIndex = 14;
            this._statusLabel.Text = "status text";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._cityText);
            this.groupBox2.Controls.Add(this._FacultyText);
            this.groupBox2.Controls.Add(this._versionLabel);
            this.groupBox2.Controls.Add(this._authorLabel);
            this.groupBox2.Controls.Add(this._descriptionLabel);
            this.groupBox2.Location = new System.Drawing.Point(38, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 189);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Plugin properties";
            // 
            // _cityText
            // 
            this._cityText.Location = new System.Drawing.Point(13, 121);
            this._cityText.Name = "_cityText";
            this._cityText.Size = new System.Drawing.Size(486, 20);
            this._cityText.TabIndex = 4;
            this._cityText.Text = "City:";
            // 
            // _FacultyText
            // 
            this._FacultyText.Location = new System.Drawing.Point(13, 91);
            this._FacultyText.Name = "_FacultyText";
            this._FacultyText.Size = new System.Drawing.Size(486, 20);
            this._FacultyText.TabIndex = 3;
            this._FacultyText.Text = "Faculty:";
            // 
            // _versionLabel
            // 
            this._versionLabel.Location = new System.Drawing.Point(13, 151);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new System.Drawing.Size(486, 20);
            this._versionLabel.TabIndex = 2;
            this._versionLabel.Text = "Version:";
            // 
            // _authorLabel
            // 
            this._authorLabel.Location = new System.Drawing.Point(13, 61);
            this._authorLabel.Name = "_authorLabel";
            this._authorLabel.Size = new System.Drawing.Size(486, 21);
            this._authorLabel.TabIndex = 1;
            this._authorLabel.Text = "Author:";
            // 
            // _descriptionLabel
            // 
            this._descriptionLabel.Location = new System.Drawing.Point(13, 23);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new System.Drawing.Size(486, 38);
            this._descriptionLabel.TabIndex = 0;
            this._descriptionLabel.Text = "Description:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._serverURLBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this._userNameTextBox);
            this.groupBox3.Controls.Add(this._passwordTextBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(38, 233);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(525, 190);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Efee server";
            // 
            // _serverURLBox
            // 
            this._serverURLBox.Location = new System.Drawing.Point(212, 45);
            this._serverURLBox.Name = "_serverURLBox";
            this._serverURLBox.Size = new System.Drawing.Size(256, 26);
            this._serverURLBox.TabIndex = 15;
            this._serverURLBox.Text = "https://efee.etf.unibl.org/ectt";
            this._serverURLBox.TextChanged += new System.EventHandler(this.checkInput);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(46, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 29);
            this.label1.TabIndex = 14;
            this.label1.Text = "Server address:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _exportJSON
            // 
            this._exportJSON.BackColor = System.Drawing.SystemColors.Control;
            this._exportJSON.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._exportJSON.Location = new System.Drawing.Point(38, 479);
            this._exportJSON.Name = "_exportJSON";
            this._exportJSON.Size = new System.Drawing.Size(261, 38);
            this._exportJSON.TabIndex = 17;
            this._exportJSON.Text = "Export JSON";
            this._exportJSON.UseVisualStyleBackColor = false;
            this._exportJSON.Click += new System.EventHandler(this._exportJSON_Click);
            // 
            // _exportXML
            // 
            this._exportXML.BackColor = System.Drawing.SystemColors.Control;
            this._exportXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._exportXML.Location = new System.Drawing.Point(305, 479);
            this._exportXML.Name = "_exportXML";
            this._exportXML.Size = new System.Drawing.Size(258, 38);
            this._exportXML.TabIndex = 18;
            this._exportXML.Text = "Export XML";
            this._exportXML.UseVisualStyleBackColor = false;
            this._exportXML.Click += new System.EventHandler(this._exportXml_Click);
            // 
            // OCTT_EfeeTTForm
            // 
            this.AcceptButton = this._postOnServerButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(630, 716);
            this.Controls.Add(this._exportXML);
            this.Controls.Add(this._exportJSON);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._statusLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._postOnServerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OCTT_EfeeTTForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export to MySql database";
            this.Closed += new System.EventHandler(this.OCTT_EfeeTTForm_Closed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void _exportButton_Click(object sender, System.EventArgs e)
		{
            try
            {
                _backWorker = new BackgroundWorker();
                _backWorker.WorkerReportsProgress = true;
                _backWorker.WorkerSupportsCancellation = true;

                _backWorker.DoWork += new DoWorkEventHandler(_backWorkerServer_DoWork);
                _backWorker.ProgressChanged += new ProgressChangedEventHandler(_backWorker_ProgressChanged);
                _backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backWorker_RunWorkerCompleted);

                _backWorker.RunWorkerAsync();
            }
            catch { }

		}


        void _backWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        void _backWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                string s = e.UserState as string;
                _statusLabel.Text = s;
            }

        }

        void _backWorkerJSON_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OCTT_EfeeOperations.DoExportJson(_backWorker, e);
            }
            catch { }
        }

        void _backWorkerServer_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OCTT_EfeeOperations.DoPostOnServer(_backWorker, e,
                    new[] {_serverURLBox, _userNameTextBox, _passwordTextBox},
                    new[] {_exportJSON, _postOnServerButton, _exportXML});
            }
            catch (Exception ex)
            {

            }
        }


        private void checkInput(object sender, System.EventArgs e)
		{
			if(_serverURLBox.Text.Trim()!="" &&
				_passwordTextBox.Text.Trim()!="" &&
				_userNameTextBox.Text.Trim()!="")
			{
				_postOnServerButton.Enabled=true;
			}
			else
			{
				_postOnServerButton.Enabled=false;
			}		
		}

		private void OCTT_EfeeTTForm_Closed(object sender, System.EventArgs e)
		{
			this.Dispose();
			OCTT_EFEE_EXPLG.Dispose();
		}

        private void _exportJSON_Click(object sender, EventArgs e)
        {
            try
            {
                _backWorker = new BackgroundWorker();
                _backWorker.WorkerReportsProgress = true;
                _backWorker.WorkerSupportsCancellation = true;

                _backWorker.DoWork += new DoWorkEventHandler(_backWorkerJSON_DoWork);
                _backWorker.ProgressChanged += new ProgressChangedEventHandler(_backWorker_ProgressChanged);
                _backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backWorker_RunWorkerCompleted);

                _backWorker.RunWorkerAsync();
            }
            catch { }
        }

        private void _exportXml_Click(object sender, EventArgs e)
        {
            try
            {
                _backWorker = new BackgroundWorker();
                _backWorker.WorkerReportsProgress = true;
                _backWorker.WorkerSupportsCancellation = true;

                _backWorker.DoWork += new DoWorkEventHandler(_backWorkerXML_DoWork);
                _backWorker.ProgressChanged += new ProgressChangedEventHandler(_backWorker_ProgressChanged);
                _backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backWorker_RunWorkerCompleted);

                _backWorker.RunWorkerAsync();
            }
            catch { }
        }

        private void _backWorkerXML_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OCTT_EfeeOperations.DoExportXml(_backWorker, e);
            }
            catch (Exception ex){ MessageBox.Show(ex.StackTrace); }
        }
    }
}
