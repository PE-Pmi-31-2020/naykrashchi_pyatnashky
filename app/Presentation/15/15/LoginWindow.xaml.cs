// <copyright file="LoginWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
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
    using System.Windows.Shapes;
    using DAL;

    /// <summary>
    /// Interaction logic for LoginWindow.xaml.
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWindow"/> class.
        /// </summary>
        public LoginWindow()
        {
            this.InitializeComponent();
            if (App.Current.Properties.Contains(AppPropertyKeys.Login))
            {
                this.GoToMainWindow();
            }

            this.LogInButton.Click += this.Login;
            this.SignUpButton.Click += this.SignUp;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            int playerID;
            string login = this.LoginInput.Text.Trim();
            string password = this.PasswordInput.Password;
            bool successfullyLogged = DBManager.LogIn(login, password);
            if (successfullyLogged)
            {
                this.SaveRememberMeValue();
                this.StoreUserData(login, password);
                this.GoToMainWindow();
            }
            else
            {
                MessageBox.Show("Wrong login or password");
            }
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            int playerID;
            string login = this.LoginInput.Text.Trim();
            string password = this.PasswordInput.Password;
            bool successfullySignedUp = true;
            DBManager.AddUser(login, password);
            if (successfullySignedUp)
            {
                this.SaveRememberMeValue();
                this.StoreUserData(login, password);
                this.GoToMainWindow();
            }
            else
            {
                MessageBox.Show("Login already exists");
            }
        }

        private void StoreUserData(string login, string password)
        {
            App.Current.Properties[AppPropertyKeys.Login] = login;
            App.Current.Properties[AppPropertyKeys.Password] = password;
        }

        private void SaveRememberMeValue()
        {
            App.Current.Properties[AppPropertyKeys.RememberMe] = this.RememberMeCheck.IsChecked;
        }

        private void GoToMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
