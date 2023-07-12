using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Hypercasual
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class Item : MonoBehaviour, IPickable
    {
        public FoodType FoodType;
        private ObjectPool<Item> _objectPool;
        private Renderer _renderer;
        public bool IsProcessed { get; set; } = false;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Initialize(ObjectPool<Item> objectPool)
        {
            _objectPool = objectPool;
        }

        public void Hide()
        {
            _objectPool.Release(this);
        }

        private void Update()
        {
            if (!_renderer.isVisible)
            {
                Debug.Log("release");
                _objectPool.Release(this);
            }
            
            if (!IsProcessed)
                transform.Translate(Time.deltaTime * .5f * Vector3.right);
        }
    }
}