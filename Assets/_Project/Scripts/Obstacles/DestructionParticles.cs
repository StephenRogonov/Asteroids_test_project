using System;
using UnityEngine;

namespace _Project.Scripts.Obstacles
{
    public class DestructionParticles : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        public event Action<DestructionParticles> Destroyed;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Disable;
        }

        private void OnDisable()
        {
            Destroyed?.Invoke(this);
        }

        public void PlayParticleEffect()
        {
            _particleSystem.Play();
        }
    }
}