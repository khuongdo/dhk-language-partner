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

namespace DHK_Easy_Flash_Card
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void btn_BrwExcelFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dl = new Microsoft.Win32.OpenFileDialog();
            dl.Multiselect = false;
            dl.Filter = "Excel|*.xlsx";
            if (dl.ShowDialog() == true)
            {
                ((MainViewModel)this.DataContext).ExcelPath = dl.FileName;
            }
        }

        private void btn_BrwOutput_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dl = new System.Windows.Forms.FolderBrowserDialog();
            if (dl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((MainViewModel)this.DataContext).OutputPath = dl.SelectedPath;
            }

        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_SavetoPdf_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)this.DataContext).CreatePDF();
            MessageBox.Show("Done");
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)this.DataContext).ExcelPath = "";
            ((MainViewModel)this.DataContext).Front = ((MainViewModel)this.DataContext).Back = "";
        }
    }
}
