using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentFamilies;
using System.Windows.Forms.DataVisualization.Charting;

namespace IntervalAnalysis
{
	public partial class Form1 : Form
	{
		W_Family w;

		public Form1()
		{
			InitializeComponent();
			//
			ngram_listBox.Items.Add(NGrams.unigram);
			ngram_listBox.Items.Add(NGrams.bigram);
			ngram_listBox.Items.Add(NGrams.trigram);
			ngram_listBox.Items.Add(NGrams.fourgram);
			//
			TurnAllOff();
		}

		private void Start()
		{
			backgroundWorker1.RunWorkerAsync();
		}

		private void show_button_Click(object sender, EventArgs e)
		{
			if (ngram_listBox.SelectedItem == null || words_comboBox.SelectedItem == null)
			{
				MessageBox.Show("Select a N-Gram and a Word");
				return;
			}

			int ngram = (int)ngram_listBox.SelectedItem;
			int word_i = words_comboBox.SelectedIndex;
			
			

			Series s = word_chart.Series.Add(w.appears_of_words[ngram][word_i].word);
			s.ChartType = SeriesChartType.Line;
			s.BorderWidth = 4;
			s.LabelBorderWidth = 2;

			foreach (var item in w.appears_of_words[ngram][word_i].appears_in_years)
			{
				DataPoint point = new DataPoint(item.year, item.freq);
				point.Label = item.appearInArticles.ToString();
				s.Points.Add(point);
			}
		}

		private void ngram_listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			words_comboBox.Items.Clear();
			words_comboBox.Items.AddRange((from x in w.appears_of_words[(int)ngram_listBox.SelectedItem]
										   select x.word).ToArray());

		}

		private void button1_Click(object sender, EventArgs e)
		{
			word_chart.Series.Clear();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			w = new W_Family(dir_of_conference);
			backgroundWorker1.ReportProgress(100);
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			wait_label.Visible = false;
			TurnAllOn();
			MessageBox.Show("I'm ready...");
		}

		private void TurnAllOn()
		{
			label1.Visible = true;
			label2.Visible = true;
			button1.Visible = true;
			show_button.Visible = true;
			ngram_listBox.Visible = true;
			words_comboBox.Visible = true;
			save_button.Visible = true;
		}

		private void TurnAllOff()
		{
			label1.Visible = false;
			label2.Visible = false;
			button1.Visible = false;
			show_button.Visible = false;
			ngram_listBox.Visible = false;
			words_comboBox.Visible = false;
			save_button.Visible = false;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			folderBrowserDialog1.ShowDialog();
			if (folderBrowserDialog1.SelectedPath == "")
			{
				return;
			}
			dir_of_conference = folderBrowserDialog1.SelectedPath;
			Start();
			wait_label.Visible = true;
			button2.Visible = false;
		}

		string dir_of_conference;

		private void save_button_Click(object sender, EventArgs e)
		{
			saveFileDialog1.Filter = "bin files (*.bin)|*.bin";

			string path_to_save = GetFile(saveFileDialog1);
			if (path_to_save == null)
			{
				return;
			}


			List<List<WordsAppearance>> tmp = new List<List<WordsAppearance>>();
			for (int i = 0; i < 4; i++)
			{
				tmp.Add(w.appears_of_words[i]);
			}

			System.Xml.Serialization.XmlSerializer writer =
			new System.Xml.Serialization.XmlSerializer(tmp.GetType());

			System.IO.StreamWriter file = new System.IO.StreamWriter(path_to_save);
			writer.Serialize(file, tmp);
			file.Close();
		}

		private void load_button_Click(object sender, EventArgs e)
		{
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.Filter = "bin files (*.bin)|*.bin";

			string path_to_load = GetFile(openFileDialog1);
			if (path_to_load == null)
			{
				return;
			}

			if (backgroundWorker1.IsBusy)
			{
				wait_label.Visible = false;
				backgroundWorker1.CancelAsync();
			}

			List<List<WordsAppearance>> tmp;

			System.Xml.Serialization.XmlSerializer reader =
			new System.Xml.Serialization.XmlSerializer(typeof(List<List<WordsAppearance>>));

			System.IO.StreamReader file = new System.IO.StreamReader(path_to_load);
			tmp = (List<List<WordsAppearance>>)reader.Deserialize(file);

			w = new W_Family();
			for (int i = 0; i < 4; i++)
			{
				w.appears_of_words[i] = tmp[i];
			}

			file.Close();
			button2.Visible = false;
			TurnAllOn();
		}

		private static string GetFile(FileDialog dlg)
		{
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				//returns a string for the directory
				return dlg.FileName;
			}

			return null; //not sure what you will return if they cancel
		}
	}
}
