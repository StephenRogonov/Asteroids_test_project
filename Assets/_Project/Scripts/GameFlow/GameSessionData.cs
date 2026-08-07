using _Project.Scripts.PlayerWeapons;
using System;

namespace _Project.Scripts.GameFlow
{
    public class GameSessionData : IDisposable
    {
        private WeaponTrigger _weaponTrigger;
        private ShipLaserAttack _shipLaserAttack;
        private MissilesFactory _missilesFactory;

        public int TotalScore { get; set; }
        public int TotalMissilesShot { get; set; }
        public int TotalLaserShot { get; set; }
        public int TotalAsteroidsDestroyed { get; set; }
        public int TotalEnemiesDestroyed { get; set; }

        public GameSessionData(
            WeaponTrigger weaponTrigger,
            MissilesFactory missilesFactory
            )
        {
            _weaponTrigger = weaponTrigger;
            _missilesFactory = missilesFactory;
        }

        public void Init(
            ShipLaserAttack shipLaserAttack
            )
        {
            _shipLaserAttack = shipLaserAttack;
        }

        public void SubscribeToAnalyticsEvents()
        {
            _weaponTrigger.MissileShot += ShipMissileShot;
            _weaponTrigger.LaserShot += ShipLaserShot;
            _shipLaserAttack.AsteroidDestroyed += AsteroidDestroyed;
            _shipLaserAttack.EnemyDestroyed += EnemyDestroyed;
            _missilesFactory.AsteroidDestroyed += AsteroidDestroyed;
            _missilesFactory.EnemyDestroyed += EnemyDestroyed;
        }

        public void AddScore(int score)
        {
            TotalScore += score;
        }

        private void ShipMissileShot()
        {
            TotalMissilesShot++;
        }

        private void ShipLaserShot()
        {
            TotalLaserShot++;
        }

        private void AsteroidDestroyed()
        {
            TotalAsteroidsDestroyed++;
        }

        private void EnemyDestroyed()
        {
            TotalEnemiesDestroyed++;
        }

        public void Dispose()
        {
            _weaponTrigger.MissileShot -= ShipMissileShot;
            _weaponTrigger.LaserShot -= ShipLaserShot;
            _shipLaserAttack.AsteroidDestroyed -= AsteroidDestroyed;
            _shipLaserAttack.EnemyDestroyed -= EnemyDestroyed;
            _missilesFactory.AsteroidDestroyed -= AsteroidDestroyed;
            _missilesFactory.EnemyDestroyed -= EnemyDestroyed;
        }
    }
}
