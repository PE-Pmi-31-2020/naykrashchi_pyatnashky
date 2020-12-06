// <copyright file="MainWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="login"> player's login. </param>
        /// <param name="password"> player's password. </param>
        public MainWindow()
        {
            this.InitializeComponent();
            this.NicknameLabel.Content = App.Current.Properties[AppPropertyKeys.Login];
            this.ManageImagesButton.Click += this.FakeManageImages;
            this.PlayButton.Click += this.FakePlay;
            this.ExitButton.Click += this.OnClickExit;
            this.MatchHistoryButton.Click += this.OnClickMatchHistory;
        }

        /// <summary>
        /// Gets login.
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string Password { get; }

        private void FakePlay(object sender, RoutedEventArgs e)
        {
            GameWindow window = new GameWindow();
            window.Show();
            this.Close();
        }

        private void FakeManageImages(object sender, RoutedEventArgs e)
        {
            ManageImagesWindow window = new ManageImagesWindow();
            window.Show();
            this.Close();
        }

        private void OnClickMatchHistory(object sender, RoutedEventArgs e)
        {
            MatchHistoryWindow window = new MatchHistoryWindow();
            window.Show();
            this.Close();
        }

        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
