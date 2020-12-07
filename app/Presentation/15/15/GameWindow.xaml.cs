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

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
            this.Duration = 0;
            this.GameSize = 4;
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

        private void OnClickFieldCell(object sender, RoutedEventArgs e)
        {
            if (this.Match.Turns == 0)
            {
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

            this.UpdateButtonNumbers();
            if (this.Match.Solved())
            {
                this.SaveMatch();
                MessageBox.Show($"Your score: {1000000 - (this.Duration * this.Match.Turns)}", "Result");
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
        }

        private void SaveMatch()
        {
            DAL.DBManager.AddMatch(new Match()
            {
                UserId = (int)App.Current.Properties[AppPropertyKeys.UserID],
                Duration = this.Duration,
                DateTime = this.MatchStartDateTime,
                Score = 1000000 - (this.Duration * this.Match.Turns),
                Result = true,
                Turns = this.Match.Turns,
            });
        }

        private void UpdateButtonNumbers()
        {
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    if (this.Match.Layout[i][j] != 0)
                    {
                        this.GameField[i, j].Content = this.Match.Layout[i][j];
                    }
                    else
                    {
                        this.GameField[i, j].Content = string.Empty;
                    }
                }
            }
        }

        private void InitGame()
        {
            double buttonHeight = this.Height / 2 / this.GameSize;
            double buttonWidth = this.Width / 2 / this.GameSize;

            this.GameField = new Button[this.GameSize, this.GameSize];
            this.Match = new Game(this.GameSize);

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
                        this.GameField[i, j].Content = this.Match.Layout[i][j];
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
