using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public event EventHandler TitleTextBox_GotFocus;
        public event EventHandler AnswerTextBox_GotFocus;

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

        private void TextBoxTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            TitleTextBox_GotFocus?.Invoke(sender, e);
        }

        private void TextBoxAnswer_GotFocus(object sender, RoutedEventArgs e)
        {
            AnswerTextBox_GotFocus?.Invoke(sender, e);
        }

        public void FocusTextBoxTitle()
        {
            Keyboard.Focus(TextBoxTitle);
        }

        public void FocusTextBoxAnswer()
        {
            Keyboard.Focus(TextBoxAnswer);
        }
    }
}
