using BingoUtils.UI.Shared.UserControls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class CreateGameViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionHolder> AddedQuestions { get; private set; }

        public RelayCommand<string> LastGotFocusCommand { get; private set; }
        public RelayCommand<string> ValidateTextCommand { get; private set; }
        public RelayCommand SaveGameCommand { get; private set; }

        private Brush _DisciplinaBorderBrush = Brushes.LightGray;
        private Brush _AssuntoBorderBrush = Brushes.LightGray;

        public Brush DisciplinaBorderBrush
        {
            get
            {
                return _DisciplinaBorderBrush;
            }
            set
            {
                Set(ref _DisciplinaBorderBrush, value);
            }
        }
        public Brush AssuntoBorderBrush
        {
            get
            {
                return _AssuntoBorderBrush;
            }
            set
            {
                Set(ref _AssuntoBorderBrush, value);
            }
        }

        private string _Disciplina;
        private string _Assunto;

        public string Disciplina
        {
            get
            {
                return _Disciplina;
            }
            set
            {
                Set(ref _Disciplina, value);
            }
        }
        public string Assunto
        {
            get
            {
                return _Assunto;
            }
            set
            {
                Set(ref _Assunto, value);
            }
        }

        public bool SaveOnDefaults { get; set; }

        public CreateGameViewModel()
        {
            InitializeCommands();

            AddedQuestions = new ObservableCollection<QuestionHolder>();

            AddedQuestions.Add(new QuestionHolder());
        }

        private void InitializeCommands()
        {
            LastGotFocusCommand = new RelayCommand<string>((x) =>
            {
                var last = AddedQuestions.LastOrDefault();

                if (last == null || (!string.IsNullOrEmpty(last?.Title) || !string.IsNullOrEmpty(last?.Answer)))
                {
                    var holder = new QuestionHolder();
                    AddedQuestions.Add(holder);

                    holder.Loaded += (s, e) => LastGotFocusCommand.Execute(x);
                }

                if (x == "Title")
                {
                    AddedQuestions.LastOrDefault()?.FocusTextBoxTitle();
                }
                else
                {
                    AddedQuestions.LastOrDefault()?.FocusTextBoxAnswer();
                }
            });

            ValidateTextCommand = new RelayCommand<string>((x) =>
            {
                if(x == "Disciplina")
                {
                    DisciplinaBorderBrush = (string.IsNullOrWhiteSpace(Disciplina) ? Brushes.Red : Brushes.LightGray);
                }
                else if(x == "Assunto")
                {
                    AssuntoBorderBrush = (string.IsNullOrWhiteSpace(Assunto) ? Brushes.Red : Brushes.LightGray);
                }
            });

            SaveGameCommand = new RelayCommand(SaveGame);
        }

        private void SaveGame()
        {
            if(string.IsNullOrWhiteSpace(Disciplina) || string.IsNullOrWhiteSpace(Assunto))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios antes de continuar", "ERRO:", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bingo", "Jogos", Disciplina);
            string file = Path.Combine(path, string.Format("{0}.csv", Assunto));
            string fileContent;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("{0};{1}", Disciplina, Assunto));

            foreach(QuestionHolder holder in AddedQuestions)
            {
                builder.AppendLine(string.Format("{0};{1}", holder.Title, holder.Answer));
            }

            fileContent = builder.ToString();

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            SaveFileDialog SaveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "csv",
                FileName = Assunto,
            };

            if(SaveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(SaveFileDialog.FileName, false, Encoding.GetEncoding("WINDOWS-1252")))
                {
                    writer.Write(fileContent);
                }
            }

            if(SaveOnDefaults)
            {
                if (File.Exists(file))
                {
                    var result = MessageBox.Show("Já existe um jogo com este nome nos seus jogos padrão, deseja sobrescrevê-lo?", "ATENÇÃO", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                using (StreamWriter writer = new StreamWriter(file, false, Encoding.GetEncoding("WINDOWS-1252")))
                {
                    writer.Write(fileContent);
                }
            }
        }
    }
}
