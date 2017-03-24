using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Languages.Implementation;
using Languages.Interfaces;
using _512kBChecker.Properties;

namespace _512kBChecker
{
    public partial class Main : Form
    {
        private readonly FolderBrowserDialog _fbd = new FolderBrowserDialog();
        private readonly ILanguageManager _lm = new LanguageManager();

        public Main()
        {
            Init();
        }

        private void Init()
        {
            InitializeComponent();
            LoadTitleAndDescription();
            InitializeLanguageManager();
            LoadLanguagesToCombo();
        }

        private void LoadTitleAndDescription()
        {
            Text = Application.ProductName + Resources.EmptyString + Application.ProductVersion;
        }

        private void buttonChooseFolder_Click(object sender, EventArgs e)
        {
            ClearTextBox();
            var result = _fbd.ShowDialog();
            if (result == DialogResult.OK)
                SearchFiles(_fbd.SelectedPath);
        }

        private void ClearTextBox()
        {
            richTextBoxFiles.Text = "";
        }

        private void SearchFiles(string directory)
        {
            try
            {
                var gifs = Directory.EnumerateFiles(directory, "*.gif", SearchOption.AllDirectories).ToList();
                CheckFiles(gifs);
                CheckIfFilesConcerned();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckFiles(IEnumerable<string> gifs)
        {
            foreach (var file in gifs)
                CheckFile(file);
        }

        private void CheckFile(string file)
        {
            if (new FileInfo(file).Length >= 512000)
                richTextBoxFiles.AppendText(file + Environment.NewLine);
        }

        private void CheckIfFilesConcerned()
        {
            if (!string.IsNullOrWhiteSpace(richTextBoxFiles.Text) && richTextBoxFiles.Lines.Any()) return;
            var title = _lm.GetCurrentLanguage().GetWord("NoFilesFoundTitle");
            var text = _lm.GetCurrentLanguage().GetWord("NoFilesFoundText");
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeLanguageManager()
        {
            _lm.SetCurrentLanguage("de-DE");
            _lm.OnLanguageChanged += OnLanguageChanged;
        }

        private void LoadLanguagesToCombo()
        {
            foreach (var lang in _lm.GetLanguages())
                comboBoxLanguage.Items.Add(lang.Name);
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lm.SetCurrentLanguageFromName(comboBoxLanguage.SelectedItem.ToString());
        }

        private void OnLanguageChanged(object sender, EventArgs eventArgs)
        {
            buttonChooseFolder.Text = _lm.GetCurrentLanguage().GetWord("ChooseFolder");
            _fbd.Description = _lm.GetCurrentLanguage().GetWord("SearchFolder");
        }
    }
}