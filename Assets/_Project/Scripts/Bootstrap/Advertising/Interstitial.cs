using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.Bootstrap.Advertising
{
    public class Interstitial : IInterstitial, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private string _androidAdUnitId = "Interstitial_Android";
        private string _iOsAdUnitId = "Interstitial_iOS";

        private string _adUnitId;

        private Action OnAdShown;

        public Interstitial()
        {
            _adUnitId = Application.platform == RuntimePlatform.IPhonePlayer
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        public void ShowAd(Action onAdShown)
        {
            OnAdShown = onAdShown;
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            OnAdShown?.Invoke();
            OnAdShown = null;
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Interstitial Ad Loaded");
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log("Interstitial Ad Show Started");
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log("Interstitial Ad Clicked");
        }
    }
}