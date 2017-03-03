using System.Windows;
using System.Windows.Controls;

namespace BingoUtils.UI.Shared.UserControls
{
    /// <summary>
    /// Interaction logic for QuestionHolder.xaml
    /// </summary>
    public partial class QuestionHolder : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(QuestionHolder), new UIPropertyMetadata());

        public static readonly DependencyProperty AnswerProperty =
            DependencyProperty.Register("Answer", typeof(string), typeof(QuestionHolder), new UIPropertyMetadata());

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        public string Answer
        {
            get
            {
                return (string) GetValue(AnswerProperty);
            }
            set
            {
                SetValue(AnswerProperty, value);
            }
        }

        public QuestionHolder()
        {
            InitializeComponent();
        }
    }
}
