// <copyright file="MatchSettingsWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MatchSettingsWindow.xaml.
    /// </summary>
    public partial class MatchSettingsWindow : Window
    {
        bool customImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSettingsWindow"/> class.
        /// </summary>
        public MatchSettingsWindow()
        {
            this.InitializeComponent();
            this.OKButton.Click += this.OnClickOKButton;
            this.ChooseCustomImage.Click += this.OnClickChooseCustomImage;
            this.customImage = false;
        }

        private void OnClickOKButton(object sender, RoutedEventArgs e)
        {
            int size = this.radio4.IsChecked.Value ? 4 : this.radio5.IsChecked.Value ? 5 : 6;
            GameWindow window = new GameWindow(size, this.customImage);
            window.Show();
            this.Close();
        }

        private void OnClickChooseCustomImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|Images(*.jpg; *.png; *.jpeg)|*.jpg; *.png; *.jpeg";
            openFileDialog.FilterIndex = 2;
            if (openFileDialog.ShowDialog() == true)
            {
                App.Current.Properties[AppPropertyKeys.CustomImagePath] = openFileDialog.FileName;
                this.FilenameLabel.Content = openFileDialog.SafeFileName;
                this.customImage = true;
            }
        }
    }
}
