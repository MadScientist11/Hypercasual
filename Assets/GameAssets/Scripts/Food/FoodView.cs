using System;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;

namespace Hypercasual.Food
{
    [RequireComponent(typeof(Rigidbody), typeof(Renderer))]
    public class FoodView : MonoBehaviour
    {
        public bool IsProcessed { get; set; } = false;

        public Transform CachedTransform { get; private set; }

        public FoodType FoodType;
        private ObjectPoolM<FoodView> _objectPool;

        private readonly float _inHandScaleFactor = 0.6f;
        private IGameFactory _gameFactory;

        private Vector3 _initialSize;
        private Quaternion _initialRotation;

        public void Initialize(ObjectPoolM<FoodView> objectPool, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _objectPool = objectPool;
        }

        private void Awake()
        {
            CachedTransform = transform;
            _initialSize = CachedTransform.localScale;
            _initialRotation = CachedTransform.rotation;
        }

        public void SwitchState(FoodState state)
        {
            switch (state)
            {
                case FoodState.OnTheAssemblyLine:
                    CachedTransform.rotation = _initialRotation;
                    CachedTransform.localScale = _initialSize;
                    GetComponent<Rigidbody>().isKinematic = true;
                    CachedTransform.SetParent(_gameFactory.FoodParent.transform);
                    IsProcessed = false;
                    break;
                case FoodState.InThePlayerHand:
                    IsProcessed = true;

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
            _objectPool.Release(this);
        }
    }
}