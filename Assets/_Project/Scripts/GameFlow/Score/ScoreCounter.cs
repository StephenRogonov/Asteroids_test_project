namespace _Project.Scripts.Obstacles.Score
{
    public class ScoreCounter
    {
        private int _totalScore;

        public int TotalScore => _totalScore;
 
        public void AddScore(int score)
        {
            _totalScore += score;
        }
    }
}
