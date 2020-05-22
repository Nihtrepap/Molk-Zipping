using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public Nullable<bool> saveFile;
        MainWindow main;
        PackMethods pack = new PackMethods();

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
            saveFile = openSaveFile.ShowDialog();
            if (saveFile != true) return;
            
            saveTo = openSaveFile.FileName + ".molk";
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
                if (main.Unpack.Visibility == Visibility.Visible) { string cmd = $"unmolk -l \"{fileOpen}\" > {tmp}"; processCmd.StandardInput.WriteLine(cmd); }
                else { string cmd = $"dir /B \"{fileOpen}\" > {tmp}"; processCmd.StandardInput.WriteLine(cmd); }
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
                if (main.Unpack.Visibility == Visibility.Visible)
                {
                    string line = "";
                    main.TxtInsideMolk.Text = File.ReadAllText(tmp, Encoding.UTF8);

                    FileStream fStream = new FileStream(tmp, FileMode.Open, FileAccess.Read);
                    StreamReader streamR = new StreamReader(fStream, Encoding.UTF8);
                    line = streamR.ReadLine();
                    Split_Unpack(line);
                }
                else
                {
                    Get_Folder_Split(tmp);                  
                }
            }
            catch (Exception e) { MessageBox.Show("WRONG - Give this message to the Developers ===>\n" + e, "Molk found error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        /// <summary>
        /// Opens program if .molk file clicked from a folder.
        /// </summary>
        public void Open_Molk_File()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                main.Main.Visibility = Visibility.Hidden; main.Unpack.Visibility = Visibility.Visible;
                opened = args[0];
                Cmd_run(opened);
            }
        }

        private void Split_Unpack(string line)
        {
            string type = "";
            string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string name = split[0];
            if (split.Length != 2) { type = "molk"; }
            else { type = split[1]; }
            string[] splitTwo = type.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            name = splitTwo[0];
            type = splitTwo[1];

            main.packList.Add(new Pack(name, type));
            main.GridPack.Items.Refresh();
        }

        private void Get_Folder_Split(string tmp)
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

                main.packList.Add(new Pack(name, type));
                main.GridPack.Items.Refresh();
            }
            fStream.Close();
        }
    }
}

