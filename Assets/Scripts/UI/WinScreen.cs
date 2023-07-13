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
        private Action _toMenu;

        public void Initialize(Action loadNextLevel, Action toMenu)
        {
            _toMenu = toMenu;
            _loadNextLevel = loadNextLevel;
            _nextLevelButton.onClick.AddListener(LoadNextLevel);
            _menuButton.onClick.AddListener(ToMenu);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(LoadNextLevel);
            _menuButton.onClick.RemoveListener(ToMenu);
        }

        private void LoadNextLevel() =>
            _loadNextLevel?.Invoke();

        private void ToMenu() =>
            _toMenu?.Invoke();
    }
}