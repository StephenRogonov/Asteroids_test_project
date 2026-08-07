using _Project.Scripts.GameFlow;
using _Project.Scripts.Player;
using _Project.Scripts.Sounds;
using _Project.Scripts.UI;
using System;

namespace _Project.Scripts.PlayerWeapons
{
    public class WeaponTrigger : IPause, IDisposable
    {
        private HudModel _hudModel;
        private ShipLaserAttack _shipLaserAttack;
        private ShipMissilesAttack _shipMissilesAttack;
        private PauseSwitcher _pauseHandler;
        private SoundFactory _soundFactory;

        private bool _isPaused;

        public event Action MissileShot;
        public event Action LaserShot;

        public WeaponTrigger(
            HudModel hudModel,
            PauseSwitcher pauseHandler,
            SoundFactory soundFactory
            )
        {
            _hudModel = hudModel;
            _pauseHandler = pauseHandler;
            _soundFactory = soundFactory;

            _pauseHandler.Add(this);
        }

        public void Init(ShipLaserAttack shipLaserAttack, ShipMissilesAttack shipMissilesAttack)
        {
            _shipLaserAttack = shipLaserAttack;
            _shipMissilesAttack = shipMissilesAttack;
        }

        public void ShootMissile()
        {
            if (_isPaused == false)
            {
                MissileShot?.Invoke();
                _soundFactory.PlaySound(AudioID.MissileShot);
                _shipMissilesAttack.PerformShot();
            }
        }

        public void ShootLaser()
        {
            if (_hudModel.LaserShotsCount > 0 && _isPaused == false)
            {
                LaserShot?.Invoke();
                _soundFactory.PlaySound(AudioID.LaserShot);
                _shipLaserAttack.PerformShot();
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        public void Dispose()
        {
            _pauseHandler.Remove(this);
        }
    }
}