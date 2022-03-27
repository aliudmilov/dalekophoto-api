using System;
using System.Collections.Generic;
using System.IO;
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

namespace PhotoManager
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] albumFolders = Directory.GetDirectories(@"D:\dalekophoto");
            foreach (string folder in albumFolders)
            {
                string[] sizeFolders = Directory.GetDirectories(folder);
                foreach (string sizeFolder in sizeFolders)
                {
                    Directory.CreateDirectory(folder.Replace("dalekophoto", "dalekophoto-prod"));
                    Directory.Move(sizeFolder, sizeFolder.Replace("dalekophoto", "dalekophoto-prod"));
                }
            }
        }
    }
}
