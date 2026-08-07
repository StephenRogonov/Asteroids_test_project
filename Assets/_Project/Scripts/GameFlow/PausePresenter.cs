using _Project.Scripts.Sounds;
using _Project.Scripts.UI;
using System;

namespace _Project.Scripts.GameFlow
{
    public class PausePresenter : IDisposable
    {
        private PauseModel _model;
        private PauseView _view;
        private GameSessionData _gameSessionData;

        public PausePresenter(
            PauseModel model, 
            GameSessionData gameSessionData
            )
        {
            _model = model;
            _gameSessionData = gameSessionData;
        }

        public void Init(PauseView pauseView)
        {
            _view = pauseView;

            _model.Paused += EnableView;
            _view.ContinueClicked += ContinueGame;
            _view.ExitClicked += Exit;
        }

        public void EnableView()
        {
            _view.UpdateScore(_gameSessionData.TotalScore);
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
