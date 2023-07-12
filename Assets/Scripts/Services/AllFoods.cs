using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hypercasual.Services
{
    [CreateAssetMenu(menuName = "StaticData/AllFoods", fileName = "AllFoods")]
    public class AllFoods : ScriptableObject, IEnumerable<Food>, IStaticData
    {
        [SerializeField] private Food[] _foodList;
        public Food this[int index] => _foodList[index];

        public Food GetFood(FoodType type)
        {
            foreach (Food food in _foodList)
            {
                if (food.Type == type)
                    return food;
            }

            return null;
        }

        public IEnumerator<Food> GetEnumerator()
        {
            return ((IEnumerable<Food>)_foodList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}