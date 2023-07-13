using Hypercasual.Food;
using UnityEngine;

namespace Hypercasual.Level
{
    [CreateAssetMenu(menuName = "Game/Level", fileName = "Level_")]
    public class Level : ScriptableObject
    {
        public FoodType Food;
        public int FoodCount;
    }
}