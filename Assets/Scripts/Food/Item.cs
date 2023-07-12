using System;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;
using UnityEngine.Pool;

namespace Hypercasual
{
    public enum FoodState
    {
        OnTheAssemblyLine = 0,
        InThePlayerHand = 1,
        FallInBasket = 2,
    }

    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class Item : MonoBehaviour, IPickable
    {
        public bool IsProcessed { get; set; } = false;

        public Transform CachedTransform { get; private set; }

        public FoodType FoodType;
        private ObjectPool<Item> _objectPool;

        private readonly float _inHandScaleFactor = 0.1f;
        private IGameFactory _gameFactory;

        public void Initialize(ObjectPool<Item> objectPool, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _objectPool = objectPool;
        }

        private void Awake()
        {
            CachedTransform = transform;
        }

        public void SwitchState(FoodState state)
        {
            switch (state)
            {
                case FoodState.OnTheAssemblyLine:
                    CachedTransform.localScale = Vector3.one * 0.3f;
                    GetComponent<Rigidbody>().isKinematic = true;
                    CachedTransform.SetParent(_gameFactory.FoodParent.transform);
                    break;
                case FoodState.InThePlayerHand:
                    break;
                case FoodState.FallInBasket:
                    CachedTransform.localScale = Vector3.one * _inHandScaleFactor;
                    GetComponent<Rigidbody>().isKinematic = false;
                    CachedTransform.SetParent(null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }


        public void Hide()
        {
            if (IsProcessed)
                _objectPool.Dispose();
            _objectPool.Release(this);
        }
    }
}