using _Project.Scripts.Common;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using System;

namespace _Project.Scripts.UI
{
    public class PauseModel
    {
        private PauseSwitcher _pauseHandler;
        private MobileControls _mobileControls;
        private SceneSwitcher _sceneSwitcher;
        private ScoreCounter _scoreCounter;

        public event Action<int> Paused;

        public PauseModel(
            PauseSwitcher pauseHandler, 
            SceneSwitcher sceneSwitcher,
            ScoreCounter scoreCounter
            )
        {
            _pauseHandler = pauseHandler;
            _sceneSwitcher = sceneSwitcher;
            _scoreCounter = scoreCounter;
        }

        public void Init(MobileControls mobileControls)
        {
            _mobileControls = mobileControls;
        }

        public void PauseGame()
        {
            _pauseHandler.PauseAll();
            _mobileControls.BlockButtons();
            Paused?.Invoke(_scoreCounter.TotalScore);
        }

        public void UnpauseGame()
        {
            _mobileControls.UnblockButtons();
            _pauseHandler.UnpauseAll();
        }

        public void ExitToMainMenu()
        {
            _sceneSwitcher.LoadMenu();
        }
    }
}