using UnityEngine;

namespace Hypercasual.Services
{
    [CreateAssetMenu(menuName = "Game/Food", fileName = "Food")]
    public class Food : ScriptableObject
    {
        public FoodType Type;
        public GameObject Prefab;
    }
}