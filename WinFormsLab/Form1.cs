using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

// https://stackoverflow.com/questions/34064499/how-to-set-cell-color-in-tablelayoutpanel-dynamically/34088085
// https://stackoverflow.com/questions/14065481/use-list-to-populate-labels-c-sharp
// https://www.howtosolutions.net/2013/01/creating-a-new-popup-window-in-winform-using-csharp/
// https://docs.microsoft.com/pl-pl/dotnet/api/system.windows.forms.savefiledialog?view=net-5.0
// https://www.codeproject.com/Questions/728551/Remove-flickering-due-to-TableLayoutPanel-Panel-in

namespace WinFormsLab
{
    public partial class Form1 : Form
    {
        bool gameMode = true;
        int cellSize = 30;
        public int rows = 10;
        public int columns = 10;
        Color[,] bgColors = new Color[10, 10];
        public bool[,] isColored = new bool[10, 10];
        bool[,] isCrossed = new bool[10, 10];
        public int coloredAmount = 0;
        List<Label> columnLabels = new List<Label>();
        List<Label> rowLabels = new List<Label>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // SetDoubleBuffered(tableLayoutPanel1);
            contextMenuStrip1.Items.Add("Random", null, Random_Click);
            contextMenuStrip1.Items.Add("Choose puzzle", null, Choose_Click);
            contextMenuStrip1.Items.Add("Load puzzle", null, Load_Click);
            contextMenuStrip2.Items.Add("Create puzzle", null, Create_Click);
            
            tableLayoutPanel1.Location = new Point((this.Width - tableLayoutPanel1.Width) / 2, (this.Height - tableLayoutPanel1.Height) / 2);
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                    bgColors[i, j] = Color.White;
            
            randomPuzzle();
            setTextOfLabels();
            winLabel.Location = new Point((this.Width - winLabel.Width) / 2, (this.Height - 2 * winLabel.Height));
        }

        private void GameNewButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(newGameButton, new Point(0, newGameButton.Height));
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(createButton, new Point(0, createButton.Height));
        }

        private void Random_Click(object sender, EventArgs e)
        {
            var popupForm = new RandomForm(this);
            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            setColors();
            createTableLayoutPanel();
            createToGameMode();
            randomPuzzle();
            createLabels();
            tableLayoutPanel1.Enabled = true;
            winLabel.Visible = false;
        }

        private void Choose_Click(object sender, EventArgs e)
        {
            var popupForm = new ChooseForm(this);
            popupForm.Text = "Create your own Nonogram puzzle";
            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            createTableLayoutPanel();
            createToGameMode();
            createLabels();
            tableLayoutPanel1.Enabled = true;
            winLabel.Visible = false;
        }
        private void Load_Click(object sender, EventArgs e)
        {
            Stream loadStream;
            OpenFileDialog loadFileDialog = new OpenFileDialog();
            bool correctFile = false;

            loadFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.RestoreDirectory = true;

            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((loadStream = loadFileDialog.OpenFile()) != null)
                {
                    correctFile = loadJsonSerialization(loadStream);
                    loadStream.Close();
                }
            }

            if (correctFile)
            {
                createTableLayoutPanel();
                createToGameMode();
                createLabels();
                tableLayoutPanel1.Enabled = true;
                winLabel.Visible = false;
            }
        }

        private void Create_Click(object sender, EventArgs e)
        {
            var popupForm = new RandomForm(this);
            popupForm.Text = "Create your own Nonogram puzzle";
            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            setColors();
            createTableLayoutPanel();
            gameToCreateMode();
            createLabels();
            tableLayoutPanel1.Enabled = true;
            winLabel.Visible = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Stream saveStream;  
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!saveFileDialog.FileName.EndsWith(".json"))
                    saveFileDialog.FileName = saveFileDialog.FileName + ".json";
                if ((saveStream = saveFileDialog.OpenFile()) != null)
                {
                    saveJsonSerialization(saveStream);
                    saveStream.Close();
                }
            }
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                Left_Click(sender, e);
            else if (me.Button == MouseButtons.Right)
                Right_Click(sender, e);
        }

        private void Left_Click(object sender, EventArgs e)
        {
            Point p = PointToClient(MousePosition);
            int i = (p.X - tableLayoutPanel1.Location.X) / cellSize;
            int j = (p.Y - tableLayoutPanel1.Location.Y) / cellSize;
            isCrossed[i, j] = false;
            if (bgColors[i, j] == Color.White)
            {
                bgColors[i, j] = Color.Black;
                if (isColored[i, j])
                    coloredAmount--;
                else
                    coloredAmount++;
            }
            else
            {
                bgColors[i, j] = Color.White;
                if (isColored[i, j])
                    coloredAmount++;
                else
                    coloredAmount--;
            }
            tableLayoutPanel1.Refresh();

            if (!gameMode)
                setTextOfLabels(i, j);
            else if (coloredAmount == 0)
                wonGame();
        }

        private void Right_Click(object sender, EventArgs e)
        {
            Point p = PointToClient(MousePosition);
            int positionX = (p.X - tableLayoutPanel1.Location.X) / cellSize;
            int positionY = (p.Y - tableLayoutPanel1.Location.Y) / cellSize;
            if (bgColors[positionX, positionY] == Color.Black)
            {
                bgColors[positionX, positionY] = Color.White;
                if (gameMode)
                {
                    if (isColored[positionX, positionY])
                        coloredAmount++;
                    else
                        coloredAmount--;
                    if (coloredAmount == 0)
                        wonGame();
                }
                else
                    setTextOfLabels(positionX, positionY);
            }
            isCrossed[positionX, positionY] = !isCrossed[positionX, positionY];
            tableLayoutPanel1.Refresh();
        }

        private void randomPuzzle()
        {
            Random random = new Random();
            int n;
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                {
                    n = random.Next(0, 2);
                    if (n == 0)
                    {
                        isColored[i, j] = true;
                        coloredAmount++;
                    }
                }
        }

        public void insertValues(allInformations info)
        {
            this.columns = info.columnCount;
            this.rows = info.rowCount;
            this.setColors();
            this.isColored = new bool[info.columnCount, info.rowCount];
            this.coloredAmount = 0;
            for (int i = 0; i < info.columnCount; i++)
                for (int j = 0; j < info.rowCount; j++)
                {
                    this.isColored[i, j] = info.isColored[i * info.rowCount + j];
                    if (this.isColored[i, j])
                        this.coloredAmount++;
                }
        }

        private void createTableLayoutPanel()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowCount = rows;
            tableLayoutPanel1.ColumnCount = columns;
            for (int i = 0; i < rows; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29));
            }
            for (int i = 0; i < columns; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 29));
            }
            tableLayoutPanel1.Size = new Size(columns * cellSize, rows * cellSize);
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (var b = new SolidBrush(bgColors[e.Column, e.Row]))
            {      
                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.Black, 3))
            {
                for (int i = 0; i < columns; i++)
                    for (int j = 0; j < rows; j++)
                        if (isCrossed[i, j])
                        {
                            e.Graphics.DrawLine(pen, new Point(i * 30 + 3, j * 30 + 3), new Point((i + 1) * 30 - 3, (j + 1) * 30 - 3));
                            e.Graphics.DrawLine(pen, new Point((i + 1) * 30 - 3, j * 30 + 3), new Point(i * 30 + 3, (j + 1) * 30 - 3));
                        }
            }
        }

        private void setColors()
        {
            bgColors = new Color[columns, rows];
            isColored = new bool[columns, rows];
            isCrossed = new bool[columns, rows];
            coloredAmount = 0;
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                {
                    bgColors[i, j] = Color.White;
                    isColored[i, j] = false;
                    isCrossed[i, j] = false;
                }
        }

        private void createLabels()
        {
            foreach (var item in columnLabels)
                this.Controls.Remove(item);
            foreach (var item in rowLabels)
                this.Controls.Remove(item);
            columnLabels.Clear();
            rowLabels.Clear();
            setTextOfLabels();
        }

        private void setTextOfLabels(int i, int j)
        {
            int current = 0;
            columnLabels[i].Text = "";
            for (int k = 0; k < rows; k++)
            {
                if (bgColors[i, k] == Color.Black)
                    current++;
                else if (current > 0)
                {
                    columnLabels[i].Text += "\n" + current.ToString();
                    current = 0;
                }
            }
            if (current > 0)
                columnLabels[i].Text += "\n" + current.ToString();
            if (columnLabels[i].Text == "")
                columnLabels[i].Text += "0";

            current = 0;
            rowLabels[j].Text = "";
            for (int k = 0; k < columns; k++)
            {
                if (bgColors[k, j] == Color.Black)
                    current++;
                else if (current > 0)
                {
                    rowLabels[j].Text += current.ToString() + " ";
                    current = 0;
                }
            }
            if (current > 0)
                rowLabels[j].Text += current.ToString() + " ";
            if (rowLabels[j].Text == "")
                rowLabels[j].Text += "0 ";
        }

        private void setTextOfLabels()
        {
            int current;
            for (int i = 0; i < columns; i++)
            {
                columnLabels.Add(new Label()
                {
                    Name = "column" + i.ToString(),
                    Font = new Font("Arial", 8),
                    ForeColor = Color.Black,
                    Size = new Size(cellSize, rows * cellSize / 2),
                    TextAlign = ContentAlignment.BottomCenter,
                });

                current = 0;
                for (int j = 0; j < rows; j++)
                {
                    if (isColored[i, j] == true)
                        current++;
                    else if (current > 0)
                    {
                        columnLabels[i].Text += "\n" + current.ToString();
                        current = 0;
                    }
                }
                if (current > 0)
                    columnLabels[i].Text += "\n" + current.ToString();
                if (columnLabels[i].Text == "")
                    columnLabels[i].Text += "0";
            }

            for (int i = 0; i < rows; i++)
            { 
                rowLabels.Add(new Label()
                {
                    Name = "row" + i.ToString(),
                    Font = new Font("Arial", 8),
                    ForeColor = Color.Black,
                    Size = new Size(columns * cellSize / 2, cellSize),
                    TextAlign = ContentAlignment.MiddleRight,
                });

                current = 0;
                for (int j = 0; j < columns; j++)
                {
                    if (isColored[j, i] == true)
                        current++;
                    else if (current > 0)
                    {
                        rowLabels[i].Text += current.ToString() + " ";
                        current = 0;
                    }
                }
                if (current > 0)
                    rowLabels[i].Text += current.ToString() + " ";
                if (rowLabels[i].Text == "")
                    rowLabels[i].Text += "0 ";
            }
            placeLabels();
        }

        private void placeLabels()
        {
            int topvalue = tableLayoutPanel1.Location.Y - rows * cellSize / 2;
            int leftvalue = tableLayoutPanel1.Location.X + 1;

            foreach (Label item in columnLabels)
            {

                item.Left = leftvalue;
                item.Top = topvalue;
                this.Controls.Add(item);
                leftvalue += cellSize;
            }

            topvalue = tableLayoutPanel1.Location.Y + 1;
            leftvalue = tableLayoutPanel1.Location.X - columns * cellSize / 2;
            foreach (Label item in rowLabels)
            {
                item.Left = leftvalue;
                item.Top = topvalue;
                this.Controls.Add(item);
                topvalue += cellSize;
            }
        }

        private void saveJsonSerialization(Stream saveStream)
        {
            var info = new allInformations();
            info.title = puzzleTitleTextBox.Text;
            info.difficulty = difficultyTextBox.Text;
            info.columnCount = columns;
            info.rowCount = rows;
            info.isColored = new bool[columns * rows];
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                    if (bgColors[i, j] == Color.Black)
                        info.isColored[i * rows + j] = true;

            string serialization = JsonSerializer.Serialize<allInformations>(info);
            StreamWriter sw = new StreamWriter(saveStream);
            sw.Write(serialization);
            sw.Close();
        }

        private bool loadJsonSerialization(Stream loadStream)
        {
            StreamReader sr = new StreamReader(loadStream);
            string serialization = sr.ReadToEnd();
            allInformations info = JsonSerializer.Deserialize<allInformations>(serialization);
            if (info.columnCount < 2 || info.columnCount > 15 || info.rowCount < 2 || info.rowCount > 15 || 
                info.title == null || info.difficulty == null || info.isColored.Length != info.columnCount * info.rowCount)
            {
                var invalidForm = new InvalidFileForm();
                invalidForm.ShowDialog();
                sr.Close();
                return false;
            }

            insertValues(info);
            sr.Close();
            return true;
        }

        private void createToGameMode()
        {
            if (!gameMode)
            {
                gameMode = true;
                puzzleSettingGroupBox.Visible = false;
            }
            tableLayoutPanel1.Location = new Point((this.Width - tableLayoutPanel1.Width) / 2, (this.Height - tableLayoutPanel1.Height) / 2);
        }

        private void gameToCreateMode()
        {
            if (gameMode)
            {
                gameMode = false;
                puzzleSettingGroupBox.Visible = true;
            }
            tableLayoutPanel1.Location = new Point((this.Width * 7 / 8) - (tableLayoutPanel1.Width), (this.Height - tableLayoutPanel1.Height) / 2);
        }

        private void wonGame()
        {
            winLabel.Visible = true;
            tableLayoutPanel1.Enabled = false;
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }

    public struct allInformations
    {
        public string title { get; set; }
        public string difficulty { get; set; }
        public int columnCount { get; set; }
        public int rowCount { get; set; }
        public bool[] isColored { get; set; }
    };

}
