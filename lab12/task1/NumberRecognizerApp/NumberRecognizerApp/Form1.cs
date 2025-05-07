using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NumberRecognizerApp
{
    public partial class Form1 : Form
    {
        string inputFilePath = "";
        string outputFilePath = "output.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputFilePath = ofd.FileName;
                txtInput.Text = File.ReadAllText(inputFilePath);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                MessageBox.Show("Спочатку завантажте текст.");
                return;
            }

            string inputText = txtInput.Text;
            var results = NumberRecognizer.RecognizeNumber(inputText, Culture.English);
            string modifiedText = inputText;
            string resultLog = "";

            foreach (var result in results)
            {
                string originalText = result.Text;
                string resolvedValue = result.Resolution["value"].ToString();
                int start = result.Start;
                int end = result.End;


                resultLog += $"Розпізнаний текст (число): {originalText}\r\n";
                resultLog += $"Початковий індекс у рядку: {start}\r\n";
                resultLog += $"Кінцевий індекс у рядку: {end}\r\n";
                resultLog += $"Розпізнане значення числа: {resolvedValue}\r\n\r\n";

                // Замінити словесне число на цифру
                modifiedText = modifiedText.Replace(originalText, resolvedValue);
            }

            txtOutput.Text = modifiedText;
            txtLog.Text = resultLog;

            File.WriteAllText(outputFilePath, modifiedText);
            MessageBox.Show("Результат збережено у 'output.txt'");
        }
    }
}
