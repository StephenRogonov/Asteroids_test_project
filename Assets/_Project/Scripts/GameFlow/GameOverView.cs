using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

namespace _Project.Scripts.GameFlow
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _scoreText;

        public event Action RestartClicked;
        public event Action ContinueClicked;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(Restart);
            _restartButton.onClick.AddListener(DisableObject);
            _continueButton.onClick.AddListener(Continue);
            _continueButton.onClick.AddListener(DisableObject);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
        }

        private void Restart()
        {
            RestartClicked?.Invoke();
        }

        private void Continue()
        {
            ContinueClicked?.Invoke();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = "Final score: " + score;
        }
    }
}
