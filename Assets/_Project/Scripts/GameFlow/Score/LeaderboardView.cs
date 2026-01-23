using _Project.Scripts.AddressablesHandling;
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
            _leaderboardEntryPrefab = await _assetLoader.LoadAsset<LeaderboardEntryView>(AssetsIDs.LEADERBOARD_SCORE_ENTRY);
            _assetLoader.UnloadAsset();
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
            gameObject.SetActive(true);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}
