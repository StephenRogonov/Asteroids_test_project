using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameFlow
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        public event Action RestartClicked;
        public event Action ContinueClicked;
        public event Action ExitClicked;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(Restart);
            _continueButton.onClick.AddListener(Continue);
            _exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        private void Restart()
        {
            RestartClicked?.Invoke();
            DisableObject();
        }

        private void Continue()
        {
            ContinueClicked?.Invoke();
            DisableObject();
        }

        private void Exit()
        {
            ExitClicked?.Invoke();
            DisableObject();
        }

        public void EnableObject()
        {
            _canvasGroup.alpha = 0;
            _rectTransform.localScale = new Vector3(1f, 0f, 1f);
            gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(1f, 0.4f))
                .Join(_rectTransform.DOScale(Vector3.one, 0.4f)
                .SetEase(Ease.OutBack));
        }

        private void DisableObject()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(0f, 0.4f))
                .Join(_rectTransform.DOScale(new Vector3(1f, 0f, 1f), 0.2f)
                .SetEase(Ease.InBack))
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = "Final score: " + score;
        }
    }
}
