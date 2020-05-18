using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MolkZipping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Pack> packList = new List<Pack>();
        private bool menuClick = false;

        public MainWindow()
        {
            InitializeComponent();
            packList.Add(new Pack("Testing", "txt", 124.3));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button btn)
            {
                if (btn.Name == "BtnPack") { Main.Visibility = Visibility.Hidden; Pack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnUnpack") { Main.Visibility = Visibility.Hidden; Unpack.Visibility = Visibility.Visible; }
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

        private void Mouse_Leave_Menu(object sender, MouseEventArgs e)
        {
            Advanced.Visibility = Visibility.Hidden;
            menuClick = false;
        }

        private void Open_File_Dialog()
        {
            string opened;

            OpenFileDialog openFileWindow = new OpenFileDialog();

            openFileWindow.ShowDialog();


            opened = openFileWindow.FileName.ToString();

            Cmd_run(opened);



        }

        private void Cmd_run(string fileOpen)
        {
            string command = fileOpen;

            Process processCmd = new Process();

            processCmd.StartInfo.FileName = "cmd.exe";
            processCmd.StartInfo.RedirectStandardInput = true;
            processCmd.StartInfo.RedirectStandardOutput = true;
            processCmd.StartInfo.RedirectStandardError = true;
            processCmd.StartInfo.CreateNoWindow = true;
            processCmd.StartInfo.UseShellExecute = false;
            processCmd.Start();
            processCmd.StandardInput.WriteLine($"{command} > tmp.txt");

            processCmd.StandardInput.Flush();
            processCmd.StandardInput.Close();
            processCmd.WaitForExit();

        }
    }
}
