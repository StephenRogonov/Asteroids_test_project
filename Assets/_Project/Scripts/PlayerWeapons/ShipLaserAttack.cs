using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.Obstacles;
using _Project.Scripts.PlayerWeapons.Configs;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.PlayerWeapons
{
    public class ShipLaserAttack : MonoBehaviour
    {
        [SerializeField] private Transform _laserGun;

        private IAssetLoader _assetLoader;
        private IInstantiator _instantiator;
        private LaserBeam _laserBeamPrefab;
        private LaserBeam _laserBeam;
        private ShipLaserConfig _laserConfig;
        private GameConfig _gameConfig;
        private RaycastHit2D[] _obstaclesToDestroy;

        public event Action AsteroidDestroyed;
        public event Action EnemyDestroyed;

        public async void Init(
            ShipLaserConfig shipLaserConfig,
            DataPersistenceHandler dataPersistenceHandler,
            IInstantiator instantiator,
            IAssetLoader assetLoader
            )
        {
            _laserConfig = shipLaserConfig;
            _gameConfig = dataPersistenceHandler.GameConfig;
            _instantiator = instantiator;
            _assetLoader = assetLoader;

            _laserBeamPrefab = await _assetLoader.LoadPrefabByID<LaserBeam>(AssetsIDs.LASER_BEAM);
        }

        public void PerformShot()
        {
            StartCoroutine(DisplayLaserBeam());
            HitTargetsWithLaser();
        }

        public IEnumerator DisplayLaserBeam()
        {
            _laserBeam = _instantiator.InstantiatePrefabForComponent<LaserBeam>(_laserBeamPrefab);
            _laserBeam.transform.position = _laserGun.position;
            _laserBeam.transform.rotation = _laserGun.rotation;
            _laserBeam.gameObject.SetActive(true);
            yield return new WaitForSeconds(_gameConfig.LaserBeamLifetime);
            Destroy(_laserBeam.gameObject);
        }

        private void HitTargetsWithLaser()
        {
            _obstaclesToDestroy = Physics2D.RaycastAll(transform.position, transform.up, _gameConfig.LaserDistance, _laserConfig.LayersToDestroy);
            IDamageable damageable = null;

            foreach (RaycastHit2D obstacle in _obstaclesToDestroy)
            {
                damageable = obstacle.transform.GetComponent<IDamageable>();
                CountDestroyedObstacles(damageable);
                damageable.TakeHit(HitType.Laser);
            }
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

        public void UnloadAssets()
        {
            _assetLoader.Unload(AssetsIDs.LASER_BEAM);
        }
    }
}