﻿// <copyright file="MatchHistoryWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
    using BLL;

    /// <summary>
    /// Interaction logic for MatchHistoryWindow.xaml.
    /// </summary>
    public partial class MatchHistoryWindow : UserControl
    {
        private GridViewColumnHeader lastHeaderClicked = null;
        private ListSortDirection lastDirection = ListSortDirection.Ascending;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchHistoryWindow"/> class.
        /// </summary>
        public MatchHistoryWindow()
        {
            this.InitializeComponent();
            this.Loaded += new RoutedEventHandler(this.SetTitle);
            this.BackButton.Click += this.OnClickBack;

            this.Lines = new ObservableCollection<MatchHistoryLine>();
            try
            {
                var matches = DBManager.GetUserMatches((int)App.Current.Properties[AppPropertyKeys.UserID], true);
                foreach (var katka in matches)
                {
                    this.Lines.Add(new MatchHistoryLine(katka.MatchId, katka.Duration, katka.Score, katka.Turns, katka.DateTime, katka.Size));
                }
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                Logger.Log.Error(ex);
            }

            this.MatchesList.ItemsSource = this.Lines;
        }

        /// <summary>
        /// gets or sets collection of lines from database to be displayed in window.
        /// </summary>
        public ObservableCollection<MatchHistoryLine> Lines { get; set; }

        private void SetTitle(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Title = "Match history";
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Content = window;
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != this.lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (this.lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    this.Sort(sortBy, direction);

                    this.lastHeaderClicked = headerClicked;
                    this.lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(this.MatchesList.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
