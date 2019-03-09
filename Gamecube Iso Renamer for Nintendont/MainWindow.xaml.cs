using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gamecube_Iso_Renamer_for_Nintendont
{
    public partial class MainWindow : Window
    {
        private string statusText;
        private int fileCount;

        public MainWindow()
        {
            InitializeComponent();
            //statusTextBlock.Text = " \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf ";
            //scrollViewer.ScrollToBottom();
            finishedTextBlock.Visibility = Visibility.Hidden;
        }

        private void OpenFolderDialog(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    folderPathTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void RenameClicked(object sender, RoutedEventArgs e)
        {
            statusText = "";
            fileCount = 0;
            finishedTextBlock.Visibility = Visibility.Hidden;
            GetFileCount(folderPathTextBox.Text);
            Rename(folderPathTextBox.Text);
        }

        public void GetFileCount(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            FileInfo[] info = dirInfo.GetFiles("*.iso*");
            foreach (FileInfo f in info)
            {
                fileCount++;
            }

            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            foreach (DirectoryInfo directory in subDirectories)
            {
                Rename(directory.FullName);
            }
        }

        public void Rename(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            bool isDisc2 = false;

            FileInfo[] info = dirInfo.GetFiles("*.iso*");
            foreach (FileInfo f in info)
            {
                string curDir = System.IO.Path.GetDirectoryName(f.FullName);
                if (curDir.Contains("Disc 2"))
                {
                    isDisc2 = true;
                    string disc1Dir = curDir.Replace("Disc 2", "Disc 1");
                    File.Move(f.FullName, System.IO.Path.Combine(disc1Dir, "disc2.iso"));
                    Directory.Delete(curDir);
                    string disc1ShortenedDir = disc1Dir.Replace(" (Disc 1)", "");
                    Directory.Move(disc1Dir, disc1ShortenedDir);
                    statusText += System.IO.Path.GetFileName(f.FullName) + " moved and renamed to disc2.iso \n";
                }
                else
                {
                    if (!f.Name.Equals("game.iso") && !f.Name.Equals("disc2.iso"))
                    {
                        File.Move(f.FullName, System.IO.Path.Combine(curDir, "game.iso"));
                        statusText += System.IO.Path.GetFileName(f.FullName) + " renamed to game.iso \n";
                    }
                }

                statusTextBlock.Text = statusText;
                scrollViewer.ScrollToBottom();
                fileCount--;
            }

            if (!isDisc2)
            {
                foreach (DirectoryInfo directory in subDirectories)
                {
                    Rename(directory.FullName);
                }
            }

            if (fileCount == 0)
            {
                finishedTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
