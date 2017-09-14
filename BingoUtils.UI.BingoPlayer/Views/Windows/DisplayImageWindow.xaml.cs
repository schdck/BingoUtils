using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BingoUtils.UI.BingoPlayer.Views.Windows
{
    /// <summary>
    /// Lógica interna para DisplayImageWindow.xaml
    /// </summary>
    public partial class DisplayImageWindow : MetroWindow
    {
        public DisplayImageWindow(string path)
        {
            InitializeComponent();
            ImageDisplayer.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }
    }
}
