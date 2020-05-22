using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private int whyYouLoop = 0;
        public bool folderPick = true;

        DispatcherTimer loadingTimer = new DispatcherTimer();
        PackMethod packMethod;
        Dialog dia;

        public MainWindow()
        {
            InitializeComponent();
            dia = new Dialog(this);
            packMethod = new PackMethod(dia, this);
        }
        
        /// <summary>
        /// Handles all click events
        /// </summary>
        /// <param name="sender">The clicking object</param>
        /// <param name="e">Click event</param>
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Name == "BtnPack") { this.Title = "Molk pack"; Main.Visibility = Visibility.Hidden; Pack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnUnpack") { this.Title = "Molk unpack"; Main.Visibility = Visibility.Hidden; Unpack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnSaveTo") { dia.Save_File_Dialog(); }
                else if (btn.Name == "BtnMenu") 
                {
                    if (!menuClick) { Advanced.Visibility = Visibility.Visible; menuClick = true; }
                }
                else if (btn.Name == "BtnBackUnPack") { this.Title = "Molk tool"; Main.Visibility = Visibility.Visible; Unpack.Visibility = Visibility.Hidden; }
                else if (btn.Name == "BtnUnPackFiles") 
                { 
                    dia.Save_File_Dialog(); 
                    if (dia.saveFile == true) { Unpack.Visibility = Visibility.Hidden; Loading.Visibility = Visibility.Visible; Cmd_UnPack(); whyYouLoop = 0; } 
                }
                else if (btn.Name == "BtnChooseUnpackFiles") { dia.Open_File_Dialog(); GridUnpack.ItemsSource = packList; GridUnpack.Items.Refresh(); }
                else if(btn.Name == "BtnClearUnPackData") { TxtInsideMolk.Text = ""; packList.Clear(); GridUnpack.Items.Refresh();  }

                else if (btn.Name == "BtnBackPack") { this.Title = "Molk tool"; Main.Visibility = Visibility.Visible; Pack.Visibility = Visibility.Hidden; }
                else if (btn.Name == "BtnChoosePackFile") { GridPack.ItemsSource = packList; dia.Open_File_Dialog();  }   
                else if (btn.Name == "BtnClearPackData") { packList.Clear(); GridPack.Items.Refresh();  }
                else if(btn.Name == "BtnPackFiles") 
                {
                    if (dia.opened != null || dia.opened != "")
                    {
                        MessageBoxResult answer = MessageBox.Show("Do you really want to pack files?", "title", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                        switch (answer)
                        {
                            case MessageBoxResult.Yes:
                                packMethod.Cmd_Pack();
                                whyYouLoop = 0;
                                break;
                            default: break;
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
        /// Simple ReadAllText method from File class.
        /// Reads programs tmp.txt file to get value into string.
        /// </summary>
        /// <param name="tmp">Path for program tmp file</param>
        public void File_Reader(string tmp)
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
        public void Get_Fileinfo(string tmp)
        {
            try
            {
                if (Unpack.Visibility == Visibility.Visible)
                {
          
                    TxtInsideMolk.Text = File.ReadAllText(tmp, Encoding.UTF8);
                    string line;
                    string type = "";
                    FileStream fStream = new FileStream(tmp, FileMode.Open, FileAccess.Read);
                    StreamReader streamR = new StreamReader(fStream, Encoding.UTF8);

                    line = streamR.ReadLine();
                    
                    string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string name = split[0];
                    if (split.Length != 2) { type = "molk"; }
                    else { type = split[1]; }
                    string[] splitTwo = type.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    name = splitTwo[0];
                    type = splitTwo[1];

                    packList.Add(new Pack(name, type));
                    GridPack.Items.Refresh();
                    
                }
                else
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
                }
            }catch(Exception e) { MessageBox.Show("WRONG - Give this message to the Developers ===>\n"+e,"Molk found error",MessageBoxButton.OK,MessageBoxImage.Error); }
        }

        /// <summary>
        /// Handles screen switch when loading.
        /// Starts a timer.
        /// </summary>
        public void Loading_Screen()
        {
            if (Pack.Visibility == Visibility.Visible)
            {
                Pack.Visibility = Visibility.Hidden;
                Loading.Visibility = Visibility.Visible;
                loadingTimer.Tick += Done_Loading;
                loadingTimer.Interval = new TimeSpan(0, 0, 3);

                loadingTimer.Start();
                loadingTimer.IsEnabled = true;
                packList.Clear();
                GridPack.Items.Refresh();
            }
            else
            {
                loadingTimer.Start();
                Unpack.Visibility = Visibility.Hidden;
                Loading.Visibility = Visibility.Visible;
                loadingTimer.Tick += Done_Loading;
                loadingTimer.Interval = new TimeSpan(0, 0, 3);

                loadingTimer.IsEnabled = true;
                packList.Clear();
                GridUnpack.Items.Refresh();
            }
        }

        /// <summary>
        /// Handles events to occure when loading is done.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="e"></param>
        public void Done_Loading(object timer, EventArgs e)
        {
            if (whyYouLoop == 0)
            {
                Main.Visibility = Visibility.Visible;
                Loading.Visibility = Visibility.Hidden;
                MessageBox.Show($"File(s) succesfully molked!\n File saved at: {dia.saveTo} ", "Molk zipping tool", MessageBoxButton.OK, MessageBoxImage.Information);
                dia.saveTo = "";
                dia.opened = "";
                loadingTimer.IsEnabled = false;
                loadingTimer.Stop();
                whyYouLoop++;
            }
            else return;
        }

        private void Cmd_UnPack()
        {
            try
            {
                //string _directory = "-r";
                //string cmdFile = $@"molk {_directory} {saveTo} {opened}";
               // string cmdFolder = $@"molk {_directory} {saveTo} {opened}\.*";

                Process processCmd = new Process();

                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.Start();

                processCmd.StandardInput.WriteLine($@"unmolk {dia.opened} -d {dia.saveTo}");//-d {saveTo}");
             
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
    }
}
