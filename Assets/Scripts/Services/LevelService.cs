using System;

namespace Hypercasual.Services
{
    public interface ILevelService : IService
    {
        event Action<LevelInfo> LevelStatusChanged;
        event Action OnLevelCompleted;
        LevelInfo CurrentLevel { get; }
        void LoadNextLevel();
        void CheckFood(Item food);
    }

    public class LevelService : ILevelService
    {
        public event Action<LevelInfo> LevelStatusChanged;
        public event Action OnLevelCompleted;

        private int _currentLevelIndex;
        public LevelInfo CurrentLevel { get; private set; }

        private readonly IDataProvider _dataProvider;

        public LevelService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Initialize()
        {
        }


        public void LoadNextLevel()
        {
            AllLevels allLevels = _dataProvider.GetData<AllLevels>();
            Level levelData = allLevels[_currentLevelIndex];
            CurrentLevel = new LevelInfo()
            {
                Food = levelData.Food,
                FoodCount = levelData.FoodCount
            };
            _currentLevelIndex++;
        }

        public void CheckFood(Item food)
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