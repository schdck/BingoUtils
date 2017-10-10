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
        private Cartela[] _Cartelas;
        private string _DocumentPath;

        public PdfGeneratorWindow(List<Question> gameQuestions, Cartela[] cartelas, string documentPath)
        {
            _GameQuestions = gameQuestions;
            _Cartelas = cartelas;
            _DocumentPath = documentPath;

            InitializeComponent();

            Loaded += PdfGeneratorWindow_Loaded;
        }

        private void PdfGeneratorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Documento PDF
            PdfDocument document = new PdfDocument();

            foreach(Cartela c in _Cartelas)
            {
                // Diz para o Thread da tela fazer...
                Dispatcher.Invoke(() =>
                {
                    // Limpa os elementos da tela
                    uGrid.Children.Clear();

                    // Desenhar n questões na tela
                    foreach (int id in c.GetIds())
                    {
                        // Pega a questão a ser desenhada
                        Question q = _GameQuestions[id - 1];

                        uGrid.Children.Add(new QuestionDisplayer()
                        {
                            // Adiciona ao UniformGrid
                            DataContext = new QuestionDisplayerViewModel(true)
                            {
                                QuestionImagePath = q.AnswerImagePath,
                                QuestionTitle = q.Answer
                            },
                            Margin = new Thickness(0.5),
                            Padding = new Thickness(2),
                            Background = Brushes.White
                        });
                    }
                });

                // Diz para o Thread da tela fazer [com baixa prioridade - para esperar a tela atualizar]...
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
