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
        private const string FeaturedIdentifier = "_FEATURED";
        private const string UatFolder = "dalekophoto";
        private const string UatRootPath = @$"D:\{UatFolder}";
        private const string ProdRootFolder = "dalekophoto-prod";
        private const string ProdRootPath = @$"D:\{ProdRootFolder}";
        private const string Size2560Folder = "size-2560";
        private const string Size1280Folder = "size-1280";
        private const string Size640Folder = "size-640";
        private static readonly string[] AllSizeFolders = new string[]
        {
            Size2560Folder,
            Size1280Folder,
            Size640Folder,
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] albumFolders = Directory.GetDirectories(UatRootPath);
            foreach (string folder in albumFolders)
            {
                string[] sizeFolders = Directory.GetDirectories(folder);
                foreach (string sizeFolder in sizeFolders)
                {
                    Directory.CreateDirectory(folder.Replace(UatFolder, ProdRootFolder));
                    Directory.Move(sizeFolder, sizeFolder.Replace(UatFolder, ProdRootFolder));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] prodAlbumPaths = Directory.GetDirectories(ProdRootPath);
            foreach (string prodAlbumPath in prodAlbumPaths)
            {
                foreach (string sizeFolder in AllSizeFolders)
                {
                    string prodSizeAlbumPath = Path.Combine(prodAlbumPath, sizeFolder);
                    foreach (string prodFile in Directory.GetFiles(prodSizeAlbumPath))
                    {
                        if (prodFile.Contains(FeaturedIdentifier))
                        {
                            string noFeaturePath = prodFile.Replace(FeaturedIdentifier,string.Empty);
                            File.Move(prodFile, noFeaturePath);
                        }
                    }
                }
            }

            string[] uatAlbumPaths = Directory.GetDirectories(UatRootPath);
            foreach (string uatAlbumPath in uatAlbumPaths)
            {
                foreach (string uatFile in Directory.GetFiles(uatAlbumPath))
                {
                    if (uatFile.Contains(FeaturedIdentifier))
                    {
                        string rootFolder = uatAlbumPath.Replace(UatFolder, ProdRootFolder);
                        string fileName = Path.GetFileName(uatFile);
                        string lookupFilename = fileName.Replace(FeaturedIdentifier, string.Empty);

                        foreach (string sizeFolder in AllSizeFolders)
                        {
                            string largeFile = Path.Combine(rootFolder, sizeFolder, lookupFilename);
                            File.Move(largeFile, Path.Combine(rootFolder, sizeFolder, fileName));
                        }
                    }
                }
            }
        }
    }
}
