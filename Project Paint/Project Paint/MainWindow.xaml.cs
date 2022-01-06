using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Project_Paint
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

        private void TwitterButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            
        }


        private void Color_Table_Selected(object sender, RoutedEventArgs e)
        {
            var dlg = new ColorDialog(); //Khởi tạo đối tượng ColorDialog 

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Nếu nhấp vào nút OK trên hộp thoại
            {
                color_react.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Brush_Button_Click(object sender, RoutedEventArgs e)
        {
            var colorBlack = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            brushButton.Foreground = colorBlack;

            
        }

        private void Shape_Selected(object sender, RoutedEventArgs e)
        {
            var colorGray = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 128, 128));
            if(brushButton!=null)
                brushButton.Foreground = colorGray;
        }

    }
}
