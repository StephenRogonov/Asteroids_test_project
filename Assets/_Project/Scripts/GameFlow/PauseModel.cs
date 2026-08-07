using _Project.Scripts.GameFlow;
using _Project.Scripts.Sounds;
using System;

namespace _Project.Scripts.UI
{
    public class PauseModel
    {
        private PauseSwitcher _pauseSwitcher;
        private GameStateLoader _gameExit;
        private SoundFactory _soundFactory;

        public event Action Paused;
        public event Action ExitTriggered;

        public PauseModel(
            PauseSwitcher pauseSwitcher, 
            GameStateLoader gameExit,
            SoundFactory soundFactory
            )
        {
            _pauseSwitcher = pauseSwitcher;
            _gameExit = gameExit;
            _soundFactory = soundFactory;
        }

        public void PauseGame()
        {
            _pauseSwitcher.PauseAll();
            Paused?.Invoke();
        }

        public void UnpauseGame()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _pauseSwitcher.UnpauseAll();
        }

        public void ExitToMainMenu()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _gameExit.ExitToMainMenu();
        }
    }
}