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

            FileInfo[] info = dirInfo.GetFiles("*.iso*");
            foreach (FileInfo f in info)
            {
                string curDir = System.IO.Path.GetDirectoryName(f.FullName);
                statusText += System.IO.Path.GetFileName(f.FullName) + " renamed to game.iso \n";
                statusTextBlock.Text = statusText;
                //Console.WriteLine(curDir);
                File.Move(f.FullName, System.IO.Path.Combine(curDir, "game.iso"));
                fileCount--;
            }

            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            foreach (DirectoryInfo directory in subDirectories)
            {
                Rename(directory.FullName);
            }

            if (fileCount == 0)
            {
                finishedTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
