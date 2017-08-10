using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyManifests
{
    public partial class BackgroundWorkerDialog : Form
    {
        //This form exposes a background worker which can be used to perform operations
        //in the background. Set the DoWork event handler of this form's background worker
        //to the method which should be executed, and then call the Start method. This form
        //reports the progress of the background worker to the user.

        private Exception MyException;
        private bool MyCancelled;

        public BackgroundWorkerDialog()
        {
            InitializeComponent();
            MyCancelled = false;
            MyException = null;
        }

        public bool Cancelled
        {
            get { return MyCancelled; }
        }

        public void Start()
        {
            StatusLabel.Text = "Starting...";
            MyBackgroundWorker.RunWorkerAsync();
            this.ShowDialog();
        }

        public BackgroundWorker Worker
        {
            get { return MyBackgroundWorker; }
            set { MyBackgroundWorker = value; }
        }

        private void MyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusLabel.Text = e.UserState.ToString();
        }

        //private void MyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        MessageBox.Show(e.Error.Message);
        //    }
        //    else if (e.Result != null)
        //    {
        //        MessageBox.Show(e.Result.ToString());
        //    }
        //    this.Close();
        //}

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                CloseButton.Enabled = false;
                StatusLabel.Text = "Attempting to cancel, please wait...";
                MyBackgroundWorker.CancelAsync();
                MyCancelled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when cancelling operation: " + ex.Message);
            }
        }

        private void MyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MyException = e.Error;
            this.Hide();
        }

        public Exception ThrownException
        {
            get { return MyException; }
        }


    }
}
