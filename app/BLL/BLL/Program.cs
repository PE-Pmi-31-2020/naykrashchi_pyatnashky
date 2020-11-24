// <copyright file="Program.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace BLL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum Moves
    {
        Up,
        Left,
        Right,
        Down,
    }

    public class Game
    {
        public int Match_id { get; set; }

        public int User_id { get; set; }

        public int Duration { get; set; }

        public DateTime Date_time { get; set; }

        public int Score { get; set; }

        public bool Result  { get; set; }

        public int Size { get; set; }

        public List<List<int>> Layout { get; set; }

        public int Turns { get; set; }

        public string Custom_picture { get; set; }

        public Game()
        {
        }

        public Game(int m_id, int u_id, int s, string cp)
        {
            this.Match_id = m_id;
            this.User_id = u_id;
            this.Size = s;
            this.Layout = this.Layout_init();
            this.Duration = 0;
            this.Date_time = DateTime.Now;
            this.Score = 0;
            this.Result = false;
            this.Turns = 0;
            this.Custom_picture = cp;
        }

        public string Hash_layout()
        {
            string hash = string.Empty;
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    hash += Convert.ToChar(this.Layout[i][j]);
                }
            }

            return hash;
        }

        public void Unhash(string s)
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this.Layout[i][j] = Convert.ToInt32(s[(i * this.Size) + j]);
                }
            }
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

            return (inversions & 1) == 0;
        }

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

            if (this.moveTo(x, y))
            {
                this.Turns++;
            }
        }

        private bool moveTo(int x, int y)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if(Layout[i][j] == 0)
                    {
                        int xTo = i + y;
                        int yTo = j + x;
                        if(xTo >= 0 && xTo < Size && yTo >= 0 && yTo < Size)
                        {
                            Layout[i][j] = Layout[xTo][yTo];
                            Layout[xTo][yTo] = 0;
                            return true;
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        public void print()
        {
            Console.Clear();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(Layout[i][j]);
                    Console.Write('\t');
                }
                Console.WriteLine();
            }
            Console.WriteLine($"turns: {Turns}");
        }
    }
    class program
    {
        static void Main(string[] args)
        {
            Game g = new Game(1,1,4,"s");
            while(!g.Solved())
            {
                g.print();
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
