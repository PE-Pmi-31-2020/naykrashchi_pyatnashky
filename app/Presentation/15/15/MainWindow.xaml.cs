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
    using DAL;

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
            this.NewGameButton.Click += this.OnClickNewGame;
            this.ExitButton.Click += this.OnClickExit;
            this.MatchHistoryButton.Click += this.OnClickMatchHistory;
            this.LogOutButton.Click += this.OnClickLogOut;
            this.DeleteAccButton.Click += this.OnClickDeleteAcc;
            this.ContinueGameButton.Click += this.OnClickContinueGame;
        }

        /// <summary>
        /// Gets login.
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string Password { get; }

        private void OnClickNewGame(object sender, RoutedEventArgs e)
        {
            MatchSettingsWindow window = new MatchSettingsWindow();
            window.Show();
            this.Close();
        }

        private void OnClickContinueGame(object sender, RoutedEventArgs e)
        {
            MatchPickWindow window = new MatchPickWindow();
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

        private void OnClickLogOut(object sender, RoutedEventArgs e)
        {
            App.Current.Properties.Remove(AppPropertyKeys.UserID);
            App.Current.Properties.Remove(AppPropertyKeys.Login);
            App.Current.Properties.Remove(AppPropertyKeys.Password);
            App.Current.Properties.Remove(AppPropertyKeys.RememberMe);
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Close();
        }

        private void OnClickDeleteAcc(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Do you want to delete this account permanently?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                DBManager.DeleteUser((int)App.Current.Properties[AppPropertyKeys.UserID]);
                this.OnClickLogOut(sender, e);
            }
        }
    }
}
