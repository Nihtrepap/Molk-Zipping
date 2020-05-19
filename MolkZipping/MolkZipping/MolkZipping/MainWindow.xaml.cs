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
using System.Windows.Threading;

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
        private string saveTo;
        private string opened;
        DispatcherTimer loadingTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Handles all click events
        /// </summary>
        /// <param name="sender">The clicking object</param>
        /// <param name="e">Click event</param>
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
                else if(btn.Name == "BtnPackFiles") 
                {
                    if (opened != null || opened == "")
                    {
                        MessageBoxResult answer = MessageBox.Show("Do you really want to pack files?", "title", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                        switch (answer)
                        {
                            case MessageBoxResult.Yes:
                                Cmd_Pack();
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not find any files to pack.\n Please check files and try again.", "Molk did not pack", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
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

        /// <summary>
        /// Method for all radiobuttons
        /// </summary>
        /// <param name="sender">radiobutton</param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method is used when mouse leaves the menu bar.
        /// Closes the menubar automaticly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_Leave_Menu(object sender, MouseEventArgs e)
        {
            Advanced.Visibility = Visibility.Hidden;
            menuClick = false;
        }

        /// <summary>
        /// This method saves the file path into a string
        /// and adds .molk extension. 
        /// Also only make it possible for the user to save into molk extension.
        /// </summary>
        private void Save_File_Dialog()
        {
            SaveFileDialog openSaveFile = new SaveFileDialog();
            openSaveFile.Filter = "molk files (*.molk)|*.molk*";
            openSaveFile.FilterIndex = 1;
            openSaveFile.FileName = "Molkzipping";
            openSaveFile.ShowDialog();
            saveTo = openSaveFile.FileName + ".molk";
            SaveToText.Text = $"{saveTo}";

        }

        /// <summary>
        /// Used to open a file dialog.
        /// Saves the chosen file path.
        /// </summary>
        /// <seealso cref="Cmd_run(string)"/>
        private void Open_File_Dialog()
        {
            CommonOpenFileDialog openFileWindow = new CommonOpenFileDialog();
            openFileWindow.IsFolderPicker = folderPick;
            
            if (openFileWindow.ShowDialog() != CommonFileDialogResult.Ok) return;
            else opened = openFileWindow.FileName;
        
            Cmd_run(opened);
            
        }

        /// <summary>
        /// Takes information about the chosen file from
        /// file dialog window.
        /// Information gained: 
        /// -Filename
        /// -Date
        /// -Size
        /// -Time
        /// </summary>
        /// <param name="fileOpen">Chosen file from file dialog window</param>
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

                string cmd = $"dir /B \"{fileOpen}\" > {tmp}";
                
                processCmd.StandardInput.WriteLine(cmd);
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                File_Reader(tmp);

                Get_Fileinfo(tmp);

            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Simple ReadAllText method from File class.
        /// Reads programs tmp.txt file to get value into string.
        /// </summary>
        /// <param name="tmp">Path for program tmp file</param>
        private void File_Reader(string tmp)
        {
            string read;
            read = File.ReadAllText(tmp, Encoding.UTF8);
            
        }

        /// <summary>
        /// Reads the programs tmp file to gain information from
        /// the chosen file in the file dialog window.
        /// Adds the info into the datagrid view.
        /// </summary>
        /// <param name="tmp">Contains tmp.txt data</param>
        private void Get_Fileinfo(string tmp)
        {
            try
            {
                string line;
                string type = "";
                FileStream fStream = new FileStream(tmp, FileMode.Open, FileAccess.Read);
                StreamReader streamR = new StreamReader(fStream, Encoding.UTF8);

                while ((line = streamR.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string name = split[0];
                    if (split.Length != 2) { type = "folder"; }
                    else { type = split[1]; }

                    packList.Add(new Pack(name, type));
                    GridPack.Items.Refresh();
                }
                fStream.Close();
            }catch(Exception e) { MessageBox.Show("WRONG - Give this message to the Developers ===>\n"+e,"Molk found error",MessageBoxButton.OK,MessageBoxImage.Error); }
        }

        private void Cmd_Pack()
        {
            try
            {
                string _directory = " ";

                Process processCmd = new Process();

                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.Start();

                string cmd = $@"molk {_directory} {saveTo} {opened}";

                processCmd.StandardInput.WriteLine(cmd);
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                Loading_Screen();
            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Handles screen switch when loading.
        /// Starts a timer.
        /// </summary>
        private void Loading_Screen()
        {
            Pack.Visibility = Visibility.Hidden;
            Loading.Visibility = Visibility.Visible;
            loadingTimer.Tick += Done_Loading;           
            loadingTimer.Interval = new TimeSpan(0,0,3);       

            loadingTimer.Start();
            loadingTimer.IsEnabled = true;
        }

        /// <summary>
        /// Handles events to occure when loading is done.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="e"></param>
        private void Done_Loading(object timer, EventArgs e)
        {
            loadingTimer.IsEnabled = false;
            loadingTimer.Stop();
            packList.Clear();
            GridPack.Items.Refresh();
            Main.Visibility = Visibility.Visible;
            Loading.Visibility = Visibility.Hidden;
            opened = "";
            MessageBox.Show("File(s) succesfully molked! ","Molk zipping tool", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
