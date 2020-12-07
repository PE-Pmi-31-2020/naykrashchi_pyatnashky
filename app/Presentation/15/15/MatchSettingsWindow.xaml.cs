// <copyright file="MatchSettingsWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MatchSettingsWindow.xaml.
    /// </summary>
    public partial class MatchSettingsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSettingsWindow"/> class.
        /// </summary>
        public MatchSettingsWindow()
        {
            this.InitializeComponent();
            this.OKButton.Click += this.OnClickOKButton;
        }

        private void OnClickOKButton(object sender, RoutedEventArgs e)
        {
            int size = this.radio4.IsChecked.Value ? 4 : this.radio5.IsChecked.Value ? 5 : 6;
            GameWindow window = new GameWindow(size);
            window.Show();
            this.Close();
        }
    }
}
