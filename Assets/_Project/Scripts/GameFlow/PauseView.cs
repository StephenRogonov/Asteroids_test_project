using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Project.Scripts.GameFlow
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

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
            _scoreText.text = "Score: " + score;
        }
    }
}