using _Project.Scripts.Player;
using _Project.Scripts.PlayerWeapons;
using _Project.Scripts.GameFlow;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts.Bootstrap.Analytics
{
    public class AnalyticsService : IDisposable
    {
        private ShipMovement _shipMovement;
        private ShipCollision _shipCollision;
        private WeaponTrigger _weaponTrigger;
        private GameSessionData _gameSessionData;

        public void Init(
            ShipMovement shipMovement,
            ShipCollision shipCollision,
            GameSessionData gameSessionData,
            WeaponTrigger weaponTrigger
            )
        {
            _shipMovement = shipMovement;
            _shipCollision = shipCollision;
            _gameSessionData = gameSessionData;
            _weaponTrigger = weaponTrigger;

            SubscribeToAnalyticsEvents();
        }

        public void LogEventWithoutParameters(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        public void LogEventWithParameters(string eventName, Dictionary<string, int> parameters)
        {
            Parameter[] pars = new Parameter[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var item = parameters.ElementAt(i);
                pars[i] = new Parameter(item.Key, item.Value);
            }

            FirebaseAnalytics.LogEvent(eventName, pars);
        }

        public void LogEndGame()
        {
            Dictionary<string, int> parameters = new Dictionary<string, int>()
            {
                { LoggingEvents.MISSILES_TOTAL, _gameSessionData.TotalMissilesShot },
                { LoggingEvents.LASER_TOTAL, _gameSessionData.TotalLaserShot },
                { LoggingEvents.ASTEROIDS_DESTROYED, _gameSessionData.TotalAsteroidsDestroyed },
                { LoggingEvents.ENEMIES_DESTROYED, _gameSessionData.TotalEnemiesDestroyed }
            };

            LogEventWithParameters(LoggingEvents.END_GAME, parameters);
        }

        public void SubscribeToAnalyticsEvents()
        {
            _shipMovement.GameStarted += LogStartGame;
            _shipCollision.Crashed += LogEndGame;
            _weaponTrigger.LaserShot += ShipLaserShot;
        }

        public void Dispose()
        {
            _shipMovement.GameStarted -= LogStartGame;
            _shipCollision.Crashed -= LogEndGame;
            _weaponTrigger.LaserShot -= ShipLaserShot;
        }

        private void LogStartGame()
        {
            LogEventWithoutParameters(LoggingEvents.START_GAME);
        }

        private void ShipLaserShot()
        {
            LogEventWithoutParameters(LoggingEvents.LASER_SHOT);
        }
    }
}