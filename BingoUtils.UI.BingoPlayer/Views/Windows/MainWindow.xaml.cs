using MahApps.Metro.Controls;

namespace BingoUtils.UI.BingoPlayer.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance { get; private set; }

        private AnswerWindow AnswerWindow;

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            AnswerWindow = new AnswerWindow();

            Closing += (s, e) => { try { AnswerWindow.Close(); } catch { } };

            AnswerWindow.Closing += (s, e) => { try { Close(); } catch { } };

            AnswerWindow.Show();
        }
    }
}