using System;

namespace _Project.Scripts.GameFlow
{
    public class GameOverPresenter : IDisposable
    {
        private GameOverModel _model;
        private GameOverView _view;

        public GameOverPresenter(
            GameOverModel model
            )
        {
            _model = model;
        }

        public void Init(GameOverView gameOverView)
        {
            _view = gameOverView;

            _model.GameOverTriggered += UpdateView;
            _view.RestartClicked += RestartGame;
            _view.ContinueClicked += ContinueGame;
        }

        private void RestartGame()
        {
            _model.RestartGame();
        }

        private void ContinueGame()
        {
            _model.Continue();
        }

        private void UpdateView(int score)
        {
            _view.UpdateScore(score);
            _view.EnableObject();
        }

        public void Dispose()
        {
            _model.GameOverTriggered -= UpdateView;
            _view.RestartClicked -= RestartGame;
            _view.ContinueClicked -= ContinueGame;
        }
    }
}