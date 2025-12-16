using System;

namespace _Project.Scripts.Obstacles.Score
{
    [Serializable]
    public class ScoreEntry
    {
        public DateTime ScoreDate;
        public int Score;

        public ScoreEntry(DateTime scoreDate, int score)
        {
            ScoreDate = scoreDate;
            Score = score;
        }
    }
}
