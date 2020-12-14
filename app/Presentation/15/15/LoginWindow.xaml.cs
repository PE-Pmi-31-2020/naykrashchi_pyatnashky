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
            this.ExitButton.Click += this.OnClickExit;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            int userID = -1;
            string login = this.LoginInput.Text.Trim();
            string password = this.PasswordInput.Password;
            try
            {
                bool successfullyLogged = DBManager.LogIn(login, password, ref userID);
                if (successfullyLogged)
                {
                    this.SaveRememberMeValue();
                    this.StoreUserData(userID, login, password);
                    this.GoToMainWindow();
                }
                else
                {
                    MessageBox.Show("Wrong login or password");
                }
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            string login = this.LoginInput.Text.Trim();
            string password = this.PasswordInput.Password;
            try
            {
                int userID = DBManager.AddUser(login, password);
                this.SaveRememberMeValue();
                this.StoreUserData(userID, login, password);
                this.GoToMainWindow();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                if (ex.InnerException.Message.Split(":")[0] == "23505")
                {
                    MessageBox.Show("Login already taken");
                }
                else
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void StoreUserData(int userID, string login, string password)
        {
            App.Current.Properties[AppPropertyKeys.UserID] = userID;
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
