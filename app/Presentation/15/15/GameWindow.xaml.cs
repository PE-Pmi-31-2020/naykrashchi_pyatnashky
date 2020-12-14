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
    using System.Windows.Threading;
    using BLL;
    using DAL;

    /// <summary>
    /// Interaction logic for GameWindow.xaml.
    /// </summary>
    public partial class GameWindow : Window
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
        public GameWindow(int gameSize, bool customImage)
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
            this.SaveMatchButton.Click += this.OnClickSaveMatch;
            this.isFirstMove = true;
            this.Duration = 0;
            this.GameSize = gameSize;
            this.Match = new Game(this.GameSize);
            this.customImage = customImage;
            this.MatchStartDateTime = DateTime.Now;
            this.databaseMatchId = -1;
            if (this.customImage)
            {
                this.CropImage();
            }

            this.InitGame();
        }

        public GameWindow(Match match)
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
            this.SaveMatchButton.Click += this.OnClickSaveMatch;
            this.isFirstMove = true;
            this.Duration = match.Duration.Value;
            this.TimeLabel.Content = this.Duration;
            this.GameSize = match.Size.Value;
            this.Match = new Game(match.Layout, match.Turns.Value);
            this.TurnsLabel.Content = "Turns: " + this.Match.Turns;
            this.customImage = match.CustomPicture == null ? false : true;
            this.MatchStartDateTime = DateTime.Now;
            this.databaseMatchId = match.MatchId;
            if (this.customImage)
            {
                App.Current.Properties[AppPropertyKeys.CustomImagePath] = match.CustomPicture;
                this.CropImage();
            }

            this.InitGame();
        }

        public Game Match { get; set; }

        public int GameSize { get; set; }

        Button[,] GameField { get; set; }

        public int Duration { get; set; }

        public DateTime MatchStartDateTime { get; set; }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
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

                    DBManager.AddMatch(katka);
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
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
                window.Show();
                this.Close();
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

        private void InitGame()
        {
            double buttonHeight = this.Height / 2 / this.GameSize;
            double buttonWidth = this.Width / 2 / this.GameSize;

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

        private void UpdateDuration(object sender, EventArgs e)
        {
            this.Duration += 1;
            this.TimeLabel.Content = this.Duration;
        }
    }
}
