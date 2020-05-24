using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MolkZipping
{
    public class PackMethods
    {
        DispatcherTimer loadingTimer = new DispatcherTimer();

        MainWindow main;
        Dialog dia;

        public int WhyYouLoop = 0;

        string tmp = @"tmp.txt";
        string tmpErr = @"tmpErr.txt";
        
        public PackMethods() { }

        public PackMethods(Dialog dia, MainWindow main)
        {
            this.dia = dia;
            this.main = main;
        }

        public void Cmd_Pack()
        {
            try
            {
                string _directory = "-r";

                Process processCmd = new Process();

                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.Start();

                string cmd = $@"molk {_directory} {dia.SaveTo} {dia.FileOpen} > {tmp} 2> {tmpErr}";

                processCmd.StandardInput.WriteLine(cmd);
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                Check_File_Exist();
            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Cmd_UnPack()
        {
            try
            {

                Process processCmd = new Process();
                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.Start();
                processCmd.StandardInput.WriteLine($"unmolk \"{dia.FileOpen}\" -d {dia.SaveTo} > {tmp} 2> {tmpErr}");
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                Check_File_Exist();
              
            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Check_File_Exist()
        {
            string[] check = {"cannot find" , "not matched"};
            string output;
            output = File.ReadAllText(tmpErr, Encoding.UTF8);
            if (check.Any(output.Contains)) { MessageBox.Show("The file cannot be found. Please try and choose file again", "Molk cannot find file", MessageBoxButton.OK, MessageBoxImage.Information); dia.Clear_Datatables(); }
            else Loading_Screen();
        }

        /// <summary>
        /// Handles screen switch when loading.
        /// Starts a timer.
        /// </summary>
        public void Loading_Screen()
        {
            if (main.Pack.Visibility == Visibility.Visible)
            {
                main.Pack.Visibility = Visibility.Hidden;
                main.Loading.Visibility = Visibility.Visible;
                loadingTimer.Tick += Done_Loading;
                loadingTimer.Interval = new TimeSpan(0, 0, 3);

                loadingTimer.Start();
                loadingTimer.IsEnabled = true;
                main.PackList.Clear();
                main.GridPack.Items.Refresh();
            }
            else
            {
                main.Unpack.Visibility = Visibility.Hidden;
                main.Loading.Visibility = Visibility.Visible;
                loadingTimer.Tick += Done_Loading;
                loadingTimer.Interval = new TimeSpan(0, 0, 3);

                loadingTimer.Start();
                loadingTimer.IsEnabled = true;
                main.TxtInsideMolk.Text = "";
                main.PackList.Clear();
                main.GridUnpack.Items.Refresh();
            }
        }

        /// <summary>
        /// Handles events to occure when loading is done.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="e"></param>
        public void Done_Loading(object timer, EventArgs e)
        {
            if (WhyYouLoop == 0)
            {
                main.Main.Visibility = Visibility.Visible;
                main.Loading.Visibility = Visibility.Hidden;
                MessageBox.Show($"File(s) succesfully molked!\n File saved at: {dia.SaveTo} ", "Molk zipping tool", MessageBoxButton.OK, MessageBoxImage.Information);
                dia.SaveTo = "";
                dia.FileOpen = "";
                loadingTimer.IsEnabled = false;
                loadingTimer.Stop();
                WhyYouLoop++;
            }
            else return;
        }
  
    }
}
