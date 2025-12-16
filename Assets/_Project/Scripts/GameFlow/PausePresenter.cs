using _Project.Scripts.UI;
using System;

namespace _Project.Scripts.GameFlow
{
    public class PausePresenter : IDisposable
    {
        private PauseModel _model;
        private PauseView _view;

        public PausePresenter(PauseModel model)
        {
            _model = model;
        }

        public void Init(PauseView pauseView)
        {
            _view = pauseView;

            _model.Paused += EnableView;
            _view.ContinueClicked += ContinueGame;
            _view.ExitClicked += Exit;
        }

        public void EnableView(int score)
        {
            _view.UpdateScore(score);
            _view.EnableObject();
        }

        public void ContinueGame()
        {
            _model.UnpauseGame();
        }

        public void Exit()
        {
            _model.ExitToMainMenu();
        }

        public void Dispose()
        {
            _model.Paused -= EnableView;
            _view.ContinueClicked -= ContinueGame;
            _view.ExitClicked -= Exit;
        }
    }
}
