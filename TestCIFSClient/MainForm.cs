// 
// Copyright (C) 2008 Jordi Martín Cardona
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCIFSClient
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btLlimpia;
		private System.Windows.Forms.TextBox txConsola;
		private System.Windows.Forms.Button btExecuta;
		private System.Windows.Forms.Label titol;
		private System.Windows.Forms.Button btSurt;
		private System.Windows.Forms.TextBox txComanda;
		private CifsConsole cifsconsole ;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.cifsconsole = new CifsConsole();
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.txComanda = new System.Windows.Forms.TextBox();
			this.btSurt = new System.Windows.Forms.Button();
			this.titol = new System.Windows.Forms.Label();
			this.btExecuta = new System.Windows.Forms.Button();
			this.txConsola = new System.Windows.Forms.TextBox();
			this.btLlimpia = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txComanda
			// 
			this.txComanda.Location = new System.Drawing.Point(16, 56);
			this.txComanda.Name = "txComanda";
			this.txComanda.Size = new System.Drawing.Size(520, 20);
			this.txComanda.TabIndex = 6;
			this.txComanda.Text = "";
			this.txComanda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxComandaKeyPress);
			// 
			// btSurt
			// 
			this.btSurt.Location = new System.Drawing.Point(544, 416);
			this.btSurt.Name = "btSurt";
			this.btSurt.TabIndex = 12;
			this.btSurt.Text = "Surt";
			this.btSurt.Click += new System.EventHandler(this.BtSurtClick);
			// 
			// titol
			// 
			this.titol.Location = new System.Drawing.Point(16, 16);
			this.titol.Name = "titol";
			this.titol.TabIndex = 13;
			this.titol.Text = "Consola CIFSClient test";
			this.titol.AutoSize=true;
			this.titol.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			// 
			// btExecuta
			// 
			this.btExecuta.Location = new System.Drawing.Point(544, 56);
			this.btExecuta.Name = "btExecuta";
			this.btExecuta.TabIndex = 14;
			this.btExecuta.Text = "Executa";
			this.btExecuta.Click += new System.EventHandler(this.BtExecutaClick);
			// 
			// txConsola
			// 
			this.txConsola.BackColor = System.Drawing.Color.White;
			this.txConsola.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txConsola.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txConsola.Location = new System.Drawing.Point(16, 96);
			this.txConsola.Multiline = true;
			this.txConsola.Name = "txConsola";
			this.txConsola.ReadOnly = true;
			this.txConsola.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txConsola.Size = new System.Drawing.Size(608, 304);
			this.txConsola.TabIndex = 8;
			this.txConsola.Text = "";
			// 
			// btLlimpia
			// 
			this.btLlimpia.Location = new System.Drawing.Point(456, 416);
			this.btLlimpia.Name = "btLlimpia";
			this.btLlimpia.TabIndex = 10;
			this.btLlimpia.Text = "Neteja";
			this.btLlimpia.Click += new System.EventHandler(this.BtLlimpiaClick);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 445);
			this.Controls.Add(this.btExecuta);
			this.Controls.Add(this.titol);
			this.Controls.Add(this.btSurt);
			this.Controls.Add(this.btLlimpia);
			this.Controls.Add(this.txConsola);
			this.Controls.Add(this.txComanda);
			this.Name = "MainForm";
			this.Text = "CIFSClient Test";
			this.ResumeLayout(false);
		}
		#endregion

		

		void BtExecutaClick(object sender, System.EventArgs e)
		{
			txConsola.Text+=this.cifsconsole.addComand(txComanda.Text);
			txComanda.Text="";
			txConsola.SelectionStart = txConsola.Text.Length;
			txConsola.ScrollToCaret();
			txConsola.Refresh();
		}
		
		void TxComandaKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13){
				txConsola.Text+=this.cifsconsole.addComand(txComanda.Text);
				txComanda.Text="";
				txConsola.SelectionStart = txConsola.Text.Length;
				txConsola.ScrollToCaret();
				txConsola.Refresh();
			}
		}
		
		void BtLlimpiaClick(object sender, System.EventArgs e)
		{
			txConsola.Text="";
		}
		
		void BtSurtClick(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		
	}
}
