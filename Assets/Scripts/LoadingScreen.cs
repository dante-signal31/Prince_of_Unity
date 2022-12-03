using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prince
{
    /// <summary>
    /// Component to update loading screen progress bar.
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to operate over loading screen UI.")] 
        [SerializeField] private UIDocument loadingScreenUI;
        
        private ProgressBar _progressBar;
        private VisualElement _container;
        
        public float ProgressBarMaxValue => _progressBar.highValue;

        private void OnEnable()
        {
            _progressBar = (ProgressBar)loadingScreenUI.rootVisualElement.Q(name: "LoadingProgress");
            _container = loadingScreenUI.rootVisualElement.Q(name: "Container");
        }

        private void Start()
        {
            _container.visible = false;
        }

        public void ShowLoadingScreen()
        {
            ResetProgressBar();
            _container.visible = true;
        }

        public void HideLoadingScreen()
        {
            _container.visible = false;
        }

        /// <summary>
        /// Set progress bar to zero.
        /// </summary>
        public void ResetProgressBar()
        {
            _progressBar.value = 0;
        }

        /// <summary>
        /// Set progress bar new value.
        /// </summary>
        /// <param name="value">New value for progress bar.</param>
        public void SetProgressBarValue(float value)
        {
            _progressBar.value = value;
        }
    }
}