using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.Player;
using _Project.Scripts.PlayerWeapons;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Obstacles.Enemy
{
    public class EnemyMovement : MonoBehaviour, IDamageable, IPause, IScore
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;

        private PauseSwitcher _pauseHandler;
        private GameSessionData _gameSessionData;
        private GameConfig _gameConfig;

        private Rigidbody2D _rigidbody;
        private Transform _player;
        private Vector2 _playerDirection;
        
        private int _scoreValue;
        private bool _isPaused;

        public ObstacleType ObstacleType { get; set; }

        public event Action<EnemyMovement, Vector3> Destroyed;

        [Inject]
        private void Construct(
            PauseSwitcher pauseHandler,
            GameSessionData gameSessionData,
            DataPersistenceHandler dataPersistenceHandler
            )
        {
            _pauseHandler = pauseHandler;
            _gameSessionData = gameSessionData;
            _gameConfig = dataPersistenceHandler.GameConfig;
        }

        public void Init(ShipMovement shipMovement)
        {
            _player = shipMovement.transform;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            ObstacleType = ObstacleType.Enemy;
            _scoreValue = _gameConfig.EnemyScoreValue;
        }

        private void OnEnable()
        {
            _pauseHandler.Add(this);
        }

        private void OnDisable()
        {
            _pauseHandler.Remove(this);
        }

        private void Update()
        {
            GetPlayerDirection();
        }

        private void FixedUpdate()
        {
            RotateTowardsPlayer();

            if (_isPaused == false)
            {
                SetVelocity();
            }
        }

        private void GetPlayerDirection()
        {
            _playerDirection = (_player.position - transform.position).normalized;
        }

        private void RotateTowardsPlayer()
        {
            if (_playerDirection == Vector2.zero)
            {
                return;
            }

            Quaternion playerRotation = Quaternion.LookRotation(transform.forward, _playerDirection);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, _rotationSpeed * Time.deltaTime);

            _rigidbody.SetRotation(rotation);
        }

        private void SetVelocity()
        {
            if (_playerDirection == Vector2.zero)
            {
                _rigidbody.linearVelocity = Vector2.zero;
            }
            else
            {
                _rigidbody.linearVelocity = transform.up * _speed;
            }
        }

        public void TakeHit(HitType hitType)
        {
            if (hitType != HitType.Ship)
            {
                Score();
            }

            DestroyObject();
        }

        public void DestroyObject()
        {
            Destroyed?.Invoke(this, gameObject.transform.position);
            gameObject.SetActive(false);
        }

        public void Pause()
        {
            _isPaused = true;
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }

        public void Unpause()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _isPaused = false;
        }

        public void Score()
        {
            _gameSessionData.AddScore(_scoreValue);
        }
    }
}