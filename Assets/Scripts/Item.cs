using System;
using Hypercasual.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Hypercasual
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class Item : MonoBehaviour, IPickable
    {
        public bool IsProcessed { get; set; } = false;

        public Transform CachedTransform { get; private set; }

        public FoodType FoodType;
        private ObjectPool<Item> _objectPool;

        public void Initialize(ObjectPool<Item> objectPool)
        {
            _objectPool = objectPool;
        }

        private void Awake()
        {
            CachedTransform = transform;
        }

        public void Hide()
        {
            _objectPool.Release(this);
        }
    }
}