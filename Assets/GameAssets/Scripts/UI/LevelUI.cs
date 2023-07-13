﻿using Hypercasual.Level;
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
        private ILevelService _levelService;

        [Inject]
        public void Construct(ILevelService levelService)
        {
            _levelService = levelService;
        }

        private void OnEnable()
        {
            _objectiveText.text = $"Collect {_levelService.CurrentLevel.FoodCount} {_levelService.CurrentLevel.Food}s";
            UpdateCount();
            _levelService.LevelStatusChanged += OnLevelStatusChanged;
        }

        private void OnDisable()
        {
            _levelService.LevelStatusChanged -= OnLevelStatusChanged;
        }

        private void OnLevelStatusChanged(LevelInfo levelStatus)
        {
            UpdateCount();
        }

        private void UpdateCount()
        {
            _foodCountText.text = $"Remaining Count: {_levelService.CurrentLevel.FoodCount}";
        }
    }
}