using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.UI.Shared.UserControls;
using BingoUtils.UI.Shared.UserControls.ViewModel;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BingoUtils.UI.BingoPlayer.Views.Windows
{
    /// <summary>
    /// Lógica interna para PdfGeneratorWindow.xaml
    /// </summary>
    public partial class PdfGeneratorWindow : Window
    {
        private List<Question> _GameQuestions;
        private Card[] _Cards;
        private string _DocumentPath;

        public int AmountOfColumns { get; private set; }

        public PdfGeneratorWindow(List<Question> gameQuestions, Card[] cartelas, string documentPath)
        {
            DataContext = this;

            _GameQuestions = gameQuestions;
            _Cards = cartelas;
            _DocumentPath = documentPath;

            CalculateAmountOfColumns();

            InitializeComponent();

            Loaded += PdfGeneratorWindow_Loaded;
        }

        private void CalculateAmountOfColumns()
        {
            if(_Cards[0] == null)
            {
                return;
            }

            int count = _Cards[0].QuestionsCount; 

            if(count % 3 == 0)
            {
                AmountOfColumns = 3;
            }
            else if (count % 5 == 0)
            {
                AmountOfColumns = 5;
            }
            else
            {
                AmountOfColumns = 4;
            }
        }

        private void PdfGeneratorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PdfDocument document = new PdfDocument();

            foreach(Card c in _Cards)
            {
                Dispatcher.Invoke(() =>
                {
                    uGrid.Children.Clear();

                    foreach (int id in c.GetIds())
                    {
                        Question q = _GameQuestions[id - 1];

                        FrameworkElement displayedElement;

                        if (string.IsNullOrEmpty(q.Answer))
                        {
                            displayedElement = new Viewbox()
                            {
                                Child = new Image()
                                {
                                    Source = BitmapImageHelper.BitmapFromUri(new Uri(q.AnswerImagePath))
                                }
                            };
                        }
                        else if (string.IsNullOrEmpty(q.AnswerImagePath))
                        {
                            displayedElement = new Viewbox()
                            {
                                Child = new TextBlock()
                                {
                                    Text = q.Answer,
                                },
                                StretchDirection = StretchDirection.DownOnly
                            };
                        }
                        else
                        {
                            var viewboxImage = new Viewbox()
                            {
                                Child = new Image()
                                {
                                    Source = BitmapImageHelper.BitmapFromUri(new Uri(q.AnswerImagePath))
                                }
                            };

                            var viewboxText = new Viewbox()
                            {
                                Child = new TextBlock()
                                {
                                    Text = q.Answer,
                                },
                                StretchDirection = StretchDirection.DownOnly
                            };

                            var grid = new Grid()
                            {
                                Background = Brushes.White
                            };

                            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5) });
                            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                            Grid.SetRow(viewboxText, 0);
                            Grid.SetRow(viewboxImage, 2);

                            grid.Children.Add(viewboxText);
                            grid.Children.Add(viewboxImage);

                            displayedElement = grid;
                        }

                        var child = new Border()
                        {
                            Padding = new Thickness(10),
                            Background = Brushes.White,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1),
                            Child = displayedElement
                        };

                        uGrid.Children.Add(child);
                    }
                });

                Dispatcher.Invoke(new Action(() =>
                {
                    PdfHelper.DrawPictureOfControlToPdf(document, this);
                }), DispatcherPriority.ContextIdle, null);

            }

            document.Save(_DocumentPath);
            
            Close();
        }
    }
}
