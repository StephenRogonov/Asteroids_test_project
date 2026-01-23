using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Player;
using System;
using System.Collections.Generic;

namespace _Project.Scripts.Obstacles.Score
{
    public class LeaderboardModel : IDataPersistence, IDisposable
    {
        private DataPersistenceHandler _dataPersistenceHandler;
        private ShipCollision _shipCollision;
        private ScoreCounter _scoreCounter;
        private GameOverModel _gameOverModel;
        private int _maxLeaderboardEntries = 5;
        private int _minScoreEntryIndex;
        private DateTime _currentDateTime;
        private bool _sameRun;
        private bool _leaderboardChanged;

        private List<ScoreEntry> _leaderboard = new();

        public event Action GameOverTriggered;

        public LeaderboardModel(
            DataPersistenceHandler dataPersistenceHandler,
            ScoreCounter scoreCounter,
            GameOverModel gameOverModel
            )
        {
            _dataPersistenceHandler = dataPersistenceHandler;
            _scoreCounter = scoreCounter;
            _gameOverModel = gameOverModel;
        }

        public void Init(ShipCollision shipCollision)
        {
            _shipCollision = shipCollision;
            _dataPersistenceHandler.AddDataObject(this);
            _leaderboard.Clear();
            _leaderboard.AddRange(_dataPersistenceHandler.PlayerData.Leaderboard);

            _shipCollision.Crashed += HandleNewScore;
            _gameOverModel.ContinueButtonClicked += ContinueCurrentRun;
        }

        public List<ScoreEntry> GetSortedLeaderboard()
        {
            for (int i = 1; i < _leaderboard.Count; i++)
            {
                for (int j = 0; j < _leaderboard.Count - 1; j++)
                {
                    if ((_leaderboard[j].Score < _leaderboard[j + 1].Score) ||
                        ((_leaderboard[j].Score == _leaderboard[j + 1].Score) &&
                        (_leaderboard[j].ScoreDate > _leaderboard[j + 1].ScoreDate)))
                    {
                        ScoreEntry temp = _leaderboard[j];
                        _leaderboard[j] = _leaderboard[j + 1];
                        _leaderboard[j + 1] = temp;
                    }
                }
            }

            return _leaderboard;
        }

        private async void HandleNewScore()
        {
            if (_sameRun == true)
            {
                foreach (ScoreEntry score in _leaderboard)
                {
                    if (score.ScoreDate == _currentDateTime)
                    {
                        _currentDateTime = DateTime.Now;
                        score.Score = _scoreCounter.TotalScore;
                        score.ScoreDate = _currentDateTime;
                    }
                }

                await _dataPersistenceHandler.SavePlayerData();
            }
            else if (_leaderboard.Count >= _maxLeaderboardEntries)
            {
                FindMinScoreIndex();

                if (_scoreCounter.TotalScore > _leaderboard[_minScoreEntryIndex].Score)
                {
                    if (CheckDuplicateMinScores())
                    {
                        FindMaxDateIndex();
                    }

                    _leaderboard.RemoveAt(_minScoreEntryIndex);
                    _currentDateTime = DateTime.Now;
                    _leaderboard.Add(new ScoreEntry(_currentDateTime, _scoreCounter.TotalScore));
                    _leaderboardChanged = true;
                    await _dataPersistenceHandler.SavePlayerData();
                }
            }
            else
            {
                _currentDateTime = DateTime.Now;
                _leaderboard.Add(new ScoreEntry(_currentDateTime, _scoreCounter.TotalScore));
                _leaderboardChanged = true;
                await _dataPersistenceHandler.SavePlayerData();
            }

            GameOverTriggered?.Invoke();
        }

        private void FindMinScoreIndex()
        {
            for (int i = 1; i < _leaderboard.Count; i++)
            {
                if (_leaderboard[_minScoreEntryIndex].Score > _leaderboard[i].Score)
                {
                    _minScoreEntryIndex = i;
                }
            }
        }

        private bool CheckDuplicateMinScores()
        {
            int minScoreEntriesCount = 0;

            foreach (ScoreEntry entry in _leaderboard)
            {
                if (entry.Score == _leaderboard[_minScoreEntryIndex].Score)
                {
                    minScoreEntriesCount++;
                }
            }

            if (minScoreEntriesCount > 1)
            {
                return true;
            }

            return false;
        }

        private void FindMaxDateIndex()
        {
            int minScore = _leaderboard[_minScoreEntryIndex].Score;
            DateTime maxDate = _leaderboard[_minScoreEntryIndex].ScoreDate;

            for (int i = 0; i < _leaderboard.Count; i++)
            {
                if (_leaderboard[i].Score == minScore &&
                    _leaderboard[i].ScoreDate > maxDate)
                {
                    _minScoreEntryIndex = i;
                    maxDate = _leaderboard[i].ScoreDate;
                }
            }
        }

        private void ContinueCurrentRun()
        {
            if (_sameRun == false && _leaderboardChanged == true)
            {
                _sameRun = true;
            }
        }

        public void ShowGameOverMenu()
        {
            _gameOverModel.ActivateView();
        }

        public void Dispose()
        {
            _shipCollision.Crashed -= HandleNewScore;
            _dataPersistenceHandler.RemoveDataObject(this);
        }

        public void SaveData(PlayerData data)
        {
            data.Leaderboard.Clear();
            data.Leaderboard.AddRange(_leaderboard);
        }
    }
}
