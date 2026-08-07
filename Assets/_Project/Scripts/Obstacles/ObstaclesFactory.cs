using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.Obstacles.Asteroids;
using _Project.Scripts.Obstacles.Enemy;
using _Project.Scripts.Player;
using _Project.Scripts.Sounds;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles
{
    public class ObstaclesFactory : IDisposable
    {
        private IAssetLoader _assetLoader;
        private GameConfig _gameConfig;
        private IInstantiator _instantiator;
        private ShipMovement _shipMovement;
        private ShipCollision _shipCollision;
        private Pool<Asteroid> _asteroidsPool;
        private Pool<EnemyMovement> _enemiesPool;
        private Pool<DestructionParticles> _destructionParticlesPool;
        private Asteroid _asteroidPrefab;
        private EnemyMovement _enemyPrefab;
        private DestructionParticles _destructionParticlePrefab;
        private SoundFactory _soundFactory;

        private List<Asteroid> _asteroidsSpawned = new();
        private List<EnemyMovement> _enemiesSpawned = new();
        private List<DestructionParticles> _particlesSpawned = new();

        public ObstaclesFactory(
            IAssetLoader assetLoader,
            DataPersistenceHandler dataPersistence,
            IInstantiator instantiator,
            SoundFactory soundFactory
            )
        {
            _assetLoader = assetLoader;
            _gameConfig = dataPersistence.GameConfig;
            _instantiator = instantiator;
            _soundFactory = soundFactory;

            CreatePools();
        }

        public void Init(ShipMovement shipMovement, ShipCollision shipCollision)
        {
            _shipMovement = shipMovement;
            _shipCollision = shipCollision;
            _shipCollision.Crashed += ShowShipDestruction;
        }

        public void Dispose()
        {
            _shipCollision.Crashed -= ShowShipDestruction;
        }

        public async void CreatePools()
        {
            _asteroidPrefab = await _assetLoader.LoadPrefabByID<Asteroid>(AssetsIDs.ASTEROID);
            _asteroidsPool = _instantiator.Instantiate<Pool<Asteroid>>(new object[]
            {
                _asteroidPrefab, _gameConfig.AsteroidsPoolInitialSize
            });

            _enemyPrefab = await _assetLoader.LoadPrefabByID<EnemyMovement>(AssetsIDs.ENEMY);
            _enemiesPool = _instantiator.Instantiate<Pool<EnemyMovement>>(new object[] 
            { 
                _enemyPrefab, _gameConfig.EnemiesPoolInitialSize 
            });

            _destructionParticlePrefab = await _assetLoader.LoadPrefabByID<DestructionParticles>(AssetsIDs.DESTRUCTION_PARTICLES);
            _destructionParticlesPool = _instantiator.Instantiate<Pool<DestructionParticles>>(new object[]
            {
                _destructionParticlePrefab, _gameConfig.DestructionParticlesPoolInitialSize
            });
        }

        public void UnloadGameAssets()
        {
            _assetLoader.Unload(AssetsIDs.ASTEROID);
            _assetLoader.Unload(AssetsIDs.ENEMY);
            _assetLoader.Unload(AssetsIDs.DESTRUCTION_PARTICLES);
        }

        public void GetAsteroid()
        {
            Asteroid asteroid = _asteroidsPool.Get();

            if (_asteroidsSpawned.Contains(asteroid))
            {
                _asteroidsSpawned.Remove(asteroid);
            }

            _asteroidsSpawned.Add(asteroid);

            asteroid.Destroyed -= StartAsteroidDestructionSequence;
            asteroid.Destroyed += StartAsteroidDestructionSequence;

            asteroid.transform.localScale = new Vector2(1.5f, 1.5f);
            Vector3 spawnOffset = GetRandomSpawnPosition();
            float upDirectionAngleOffset = UnityEngine.Random.Range(-_gameConfig.AsteroidAngleOffset, _gameConfig.AsteroidAngleOffset);
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
            enemy.Destroyed -= StartEnemyDestructionSequence;
            enemy.Destroyed += StartEnemyDestructionSequence;

            Vector3 spawnOffset = GetRandomSpawnPosition();
            enemy.transform.position = spawnOffset;
            enemy.Init(_shipMovement);
            enemy.gameObject.SetActive(true);
        }

        private void StartAsteroidDestructionSequence(Asteroid asteroid, Vector3 destructionPosition, AsteroidType type)
        {
            if (type == AsteroidType.Asteroid)
            {
                for (int i = 0; i < 2; i++)
                {
                    SpawnAsteroidShards(destructionPosition);
                }
            }

            PlayDestructionParticle(destructionPosition);
            RemoveSpawnedAsteroid(asteroid);
            _asteroidsPool.Return(asteroid);
        }

        private void SpawnAsteroidShards(Vector3 destructionPosition)
        {
            Asteroid shard = _asteroidsPool.Get();

            if (_asteroidsSpawned.Contains(shard))
            {
                _asteroidsSpawned.Remove(shard);
            }

            _asteroidsSpawned.Add(shard);

            shard.Destroyed -= StartAsteroidDestructionSequence;
            shard.Destroyed += StartAsteroidDestructionSequence;

            shard.transform.position = destructionPosition;
            shard.SetType(AsteroidType.Shard);
            shard.transform.localScale = new Vector2(0.75f, 0.75f);
            shard.gameObject.SetActive(true);
            shard.Move();
        }

        private void StartEnemyDestructionSequence(EnemyMovement enemy, Vector3 destructionPosition)
        {
            PlayDestructionParticle(destructionPosition);
            RemoveSpawnedEnemy(enemy);
            _enemiesPool.Return(enemy);
        }

        private void ShowShipDestruction()
        {
            PlayDestructionParticle(_shipMovement.transform.position);
        }

        private void PlayDestructionParticle(Vector3 position)
        {
            DestructionParticles particle = _destructionParticlesPool.Get();
            _particlesSpawned.Add(particle);
            particle.Destroyed -= ReturnParticlesToPool;
            particle.Destroyed += ReturnParticlesToPool;
            particle.transform.position = position;
            particle.gameObject.SetActive(true);
            _soundFactory.PlaySound(AudioID.Explosion);
        }

        private void ReturnParticlesToPool(DestructionParticles particles)
        {
            RemoveSpawnedParticle(particles);
            _destructionParticlesPool.Return(particles);
        }

        public void ReturnSpawnedToPool()
        {
            int spawnedAsteroidsCount = _asteroidsSpawned.Count;
            int spawnedEnemiesCount = _enemiesSpawned.Count;

            for (int i = 0; i < spawnedAsteroidsCount; i++)
            {
                _asteroidsSpawned[0].SetType(AsteroidType.Shard);
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

        private void RemoveSpawnedParticle(DestructionParticles particles)
        {
            _particlesSpawned.Remove(particles);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return UnityEngine.Random.insideUnitCircle.normalized * _gameConfig.SpawnDistance;
        }
    }
}