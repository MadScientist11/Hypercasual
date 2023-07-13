using UnityEngine;

namespace Hypercasual.Food
{
    [CreateAssetMenu(menuName = "Game/Food", fileName = "Food")]
    public class Food : ScriptableObject
    {
        public FoodType Type;
        public GameObject Prefab;
    }
}