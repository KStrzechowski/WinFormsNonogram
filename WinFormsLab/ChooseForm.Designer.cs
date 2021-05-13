
namespace WinFormsLab
{
    partial class ChooseForm
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
            this.listView = new System.Windows.Forms.ListView();
            this.titleHeader = new System.Windows.Forms.ColumnHeader();
            this.widthHeader = new System.Windows.Forms.ColumnHeader();
            this.heightHeader = new System.Windows.Forms.ColumnHeader();
            this.difficultyheader = new System.Windows.Forms.ColumnHeader();
            this.directoryTextBox = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.loadPuzzleButton = new System.Windows.Forms.Button();
            this.chooseDirectoryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleHeader,
            this.widthHeader,
            this.heightHeader,
            this.difficultyheader});
            this.listView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 39);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(648, 285);
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // titleHeader
            // 
            this.titleHeader.Text = "Title";
            this.titleHeader.Width = 200;
            // 
            // widthHeader
            // 
            this.widthHeader.Text = "Width";
            // 
            // heightHeader
            // 
            this.heightHeader.Text = "Heigth";
            // 
            // difficultyheader
            // 
            this.difficultyheader.Text = "Difficulty";
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.directoryTextBox.Location = new System.Drawing.Point(12, 6);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(539, 22);
            this.directoryTextBox.TabIndex = 4;
            // 
            // refreshButton
            // 
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshButton.Location = new System.Drawing.Point(12, 341);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // loadPuzzleButton
            // 
            this.loadPuzzleButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadPuzzleButton.Location = new System.Drawing.Point(570, 341);
            this.loadPuzzleButton.Name = "loadPuzzleButton";
            this.loadPuzzleButton.Size = new System.Drawing.Size(90, 23);
            this.loadPuzzleButton.TabIndex = 6;
            this.loadPuzzleButton.Text = "Load puzzle";
            this.loadPuzzleButton.UseVisualStyleBackColor = true;
            this.loadPuzzleButton.Click += new System.EventHandler(this.loadPuzzleButton_Click);
            // 
            // chooseDirectoryButton
            // 
            this.chooseDirectoryButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chooseDirectoryButton.Location = new System.Drawing.Point(557, 6);
            this.chooseDirectoryButton.Name = "chooseDirectoryButton";
            this.chooseDirectoryButton.Size = new System.Drawing.Size(103, 23);
            this.chooseDirectoryButton.TabIndex = 7;
            this.chooseDirectoryButton.Text = "Choose directory";
            this.chooseDirectoryButton.UseVisualStyleBackColor = true;
            this.chooseDirectoryButton.Click += new System.EventHandler(this.chooseDirectoryButton_Click);
            // 
            // ChooseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 383);
            this.Controls.Add(this.chooseDirectoryButton);
            this.Controls.Add(this.loadPuzzleButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.directoryTextBox);
            this.Controls.Add(this.listView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChooseForm";
            this.Text = "ChooseForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader titleHeader;
        private System.Windows.Forms.ColumnHeader widthHeader;
        private System.Windows.Forms.ColumnHeader heightHeader;
        private System.Windows.Forms.ColumnHeader difficultyheader;
        private System.Windows.Forms.TextBox directoryTextBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button loadPuzzleButton;
        private System.Windows.Forms.Button chooseDirectoryButton;
    }
}