using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public enum moves
    {
        up,
        left,
        right,
        down
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
            return (inversions & 1) == 1;
        }

        public bool solved()
        {
            int prev = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(i != size - 1 || j != size - 1)
                    {
                        if (prev >= layout[i][j])
                            return false;
                        prev = layout[i][j];
                    }
                    else
                    {
                        return layout[i][j] == 0;
                    }
                }
            }
            return true;
        }
        

        public void move(moves m)
        {
            if(m == moves.up)
            {
                if(up())
                {
                    turns++;
                }
            }
            if (m == moves.left)
            {
                if (left())
                {
                    turns++;
                }
            }
            if (m == moves.down)
            {
                if (down())
                {
                    turns++;
                }
            }
            if (m == moves.right)
            {
                if (right())
                {
                    turns++;
                }
            }
        }

        private bool down()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(layout[i][j] == 0)
                    {
                        if (i == 0)
                        {
                            return false;
                        }
                        else
                        {
                            layout[i][j] = layout[i - 1][j];
                            layout[i - 1][j] = 0;
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        private bool right()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (layout[i][j] == 0)
                    {
                        if (j == 0)
                        {
                            return false;
                        }
                        else
                        {
                            layout[i][j] = layout[i][j - 1];
                            layout[i][j - 1] = 0;
                            return true;
                        }
                    }
                }
            }
            return true;
        }
        private bool up()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (layout[i][j] == 0)
                    {
                        if (i == size - 1)
                        {
                            return false;
                        }
                        else
                        {
                            layout[i][j] = layout[i + 1][j];
                            layout[i + 1][j] = 0;
                            return true;
                        }
                    }
                }
            }
            return true;
        }
        private bool left()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (layout[i][j] == 0)
                    {
                        if (j == size - 1)
                        {
                            return false;
                        }
                        else
                        {
                            layout[i][j] = layout[i][j + 1];
                            layout[i][j + 1] = 0;
                            return true;
                        }
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
                        g.move(moves.up);
                        break;
                    case ConsoleKey.DownArrow:
                        g.move(moves.down);
                        break;
                    case ConsoleKey.LeftArrow:
                        g.move(moves.left);
                        break;
                    case ConsoleKey.RightArrow:
                        g.move(moves.right);
                        break;
                }
            }
            Console.WriteLine("SOLVED!");
        }
    }

}
