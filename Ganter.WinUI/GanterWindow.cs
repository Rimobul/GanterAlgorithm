using Ganter.Algorithm;
using Ganter.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ganter.WinUI
{
    public partial class GanterWindow : Form
    {
        public GanterWindow()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            Stream fileStream = null;
            if ((openFileDialog.ShowDialog() == DialogResult.OK) && (fileStream = openFileDialog.OpenFile()) != null)
            {
                string fileName = openFileDialog.FileName;
                string defaultTrue = txtDefaultTrue.Text;
                string defaultFalse = txtDefaultFalse.Text;

                if (string.IsNullOrWhiteSpace(defaultFalse) || string.IsNullOrWhiteSpace(defaultTrue))
                {
                    MessageBox.Show("Both true and false value representatives have to be set!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string separator = txtSeparator.Text;

                if (string.IsNullOrEmpty(separator))
                {
                    MessageBox.Show("Separator must be a string sequence or a character (including whitespaces)!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (fileStream)
                {
                    try
                    {
                        CsvParser parser = new CsvParser(fileStream, defaultTrue, defaultFalse, separator);
                        FormalContext context = parser.ParseContext();

                        GenerateOutput(context);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        try
                        {
                            fileStream.Dispose();
                        }
                        catch { }
                    }
                }
            }

            try
            {
                fileStream.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            GenerateOutput(TestContexts.GenerateOneToSeven());

            List<Item> items = GetItems();
            List<Algorithm.Attribute> attributes = GetAttributes();
            bool[,] matrix = GetMatrix(items.Count, attributes.Count);

            FormalContext context = new FormalContext(attributes, items, matrix, true);

            GenerateOutput(context);
        }

        private void GenerateOutput(FormalContext context)
        {
            if (rbTranReduction.Checked)
            {
                Lattice lattice = new Lattice(context.PerformAlgorithm());

                SaveIntoFile(lattice.ToString());
            }
            else
            {

            }
        }

        private void SaveIntoFile(string output)
        {
            if(string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                MessageBox.Show("Output folder cannot be empty!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(!Directory.Exists(txtOutputPath.Text))
            {
                Directory.CreateDirectory(txtOutputPath.Text);
            }

            string filePath = Path.Combine(txtOutputPath.Text, new string(DateTime.Now.ToString().Where(c => char.IsLetterOrDigit(c)).ToArray()));
            filePath += ".txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(output);
            }
        }

        private bool[,] GetMatrix(int itemsCount, int attributesCount)
        {
            bool[,] result = new bool[itemsCount, attributesCount];
            for (int i = 1; i <= itemsCount; i++)
            {
                for (int j = 1; j <= attributesCount; j++)
                {
                    string controlName = "chk" + i.ToString() + j.ToString();
                    object control = Controls.Find(controlName, true).FirstOrDefault();
                    if (control != null && control.GetType() == typeof(CheckBox))
                    {
                        result[i - 1, j - 1] = (control as CheckBox).Checked;
                    }
                }
            }
            return result;
        }

        private List<Item> GetItems()
        {
            List<Item> result = new List<Item>();
            if (!string.IsNullOrWhiteSpace(txtItem1.Text))
            {
                result.Add(new Item() { Name = txtItem1.Text });
                if (!string.IsNullOrWhiteSpace(txtItem2.Text))
                {
                    result.Add(new Item() { Name = txtItem2.Text });
                    if (!string.IsNullOrWhiteSpace(txtItem3.Text))
                    {
                        result.Add(new Item() { Name = txtItem3.Text });
                        if (!string.IsNullOrWhiteSpace(txtItem4.Text))
                        {
                            result.Add(new Item() { Name = txtItem4.Text });
                            if (!string.IsNullOrWhiteSpace(txtItem5.Text))
                            {
                                result.Add(new Item() { Name = txtItem5.Text });
                                if (!string.IsNullOrWhiteSpace(txtItem6.Text))
                                {
                                    result.Add(new Item() { Name = txtItem6.Text });
                                    if (!string.IsNullOrWhiteSpace(txtItem7.Text))
                                    {
                                        result.Add(new Item() { Name = txtItem7.Text });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private List<Algorithm.Attribute> GetAttributes()
        {
            List<Algorithm.Attribute> result = new List<Algorithm.Attribute>();
            if (!string.IsNullOrWhiteSpace(txtAtt1.Text))
            {
                result.Add(new Algorithm.Attribute() { Name = txtAtt1.Text });
                if (!string.IsNullOrWhiteSpace(txtAtt2.Text))
                {
                    result.Add(new Algorithm.Attribute() { Name = txtAtt2.Text });
                    if (!string.IsNullOrWhiteSpace(txtAtt3.Text))
                    {
                        result.Add(new Algorithm.Attribute() { Name = txtAtt3.Text });
                        if (!string.IsNullOrWhiteSpace(txtAtt4.Text))
                        {
                            result.Add(new Algorithm.Attribute() { Name = txtAtt4.Text });
                            if (!string.IsNullOrWhiteSpace(txtAtt5.Text))
                            {
                                result.Add(new Algorithm.Attribute() { Name = txtAtt5.Text });
                                if (!string.IsNullOrWhiteSpace(txtAtt6.Text))
                                {
                                    result.Add(new Algorithm.Attribute() { Name = txtAtt6.Text });
                                    if (!string.IsNullOrWhiteSpace(txtAtt7.Text))
                                    {
                                        result.Add(new Algorithm.Attribute() { Name = txtAtt7.Text });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void rbFull_CheckedChanged(object sender, EventArgs e)
        {
            chkVisualization.Enabled = !rbFull.Checked;
        }
    }
}
