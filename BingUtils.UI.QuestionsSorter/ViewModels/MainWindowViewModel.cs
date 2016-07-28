using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BingUtils.UI.QuestionsSorter.ViewModels
{
    public class MainWindowViewModel : DefaultViewModel
    {
        public SimpleDelegateCommand GenerateCommand { get; private set; }

        private bool _HasGeneratedGrid;
        
        public bool HasGeneratedGrid
        {
            get
            {
                return _HasGeneratedGrid;
            }
            private set
            {
                _HasGeneratedGrid = value;
                NotifyPropertyChanged();
            }
        }
        public double? CartelasCount { get; set; }
        public double? QuestionsCount { get; set; }
        public double? QuestionsPerCartelaCount { get; set; }
        public double? MaxSemelhanca { get; set; }

        public ObservableCollection<object> CartelasList { get; private set; }

        public MainWindowViewModel()
        {
            CartelasList = new ObservableCollection<object>();
            GenerateCommand = new SimpleDelegateCommand((x) => GenerateGrid());
        }

        public void GenerateGrid()
        {
            HasGeneratedGrid = true;
            for (int i = 0; i < CartelasCount; i++)
            {
                CartelasList.Add(new { Id = i });
            }
            
        }
    }
}