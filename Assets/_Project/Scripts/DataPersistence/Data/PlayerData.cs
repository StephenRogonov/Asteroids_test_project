using _Project.Scripts.Obstacles.Score;
using System;
using System.Collections.Generic;

namespace _Project.Scripts.DataPersistence
{
    [Serializable]
    public class PlayerData
    {
        public DateTime SaveDateTime {  get; set; }
        public bool NoAdsPurchased { get; set; }
        public List<ScoreEntry> Leaderboard { get; set; }

        public PlayerData()
        {
            SaveDateTime = DateTime.Now;
            NoAdsPurchased = false;
            Leaderboard = new List<ScoreEntry>();
        }
    }
}

