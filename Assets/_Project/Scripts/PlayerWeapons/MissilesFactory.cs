using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.Obstacles;
using _Project.Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.PlayerWeapons
{
    public class MissilesFactory
    {
        private IAssetLoader _assetLoader;
        private GameConfig _gameConfig;
        private Transform _shipShootingPoint;
        private Pool<Missile> _missilesPool;
        private Missile _missilePrefab;
        private IInstantiator _instantiator;

        public event Action AsteroidDestroyed;
        public event Action EnemyDestroyed;

        public MissilesFactory(
            IAssetLoader assetLoader,
            DataPersistenceHandler dataPersistenceHandler,
            IInstantiator instantiator
            )
        {
            _assetLoader = assetLoader;
            _gameConfig = dataPersistenceHandler.GameConfig;
            _instantiator = instantiator;

            CreatePool();
        }

        public void Init(ShipMovement shipMovement)
        {
            _shipShootingPoint = shipMovement.transform.GetComponentInChildren<ShootingPoint>().transform;
        }

        private async void CreatePool()
        {
            _missilePrefab = await _assetLoader.LoadAsset<Missile>(AssetsIDs.MISSILE);
            _missilesPool = _instantiator.Instantiate<Pool<Missile>>(new object[]
            { 
                _missilePrefab, _gameConfig.MissilesPoolInitialSize 
            });
            _assetLoader.UnloadAsset();
        }

        public Missile GetMissile()
        {
            Missile missile = _missilesPool.Get();
            missile.Destroyed -= _missilesPool.Return;
            missile.ObstacleHit -= CountDestroyedObstacles;
            missile.Destroyed += _missilesPool.Return;
            missile.ObstacleHit += CountDestroyedObstacles;

            missile.transform.position = _shipShootingPoint.position;
            missile.transform.rotation = _shipShootingPoint.rotation;
            missile.gameObject.SetActive(true);

            return missile;
        }

        private void CountDestroyedObstacles(IDamageable obstacle)
        {
            if (obstacle.ObstacleType == ObstacleType.Asteroid)
            {
                AsteroidDestroyed?.Invoke();
            }
            else if (obstacle.ObstacleType == ObstacleType.Enemy)
            {
                EnemyDestroyed?.Invoke();
            }
        }
    }
}