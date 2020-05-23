﻿using System;
using System.Diagnostics;
using System.Windows;

namespace MolkZipping
{
    public class PackMethods
    {

        MainWindow main;
        Dialog dia;

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

                string cmd = $@"molk {_directory} {dia.saveTo} {dia.opened}";

                processCmd.StandardInput.WriteLine(cmd);
                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                main.Loading_Screen();
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

                processCmd.StandardInput.WriteLine($@"unmolk {dia.opened} -d {dia.saveTo}");

                processCmd.StandardInput.Flush();
                processCmd.StandardInput.Close();
                processCmd.WaitForExit();

                main.Loading_Screen();
            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
      
    }
}
