using System;

namespace _Project.Scripts.Bootstrap.Configs
{
    [Serializable]
    public class GameConfig
    {
        public int AsteroidScoreValue { get; set; }
        public int ShardScoreValue { get; set; }
        public int EnemyScoreValue { get; set; }
        public int AsteroidsPoolInitialSize { get; set; }
        public float AsteroidsSpawnRate { get; set; }
        public float AsteroidAngleOffset { get; set; }
        public int EnemiesPoolInitialSize { get; set; }
        public float EnemiesSpawnRate { get; set; }
        public float SpawnDistance { get; set; }
        public int DestructionParticlesPoolInitialSize { get; set; }
        public int AudioSourcesPoolInitialSize { get; set; }
        public int LaserShotsStartCount { get; set; }
        public float LaserShotRestorationTime { get; set; }
        public float LaserBeamLifetime { get; set; }
        public float LaserDistance { get; set; }
        public int MissilesPoolInitialSize { get; set; }
        public float ShipAcceleration { get; set; }
        public float ShipMaxSpeed { get; set; }
        public float ShipRotationSpeed { get; set; }
        public string AndroidGameId { get; set; }
        public string IosGameId { get; set; }
    }
}