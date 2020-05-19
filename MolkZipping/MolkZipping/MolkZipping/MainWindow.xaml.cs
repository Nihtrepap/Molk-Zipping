using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace MolkZipping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Pack> packList = new List<Pack>();
        private bool menuClick = false;
        private bool folderPick = true;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button btn)
            {
                if (btn.Name == "BtnPack") { Main.Visibility = Visibility.Hidden; Pack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnUnpack") { Main.Visibility = Visibility.Hidden; Unpack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnSaveTo") { Save_File_Dialog(); }
                else if (btn.Name == "BtnMenu") {
                    if (!menuClick) { Advanced.Visibility = Visibility.Visible; menuClick = true; }
                }
                else if (btn.Name == "BtnBackUnPack") { Main.Visibility = Visibility.Visible; Unpack.Visibility = Visibility.Hidden; }
                else if (btn.Name == "BtnBackPack") { Main.Visibility = Visibility.Visible; Pack.Visibility = Visibility.Hidden; }
                else if (btn.Name == "BtnChoosePackFile") { GridPack.ItemsSource = packList; Open_File_Dialog(); }
            }
            else if (sender is Image btnImage)
            {
                if (btnImage.Name == "question") { System.Diagnostics.Process.Start("https://testpalatset.wordpress.com/"); }
                else if (btnImage.Name == "Exit") { Application.Current.Shutdown(); }
                else if(btnImage.Name == "Instagram") { System.Diagnostics.Process.Start("https://www.instagram.com/molkutbildning/"); }
                else if(btnImage.Name == "Facebook") { System.Diagnostics.Process.Start("https://www.facebook.com/molkutbildning/"); }
                else if(btnImage.Name == "Web") { System.Diagnostics.Process.Start("https://www.molk.se/"); }
            }
        }

        private void Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectFolder.IsChecked == true)
            {
                folderPick = true;
            }
            else if (SelectFile.IsChecked == true)
            {
                folderPick = false;
            }
        }

        private void Mouse_Leave_Menu(object sender, MouseEventArgs e)
        {
            Advanced.Visibility = Visibility.Hidden;
            menuClick = false;
        }

        private void Save_File_Dialog()
        {
            string saveTo;
            CommonOpenFileDialog openSaveTo = new CommonOpenFileDialog();
            openSaveTo.IsFolderPicker = true;

            if (openSaveTo.ShowDialog() != CommonFileDialogResult.Ok)
            {
                SaveToText.Text = "You have not selected any folder.";
                return;
            }
            else saveTo = openSaveTo.FileName;

            SaveToText.Text = $"{saveTo}";

        }

        private void Open_File_Dialog()
        {
            string opened;
            CommonOpenFileDialog openFileWindow = new CommonOpenFileDialog();
            openFileWindow.IsFolderPicker = folderPick;
            
            if (openFileWindow.ShowDialog() != CommonFileDialogResult.Ok) return;
            else opened = openFileWindow.FileName;
        
            Cmd_run(opened);
            
        }

        private void Cmd_run(string fileOpen)
        {
            try
            {
                string tmp = @"tmp.txt";

                Process processCmd = new Process();

                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.Start();

                string cmd = $"dir \"{fileOpen}\" > {tmp}";
                
                processCmd.StandardInput.WriteLine(cmd);
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                File_Reader(tmp);

                Get_Fileinfo(tmp);

            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG\n"+e);
            }
        }

        private void File_Reader(string tmp)
        {
            string read;
            read = File.ReadAllText(tmp, Encoding.UTF8);
            

        }

        private void Get_Fileinfo(string tmp)
        {
            string line;
            FileStream fStream = new FileStream(tmp, FileMode.Open, FileAccess.Read);
            StreamReader streamR = new StreamReader(fStream, Encoding.UTF8);
          

            while ((line = streamR.ReadLine()) != null)
            {
                if ((System.Text.RegularExpressions.Regex.IsMatch(line,"\\d{2}:\\d{2}")))    //"/\\d{4}" ))
                   // || System.Text.RegularExpressions.Regex.IsMatch(line, "^\\d{4}-\\d{2}-\\d{2}"))
                {
                    string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string date = split[0];
                    string time = split[1];
                    string size = split[2] + " kb";
                    string name = split[3];
                  
                    packList.Add(new Pack(name, size, time, date));
                    GridPack.Items.Refresh();
                    
                }
            }
            fStream.Close();
        }
    }
}
