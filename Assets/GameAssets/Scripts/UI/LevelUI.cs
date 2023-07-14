using Hypercasual.Level;
using Hypercasual.Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace Hypercasual.UI
{
    public class LevelUI : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI _objectiveText;
        [SerializeField] private TextMeshProUGUI _foodCountText;
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void OnEnable()
        {
            _objectiveText.text = $"Collect {_levelManager.CurrentLevel.FoodCount} {_levelManager.CurrentLevel.Food}s";
            UpdateCount();
            _levelManager.LevelStatusChanged += OnLevelStatusChanged;
        }

        private void OnDisable()
        {
            _levelManager.LevelStatusChanged -= OnLevelStatusChanged;
        }

        private void OnLevelStatusChanged(LevelInfo levelStatus)
        {
            UpdateCount();
        }

        private void UpdateCount()
        {
            _foodCountText.text = $"Remaining Count: {_levelManager.CurrentLevel.FoodCount}";
        }
    }
}