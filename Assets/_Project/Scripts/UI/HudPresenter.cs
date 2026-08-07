using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Player;
using _Project.Scripts.PlayerWeapons;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
{
    public class HudPresenter : ITickable, IPause, IDisposable
    {
        private HudModel _model;
        private HudView _view;
        private GameConfig _gameConfig;
        private ShipMovement _shipMovement;
        private PauseSwitcher _pauseHandler;
        private GameSessionData _gameSessionData;
        private CountdownTimer _timer;
        private WeaponTrigger _weaponTrigger;

        private string _shipPosition;
        private int _shipRotation;
        private float _shipSpeed;

        private bool _isPaused;
        private bool _isInitialized;

        public HudPresenter(
            HudModel model, 
            DataPersistenceHandler dataPersistenceHandler,
            PauseSwitcher pauseHandler,
            GameSessionData gameSessionData,
            WeaponTrigger weaponTrigger
            )
        {
            _model = model;
            _gameConfig = dataPersistenceHandler.GameConfig;
            _pauseHandler = pauseHandler;
            _gameSessionData = gameSessionData;
            _weaponTrigger = weaponTrigger;
        }

        public void Init(HudView hudView, ShipMovement shipMovement)
        {
            _view = hudView;
            _shipMovement = shipMovement;

            _pauseHandler.Add(this);

            _timer = new CountdownTimer();
            _timer.Reset(_gameConfig.LaserShotRestorationTime);

            LaserShotsChanged(_gameConfig.LaserShotsStartCount);
            _weaponTrigger.LaserShot += LaserFired;

            _isInitialized = true;
        }

        public void Tick()
        {
            if (_isInitialized == false)
            {
                return;
            }

            if (_isPaused == false)
            {
                CalculateShipStats();

                _timer.Tick(Time.deltaTime);
                _view.DisplayLaserRestorationTime(TimeSpan.FromSeconds(_timer.RemainingTime).ToString("mm':'ss"));
                _view.DisplayScore(_gameSessionData.TotalScore);

                if (_timer.RemainingTime < 0)
                {
                    LaserShotsChanged(1);
                    _timer.Reset(_gameConfig.LaserShotRestorationTime);
                }
            }
        }

        private void LaserFired()
        {
            LaserShotsChanged(-1);
        }

        public void LaserShotsChanged(int iterator)
        {
            _model.ChangeLaserShotsCount(iterator);
            _view.DisplayLaserShotsCount(_model.LaserShotsCount);
        }

        public void CalculateShipStats()
        {
            _shipPosition = string.Format("{0}, {1}", Mathf.Round(_shipMovement.Position.x * 100f) / 100f,
                    Mathf.Round(_shipMovement.Position.y * 100f) / 100f);
            _shipRotation = Convert.ToInt32(_shipMovement.Rotation.z) % 360;
            _shipSpeed = Mathf.Round(Vector3.Magnitude(_shipMovement.Rigidbody.linearVelocity) * 100f) / 100f;

            _view.DisplayShipStats(_shipPosition, _shipRotation, _shipSpeed);
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
            _pauseHandler.Add(this);
            _weaponTrigger.LaserShot -= LaserFired;
        }
    }
}