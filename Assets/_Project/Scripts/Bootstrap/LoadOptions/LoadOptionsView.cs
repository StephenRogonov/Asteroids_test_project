using _Project.Scripts.AddressablesHandling;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Bootstrap.LoadOptions
{
    public class LoadOptionsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _localSaveDateText;
        [SerializeField] private TMP_Text _cloudSaveDateText;
        [SerializeField] private Button _localButton;
        [SerializeField] private Button _cloudButton;

        private IAssetLoader _assetLoader;

        public event Action LocalClicked;
        public event Action CloudClicked;

        public void Init(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        private void OnEnable()
        {
            _localButton.onClick.AddListener(LoadLocalSave);
            _cloudButton.onClick.AddListener(LoadCloudSave);
        }

        private void OnDisable()
        {
            _localButton.onClick.RemoveAllListeners();
            _cloudButton.onClick.RemoveAllListeners();
        }

        public void DisplayLocalSaveDate(string localSaveDate)
        {
            _localSaveDateText.text = "Local save: " + localSaveDate;
        }

        public void DisplayCloudSaveDate(string cloudSaveDate)
        {
            _cloudSaveDateText.text = "Cloud save: " + cloudSaveDate;
        }

        private void LoadLocalSave()
        {
            LocalClicked?.Invoke();
            DisableObject();
        }

        private void LoadCloudSave()
        {
            CloudClicked?.Invoke();
            DisableObject();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
            _assetLoader.Unload(this);
        }
    }
}