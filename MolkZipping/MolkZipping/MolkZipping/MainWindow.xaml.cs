using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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
        DispatcherTimer loadingTimer = new DispatcherTimer();
        Dialog dia;
        PackMethods packMethod;

        public List<Pack> packList = new List<Pack>();

        private bool menuClick = false;
        private int whyYouLoop = 0;
        public bool folderPick = true; 

        public MainWindow()
        {
            InitializeComponent();
            dia = new Dialog(this);
            packMethod = new PackMethods(dia, this);
            dia.Open_Molk_File();
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
                else if (btn.Name == "BtnChooseUnpackFiles") { dia.Open_File_Dialog(); GridUnpack.ItemsSource = packList; GridUnpack.Items.Refresh(); }
                else if (btn.Name == "BtnUnPackFiles") 
                { 
                    dia.Save_File_Dialog(); 
                    if (dia.saveFile == true && dia.opened != null) { packMethod.Cmd_UnPack(); whyYouLoop = 0; }
                    else { MessageBox.Show("Could not find any files to unpack. Please try again.", "Molk unpacking tool", MessageBoxButton.OK, MessageBoxImage.Information); }
                }
                else if(btn.Name == "BtnClearUnPackData") { TxtInsideMolk.Text = ""; packList.Clear(); GridUnpack.Items.Refresh();  }

                else if (btn.Name == "BtnBackPack") { this.Title = "Molk tool"; Main.Visibility = Visibility.Visible; Pack.Visibility = Visibility.Hidden; }
                else if (btn.Name == "BtnChoosePackFile") { GridPack.ItemsSource = packList; dia.Open_File_Dialog();  }   
                else if (btn.Name == "BtnClearPackData") { packList.Clear(); GridPack.Items.Refresh();  }
                else if(btn.Name == "BtnPackFiles")
                {
                    dia.Save_File_Dialog();
                    if (dia.saveFile == true && dia.opened != null) { packMethod.Cmd_Pack(); whyYouLoop = 0; }
                    else { MessageBox.Show("Could not find any files to pack. Please try again.", "Molk packing tool", MessageBoxButton.OK, MessageBoxImage.Information); }
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
    }
}
