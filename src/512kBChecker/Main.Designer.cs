namespace _512kBChecker
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.richTextBoxFiles = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelSmall = new System.Windows.Forms.TableLayoutPanel();
            this.buttonChooseFolder = new System.Windows.Forms.Button();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelSmall.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxFiles
            // 
            this.richTextBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxFiles.Location = new System.Drawing.Point(3, 38);
            this.richTextBoxFiles.Name = "richTextBoxFiles";
            this.richTextBoxFiles.ReadOnly = true;
            this.richTextBoxFiles.Size = new System.Drawing.Size(427, 226);
            this.richTextBoxFiles.TabIndex = 1;
            this.richTextBoxFiles.Text = "";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.richTextBoxFiles, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelSmall, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(433, 262);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // tableLayoutPanelSmall
            // 
            this.tableLayoutPanelSmall.ColumnCount = 2;
            this.tableLayoutPanelSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelSmall.Controls.Add(this.buttonChooseFolder, 0, 0);
            this.tableLayoutPanelSmall.Controls.Add(this.comboBoxLanguage, 1, 0);
            this.tableLayoutPanelSmall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSmall.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSmall.Name = "tableLayoutPanelSmall";
            this.tableLayoutPanelSmall.RowCount = 1;
            this.tableLayoutPanelSmall.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSmall.Size = new System.Drawing.Size(427, 29);
            this.tableLayoutPanelSmall.TabIndex = 2;
            // 
            // buttonChooseFolder
            // 
            this.buttonChooseFolder.Location = new System.Drawing.Point(3, 3);
            this.buttonChooseFolder.Name = "buttonChooseFolder";
            this.buttonChooseFolder.Size = new System.Drawing.Size(120, 23);
            this.buttonChooseFolder.TabIndex = 0;
            this.buttonChooseFolder.UseVisualStyleBackColor = true;
            this.buttonChooseFolder.Click += new System.EventHandler(this.ChooseFolderClick);
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(301, 3);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(120, 21);
            this.comboBoxLanguage.TabIndex = 1;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.LanguageSelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 262);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "512kBChecker";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanelSmall.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxFiles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSmall;
        private System.Windows.Forms.Button buttonChooseFolder;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
    }
}

