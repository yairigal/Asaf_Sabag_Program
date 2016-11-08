namespace IntervalAnalysis
{
	partial class Form1
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			this.words_comboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.word_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.save_button = new System.Windows.Forms.Button();
			this.load_button = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.wait_label = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.show_button = new System.Windows.Forms.Button();
			this.ngram_listBox = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.word_chart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// words_comboBox
			// 
			this.words_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.words_comboBox.Location = new System.Drawing.Point(198, 11);
			this.words_comboBox.Name = "words_comboBox";
			this.words_comboBox.Size = new System.Drawing.Size(205, 21);
			this.words_comboBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(156, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Word:";
			// 
			// word_chart
			// 
			chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Years;
			chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea1.Name = "ChartArea1";
			this.word_chart.ChartAreas.Add(chartArea1);
			this.word_chart.Dock = System.Windows.Forms.DockStyle.Fill;
			legend1.Name = "Legend1";
			this.word_chart.Legends.Add(legend1);
			this.word_chart.Location = new System.Drawing.Point(0, 0);
			this.word_chart.Name = "word_chart";
			this.word_chart.Size = new System.Drawing.Size(763, 322);
			this.word_chart.TabIndex = 2;
			this.word_chart.Text = "chart1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.save_button);
			this.splitContainer1.Panel1.Controls.Add(this.load_button);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.wait_label);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Controls.Add(this.show_button);
			this.splitContainer1.Panel1.Controls.Add(this.ngram_listBox);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.words_comboBox);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.word_chart);
			this.splitContainer1.Size = new System.Drawing.Size(763, 369);
			this.splitContainer1.SplitterDistance = 43;
			this.splitContainer1.TabIndex = 3;
			// 
			// save_button
			// 
			this.save_button.Location = new System.Drawing.Point(514, 8);
			this.save_button.Name = "save_button";
			this.save_button.Size = new System.Drawing.Size(75, 23);
			this.save_button.TabIndex = 9;
			this.save_button.Text = "Save words";
			this.save_button.UseVisualStyleBackColor = true;
			this.save_button.Click += new System.EventHandler(this.save_button_Click);
			// 
			// load_button
			// 
			this.load_button.Location = new System.Drawing.Point(436, 8);
			this.load_button.Name = "load_button";
			this.load_button.Size = new System.Drawing.Size(72, 23);
			this.load_button.TabIndex = 8;
			this.load_button.Text = "Load words";
			this.load_button.UseVisualStyleBackColor = true;
			this.load_button.Click += new System.EventHandler(this.load_button_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(257, 9);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(126, 23);
			this.button2.TabIndex = 7;
			this.button2.Text = "Enter Conference Dir";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// wait_label
			// 
			this.wait_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wait_label.AutoSize = true;
			this.wait_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.wait_label.Location = new System.Drawing.Point(12, 2);
			this.wait_label.Name = "wait_label";
			this.wait_label.Size = new System.Drawing.Size(239, 42);
			this.wait_label.TabIndex = 6;
			this.wait_label.Text = "Please wait...";
			this.wait_label.Visible = false;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(595, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Clear";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// show_button
			// 
			this.show_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.show_button.Location = new System.Drawing.Point(676, 8);
			this.show_button.Name = "show_button";
			this.show_button.Size = new System.Drawing.Size(75, 23);
			this.show_button.TabIndex = 4;
			this.show_button.Text = "Show";
			this.show_button.UseVisualStyleBackColor = true;
			this.show_button.Click += new System.EventHandler(this.show_button_Click);
			// 
			// ngram_listBox
			// 
			this.ngram_listBox.FormattingEnabled = true;
			this.ngram_listBox.Location = new System.Drawing.Point(64, 7);
			this.ngram_listBox.Name = "ngram_listBox";
			this.ngram_listBox.Size = new System.Drawing.Size(86, 30);
			this.ngram_listBox.TabIndex = 3;
			this.ngram_listBox.SelectedIndexChanged += new System.EventHandler(this.ngram_listBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "N-Gram:";
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(763, 369);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Interval Analysis";
			((System.ComponentModel.ISupportInitialize)(this.word_chart)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox words_comboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataVisualization.Charting.Chart word_chart;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button show_button;
		private System.Windows.Forms.ListBox ngram_listBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label wait_label;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button save_button;
		private System.Windows.Forms.Button load_button;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}

