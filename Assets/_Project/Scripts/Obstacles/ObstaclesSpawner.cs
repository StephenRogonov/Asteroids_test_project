using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles
{
    public class ObstaclesSpawner : MonoBehaviour, IPause, IInitializable, IDisposable
    {
        private GameConfig _gameConfig;
        private ObstaclesFactory _obstaclesFactory;
        private PauseSwitcher _pauseHandler;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isPaused;

        [Inject]
        private void Construct(DataPersistenceHandler dataPersistenceHandler, ObstaclesFactory obstaclesFactory, PauseSwitcher pauseHandler)
        {
            _gameConfig = dataPersistenceHandler.GameConfig;
            _obstaclesFactory = obstaclesFactory;
            _pauseHandler = pauseHandler;
        }

        public void Initialize()
        {
            _pauseHandler.Add(this);
            _cancellationTokenSource = new CancellationTokenSource();
            StartSpawning();
        }

        private void StartSpawning()
        {
            AsteroidsSpawningStart(_cancellationTokenSource.Token);
            EnemiesSpawningStart(_cancellationTokenSource.Token);
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        private async UniTask AsteroidsSpawningStart(CancellationToken cancellationToken)
        {
            int delay = (int)_gameConfig.AsteroidsSpawnRate * 1000;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await UniTask.Delay(delay, cancellationToken: cancellationToken);

                if (_isPaused == false)
                {
                    _obstaclesFactory.GetAsteroid();
                }
            }
        }

        private async UniTask EnemiesSpawningStart(CancellationToken cancellationToken)
        {
            int delay = (int)_gameConfig.EnemiesSpawnRate * 1000;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await UniTask.Delay(delay, cancellationToken: cancellationToken);

                if (_isPaused == false)
                {
                    _obstaclesFactory.GetEnemy();
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _pauseHandler.Remove(this);
        }
    }
}