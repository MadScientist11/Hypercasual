using System;
using Hypercasual.Food;
using Hypercasual.Level;
using UnityEngine;

namespace Hypercasual.Services
{
    public interface ILevelManager : IService
    {
        event Action<LevelInfo> LevelStatusChanged;
        event Action OnLevelCompleted;
        LevelInfo CurrentLevel { get; }
        void LoadNextLevel();
        void CheckFood(FoodView food);
    }

    public class LevelManager : ILevelManager
    {
        public LevelInfo CurrentLevel { get; private set; }
        public event Action<LevelInfo> LevelStatusChanged;
        public event Action OnLevelCompleted;

        private int _currentLevelIndex;
        private readonly IDataProvider _dataProvider;

        public LevelManager(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Initialize()
        {
        }


        public void LoadNextLevel()
        {
            AllLevels allLevels = _dataProvider.GetData<AllLevels>();
            
            Debug.Log(_currentLevelIndex);
            Level.Level levelData = allLevels[_currentLevelIndex];
            CurrentLevel = new LevelInfo()
            {
                Food = levelData.Food,
                FoodCount = levelData.FoodCount
            };
            _currentLevelIndex++;
        }

        public void CheckFood(FoodView food)
        {
            if (CurrentLevel.Food == food.FoodType)
            {
                CurrentLevel.FoodCount--;
                LevelStatusChanged?.Invoke(CurrentLevel);
            }

            if (CurrentLevel.FoodCount == 0)
                OnLevelCompleted?.Invoke();
        }
    }
}