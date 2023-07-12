using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hypercasual.UI
{
    public class MainScreen : BaseScreen
    {
        [SerializeField] private Button _startGameButton;
        private Action _startGameAction;

        public void Initialize(Action startGameAction)
        {
            _startGameAction = startGameAction;
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
        }

        private void StartGame() =>
            _startGameAction?.Invoke();
    }
}