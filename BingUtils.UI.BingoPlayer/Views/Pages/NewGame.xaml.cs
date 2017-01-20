using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using GalaSoft.MvvmLight.Ioc;
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

namespace BingoUtils.UI.BingoPlayer.Views.Pages
{
    /// <summary>
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Page
    {
        public NewGame()
        {
            InitializeComponent();

            string key = Guid.NewGuid().ToString();
            DataContext = SimpleIoc.Default.GetInstance<NewGameViewModel>(key);
        }
    }
}
