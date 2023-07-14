using UnityEngine;

namespace Hypercasual.Food
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class FoodView : MonoBehaviour
    {
        [field: SerializeField] public FoodType FoodType { get; private set; }

        public bool IsProcessed { get; set; } = false;
        public Transform CachedTransform { get; private set; }

        private Vector3 _initialSize;
        private Quaternion _initialRotation;
        private ObjectPoolM<FoodView> _objectPool;


        public void Initialize(ObjectPoolM<FoodView> objectPool)
        {
            _objectPool = objectPool;
        }

        private void Awake()
        {
            CachedTransform = transform;
            _initialSize = CachedTransform.localScale;
            _initialRotation = CachedTransform.rotation;
        }

        public void ResetScaleAndRotation()
        {
            CachedTransform.rotation = _initialRotation;
            CachedTransform.localScale = _initialSize;
        }

        public void Hide()
        {
            _objectPool.Release(this);
        }
    }
}