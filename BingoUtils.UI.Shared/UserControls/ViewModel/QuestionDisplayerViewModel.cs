using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BingoUtils.UI.Shared.UserControls.ViewModel
{
    public class QuestionDisplayerViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private bool _IsCurrentQuestion;

        public string QuestionTitle { get; set; }

        public string QuestionImagePath { get; set; }

        public FrameworkElement DisplayedElement { get; private set; }

        public QuestionDisplayerViewModel(bool isCurrentQuestion)
        {
            _IsCurrentQuestion = isCurrentQuestion;

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
                ImageSource source = null;

                if (_IsCurrentQuestion)
                {
                    source = new BitmapImage(new Uri(QuestionImagePath));
                }
                else
                {
                    source = new FormatConvertedBitmap(new BitmapImage(new Uri(QuestionImagePath)), PixelFormats.Gray32Float, null, 0);
                }

                DisplayedElement = new Viewbox()
                {
                    Child = new Image()
                    {
                        Source = source
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
                ImageSource source = null;

                if (_IsCurrentQuestion)
                {
                    source = new BitmapImage(new Uri(QuestionImagePath));
                }
                else
                {
                    source = new FormatConvertedBitmap(new BitmapImage(new Uri(QuestionImagePath)), PixelFormats.Gray32Float, null, 0);
                }

                var viewboxImage = new Viewbox()
                {
                    Child = new Image()
                    {
                        Source = source
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
