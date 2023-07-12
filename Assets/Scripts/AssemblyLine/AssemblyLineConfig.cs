using UnityEngine;

namespace Hypercasual.AssemblyLine
{
    [CreateAssetMenu(fileName = "AssemblyLineConfig", menuName = "Game/AssemblyLineConfig")]
    public class AssemblyLineConfig : ScriptableObject
    {
        public float AssemblyLineSpeed;
        public float FoodSpawnFrequency;
    }
}