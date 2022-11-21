// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace _512kBChecker;

/// <summary>
/// The main form.
/// </summary>
public partial class Main : Form
{
    /// <summary>
    /// The folder dialog.
    /// </summary>
    private readonly FolderBrowserDialog folderDialog = new();

    /// <summary>
    /// The language manager.
    /// </summary>
    private readonly ILanguageManager languageManager = new LanguageManager();

    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
        this.Init();
    }

    /// <summary>
    /// Initializes the program.
    /// </summary>
    private void Init()
    {
        this.InitializeComponent();
        this.LoadTitleAndDescription();
        this.InitializeLanguageManager();
        this.LoadLanguagesToCombo();
    }

    /// <summary>
    /// Loads the title and description.
    /// </summary>
    private void LoadTitleAndDescription()
    {
        this.Text = $@"{Application.ProductName} {Application.ProductVersion}";
    }

    /// <summary>
    /// Handles the choose folder click event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ChooseFolderClick(object sender, EventArgs e)
    {
        this.ClearTextBox();
        var result = this.folderDialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            this.SearchFiles(this.folderDialog.SelectedPath);
        }
    }

    /// <summary>
    /// Clears the text box.
    /// </summary>
    private void ClearTextBox()
    {
        this.richTextBoxFiles.Text = string.Empty;
    }

    /// <summary>
    /// Searches the files.
    /// </summary>
    /// <param name="directory">The directory.</param>
    private void SearchFiles(string directory)
    {
        try
        {
            var gifs = Directory.EnumerateFiles(directory, "*.gif", SearchOption.AllDirectories).ToList();
            this.CheckFiles(gifs);
            this.CheckIfFilesConcerned();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Checks the files.
    /// </summary>
    /// <param name="gifs">The files to check.</param>
    private void CheckFiles(IEnumerable<string> gifs)
    {
        foreach (var file in gifs)
        {
            this.CheckFile(file);
        }
    }

    /// <summary>
    /// Checks one file.
    /// </summary>
    /// <param name="file">The file.</param>
    private void CheckFile(string file)
    {
        if (new FileInfo(file).Length >= 512000)
        {
            this.richTextBoxFiles.AppendText(file + Environment.NewLine);
        }
    }

    /// <summary>
    /// Checks whether the file is concerned of the check or not.
    /// </summary>
    private void CheckIfFilesConcerned()
    {
        if (!string.IsNullOrWhiteSpace(this.richTextBoxFiles.Text) && this.richTextBoxFiles.Lines.Any())
        {
            return;
        }

        var title = this.languageManager.GetCurrentLanguage().GetWord("NoFilesFoundTitle");
        var text = this.languageManager.GetCurrentLanguage().GetWord("NoFilesFoundText");
        MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Initializes the language manager.
    /// </summary>
    private void InitializeLanguageManager()
    {
        this.languageManager.SetCurrentLanguage("de-DE");
        this.languageManager.OnLanguageChanged += this.OnLanguageChanged!;
    }

    /// <summary>
    /// Loads the languages to the combo box.
    /// </summary>
    private void LoadLanguagesToCombo()
    {
        foreach (var lang in this.languageManager.GetLanguages())
        {
            this.comboBoxLanguage.Items.Add(lang.Name);
        }

        this.comboBoxLanguage.SelectedIndex = 0;
    }

    /// <summary>
    /// Handles the event when the selected index for the languages is changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The event args.</param>
    private void LanguageSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
        var selectedString = this.comboBoxLanguage.SelectedItem.ToString();

        if (string.IsNullOrWhiteSpace(selectedString))
        {
            return;
        }

        this.languageManager.SetCurrentLanguageFromName(selectedString);
    }

    /// <summary>
    /// Handles the language changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The event args.</param>
    private void OnLanguageChanged(object sender, EventArgs eventArgs)
    {
        this.buttonChooseFolder.Text = this.languageManager.GetCurrentLanguage().GetWord("ChooseFolder");
        this.folderDialog.Description = this.languageManager.GetCurrentLanguage().GetWord("SearchFolder");
    }
}
