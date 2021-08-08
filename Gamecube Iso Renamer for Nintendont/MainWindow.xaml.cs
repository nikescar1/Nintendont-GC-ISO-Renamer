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
        private int currentFileCount;
        private int fileCount;

        public MainWindow()
        {
            InitializeComponent();
            //statusTextBlock.Text = " \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf  \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n sdfjsdf ";
            //scrollViewer.ScrollToBottom();
            //finishedTextBlock.Visibility = Visibility.Hidden;
        }

        private void OpenFolderDialog(object sender, RoutedEventArgs re)
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

        private void RenameClicked(object sender, RoutedEventArgs re)
        {
            statusText = "";
            fileCount = 0;
            progressBar.Value = 0;
            GetFileCount(folderPathTextBox.Text);
            currentFileCount = fileCount;

            // var renamer = new CoroutineOnEnumerable();
            //renamer.Progressed += (s, e) => progressBar.Value = renamer.Progress;
            //renamer.Start(Rename(folderPathTextBox.Text));

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
                GetFileCount(directory.FullName);
            }
        }

        public void Rename(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            bool isDisc2 = false;

            FileInfo[] info = dirInfo.GetFiles("*.iso*", SearchOption.AllDirectories);
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
                    Run run = new Run(f.Name + " Disc 2 moved and renamed to disc2.iso \n");
                    run.Foreground = Brushes.DarkOliveGreen;
                    statusTextBlock.Inlines.Add(run);
                    //statusText += System.IO.Path.GetFileName(f.FullName) + " moved and renamed to disc2.iso \n";
                }
                else
                {
                    Console.WriteLine($"{dirInfo.Name} {f.Directory.Name}");
                    if (f.Directory.Name.Equals(dirInfo.Name))
                    {
                        DirectoryInfo newDir = Directory.CreateDirectory(System.IO.Path.Combine(curDir, f.Name.Substring(0, f.Name.Length - 4)));
                        curDir = newDir.FullName;

                        Run run = new Run(f.Name.Substring(0, f.Name.Length - 4) + " Directory Created \n");
                        run.Foreground = Brushes.DarkSeaGreen;
                        statusTextBlock.Inlines.Add(run);
                    }

                    if (!f.Name.Equals("game.iso") && !f.Name.Equals("disc2.iso"))
                    {
                        File.Move(f.FullName, System.IO.Path.Combine(curDir, "game.iso"));
                        Run run = new Run(f.Name + " Disc 1 renamed to game.iso \n");
                        run.Foreground = Brushes.DarkGreen;
                        statusTextBlock.Inlines.Add(run);
                        //statusText += System.IO.Path.GetFileName(f.FullName) + " renamed to game.iso \n";
                    }
                    else
                    {
                        Run run = new Run(f.FullName + " skipped because it already matches naming convention \n");
                        run.Foreground = Brushes.DarkOrange;
                        statusTextBlock.Inlines.Add(run);
                        //statusText += System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(f.FullName)) + " skipped because it already matches naming convention \n";
                    }
                }

                //statusTextBlock.Text = statusText;
                scrollViewer.ScrollToBottom();
                currentFileCount--;
            }

            //yield return (double)currentFileCount / (double)fileCount;

            //if (!isDisc2)
            //{
            //    foreach (DirectoryInfo directory in subDirectories)
            //    {
            //        Rename(directory.FullName);
            //    }
            //}
            /*
            if (currentFileCount == 0)
            {
                finishedTextBlock.Visibility = Visibility.Visible;
            }
            */
        }
    }
}
