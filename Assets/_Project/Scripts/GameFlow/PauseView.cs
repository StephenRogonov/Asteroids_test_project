using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameFlow
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _scoreText;

        public event Action ContinueClicked;
        public event Action ExitClicked;

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Continue);
            _continueButton.onClick.AddListener(DisableObject);
            _exitButton.onClick.AddListener(Exit);
            _exitButton.onClick.AddListener(DisableObject);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        private void Continue()
        {
            ContinueClicked?.Invoke();
        }

        private void Exit()
        {
            ExitClicked?.Invoke();
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
            _scoreText.text = "Score: " + score;
        }
    }
}