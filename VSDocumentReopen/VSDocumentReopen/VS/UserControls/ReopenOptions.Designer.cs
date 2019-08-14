namespace VSDocumentReopen.VS.UserControls
{
	partial class ReopenOptions
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.numericUpDown2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Size = new System.Drawing.Size(571, 105);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Max allowed documents:";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(157, 34);
			this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(160, 22);
			this.numericUpDown1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 36);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "In history menu:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 66);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "In Tool window:";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(157, 66);
			this.numericUpDown2.Margin = new System.Windows.Forms.Padding(4);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(160, 22);
			this.numericUpDown2.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 31);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(210, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Keep documents not older than:";
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Location = new System.Drawing.Point(253, 31);
			this.numericUpDown3.Margin = new System.Windows.Forms.Padding(4);
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(64, 22);
			this.numericUpDown3.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboBox1);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Controls.Add(this.numericUpDown3);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(5, 117);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(571, 180);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Document history retention:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 63);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(246, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Auto remove Non existing documents:";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(253, 64);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(18, 17);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "days",
            "hours"});
			this.comboBox1.Location = new System.Drawing.Point(324, 30);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 24);
			this.comboBox1.TabIndex = 3;
			// 
			// ReopenOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "ReopenOptions";
			this.Size = new System.Drawing.Size(580, 378);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label4;
	}
}
