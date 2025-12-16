using System;

namespace _Project.Scripts.Obstacles.Score
{
    public class LeaderboardPresenter : IDisposable
    {
        private LeaderboardModel _model;
        private LeaderboardView _view;

        public LeaderboardPresenter(LeaderboardModel leaderboardModel)
        {
            _model = leaderboardModel;
        }

        public void Init(LeaderboardView leaderboardView)
        {
            _view = leaderboardView;

            _model.GameOverTriggered += EnableView;
            _view.ContinueClicked += SwitchToGameOver;
        }

        private void EnableView()
        {
            _view.UpdateLeaderboard(_model.GetSortedLeaderboard());
            _view.EnableObject();
        }

        private void SwitchToGameOver()
        {
            _model.ShowGameOverMenu();
        }

        public void Dispose()
        {
            _model.GameOverTriggered -= EnableView;
            _view.ContinueClicked -= SwitchToGameOver;
        }
    }
}
