using BingoUtils.Domain.Entities;
using BingUtils.UI.QuestionsSorter.UI_Helpers;
using BingUtils.UI.QuestionsSorter.ValueConverters;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace BingUtils.UI.QuestionsSorter.ViewModels
{
    public class MainWindowViewModel : DefaultViewModel
    {
        public SimpleDelegateCommand GenerateCommand { get; private set; }

        private Random RandomGenerator;
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
            RandomGenerator = new Random();
        }

        public async void GenerateGrid()
        {
            #region Verificações
            if(CartelasCount == null || QuestionsCount == null || QuestionsPerCartelaCount == null)
            {
                await MainWindow.Instance.ShowMessageAsync("Dados inválidos", "A quantidade de cartelas, de questões ou de questões por cartela não está correta.\n\nVerifique seus valores e tente novamente.");
                return;
            }
            if(CartelasCount <= 1)
            {
                await MainWindow.Instance.ShowMessageAsync("Sem necessidade de cálculo", "Não há necessidade de distribuir as questões em uma cartela.\n\nAumente a quantidade de cartelas e tente novamente.");
                return;
            }
            else if(QuestionsPerCartelaCount >= QuestionsCount)
            {
                await MainWindow.Instance.ShowMessageAsync("Impossível calcular", "Não há possibilidade de distribuir tantas questões por cartela com esta quantidade de questões.\n\nDiminua a quantidade de questões por cartela ou aumente a quantidade de questões e tente novamente.");
                return;
            }

            if(MaxSemelhanca == null)
            {
                MaxSemelhanca = 60;
            }
            #endregion

            PercentageToBrushConverter.MaxSemelhanca = MaxSemelhanca.Value;

            Cartela[] cartelas;
            
            var dialog = await MainWindow.Instance.ShowProgressAsync("Gerando cartelas", "Estamos gerando suas cartelas. Isto pode levar algum tempo..");

            dialog.SetIndeterminate();
            CartelasList.Clear();
            HasGeneratedGrid = false;

            int triesAll = 0, triesSpecific = 0; ;
            bool keepTrying = true;
            bool success;

            do
            {
                triesSpecific = 0;

                do
                {
                    success = GenerateCartelas(out cartelas);
                    triesSpecific++;
                } while (!success && triesSpecific < 5);

                if(!success)
                {
                    triesAll += triesSpecific;

                    var dialogSettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Cancelar",
                        NegativeButtonText = "Continuar tentando",
                    };

                    await MainWindow.Instance.Dispatcher.Invoke(async () =>
                    {
                        await dialog.CloseAsync();

                        if (await MainWindow.Instance.ShowMessageAsync("Problema ao gerar cartelas", "Não conseguimos distribuir as cartelas para você.\nTalvez seja impossível gerar tabela usando os valores que você forneceu.\n\nSe você acredita que é possível gerar as cartelas usando os valores que você inseriu, clique em \"Continuar tentando\", para alterar os valores clique em \"Cancelar\"\n\nQuantidade de tentativas: " + triesAll, MessageDialogStyle.AffirmativeAndNegative, dialogSettings) == MessageDialogResult.Affirmative)
                        {
                            keepTrying = false;
                        }
                        else
                        {
                            dialog = await MainWindow.Instance.ShowProgressAsync("Gerando cartelas", "Estamos gerando suas cartelas. Isto pode levar algum tempo..");
                        }
                    });
                }
                else
                {
                    HasGeneratedGrid = true;

                    for (int i = 0; i < CartelasCount; i++)
                    {
                        double maiorSemelhanca = 0;

                        for(int j = 0; j < CartelasCount; j++)
                        {
                            if(i != j)
                            {
                                double semelhanca = cartelas[i].GetSemelhanca(cartelas[j]);

                                if (semelhanca > maiorSemelhanca)
                                {
                                    maiorSemelhanca = semelhanca;
                                }
                            }
                        }

                        CartelasList.Add(new { Id = cartelas[i].Id, QuantidadeDeQuestoes = cartelas[i].QuestionsCount, MaiorSemelhanca = maiorSemelhanca, Ids = cartelas[i].GetIds(", ") });
                    }
                }

            } while (keepTrying && !success);

            if(dialog.IsOpen)
            {
                await dialog.CloseAsync();
            }
        }

        public Cartela GenerateRandomCartela(int id)
        {
            Cartela c = new Cartela(id, (int) QuestionsPerCartelaCount.Value);

            while(!c.IsFull)
            {
                c.AddQuestion(RandomGenerator.Next((int)QuestionsCount.Value) + 1);
            }

            return c;
        }

        public bool GenerateCartelas(out Cartela[] cartelas)
        {
            double maiorSemelhanca;
            cartelas = new Cartela[(int)CartelasCount.Value];

            for(int i = 0; i < (int)CartelasCount.Value; i++)
            {
                int tries = 0;
                do
                {
                    maiorSemelhanca = 0;
                    cartelas[i] = GenerateRandomCartela(i + 1);

                    for (int j = i - 1; j >= 0; j--)
                    {
                        double semelhanca = cartelas[i].GetSemelhanca(cartelas[j]);
                        maiorSemelhanca = (semelhanca > maiorSemelhanca ? semelhanca : maiorSemelhanca);
                    }

                    if(++tries >= 5)
                    {
                        cartelas = null;
                        return false;
                    }
                }
                while (maiorSemelhanca > MaxSemelhanca.Value);
            }

            return true;
        }
    }
}