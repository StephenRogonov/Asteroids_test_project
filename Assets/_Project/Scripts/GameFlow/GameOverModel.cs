using _Project.Scripts.Bootstrap.Advertising;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.Obstacles;
using _Project.Scripts.Player;
using _Project.Scripts.Sounds;
using _Project.Scripts.UI;
using System;

namespace _Project.Scripts.GameFlow
{
    public class GameOverModel : IDisposable
    {
        private PauseSwitcher _pauseHandler;
        private MobileControls _mobileControls;
        private ShipCollision _shipCollision;
        private IInterstitial _interstitial;
        private IRewarded _rewarded;
        private DataPersistenceHandler _dataPersistenceHandler;
        private SceneSwitcher _sceneSwitcher;
        private ObstaclesFactory _obstaclesFactory;
        private ShipMovement _shipMovement;
        private GameSessionData _gameSessionData;
        private GameStateLoader _gameStateLoader;
        private SoundFactory _soundFactory;

        private bool _noAdsPurchased;

        private Action OnInterstitialShown;
        private Action OnRewardedShown;

        public event Action<int> GameOverTriggered;
        public event Action ContinueButtonClicked;

        public GameOverModel(
            PauseSwitcher pauseHandler,
            IInterstitial interstitial,
            IRewarded rewarded,
            DataPersistenceHandler dataPersistenceHandler,
            SceneSwitcher sceneSwitcher,
            ObstaclesFactory obstaclesFactory,
            GameSessionData gameSessionData,
            GameStateLoader gameStateLoader,
            SoundFactory soundFactory
            )
        {
            _pauseHandler = pauseHandler;
            _interstitial = interstitial;
            _rewarded = rewarded;
            _dataPersistenceHandler = dataPersistenceHandler;
            _sceneSwitcher = sceneSwitcher;
            _obstaclesFactory = obstaclesFactory;
            _gameSessionData = gameSessionData;
            _gameStateLoader = gameStateLoader;
            _soundFactory = soundFactory;
        }

        public void Init(MobileControls mobileControls, ShipMovement shipMovement, ShipCollision shipCollision)
        {
            _mobileControls = mobileControls;
            _shipMovement = shipMovement;
            _shipCollision = shipCollision;
            _noAdsPurchased = _dataPersistenceHandler.PlayerData.NoAdsPurchased;

            _shipCollision.Crashed += GameOverTrigger;
        }

        private void ShowInterstitial()
        {
            _interstitial.ShowAd(OnInterstitialShown);
        }

        private void ShowRewarded()
        {
            _rewarded.ShowAd(OnRewardedShown);
        }

        private void GameOverTrigger()
        {
            _soundFactory.StopRegisteredSound(AudioID.Acceleration);
            _soundFactory.PlaySound(AudioID.GameOver);
            _pauseHandler.PauseAll();
            _mobileControls.BlockButtons();
        }

        public void ActivateView()
        {
            GameOverTriggered?.Invoke(_gameSessionData.TotalScore);
        }

        public void Continue()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            OnRewardedShown += ContinueGame;
            ShowRewarded();
        }

        public void ContinueGame()
        {
            _pauseHandler.UnpauseAll();
            _obstaclesFactory.ReturnSpawnedToPool();
            _shipMovement.ActivateObject();
            _mobileControls.UnblockButtons();
            ContinueButtonClicked?.Invoke();
        }

        public void RestartGame()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            if (_noAdsPurchased == false)
            {
                OnInterstitialShown += ReloadScene;
                ShowInterstitial();
            }
            else if (_noAdsPurchased == true)
            {
                ReloadScene();
            }
        }

        public void ExitToMainMenu()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _gameStateLoader.ExitToMainMenu();
        }

        private void ReloadScene()
        {
            OnInterstitialShown -= ReloadScene;
            _gameStateLoader.ReloadGameScene();
        }

        public void Dispose()
        {
            OnRewardedShown -= ContinueGame;
            _shipCollision.Crashed -= GameOverTrigger;
        }
    }
}