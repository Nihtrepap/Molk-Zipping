using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MolkZipping
{
    class ButtonClick
    {
        Dialog dia;
        MainWindow main;
        PackMethods packMethod;

        public ButtonClick(Dialog dia, MainWindow main, PackMethods packMethod)
        {
            this.dia = dia;
            this.main = main;
            this.packMethod = packMethod;
        }

        public void Pack_files()
        {
            dia.Save_File_Dialog();
            if (dia.saveFile == true && dia.opened != null) { packMethod.Cmd_Pack(); main.whyYouLoop = 0; }
            else { MessageBox.Show("Could not find any files to pack. Please try again.", "Molk packing tool", MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        public void Unpack_Files()
        {
            dia.Save_File_Dialog();
            if (dia.saveFile == true && dia.opened != null) { packMethod.Cmd_UnPack(); main.whyYouLoop = 0; }
            else { MessageBox.Show("Could not find any files to unpack. Please try again.", "Molk unpacking tool", MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        public void Open_Page(Button btn)
        {
            if (btn.Name == "BtnPack") { main.Title = "Molk pack"; main.Main.Visibility = Visibility.Hidden; main.Pack.Visibility = Visibility.Visible; }
            else { main.Title = "Molk unpack"; main.Main.Visibility = Visibility.Hidden; main.Unpack.Visibility = Visibility.Visible; }
        }

        public void Get_Back(Button btn)
        {
            if (btn.Name == "BtnBackUnPack") { main.Title = "Molk tool"; main.Main.Visibility = Visibility.Visible; main.Unpack.Visibility = Visibility.Hidden; }
            else { main.Title = "Molk tool"; main.Main.Visibility = Visibility.Visible; main.Pack.Visibility = Visibility.Hidden; }
            dia.Clear_Datatables();
        }
    }
}
