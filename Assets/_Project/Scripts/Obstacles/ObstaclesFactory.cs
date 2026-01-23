using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.Obstacles.Asteroids;
using _Project.Scripts.Obstacles.Enemy;
using _Project.Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles
{
    public class ObstaclesFactory
    {
        private IAssetLoader _assetLoader;
        private GameConfig _gameConfig;
        private IInstantiator _instantiator;
        private ShipMovement _shipMovement;
        private Pool<Asteroid> _asteroidsPool;
        private Pool<EnemyMovement> _enemiesPool;
        private Asteroid _asteroidPrefab;
        private EnemyMovement _enemyPrefab;

        private List<Asteroid> _asteroidsSpawned = new();
        private List<EnemyMovement> _enemiesSpawned = new();

        public ObstaclesFactory(
            IAssetLoader assetLoader,
            DataPersistenceHandler dataPersistence,
            IInstantiator instantiator
            )
        {
            _assetLoader = assetLoader;
            _gameConfig = dataPersistence.GameConfig;
            _instantiator = instantiator;

            CreatePools();
        }

        public void Init(ShipMovement shipMovement)
        {
            _shipMovement = shipMovement;
        }

        public async void CreatePools()
        {
            _asteroidPrefab = await _assetLoader.LoadAsset<Asteroid>(AssetsIDs.ASTEROID);
            _asteroidsPool = _instantiator.Instantiate<Pool<Asteroid>>(new object[]
            {
                _asteroidPrefab, _gameConfig.AsteroidsPoolInitialSize
            });
            _assetLoader.UnloadAsset();

            _enemyPrefab = await _assetLoader.LoadAsset<EnemyMovement>(AssetsIDs.ENEMY);
            _enemiesPool = _instantiator.Instantiate<Pool<EnemyMovement>>(new object[] 
            { 
                _enemyPrefab, _gameConfig.EnemiesPoolInitialSize 
            });
            _assetLoader.UnloadAsset();
        }

        public void GetAsteroid()
        {
            Asteroid asteroid = _asteroidsPool.Get();

            if (_asteroidsSpawned.Contains(asteroid))
            {
                _asteroidsSpawned.Remove(asteroid);
            }

            _asteroidsSpawned.Add(asteroid);
            
            asteroid.Destroyed -= RemoveSpawnedAsteroid;
            asteroid.Destroyed += RemoveSpawnedAsteroid;

            asteroid.Destroyed -= _asteroidsPool.Return;
            asteroid.Destroyed += _asteroidsPool.Return;

            Vector3 spawnOffset = GetRandomSpawnPosition();
            float upDirectionAngleOffset = Random.Range(-_gameConfig.AsteroidAngleOffset, _gameConfig.AsteroidAngleOffset);
            Quaternion directionOffset = Quaternion.AngleAxis(upDirectionAngleOffset, Vector3.forward);
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -spawnOffset) * directionOffset;

            asteroid.transform.position = spawnOffset;
            asteroid.transform.rotation = rotation;
            asteroid.SetType(AsteroidType.Asteroid);
            asteroid.gameObject.SetActive(true);
            asteroid.Move();
        }

        public void GetEnemy()
        {
            EnemyMovement enemy = _enemiesPool.Get();

            _enemiesSpawned.Add(enemy);
            enemy.Destroyed -= RemoveSpawnedEnemy;
            enemy.Destroyed += RemoveSpawnedEnemy;

            enemy.Destroyed -= _enemiesPool.Return;
            enemy.Destroyed += _enemiesPool.Return;

            Vector3 spawnOffset = GetRandomSpawnPosition();
            enemy.transform.position = spawnOffset;
            enemy.Init(_shipMovement);
            enemy.gameObject.SetActive(true);
        }

        public void ReturnSpawnedToPool()
        {
            int spawnedAsteroidsCount = _asteroidsSpawned.Count;
            int spawnedEnemiesCount = _enemiesSpawned.Count;

            for (int i = 0; i < spawnedAsteroidsCount; i++)
            {
                _asteroidsSpawned[0].DestroyObject();
            }

            for (int i = 0; i < spawnedEnemiesCount; i++)
            {
                _enemiesSpawned[0].DestroyObject();
            }
        }

        private void RemoveSpawnedAsteroid(Asteroid asteroid)
        {
            _asteroidsSpawned.Remove(asteroid);
        }

        private void RemoveSpawnedEnemy(EnemyMovement enemy)
        {
            _enemiesSpawned.Remove(enemy);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return Random.insideUnitCircle.normalized * _gameConfig.SpawnDistance;
        }
    }
}