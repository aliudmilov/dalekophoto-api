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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] albumFolders = Directory.GetDirectories(@"D:\dalekophoto");
            foreach (string folder in albumFolders)
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.Contains("FEATURED"))
                    {
                        string rootFolder = folder.Replace("dalekophoto", "dalekophoto-prod");
                        string fileName = Path.GetFileName(file);
                        string lookupFilename = fileName.Replace("_FEATURED", string.Empty);

                        string largeFile = Path.Combine(rootFolder, "size-2560", lookupFilename);
                        File.Move(largeFile, Path.Combine(rootFolder, "size-2560", fileName));

                        string medFile = Path.Combine(rootFolder, "size-1280", lookupFilename);
                        File.Move(medFile, Path.Combine(rootFolder, "size-1280", fileName));

                        string smallFile = Path.Combine(rootFolder, "size-640", lookupFilename);
                        File.Move(smallFile, Path.Combine(rootFolder, "size-640", fileName));
                    }
                }
            }
        }
    }
}
