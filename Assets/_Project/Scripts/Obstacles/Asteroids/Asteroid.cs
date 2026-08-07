using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.PlayerWeapons;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles.Asteroids
{
    public class Asteroid : MonoBehaviour, IDamageable, IPause, IScore
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _destroyDistance = 25f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
        [SerializeField] private Transform _spriteTransform;

        private PauseSwitcher _pauseSwitcher;
        private GameSessionData _gameSessionData;
        private GameConfig _gameConfig;

        private Rigidbody2D _rigidbody;
        private AsteroidType _type;
        private int _scoreValue;
        private int _asteroidScoreValue;
        private int _shardScoreValue;
        private Vector2 _startPosition;
        private Vector2 _currentPosition;
        private float _distancePassed;
        private Vector2 _linearVelocity;
        private int _spritesCount;

        public ObstacleType ObstacleType { get; set; }

        public event Action<Asteroid, Vector3, AsteroidType> Destroyed;

        [Inject]
        private void Construct(
            PauseSwitcher pauseSwitcher,
            GameSessionData gameSessionData,
            DataPersistenceHandler dataPersistenceHandler
            )
        {
            _pauseSwitcher = pauseSwitcher;
            _gameSessionData = gameSessionData;
            _gameConfig = dataPersistenceHandler.GameConfig;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            ObstacleType = ObstacleType.Asteroid;
            _asteroidScoreValue = _gameConfig.AsteroidScoreValue;
            _shardScoreValue = _gameConfig.ShardScoreValue;
            _spritesCount = _sprites.Count - 1;
        }

        private void OnEnable()
        {
            _spriteRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _spritesCount)];
            _spriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-180, 180)));
            _startPosition = gameObject.transform.position;
            _pauseSwitcher.Add(this);
        }

        private void OnDisable()
        {
            _pauseSwitcher.Remove(this);
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

                if (hitType == HitType.Laser)
                {
                    SetType(AsteroidType.Shard);
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
            gameObject.SetActive(false);
            Destroyed?.Invoke(this, gameObject.transform.position, _type);
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
            _gameSessionData.AddScore(_scoreValue);
        }
    }
}