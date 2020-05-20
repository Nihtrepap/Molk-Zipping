using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MolkZipping
{
    public class Dialog
    {
        public string saveTo;
        public string opened;
        MainWindow main;

        public Dialog(MainWindow main)
        {
            this.main = main;
        }

        /// <summary>
        /// This method saves the file path into a string
        /// and adds .molk extension. 
        /// Also only make it possible for the user to save into molk extension.
        /// </summary>
        public void Save_File_Dialog()
        {
            SaveFileDialog openSaveFile = new SaveFileDialog();
            openSaveFile.Filter = "molk files (*.molk)|*.molk*";
            openSaveFile.FilterIndex = 1;
            openSaveFile.FileName = "Molkzipping";
            openSaveFile.ShowDialog();
            
            saveTo = openSaveFile.FileName + ".molk";
            main.SaveToText.Text = $"{saveTo}";
        }

        /// <summary>
        /// Used to open a file dialog.
        /// Saves the chosen file path.
        /// </summary>
        /// <seealso cref="Cmd_run(string)"/>
        public void Open_File_Dialog()
        {           
            CommonOpenFileDialog openFileWindow = new CommonOpenFileDialog();
            openFileWindow.EnsureFileExists = true;
            
            openFileWindow.Title = "Choose file(s) or folder(s)";
            if (main.Pack.Visibility == Visibility.Visible) { openFileWindow.IsFolderPicker = main.folderPick; }
            else { openFileWindow.IsFolderPicker = false; openFileWindow.Filters.Add(new CommonFileDialogFilter("molk Files","*.molk")); }
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
        public void Cmd_run(string fileOpen)
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

                main.File_Reader(tmp);

                main.Get_Fileinfo(tmp);

            }
            catch (Exception e)
            {
                MessageBox.Show("WRONG - Give this message to the developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

