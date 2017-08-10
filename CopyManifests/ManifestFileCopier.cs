using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace CopyManifests
{
    public class ManifestFileCopier
    {
        private string MyRootFolder;
        private string MyDestinationFolder;
        private BackgroundWorker MyBackgroundWorker;
        private int MyNumberProcessedFiles;
        

        public ManifestFileCopier(string rootFolder, string destinationFolder, BackgroundWorker bgw)
        {
            MyRootFolder = rootFolder;
            MyDestinationFolder = destinationFolder;
            MyBackgroundWorker = bgw;
            MyBackgroundWorker.DoWork += CopyManifests;
        }

        public int NumberProcessedFiles
        {
            get
            {
                return MyNumberProcessedFiles;
            }
        }

        private void CopyManifests(object sender, EventArgs e)
        {
            MyNumberProcessedFiles = 0;
            CopyManifests_Rec(MyRootFolder, ref MyNumberProcessedFiles);
        }

        private void CopyManifests_Rec(string rootDir, ref int numberFilesCopied)
        {
            string[] dirs;
            CopyManifestsInCatalog(rootDir, ref numberFilesCopied);
            if (MyBackgroundWorker.CancellationPending)
            {
                return;
            }
            dirs = Directory.GetDirectories(rootDir);
            foreach (string path in dirs)
            {
                CopyManifests_Rec(path, ref numberFilesCopied);
            }
        }

        private void CopyManifestsInCatalog(string path, ref int numberFilesCopied)
        {
            string[] files;
            string fileName, destFilePath;
            string reportString;
            try
            {
                files = Directory.GetFiles(path, "*.csv");
                foreach (string file in files)
                {
                    fileName = file.Substring(file.LastIndexOf(@"\") + 1);
                    if (IsNotEmpty(fileName))
                    {
                        destFilePath = MyDestinationFolder + @"\" + fileName;
                        reportString = "Copy file: " + fileName + Environment.NewLine;
                        reportString += "Number of files copied: " + numberFilesCopied.ToString();
                        MyBackgroundWorker.ReportProgress(0, reportString);
                        CopyFile(file, destFilePath);
                        numberFilesCopied++;
                    }
                    if (MyBackgroundWorker.CancellationPending)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CopyFile(string filePath, string destFilePath)
        {
            try
            {
                if (File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }
                File.Copy(filePath, destFilePath);
            }
            catch (Exception)
            {
                
            }
        }

        private bool IsNotEmpty(string str)
        {
            return str != null && str.Trim().Length != 0;
        }

    }
}
