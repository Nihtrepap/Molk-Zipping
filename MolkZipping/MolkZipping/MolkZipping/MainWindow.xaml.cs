using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            if (sender is Button btn)
            {
                if (btn.Name == "BtnPack") { Main.Visibility = Visibility.Hidden; Pack.Visibility = Visibility.Visible; }
                else if (btn.Name == "BtnUnpack") { Main.Visibility = Visibility.Hidden; Unpack.Visibility = Visibility.Visible; }
            }
            else if (sender is Image btnImage)
            {
                if (btnImage.Name == "BtnBackPack") { Main.Visibility = Visibility.Visible; Pack.Visibility = Visibility.Hidden; }
                else if(btnImage.Name == "BtnBackUnpack") { Main.Visibility = Visibility.Visible; Unpack.Visibility = Visibility.Hidden; }
                else if(btnImage.Name == "question") { System.Diagnostics.Process.Start("https://testpalatset.wordpress.com/"); }
            }
        }  
    }
}
