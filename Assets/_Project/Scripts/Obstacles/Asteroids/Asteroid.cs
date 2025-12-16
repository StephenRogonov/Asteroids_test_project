using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.PlayerWeapons;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles.Asteroids
{
    public class Asteroid : MonoBehaviour, IDamageable, IPause, IScore
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _shardSize = 0.75f;
        [SerializeField] private int _shardsAmount = 2;
        [SerializeField] private float _destroyDistance = 25f;

        private PauseSwitcher _pauseHandler;
        private ScoreCounter _scoreCounter;
        private GameConfig _gameConfig;

        private Rigidbody2D _rigidbody;
        private AsteroidType _type;
        private ObstacleType _obstacleType;
        private int _scoreValue;
        private int _asteroidScoreValue;
        private int _shardScoreValue;

        private Vector2 _startPosition;
        private Vector2 _currentPosition;
        private float _distancePassed;

        private Vector2 _linearVelocity;

        private IInstantiator _instantiator;

        public ObstacleType ObstacleType => _obstacleType;

        public event Action<Asteroid> Destroyed;

        [Inject]
        private void Construct(
            PauseSwitcher pauseHandler, 
            ScoreCounter scoreCounter, 
            IInstantiator instantiator,
            DataPersistenceHandler dataPersistenceHandler
            )
        {
            _pauseHandler = pauseHandler;
            _scoreCounter = scoreCounter;
            _instantiator = instantiator;
            _gameConfig = dataPersistenceHandler.GameConfig;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _obstacleType = ObstacleType.Asteroid;
            _asteroidScoreValue = _gameConfig.AsteroidScoreValue;
            _shardScoreValue = _gameConfig.ShardScoreValue;
        }

        private void OnEnable()
        {
            _startPosition = gameObject.transform.position;
            _pauseHandler.Add(this);
        }

        private void OnDisable()
        {
            _pauseHandler.Remove(this);
        }

        private void Update()
        {
            _currentPosition = gameObject.transform.position;
            _distancePassed = Vector2.Distance(_startPosition, _currentPosition);

            if (_distancePassed > _destroyDistance)
            {
                DestroyObject();
            }
        }

        public void SetType(AsteroidType type)
        {
            _type = type;
        }

        public void Move()
        {
            if (_type == AsteroidType.Asteroid)
            {
                _rigidbody.AddForce(transform.up * _speed);
            }
            else
            {
                _rigidbody.AddForce(UnityEngine.Random.insideUnitCircle.normalized * _speed);
            }
        }

        public void TakeHit(HitType hitType)
        {
            if (_type == AsteroidType.Asteroid)
            {
                _scoreValue = _asteroidScoreValue;

                if (hitType == HitType.Missile)
                {
                    for (int i = 0; i < _shardsAmount; i++)
                    {
                        CreateShard();
                    }
                }
            }
            else if (_type == AsteroidType.Shard)
            {
                _scoreValue = _shardScoreValue;
            }

            if (hitType != HitType.Ship)
            {
            Score();
            }

            DestroyObject();
        }

        public void DestroyObject()
        {
            if (_type == AsteroidType.Shard)
            {
                Destroy(gameObject);
            }
            else if (_type == AsteroidType.Asteroid)
            {
                gameObject.SetActive(false);
                Destroyed?.Invoke(this);
            }
        }

        private void CreateShard()
        {
            Vector2 position = transform.position;
            position += UnityEngine.Random.insideUnitCircle * 0.5f;

            Asteroid shard = _instantiator.InstantiatePrefabForComponent<Asteroid>(this);
            shard.SetType(AsteroidType.Shard);
            shard.Move();
            shard.transform.localScale = new Vector2(_shardSize, _shardSize);
        }

        public void Pause()
        {
            _linearVelocity = _rigidbody.linearVelocity;
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }

        public void Unpause()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.linearVelocity = _linearVelocity;
        }

        public void Score()
        {
            _scoreCounter.AddScore(_scoreValue);
        }
    }
}