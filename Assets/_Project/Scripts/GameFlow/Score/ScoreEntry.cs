using System;

namespace _Project.Scripts.Obstacles.Score
{
    [Serializable]
    public class ScoreEntry
    {
        public DateTime ScoreDate;
        public int Score;
        public bool IsNew;

        public ScoreEntry(DateTime scoreDate, int score, bool isNew)
        {
            ScoreDate = scoreDate;
            Score = score;
            IsNew = isNew;
        }
    }
}
