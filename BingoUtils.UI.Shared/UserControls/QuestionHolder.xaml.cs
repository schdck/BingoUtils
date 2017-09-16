using Microsoft.Win32;
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
        public event EventHandler TitleTextBox_GotFocus;
        public event EventHandler AnswerTextBox_GotFocus;

        public string Title { get; set; }
        public string Answer { get; set; }
        public string TitleImagePath { get; set; }
        public string AnswerImagePath { get; set; }

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

        private void ButtonAddAnswerImage_Click(object sender, RoutedEventArgs e)
        {
            AnswerImagePath = GetImageLocation();
        }

        private void ButtonAddTitleImage_Click(object sender, RoutedEventArgs e)
        {
            TitleImagePath = GetImageLocation();
        }

        private string GetImageLocation()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                Multiselect = false,
                CheckFileExists = true
            };
           
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }

            return null;
        }

    }
}
