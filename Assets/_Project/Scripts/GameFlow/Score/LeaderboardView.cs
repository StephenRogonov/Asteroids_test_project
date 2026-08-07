using _Project.Scripts.AddressablesHandling;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Obstacles.Score
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Transform _leaderboardTransform;
        [SerializeField] private Button _continueButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        private IAssetLoader _assetLoader;
        private LeaderboardEntryView _leaderboardEntryPrefab;

        public event Action ContinueClicked;

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Continue);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveAllListeners();
        }

        public async void Init(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
            _leaderboardEntryPrefab = await _assetLoader.LoadPrefabByID<LeaderboardEntryView>(AssetsIDs.LEADERBOARD_SCORE_ENTRY);
        }

        public void UnloadGameAssets()
        {
            _assetLoader.Unload(AssetsIDs.LEADERBOARD_SCORE_ENTRY);
        }

        public void UpdateLeaderboard(List<ScoreEntry> leaderboard)
        {
            for (int i = _leaderboardTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(_leaderboardTransform.GetChild(i).gameObject);
            }

            foreach (ScoreEntry scoreEntry in leaderboard)
            {
                Instantiate(_leaderboardEntryPrefab, _leaderboardTransform).FillScoreEntryData(scoreEntry);
            }
        }

        private void Continue()
        {
            ContinueClicked?.Invoke();
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
    }
}
