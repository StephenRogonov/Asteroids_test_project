using System.Globalization;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Obstacles.Score
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _scoreText;

        public void FillScoreEntryData(ScoreEntry scoreEntry)
        {
            
            _dateText.text = scoreEntry.ScoreDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            _scoreText.text = scoreEntry.Score.ToString();
        }
    }
}
