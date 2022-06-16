using System;
using System.Collections.Generic;
using System.Text;

namespace MyALWebScrapping.Entities
{
    public class Anime
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }

        public Anime(int rank, string name, double score)
        {
            Rank = rank;
            Name = name;
            Score = score;
        }
    }
}
