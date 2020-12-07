// <copyright file="Game.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace BLL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// enum for describing moves.
    /// </summary>
    public enum Moves
    {
#pragma warning disable SA1602
        Up,
        Left,
        Right,
        Down,
#pragma warning restore SA1602
    }

    /// <summary>
    /// core class which implements all bll logic.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="match_id">match id.</param>
        /// <param name="user_id">user id.</param>
        /// <param name="size"> size.</param>
        /// <param name="custom_picture"> custom picture.</param>
        public Game(int size)
        {
            this.Size = size;
            this.Layout = this.Layout_init();
            this.Turns = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="hash">hashed layout.</param>
        /// <param name="turns">turns.</param>
        public Game(string hash, int turns)
        {
            this.Size = (int)Math.Sqrt(hash.Length);
            this.Unhash(hash);
            this.Turns = turns;
        }

        /// <summary>
        /// Gets or sets size of a board.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets layout.
        /// </summary>
        public List<List<int>> Layout { get; set; }

        /// <summary>
        /// Gets or sets number of turns.
        /// </summary>
        public int Turns { get; set; }

        /// <summary>
        /// hash layout as a string.
        /// </summary>
        /// <returns>hash.</returns>
        public string Hash_layout()
        {
            string hash = string.Empty;
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    hash += Convert.ToChar(this.Layout[i][j] + 32);
                }
            }

            return hash;
        }

        /// <summary>
        /// unhash layout.
        /// </summary>
        /// <param name="hash">hash.</param>
        public void Unhash(string hash)
        {
            this.Layout = new List<List<int>>();
            for (int i = 0; i < this.Size; i++)
            {
                List<int> l = new List<int>();
                for (int j = 0; j < this.Size; j++)
                {
                    l.Add(Convert.ToInt32(hash[(i * this.Size) + j]) - 32);
                    //this.Layout[i][j] = Convert.ToInt32(hash[(i * this.Size) + j]) - 32;
                }

                this.Layout.Add(l);
            }
        }

        /// <summary>
        /// function that checks if puzzle is solved.
        /// </summary>
        /// <returns>true or false.</returns>
        public bool Solved()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    if (i != this.Size - 1 || j != this.Size - 1)
                    {
                        if ((i * this.Size) + j + 1 != this.Layout[i][j])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return this.Layout[i][j] == 0;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// move from coordinates x and y.
        /// </summary>
        /// <param name="x">coordinate x.</param>
        /// <param name="y">coordinate y.</param>
        public void Move(int x, int y)
        {
            if (x > 0 && this.Layout[x - 1][y] == 0)
            {
                this.Layout[x - 1][y] = this.Layout[x][y];
                this.Layout[x][y] = 0;
                this.Turns++;
            }
            else if (x < this.Size - 1 && this.Layout[x + 1][y] == 0)
            {
                this.Layout[x + 1][y] = this.Layout[x][y];
                this.Layout[x][y] = 0;
                this.Turns++;
            }
            else if (y > 0 && this.Layout[x][y - 1] == 0)
            {
                this.Layout[x][y - 1] = this.Layout[x][y];
                this.Layout[x][y] = 0;
                this.Turns++;
            }
            else if (y < this.Size - 1 && this.Layout[x][y + 1] == 0)
            {
                this.Layout[x][y + 1] = this.Layout[x][y];
                this.Layout[x][y] = 0;
                this.Turns++;
            }
        }

        /// <summary>
        /// moves.
        /// </summary>
        /// <param name="m">direction of move.</param>
        public void Move(Moves m)
        {
            int x = 0, y = 0;
            if (m == Moves.Up)
            {
                x = 0;
                y = 1;
            }
            else if (m == Moves.Left)
            {
                x = 1;
                y = 0;
            }
            else if (m == Moves.Down)
            {
                x = 0;
                y = -1;
            }
            else if (m == Moves.Right)
            {
                x = -1;
                y = 0;
            }

            if (this.MoveTo(x, y))
            {
                this.Turns++;
            }
        }

        /// <summary>
        /// move to some position.
        /// </summary>
        /// <param name="x">x to.</param>
        /// <param name="y">y to.</param>
        /// <returns>a value that show us if move is available.</returns>
        private bool MoveTo(int x, int y)
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    if (this.Layout[i][j] == 0)
                    {
                        int xTo = i + y;
                        int yTo = j + x;
                        if (xTo >= 0 && xTo < this.Size && yTo >= 0 && yTo < this.Size)
                        {
                            this.Layout[i][j] = this.Layout[xTo][yTo];
                            this.Layout[xTo][yTo] = 0;
                            return true;
                        }

                        return false;
                    }
                }
            }

            return true;
        }

        private List<List<int>> Layout_init()
        {
            List<List<int>> init_layout = new List<List<int>>();
            for (int i = 0; i < this.Size; i++)
            {
                init_layout.Add(new List<int>());
            }

            List<int> l = new List<int>();
            for (int i = 0; i < this.Size * this.Size; i++)
            {
                l.Add(i);
            }

            Random rnd = new Random();
            l = l.OrderBy(x => rnd.Next()).ToList();
            while (!this.Solvable(l))
            {
                l = l.OrderBy(x => rnd.Next()).ToList();
            }

            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    int idx = (i * this.Size) + j;
                    init_layout[i].Add(l[idx]);
                }
            }

            return init_layout;
        }

        private bool Solvable(List<int> l)
        {
            int inversions = 0;
            for (int i = 0; i < this.Size * this.Size; i++)
            {
                if (l[i] != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (l[j] > l[i])
                        {
                            inversions++;
                        }
                    }
                }

                if (l[i] == 0)
                {
                    inversions += 1 + (i / this.Size);
                }
            }

            return ((inversions + this.Size) & 1) == 0;
        }

        /// <summary>
        /// program for testing.
        /// </summary>
        public class Program
        {
            /// <summary>
            /// main to test this project separatly.
            /// </summary>
            /// <param name="args">args.</param>
            public static void Main(string[] args)
            {
                Game g = new Game(4);
                while (!g.Solved())
                {
                    //g.Print();
                    var ch = Console.ReadKey(false).Key;
                    switch (ch)
                    {
                        case ConsoleKey.UpArrow:
                            g.Move(Moves.Up);
                            break;
                        case ConsoleKey.DownArrow:
                            g.Move(Moves.Down);
                            break;
                        case ConsoleKey.LeftArrow:
                            g.Move(Moves.Left);
                            break;
                        case ConsoleKey.RightArrow:
                            g.Move(Moves.Right);
                            break;
                    }
                }

                Console.WriteLine("SOLVED!");
            }
        }
    }
}