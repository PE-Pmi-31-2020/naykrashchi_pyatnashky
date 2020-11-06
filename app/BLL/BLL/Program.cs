using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
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
                for (int j = 0; j < i; j++)
                {
                    if (l[j] > l[i])
                    {
                        inversions++;
                    }
                }
                if(l[i] == 0)
                {
                    inversions += 1 + i / 4;
                }
            }
            return (inversions & 1) == 1;
        }
    }
}
