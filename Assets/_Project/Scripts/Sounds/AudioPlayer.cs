using _Project.Scripts.GameFlow;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Sounds
{
    public class AudioPlayer : MonoBehaviour, IPause
    {
        private PauseSwitcher _pauseSwitcher;
        private AudioSource _audioSource;
        private bool _pausable;

        public event Action<AudioPlayer> Finished;

        [Inject]
        private void Construct(PauseSwitcher pauseSwitcher)
        {
            _pauseSwitcher = pauseSwitcher;
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (_pausable)
            {
                _pauseSwitcher.Add(this);
            }
        }

        private void OnDisable()
        {
            if (_pausable)
            {
                _pauseSwitcher.Remove(this);
            }
        }

        public void SetParameters(AudioClipDataSO audioClipSO)
        {
            _audioSource.clip = audioClipSO.LoadedClip;
            _audioSource.volume = audioClipSO.Volume;
            _audioSource.loop = audioClipSO.Loop;
            _audioSource.pitch = audioClipSO.Pitch;
            _pausable = audioClipSO.Pausable;
        }

        public void Play()
        {
            _audioSource.Play();
        }

        public void StopPlayback()
        {
            _audioSource.Stop();
            Finished?.Invoke(this);
            DisableObject();
        }

        public async void PlayOnce()
        {
            _audioSource.Play();

            while (_audioSource.isPlaying)
            {
                await UniTask.Yield();
            }

            Finished?.Invoke(this);
            DisableObject();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        public void DisableObject()
        {
            gameObject.SetActive(false);
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Unpause()
        {
            _audioSource.UnPause();
        }
    }
}