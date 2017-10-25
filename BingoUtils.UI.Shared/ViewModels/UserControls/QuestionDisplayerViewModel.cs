using BingoUtils.Helpers;
using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BingoUtils.UI.Shared.ViewModels.UserControls
{
    public class QuestionDisplayerViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public string QuestionTitle { get; set; }
 
        public string QuestionImagePath { get; set; }

        public FrameworkElement DisplayedElement { get; private set; }

        public QuestionDisplayerViewModel()
        {
            DisplayedElement = new Grid();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(QuestionTitle) || e.PropertyName == nameof(QuestionImagePath))
                {
                    UpdateDisplayedGrid();
                }
            };
        }

        public void UpdateDisplayedGrid()
        {
            if(string.IsNullOrEmpty(QuestionTitle) && string.IsNullOrEmpty(QuestionImagePath))
            {
                return;
            }
            else if (string.IsNullOrEmpty(QuestionTitle))
            {
                DisplayedElement = new Viewbox()
                {
                    Child = new Image()
                    {
                        Source = BitmapImageHelper.BitmapFromUri(new Uri(QuestionImagePath)),
                        Margin = new Thickness(50)
                    }
                };
            }
            else if (string.IsNullOrEmpty(QuestionImagePath))
            {
                DisplayedElement = new Viewbox()
                {
                    Child = new TextBlock()
                    {
                        Text = QuestionTitle,
                    },
                };
            }
            else
            {
                var viewboxImage = new Viewbox()
                {
                    Child = new Image()
                    {
                        Source = BitmapImageHelper.BitmapFromUri(new Uri(QuestionImagePath))
                    }
                };

                var viewboxText = new Viewbox()
                {
                    Child = new TextBlock()
                    {
                        Text = QuestionTitle,
                    }
                };

                var grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                Grid.SetRow(viewboxText, 0);
                Grid.SetRow(viewboxImage, 2);

                grid.Children.Add(viewboxText);
                grid.Children.Add(viewboxImage);

                DisplayedElement = grid;
            }
        }
    }
}
