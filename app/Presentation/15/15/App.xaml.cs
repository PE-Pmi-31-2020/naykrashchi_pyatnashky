// <copyright file="App.xaml.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using BLL;

    #pragma warning disable
    public enum AppPropertyKeys
    {
        RememberMe,
        Login,
        Password,
        UserID,
        CustomImagePath,
    }

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        #pragma warning disable
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("remember_me", FileMode.Create, isolatedStorage));

                if (App.Current.Properties.Contains(AppPropertyKeys.RememberMe) && 
                    (bool)App.Current.Properties[AppPropertyKeys.RememberMe] && 
                    App.Current.Properties.Contains(AppPropertyKeys.Login))
                {
                    srWriter.WriteLine(App.Current.Properties[AppPropertyKeys.UserID].ToString() + "\n" + 
                                        App.Current.Properties[AppPropertyKeys.Login].ToString() + "\n" +
                                        App.Current.Properties[AppPropertyKeys.Password].ToString());
                }

                srWriter.Flush();
                srWriter.Close();
            }
            catch (System.Security.SecurityException sx)
            {
                MessageBox.Show(sx.Message);
                Logger.Log.Error(sx);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.InitLogger();
<<<<<<< HEAD
=======
            Logger.Log.Info("Run program");
>>>>>>> 0631ddc3445daaad11994b1fbd74f9101e789ac2
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("remember_me", FileMode.OpenOrCreate, isolatedStorage));

                if (!srReader.EndOfStream)
                {
                    string[] userData = srReader.ReadToEnd().Split('\n');
                    App.Current.Properties[AppPropertyKeys.UserID] = Convert.ToInt32(userData[0]);
                    App.Current.Properties[AppPropertyKeys.Login] = userData[1];
                    App.Current.Properties[AppPropertyKeys.Password] = userData[2];
                    App.Current.Properties[AppPropertyKeys.RememberMe] = true;
                }
                srReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Log.Error(ex);
            }
        }
    }
}
