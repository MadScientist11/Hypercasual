using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hypercasual.UI
{
    public class WinScreen : BaseScreen
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _menuButton;
        private Action _loadNextLevel;

        public void Initialize(Action loadNextLevel)
        {
            _loadNextLevel = loadNextLevel;
            _nextLevelButton.onClick.AddListener(LoadNextLevel);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(LoadNextLevel);
        }

        private void LoadNextLevel() =>
            _loadNextLevel?.Invoke();
    }
}