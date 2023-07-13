using System.Collections;
using System.Collections.Generic;
using Hypercasual;
using Hypercasual.Food;
using Hypercasual.Services;
using UnityEngine;

namespace Hypercasual.Level
{
    [CreateAssetMenu(menuName = "Game/AllLevels", fileName = "AllLevels")]
    public class AllLevels : ScriptableObject, IEnumerable<Level>, IData
    {
        [SerializeField] private Level[] _levelList;

        public Level this[int index]
        {
            get
            {
                if (index > _levelList.Length - 1)
                {
                    Level level = ScriptableObject.CreateInstance<Level>();
                    level.Food = EnumExtensions<FoodType>.Random;
                    level.FoodCount = Random.Range(1, 6);
                    return level;
                }

                return _levelList[index];
            }
        }

        public IEnumerator<Level> GetEnumerator()
        {
            return ((IEnumerable<Level>)_levelList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}