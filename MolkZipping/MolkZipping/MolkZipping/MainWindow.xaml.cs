﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MolkZipping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dialog dia;
        PackMethods packMethod;
        ButtonClick btnClick;

        public List<FileInfo> PackList = new List<FileInfo>();

        public bool MenuClick = false;
        public bool FolderPick = true;

        public MainWindow()
        {
            InitializeComponent();
            dia = new Dialog(this);
            packMethod = new PackMethods(dia, this);
            btnClick = new ButtonClick(dia, this, packMethod);
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
                switch (btn.Name)
                {
                    case "BtnClearUnPackData": dia.Clear_Datatables(); break;
                    case "BtnPack": btnClick.Open_Page(btn); break;
                    case "BtnUnpack": btnClick.Open_Page(btn); break;
                    case "BtnMenu": { if (!MenuClick) { Advanced.Visibility = Visibility.Visible; MenuClick = true; } } break;
                    case "BtnBackUnPack": btnClick.Go_Back(btn); break;
                    case "BtnChooseUnpackFiles": dia.Open_File_Dialog(btn); break;
                    case "BtnUnPackFiles": btnClick.Unpack_Files(); break;
                    case "BtnBackPack": btnClick.Go_Back(btn); break;
                    case "BtnChoosePackFile": dia.Open_File_Dialog(btn); break;
                    case "BtnClearPackData": dia.Clear_Datatables(); break;
                    case "BtnPackFiles": btnClick.Pack_files(); break;
                    default:
                        MessageBox.Show("Strange things have happend here. Contact dev support.", "Strange things.",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                        dia.Clear_Datatables(); break;
                }
            }
            else if (sender is Image imgBtn)
            {
                switch (imgBtn.Name)
                {
                    case "question": System.Diagnostics.Process.Start("https://testpalatset.wordpress.com/"); break;
                    case "Exit": Application.Current.Shutdown(); break;
                    case "Instagram": System.Diagnostics.Process.Start("https://www.instagram.com/molkutbildning/"); break;
                    case "Facebook": System.Diagnostics.Process.Start("https://www.facebook.com/molkutbildning/"); break;
                    case "Web": System.Diagnostics.Process.Start("https://www.molk.se/"); break;
                    default:
                        MessageBox.Show("Strange things have happend here. Contact dev support.", "Strange things.",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                        dia.Clear_Datatables(); break;
                }
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
                FolderPick = true;
            }
            else if (SelectFile.IsChecked == true)
            {
                FolderPick = false;
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
            MenuClick = false;
        }

    }
}
