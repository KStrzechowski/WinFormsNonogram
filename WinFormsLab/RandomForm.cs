using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsLab
{
    public partial class RandomForm : Form
    {
        Form1 parentForm;
        int columns = 10;
        int rows = 10;
        public RandomForm(Form1 form)
        {
            InitializeComponent();
            parentForm = form;
            widthTextBox.Text = "10";
            heightTextBox.Text = "10";
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            parentForm.rows = rows;
            parentForm.columns = columns;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void widthTextBox_Validating(object sender, CancelEventArgs e)
        {
            try 
            {
                if (Convert.ToInt32(widthTextBox.Text) < 2 || Convert.ToInt32(widthTextBox.Text) > 15)
                {
                    errorProvider1.SetError(widthTextBox, "Width must be integer number in range 2-15");
                    e.Cancel = true;
                    return;
                }
                columns = Convert.ToInt32(widthTextBox.Text);
                errorProvider1.SetError(widthTextBox, string.Empty);
                e.Cancel = false;
            }
            catch (FormatException)
            {
                errorProvider1.SetError(widthTextBox, "Width must be integer number in range 2-15");
                e.Cancel = true;
                return;
            }
        }

        private void heightTextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(heightTextBox.Text) < 2 || Convert.ToInt32(heightTextBox.Text) > 15)
                {
                    errorProvider1.SetError(heightTextBox, "Height must be integer number in range 2-15");
                    e.Cancel = true;
                    return;
                }
                rows = Convert.ToInt32(heightTextBox.Text);
                errorProvider1.SetError(heightTextBox, string.Empty);
                e.Cancel = false;
            }
            catch (FormatException)
            {
                errorProvider1.SetError(heightTextBox, "Height must be integer number in range 2-15");
                e.Cancel = true;
                return;
            }
        }
    }
}
