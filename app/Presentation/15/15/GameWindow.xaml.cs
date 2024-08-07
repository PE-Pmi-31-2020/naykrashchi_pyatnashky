﻿// <copyright file="GameWindow.xaml.cs" company="OnceDaughtersAlwaysDaughters">
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
    using System.Windows.Threading;
    using BLL;
    using DAL;

    /// <summary>
    /// Interaction logic for GameWindow.xaml.
    /// </summary>
    public partial class GameWindow : UserControl
    {
        private DispatcherTimer dispatcherTimer;

        private int score;

        private bool isFirstMove;

        private bool customImage;

        private Dictionary<int, ImageBrush> imagePartsMap;

        private int databaseMatchId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow(int gameSize, bool customImage, double windowHeight, double windowWidth)
        {
            Logger.Log.Info("new game started");
            this.InitializeComponent();
            this.Loaded += new RoutedEventHandler(this.SetTitle);
            this.SizeChanged += new SizeChangedEventHandler(this.OnResize);
            this.BackButton.Click += this.OnClickBack;
            this.SaveMatchButton.Click += this.OnClickSaveMatch;
            this.isFirstMove = true;
            this.Duration = 0;
            this.GameSize = gameSize;
            this.Match = new Game(this.GameSize);
            this.customImage = false;
            this.MatchStartDateTime = DateTime.Now;
            this.databaseMatchId = -1;
            if (customImage && File.Exists(@App.Current.Properties[AppPropertyKeys.CustomImagePath].ToString()))
            {
                this.customImage = true;
                this.CropImage();
            }

            this.InitGame(windowHeight, windowWidth);
        }

        public GameWindow(Match match, double windowHeight, double windowWidth)
        {
            Logger.Log.Info("game loaded");
            this.InitializeComponent();
            this.Loaded += new RoutedEventHandler(this.SetTitle);
            this.SizeChanged += new SizeChangedEventHandler(this.OnResize);
            this.BackButton.Click += this.OnClickBack;
            this.SaveMatchButton.Click += this.OnClickSaveMatch;
            this.isFirstMove = true;
            this.Duration = match.Duration.Value;
            this.TimeLabel.Content = this.Duration;
            this.GameSize = match.Size.Value;
            this.Match = new Game(match.Layout, match.Turns.Value);
            this.TurnsLabel.Content = "Turns: " + this.Match.Turns;
            if (match.CustomPicture != null && File.Exists(match.CustomPicture))
            {
                this.customImage = true;
            }

            this.MatchStartDateTime = DateTime.Now;
            this.databaseMatchId = match.MatchId;
            if (this.customImage)
            {
                App.Current.Properties[AppPropertyKeys.CustomImagePath] = match.CustomPicture;
                this.CropImage();
            }

            this.InitGame(windowHeight, windowWidth);
        }

        public Game Match { get; set; }

        public int GameSize { get; set; }

        Button[,] GameField { get; set; }

        public int Duration { get; set; }

        public DateTime MatchStartDateTime { get; set; }

        private void SetTitle(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Title = "Pyatnashki";
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Content = window;
        }

        private void OnClickSaveMatch(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Do you want to save game to finish it later?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    if (this.databaseMatchId != -1)
                    {
                        DBManager.DeleteMatch(this.databaseMatchId);
                    }

                    Match katka = new Match()
                    {
                        UserId = (int)App.Current.Properties[AppPropertyKeys.UserID],
                        Duration = this.Duration,
                        DateTime = this.MatchStartDateTime,
                        Result = false,
                        Turns = this.Match.Turns,
                        Size = this.GameSize,
                        Layout = this.Match.Hash_layout(),
                    };
                    if (this.customImage)
                    {
                        katka.CustomPicture = App.Current.Properties[AppPropertyKeys.CustomImagePath].ToString();
                    }

                    Logger.Log.Info("Unfinished game saved.");
                    DBManager.AddMatch(katka);
                    MainWindow window = new MainWindow();
                    this.Content = window;
                }
                catch (System.InvalidOperationException ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    Logger.Log.Error(ex);
                }
            }
        }

        private void OnClickFieldCell(object sender, RoutedEventArgs e)
        {
            if (this.isFirstMove)
            {
                this.isFirstMove = false;
                this.MatchStartDateTime = DateTime.Now;
                this.dispatcherTimer = new DispatcherTimer();
                this.dispatcherTimer.Tick += new EventHandler(this.UpdateDuration);
                this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                this.dispatcherTimer.Start();
            }

            Button senderButton = sender as Button;
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    if (this.GameField[i, j] == senderButton)
                    {
                        this.Match.Move(i, j);
                        this.TurnsLabel.Content = "Turns: " + this.Match.Turns;
                        break;
                    }
                }
            }

            this.UpdateButtons();
            if (this.Match.Solved())
            {
                Logger.Log.Info("Game Finished.");
                this.dispatcherTimer.Stop();
                this.score = Math.Max(1000000 - (this.Duration * this.Match.Turns), 0);
                int status = this.SaveMatch();
                if (status == 1)
                {
                    MessageBox.Show($"Your score: {this.score}, game will not be saved due to connection errors", "Result");
                }
                else
                {
                    MessageBox.Show($"Your score: {this.score}", "Result");
                }

                MainWindow window = new MainWindow();
                this.Content = window;
            }
        }

        private void CropImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@App.Current.Properties[AppPropertyKeys.CustomImagePath].ToString());
            bitmap.EndInit();

            this.imagePartsMap = new Dictionary<int, ImageBrush>();

            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    Image croppedImage = new Image();
                    croppedImage.Height = bitmap.PixelHeight / this.GameSize;
                    croppedImage.Width = bitmap.PixelWidth / this.GameSize;
                    CroppedBitmap cb = new CroppedBitmap(bitmap, new Int32Rect((int)croppedImage.Width * j, (int)croppedImage.Height * i, (int)croppedImage.Width, (int)croppedImage.Height));

                    croppedImage.Source = cb;
                    croppedImage.Stretch = Stretch.Fill;
                    croppedImage.StretchDirection = StretchDirection.Both;
                    this.imagePartsMap.Add((i * this.GameSize) + j + 1, new ImageBrush(croppedImage.Source));
                }
            }
        }

        private int SaveMatch()
        {
            try
            {
                if (this.databaseMatchId != -1)
                {
                    DBManager.DeleteMatch(this.databaseMatchId);
                }

                DBManager.AddMatch(new Match()
                {
                    UserId = (int)App.Current.Properties[AppPropertyKeys.UserID],
                    Duration = this.Duration,
                    DateTime = this.MatchStartDateTime,
                    Score = this.score,
                    Result = true,
                    Turns = this.Match.Turns,
                    Size = this.GameSize,
                });
                Logger.Log.Info("Finished game saved.");
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                Logger.Log.Error(ex);
                return 1;
            }

            return 0;
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    if (this.Match.Layout[i][j] != 0)
                    {
                        if (this.customImage)
                        {
                            this.GameField[i, j].Background = this.imagePartsMap[this.Match.Layout[i][j]];
                        }
                        else
                        {
                            this.GameField[i, j].Content = this.Match.Layout[i][j];
                        }
                    }
                    else
                    {
                        if (this.customImage)
                        {
                            this.GameField[i, j].Background = (LinearGradientBrush)App.Current.Resources["ButtonBackgroundBrush"];
                        }
                        else
                        {
                            this.GameField[i, j].Content = string.Empty;
                        }
                    }
                }
            }
        }

        private void InitGame(double windowHeight, double windowWidth)
        {
            double buttonHeight = windowHeight / 2 / this.GameSize;
            double buttonWidth = windowWidth / 2 / this.GameSize;
            this.FieldContainer.Height = windowHeight / 2;
            this.FieldContainer.Width = windowWidth / 2;
            this.GameField = new Button[this.GameSize, this.GameSize];
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    this.GameField[i, j] = new Button();
                    this.GameField[i, j].Height = buttonHeight;
                    this.GameField[i, j].Width = buttonWidth;
                    this.GameField[i, j].FontSize = 16;
                    this.GameField[i, j].Click += this.OnClickFieldCell;
                    this.FieldContainer.Children.Add(this.GameField[i, j]);
                    if (this.Match.Layout[i][j] != 0)
                    {
                        if (this.customImage)
                        {
                            this.GameField[i, j].Background = this.imagePartsMap[this.Match.Layout[i][j]];
                        }
                        else
                        {
                            this.GameField[i, j].Content = this.Match.Layout[i][j];
                        }
                    }
                }
            }
        }

        private void OnResize(object sender, SizeChangedEventArgs e)
        {
            double windowHeight = e.NewSize.Height;
            double windowWidth = e.NewSize.Width;
            double buttonHeight = windowHeight / 2 / this.GameSize;
            double buttonWidth = windowWidth / 2 / this.GameSize;
            this.FieldContainer.Height = windowHeight / 2;
            this.FieldContainer.Width = windowWidth / 2;
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    this.GameField[i, j].Height = buttonHeight;
                    this.GameField[i, j].Width = buttonWidth;
                }
            }
        }

        private void UpdateDuration(object sender, EventArgs e)
        {
            this.Duration += 1;
            this.TimeLabel.Content = this.Duration;
        }
    }
}
