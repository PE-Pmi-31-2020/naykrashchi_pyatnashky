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
        public int match_id { get; set; }
        public int user_id { get; set; }
        public int duration { get; set; }
        public DateTime date_time { get; set; }
        public int score { get; set; }
        public bool result  { get; set; }
        public int size { get; set; }
        public List<List<int>> layout  { get; set; }
        public int turns  { get; set; }
        public string custom_picture  { get; set; }

        public Game()
        {

        }
        public Game(int m_id, int u_id, int s, string cp)
        {
            match_id = m_id;
            user_id = u_id;
            size = s;
            layout = layout_init();
            duration = 0;
            date_time = DateTime.Now;
            score = 0;
            result = false;
            turns = 0;
            custom_picture = cp;
        }

        public string hash_layout()
        {
            string hash = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    hash += Convert.ToChar(layout[i][j]);
                }
            }
            return hash;
        }

        public void unhash(string s)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    layout[i][j] = Convert.ToInt32(s[i * size + j]);
                }
            }
        }

        private List<List<int>> layout_init()
        {
            List<List<int>> init_layout = new List<List<int>>();
            for (int i = 0; i < size; i++)
            {
                init_layout.Add(new List<int>());
            }
            List<int> l = new List<int>();
            for (int i = 0; i < size * size; i++)
            {
                l.Add(i);
            }
            Random rnd = new Random();
            l = l.OrderBy(x => rnd.Next()).ToList();
            while (!solvable(l))
            {
                l = l.OrderBy(x => rnd.Next()).ToList();
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int idx = i * size + j;
                    init_layout[i].Add(l[idx]);
                }
            }
            return init_layout;
        }
        private bool solvable(List<int> l)
        {
            int inversions = 0;
            for (int i = 0; i < size*size; i++)
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
                if(l[i] == 0)
                {
                    inversions += 1 + i / size;
                }
            }
            return (inversions & 1) == 0;
        }

        public bool solved()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(i != size - 1 || j != size - 1)
                    {
                        if (i* size + j + 1 != layout[i][j])
                            return false;
                    }
                    else
                    {
                        return layout[i][j] == 0;
                    }
                }
            }
            return true;
        }
        public void move(int x, int y)
        {
            if(x>0 && layout[x-1][y] == 0)
            {
                layout[x - 1][y] = layout[x][y];
                layout[x][y] = 0;
                turns++;
            }
            else if (x < size - 1 && layout[x + 1][y] == 0)
            {
                layout[x + 1][y] = layout[x][y];
                layout[x][y] = 0;
                turns++;
            }
            else if (y > 0 && layout[x][y - 1] == 0)
            {
                layout[x][y - 1] = layout[x][y];
                layout[x][y] = 0;
                turns++;
            }
            else if (y < size - 1 && layout[x][y + 1] == 0)
            {
                layout[x][y + 1] = layout[x][y];
                layout[x][y] = 0;
                turns++;
            }
        }
        public void move(Moves m)
        {
            int x = 0, y = 0;
            if(m == Moves.Up)
            {
                x = 0;
                y = 1;
            }
            if (m == Moves.Left)
            {
                x = 1;
                y = 0;
            }
            if (m == Moves.Down)
            {
                x = 0;
                y = -1;
            }
            if (m == Moves.Right)
            {
                x = -1;
                y = 0;
            }
            if (moveTo(x, y))
                turns++;
        }
        private bool moveTo(int x, int y)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(layout[i][j] == 0)
                    {
                        int xTo = i + y;
                        int yTo = j + x;
                        if(xTo >= 0 && xTo < size && yTo >= 0 && yTo < size)
                        {
                            layout[i][j] = layout[xTo][yTo];
                            layout[xTo][yTo] = 0;
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
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(layout[i][j]);
                    Console.Write('\t');
                }
                Console.WriteLine();
            }
            Console.WriteLine($"turns: {turns}");
        }
    }
    class program
    {
        static void Main(string[] args)
        {
            Game g = new Game(1,1,4,"s");
            while(!g.solved())
            {
                g.print();
                var ch = Console.ReadKey(false).Key;
                switch (ch)
                {
                    case ConsoleKey.UpArrow:
                        g.move(Moves.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        g.move(Moves.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        g.move(Moves.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        g.move(Moves.Right);
                        break;
                }
            }
            Console.WriteLine("SOLVED!");
        }
    }

}
