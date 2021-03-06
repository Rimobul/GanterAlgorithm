﻿using Ganter.Algorithm;
using Ganter.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private Stopwatch inputStop = new Stopwatch();
        private Stopwatch ganterStop = new Stopwatch();
        private Stopwatch outputStop = new Stopwatch();

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
                        inputStop.Start();
                        CsvParser parser = new CsvParser(fileStream, defaultTrue, defaultFalse, separator);
                        parser.OnThresholdsFound += Parser_OnThresholdsFound;
                        FormalContext context = null;

                        if (chkPreprocess.Checked)
                            context = parser.PreprocessData();
                        else
                            context = parser.ParseContext();

                        inputStop.Stop();
                        lblInputTime.Text = "Input processing: " + inputStop.Elapsed.ToString("G");
                        parser.OnThresholdsFound -= Parser_OnThresholdsFound;
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

        private void Parser_OnThresholdsFound(List<Algorithm.Attribute> stepAttributes)
        {
            inputStop.Stop();

            Threshold threshold = new Threshold();
            threshold.DataBind(stepAttributes);
            if(threshold.ShowDialog() == DialogResult.OK)
            {
                var x = true;
            }

            inputStop.Start();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            inputStop.Start();
            List<Item> items = GetItems();
            List<Algorithm.Attribute> attributes = GetAttributes();
            bool[,] matrix = GetMatrix(items.Count, attributes.Count);

            try
            {
                FormalContext context = new FormalContext(attributes, items, matrix, true);
                inputStop.Stop();
                lblInputTime.Text = "Input processing: " + inputStop.Elapsed.ToString("G");
                GenerateOutput(context);
            }
            catch (Exception ex)
            {
                inputStop.Stop();
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void GenerateOutput(FormalContext context)
        {
            ganterStop.Start();
            var ganterResult = context.PerformAlgorithm();
            ganterStop.Stop();
            lblTimeGanter.Text = "Ganter algorithm: " + ganterStop.Elapsed.ToString("G");

            outputStop.Start();

            SaveIntoFile(context, ganterResult);
            outputStop.Stop();
            lblTimeOutput.Text = "Output creation: " + outputStop.Elapsed.ToString("G");
            lblTimeTotal.Text = "Total time: " + (inputStop.Elapsed + ganterStop.Elapsed + outputStop.Elapsed).ToString("G");

            inputStop.Reset();
            ganterStop.Reset();
            outputStop.Reset();
        }

        private void SaveIntoFile(FormalContext context, List<List<Algorithm.Attribute>> ganterResult)
        {
            if (string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                MessageBox.Show("Output folder cannot be empty!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(txtOutputPath.Text))
            {
                Directory.CreateDirectory(txtOutputPath.Text);
            }

            string filePath = Path.Combine(txtOutputPath.Text, new string(DateTime.Now.ToString().Where(c => char.IsLetterOrDigit(c)).ToArray()));
            filePath += ".txt";

            using (StreamWriter stream = new StreamWriter(filePath))
            {
                //using (BinaryWriter writer = new BinaryWriter(stream.BaseStream))
                //{
                //    context.WriteOutput(writer, ganterResult, rbTranReduction.Checked, chkAttributes.Checked, chkItems.Checked, txtSeparator.Text);
                //}

                context.WriteOutput(stream, ganterResult, rbTranReduction.Checked, chkAttributes.Checked, chkItems.Checked, txtSeparator.Text);
            }

            Process.Start(filePath);
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

        private void btnTestPlanets_Click(object sender, EventArgs e)
        {
            inputStop.Start();
            FormalContext context = TestContexts.GeneratePlanets();
            inputStop.Stop();
            lblInputTime.Text = "Input processing: " + inputStop.Elapsed.ToString("G");

            GenerateOutput(context);
        }

        private void btnTestPapers_Click(object sender, EventArgs e)
        {
            inputStop.Start();
            FormalContext context = TestContexts.GeneratePapers();
            inputStop.Stop();
            lblInputTime.Text = "Input processing: " + inputStop.Elapsed.ToString("G");

            GenerateOutput(context);
        }

        private void btnTestNumbers_Click(object sender, EventArgs e)
        {
            inputStop.Start();
            FormalContext context = TestContexts.GenerateOneToTen();
            inputStop.Stop();
            lblInputTime.Text = "Input processing: " + inputStop.Elapsed.ToString("G");

            GenerateOutput(context);
        }

        private void chkPreprocess_CheckedChanged(object sender, EventArgs e)
        {
            txtDefaultFalse.Enabled = !chkPreprocess.Checked;
            txtDefaultTrue.Enabled = !chkPreprocess.Checked;
        }
    }
}
