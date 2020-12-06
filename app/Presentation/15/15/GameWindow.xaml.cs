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
    using BLL;

    /// <summary>
    /// Interaction logic for GameWindow.xaml.
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            this.InitializeComponent();
            this.BackButton.Click += this.OnClickBack;
            this.GameSize = 4;
            this.InitGame();
        }

        public Game Match { get; set; }

        public int GameSize { get; set; }

        Button[,] GameField { get; set; }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GameSize; i++)
            {
                for (int j = 0; j < GameSize; j++)
                {
                    MessageBox.Show($"{this.Match.Layout[i][j]} {i} {j}");
                }
            }
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void OnClickFieldCell(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    if (this.GameField[i, j] == senderButton)
                    {
                        this.Match.Move(i, j);
                        if (this.Match.Solved())
                        {
                            MessageBox.Show("gg! ez +25(20)");  
                        }
                        break;
                    }
                }
            }

            this.UpdateButtonNumbers();
        }

        private void UpdateButtonNumbers()
        {
            for (int i = 0; i < this.GameSize; i++)
            {
                for (int j = 0; j < this.GameSize; j++)
                {
                    this.GameField[i, j].Content = this.Match.Layout[i][j];
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
                    this.GameField[i, j].Content = this.Match.Layout[i][j];
                    this.GameField[i, j].Height = buttonHeight;
                    this.GameField[i, j].Width = buttonWidth;
                    this.GameField[i, j].Click += this.OnClickFieldCell;
                    this.FieldContainer.Children.Add(this.GameField[i, j]);
                }
            }

        }
    }
}
