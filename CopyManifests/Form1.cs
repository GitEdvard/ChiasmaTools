using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CopyManifests
{
    public partial class Form1 : Form
    {
        private const string ROOT = @"L:\Labsystem\Illumina SNP genotyping\INFINIUM";
        private const string DESTINATION = @"C:\Manifest files";
        //private const string ROOT = @"C:\Småjobb\2014\Juni\Undersökning strandreferens";
        //private const string DESTINATION = @"C:\Småjobb\2014\Juni\Test";

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            DestinationTextBox.Text = DESTINATION;
            RootTextBox.Text = ROOT;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            BackgroundWorkerDialog bgw;
            ManifestFileCopier manifestFileCopier;
            try
            {
                bgw = new BackgroundWorkerDialog();
                manifestFileCopier = new ManifestFileCopier(ROOT, DESTINATION, bgw.Worker);
                bgw.Start();
                MessageBox.Show("Number of files processed: " + manifestFileCopier.NumberProcessedFiles.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + Environment.NewLine + ex.Message);
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
