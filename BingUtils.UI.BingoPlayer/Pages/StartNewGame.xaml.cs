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

namespace BingoUtils.UI.BingoPlayer.Pages
{
    /// <summary>
    /// Interaction logic for StartNewGame.xaml
    /// </summary>
    public partial class StartNewGame : Page
    {
        public StartNewGame()
        {
            InitializeComponent();
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if(files.Length > 1)
                {
                    MessageBox.Show("Por favor selecione apenas um arquivo");
                }

                MessageBox.Show(files[0]);
            }
        }
    }
}
