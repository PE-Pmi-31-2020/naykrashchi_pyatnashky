// <copyright file="MatchPickWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using DAL;

    /// <summary>
    /// Interaction logic for MatchPickWindow.xaml.
    /// </summary>
    public partial class MatchPickWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchPickWindow"/> class.
        /// </summary>
        public MatchPickWindow()
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
            this.Lines = new ObservableCollection<MatchHistoryLine>();

            var matches = DBManager.GetUserMatches((int)App.Current.Properties[AppPropertyKeys.UserID], false);
            foreach (var katka in matches)
            {
                this.Lines.Add(new MatchHistoryLine(katka.MatchId, katka.Duration, katka.Score, katka.Turns, katka.DateTime, katka.Size));
            }

            // here we will get data from database in future.
            this.MatchesList.ItemsSource = this.Lines;
        }

        /// <summary>
        /// gets or sets collection of lines from database to be displayed in window.
        /// </summary>
        public ObservableCollection<MatchHistoryLine> Lines { get; set; }

        private void StartGame(object sender, SelectionChangedEventArgs args)
        {
            MatchHistoryLine lbi = (sender as ListBox).SelectedItems[0] as MatchHistoryLine;
            Match match = DBManager.GetMatch(lbi.MatchId.Value);
            GameWindow window = new GameWindow(match);
            window.Show();
            this.Close();
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
