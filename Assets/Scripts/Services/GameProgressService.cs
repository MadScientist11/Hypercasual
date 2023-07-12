using System;
using VContainer;

namespace Hypercasual.Services
{
    [Serializable]
    public class GameProgress
    {
        public int Level;
    }

    public class GameProgressService
    {
        private AllLevels _allLevels;

        [Inject]
        public void Construct(AllLevels allLevels)
        {
            _allLevels = allLevels;
        }

        public Level GetCurrentLevel()
        {
            return _allLevels[0];
        }
    }
}