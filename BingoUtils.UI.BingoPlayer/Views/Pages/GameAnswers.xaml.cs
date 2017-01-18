using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using System.Windows.Controls;

namespace BingoUtils.UI.BingoPlayer.Views.Pages
{
    /// <summary>
    /// Interaction logic for GameAnswers.xaml
    /// </summary>
    public partial class GameAnswers : Page
    {
        public GameAnswers(GameViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }

        // Não somos tão pragmáticos quanto ao MVVM, somos? ;)
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dGrid = sender as DataGrid;

            dGrid?.ScrollIntoView(dGrid.SelectedItem ?? dGrid.Items[0]);
        }
    }
}