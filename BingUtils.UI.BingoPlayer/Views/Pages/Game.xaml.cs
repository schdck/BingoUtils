using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using System.Windows.Controls;

namespace BingoUtils.UI.BingoPlayer.Views.Pages
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public Game(GameViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
