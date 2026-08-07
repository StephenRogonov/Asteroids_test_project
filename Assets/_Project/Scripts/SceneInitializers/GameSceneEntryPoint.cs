using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Analytics;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.Player;
using _Project.Scripts.PlayerWeapons;
using _Project.Scripts.PlayerWeapons.Configs;
using _Project.Scripts.Sounds;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.SceneInitializers
{
    public class GameSceneEntryPoint : IInitializable, IDisposable
    {
        private IAssetLoader _assetLoader;
        private IInstantiator _instantiator;
        private IAudioCatalog _audioCatalog;
        private Transform _shipSpawnPosition;
        private HudView _hudView;
        private HudPresenter _hudPresenter;
        private MobileControls _mobileControls;
        private GameOverModel _gameOverModel;
        private PauseView _pauseView;
        private PausePresenter _pausePresenter;
        private GameOverView _gameOverView;
        private LeaderboardView _leaderboardView;
        private GameOverPresenter _gameOverPresenter;
        private ShipMovement _shipMovement;
        private MissilesFactory _missilesFactory;
        private PlayerInput _playerInput;
        private AnalyticsService _analyticsService;
        private GameSessionData _gameSessionData;
        private ShipLaserAttack _shipLaserAttack;
        private ShipMissilesAttack _shipMissilesAttack;
        private ShipCollision _shipCollision;
        private WeaponTrigger _weaponTrigger;
        private ObstaclesFactory _obstaclesFactory;
        private ShipLaserConfig _shipLaserConfig;
        private DataPersistenceHandler _dataPersistenceHandler;
        private PauseSwitcher _pauseSwitcher;
        private LeaderboardModel _leaderboardModel;
        private LeaderboardPresenter _leaderboardPresenter;
        private GameStateLoader _gameStateLoader;
        private LoadingOverlay _loadingOverlay;
        private SoundFactory _soundFactory;

        public GameSceneEntryPoint(
            IInstantiator instantiator,
            IAssetLoader assetLoader,
            IAudioCatalog audioCatalog,
            ShipSpawnPosition shipSpawnPosition,
            HudPresenter hudPresenter,
            GameOverModel gameOverModel,
            PausePresenter pausePresenter,
            GameOverPresenter gameOverPresenter,
            MissilesFactory missilesFactory,
            PlayerInput playerInput,
            AnalyticsService analyticsService,
            GameSessionData gameSessionData,
            WeaponTrigger weaponTrigger,
            ObstaclesFactory obstaclesFactory,
            ShipLaserConfig shipLaserConfig,
            DataPersistenceHandler dataPersistenceHandler,
            PauseSwitcher pauseSwitcher,
            LeaderboardModel leaderboardModel,
            LeaderboardPresenter leaderboardPresenter,
            GameStateLoader gameStateLoader,
            SoundFactory soundFactory
            )
        {
            _instantiator = instantiator;
            _assetLoader = assetLoader;
            _audioCatalog = audioCatalog;
            _shipSpawnPosition = shipSpawnPosition.transform;
            _hudPresenter = hudPresenter;
            _gameOverModel = gameOverModel;
            _pausePresenter = pausePresenter;
            _gameOverPresenter = gameOverPresenter;
            _missilesFactory = missilesFactory;
            _playerInput = playerInput;
            _analyticsService = analyticsService;
            _gameSessionData = gameSessionData;
            _weaponTrigger = weaponTrigger;
            _obstaclesFactory = obstaclesFactory;
            _shipLaserConfig = shipLaserConfig;
            _dataPersistenceHandler = dataPersistenceHandler;
            _pauseSwitcher = pauseSwitcher;
            _leaderboardModel = leaderboardModel;
            _leaderboardPresenter = leaderboardPresenter;
            _gameStateLoader = gameStateLoader;
            _soundFactory = soundFactory;
        }

        public async void Initialize()
        {
            await InstantiateAddressables();
            PositionShip();
            GetShipComponents();
            PassDependencies();

            _gameStateLoader.GameSceneExited += UnloadGameAssets;

            _soundFactory.PlayBackgroundMusic();
            _loadingOverlay.DisableObject();
        }

        public void Dispose()
        {
            _gameStateLoader.GameSceneExited -= UnloadGameAssets;
        }

        private async UniTask InstantiateAddressables()
        {
            _loadingOverlay = _instantiator.InstantiatePrefabForComponent<LoadingOverlay>(
                await _assetLoader.LoadPrefabByID<LoadingOverlay>(AssetsIDs.LOADING_OVERLAY));
            _loadingOverlay.EnableObject();

            _shipMovement = _instantiator.InstantiatePrefabForComponent<ShipMovement>(
                await _assetLoader.LoadPrefabByID<ShipMovement>(AssetsIDs.PLAYER_SHIP));

            _hudView = _instantiator.InstantiatePrefabForComponent<HudView>(
                await _assetLoader.LoadPrefabByID<HudView>(AssetsIDs.HUD));

            _mobileControls = _instantiator.InstantiatePrefabForComponent<MobileControls>(
                await _assetLoader.LoadPrefabByID<MobileControls>(AssetsIDs.MOBILE_CONTROLS));

            _pauseView = _instantiator.InstantiatePrefabForComponent<PauseView>(
                await _assetLoader.LoadPrefabByID<PauseView>(AssetsIDs.PAUSE_MENU));

            _gameOverView = _instantiator.InstantiatePrefabForComponent<GameOverView>(
                await _assetLoader.LoadPrefabByID<GameOverView>(AssetsIDs.GAME_OVER_MENU));

            _leaderboardView = _instantiator.InstantiatePrefabForComponent<LeaderboardView>(
                await _assetLoader.LoadPrefabByID<LeaderboardView>(AssetsIDs.LEADERBOARD_MENU));

            await _audioCatalog.LoadAssets();
        }

        private void PositionShip()
        {
            _shipMovement.transform.position = _shipSpawnPosition.position;
        }

        private void GetShipComponents()
        {
            _shipLaserAttack = _shipMovement.GetComponent<ShipLaserAttack>();
            _shipMissilesAttack = _shipMovement.GetComponent<ShipMissilesAttack>();
            _shipCollision = _shipMovement.GetComponent<ShipCollision>();
        }

        private void PassDependencies()
        {
            _shipMovement.Init(_dataPersistenceHandler, _pauseSwitcher, _soundFactory);
            _shipMovement.ActivateObject();
            _shipLaserAttack.Init(_shipLaserConfig, _dataPersistenceHandler, _instantiator, _assetLoader);
            _shipMissilesAttack.Init(_missilesFactory);
            _mobileControls.Init(_pauseSwitcher);
            _pausePresenter.Init(_pauseView);
            _gameOverPresenter.Init(_gameOverView);
            _hudPresenter.Init(_hudView, _shipMovement);
            _missilesFactory.Init(_shipMovement);
            _playerInput.Init(_shipMovement);
            _gameOverModel.Init(_mobileControls, _shipMovement, _shipCollision);
            _analyticsService.Init(_shipMovement, _shipCollision, _gameSessionData, _weaponTrigger);
            _gameSessionData.Init(_shipLaserAttack);
            _weaponTrigger.Init(_shipLaserAttack, _shipMissilesAttack);
            _obstaclesFactory.Init(_shipMovement, _shipCollision);
            _leaderboardModel.Init(_shipCollision);
            _leaderboardPresenter.Init(_leaderboardView);
            _leaderboardView.Init(_assetLoader);
            _gameStateLoader.Init(_loadingOverlay);
        }

        private void UnloadGameAssets()
        {
            _missilesFactory.UnloadGameAssets();
            _obstaclesFactory.UnloadGameAssets();
            _leaderboardView.UnloadGameAssets();
            _soundFactory.UnloadAssets();
            _shipLaserAttack.UnloadAssets();
            _assetLoader.Unload(AssetsIDs.PLAYER_SHIP);
            _assetLoader.Unload(AssetsIDs.HUD);
            _assetLoader.Unload(AssetsIDs.MOBILE_CONTROLS);
            _assetLoader.Unload(AssetsIDs.PAUSE_MENU);
            _assetLoader.Unload(AssetsIDs.GAME_OVER_MENU);
            _assetLoader.Unload(AssetsIDs.LEADERBOARD_MENU);
            _loadingOverlay.DisableObject();
            _assetLoader.Unload(AssetsIDs.LOADING_OVERLAY);
        }
    }
}