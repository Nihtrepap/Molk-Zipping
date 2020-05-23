using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace MolkZipping
{
    public class PackMethods
    {

        MainWindow main;
        Dialog dia;

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

                string cmd = $@"molk {_directory} {dia.saveTo} {dia.opened} > {tmp} 2> {tmpErr}";

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
                processCmd.StandardInput.WriteLine($"unmolk \"{dia.opened}\" -d {dia.saveTo} > {tmp} 2> {tmpErr}");
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
            else main.Loading_Screen();
        }           
    }
}
