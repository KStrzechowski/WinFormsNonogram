using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace WinFormsLab
{
    public partial class ChooseForm : Form
    {
        Form1 parentForm;
        List<allInformations> listInfo;
        int fileCount;
        public ChooseForm(Form1 form)
        {
            InitializeComponent();
            parentForm = form;
            directoryTextBox.ReadOnly = true;
        }

        private void loadPuzzleButton_Click(object sender, EventArgs e)
        {
            int itemIndex = -1;
            foreach (ListViewItem item in listView.SelectedItems)
                itemIndex = listView.Items.IndexOf(item);
            if (itemIndex == -1)
                return;

            parentForm.insertValues(listInfo[itemIndex]);
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void refreshButton_Click(object sender, EventArgs e)
        {
            if (directoryTextBox.Text == "")
                return;

            iterateFiles();
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            var loadFileDialog = new FolderBrowserDialog();

            loadFileDialog.RootFolder = Environment.SpecialFolder.Desktop;
            loadFileDialog.AutoUpgradeEnabled = false;
            if (loadFileDialog.ShowDialog() == DialogResult.OK)
                directoryTextBox.Text = loadFileDialog.SelectedPath;
            else
                return;

            iterateFiles();         
        }

        private void iterateFiles()
        {
            Stream loadStream;
            listView.Items.Clear();
            fileCount = 0;
            var info = new allInformations();
            listInfo = new List<allInformations>();

            foreach (var file in Directory.EnumerateFiles(directoryTextBox.Text))
            {
                if (file.ToString().EndsWith(".json"))
                {
                    loadStream = File.OpenRead(file);
                    if (loadJsonSerialization(loadStream, ref info))
                    {
                        listInfo.Add(info);
                        fileCount++;
                    }
                }
            }

            for (int i = 0; i < fileCount; i++)
            {
                listView.Items.Add(new ListViewItem(new[] { listInfo[i].title, listInfo[i].columnCount.ToString(),
                    listInfo[i].rowCount.ToString(), listInfo[i].difficulty }));
            }
            listView.Refresh();
        }

        private bool loadJsonSerialization(Stream loadStream, ref allInformations info)
        {
            StreamReader sr = new StreamReader(loadStream);
            string serialization = sr.ReadToEnd();
            info = JsonSerializer.Deserialize<allInformations>(serialization);
            sr.Close();
            if (info.columnCount < 2 || info.columnCount > 15 || info.rowCount < 2 || info.rowCount > 15 ||
                info.title == null || info.difficulty == null || info.isColored.Length != info.columnCount * info.rowCount)
            {
                return false;
            }
            else
                return true;
        }
    }
}
