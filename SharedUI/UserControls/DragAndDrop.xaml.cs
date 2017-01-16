﻿using Microsoft.Win32;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BingoUtils.UI.Shared.UserControls
{
    /// <summary>
    /// Interaction logic for DragAndDrop.xaml
    /// </summary>
    /// ]
    public partial class DragAndDrop : UserControl
    {
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(DragAndDrop), new UIPropertyMetadata());

        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(DragAndDrop), new UIPropertyMetadata());

        public string FilePath
        {
            get
            {
                return (string)GetValue(FilePathProperty);
            }
            set
            {
                SetValue(FilePathProperty, value);
            }
        }

        public string FileName
        {
            get
            {
                return (string)GetValue(FileNameProperty);
            }
            set
            {
                SetValue(FileNameProperty, value);
            }
        }

        public DragAndDrop()
        {
            InitializeComponent();

            SetFileUnselected();
        }

        #region Events
        private void Grid_Container_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 1)
                {
                    //DisplayMessage.DisplayError("Por favor selecione apenas um arquivo");
                    return;
                }

                SetFileSelected(files[0]);
            }
        }

        private void Grid_Container_AnyEnter(object sender, EventArgs e)
        {
            Grid_Container.Background = Brushes.LightGray;
        }

        private void Grid_Container_AnyLeave(object sender, EventArgs e)
        {
            Grid_Container.Background = Brushes.DarkGray;
        }

        private void Grid_Container_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Arquivos de jogos (.csv)|*.csv";

            bool? result = dialog.ShowDialog(); 

            if (result == true)
            {
                SetFileSelected(dialog.FileName);
            }
        }
        #endregion

        private void SetFileSelected(string file)
        {
            if(!ValidateFile(file))
            {
                //DisplayMessage.DisplayError("Este arquivo não é válido");
                return;
            }

            FilePath = file;
            FileName = Path.GetFileName(file);
            FileContainer.Content = (StackPanel) Resources["StackPanel_FileSelected"];
        }

        private void SetFileUnselected()
        {
            FileContainer.Content = (StackPanel) Resources["StackPanel_NoFileSelected"];
        }

        private bool ValidateFile(string file)
        {
            if(Path.GetExtension(file) != ".csv")
            {
                return false;
            }

            return true;
        }
    }
}