using _Project.Scripts.GameFlow;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class MobileControls : MonoBehaviour, IPause
    {
        private PauseSwitcher _pauseSwitcher;

        private CanvasGroup _mobileButtonsCanvasGroup;

        private void Awake()
        {
            _mobileButtonsCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(PauseSwitcher pauseSwitcher)
        {
            _pauseSwitcher = pauseSwitcher;
            _pauseSwitcher.Add(this);
        }

        private void OnDisable()
        {
            _pauseSwitcher.Remove(this);
        }

        public void BlockButtons()
        {
            _mobileButtonsCanvasGroup.interactable = false;
            _mobileButtonsCanvasGroup.blocksRaycasts = false;
        }

        public void UnblockButtons()
        {
            _mobileButtonsCanvasGroup.interactable = true;
            _mobileButtonsCanvasGroup.blocksRaycasts = true;
        }

        public void Pause()
        {
            BlockButtons();
        }

        public void Unpause()
        {
            UnblockButtons();
        }
    }
}