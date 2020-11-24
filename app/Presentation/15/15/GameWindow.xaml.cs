// <copyright file="GameWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
