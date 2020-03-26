using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bootsfahrschule_OWL
{
    public partial class Database : Form
    {
        string connectionString;
        string imgLocation = "";
        string imgLocation2 = "";
        


        public Database(string Username)
        {
            string Name = Username;
            Debug.Print(Username + " " + Name);            
            InitializeComponent();
            this.UserName.Text = Username;
            connectionString = ConfigurationManager.ConnectionStrings["Bootsfahrschule_OWL.Properties.Settings.DatabaseSKSConnectionString"].ConnectionString;
        }



        private void sksDataBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.sksDataBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseSKSDataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {            
            this.sksDataTableAdapter.Fill(this.databaseSKSDataSet.SksData);

        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg files(*.jpg)|*.jpg|png files(*.png)|*.png|gif files(*.gif)|*.gif|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                bild1.ImageLocation = imgLocation;
            }
        }

        private void browse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg files(*.jpg)|*.jpg|png files(*.png)|*.png|gif files(*.gif)|*.gif|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation2 = dialog.FileName.ToString();
                bild2.ImageLocation = imgLocation2;
            }
        }

    }
}
