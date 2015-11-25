using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ganter.WinUI
{
    public partial class Threshold : Form
    {
        public Threshold()
        {
            InitializeComponent();
        }

        public void DataBind(List<Algorithm.Attribute> attributes)
        {
            attributeBindingSource.DataSource = attributes;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
