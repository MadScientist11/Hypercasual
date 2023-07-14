using System;
using Hypercasual.Player;
using Hypercasual.Scopes;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
        private IGameFactory _gameFactory;

        private readonly float _inHandScaleFactor = 0.6f;
   

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
        //TODO: Refactor
        public void SwitchState(FoodState state)
        {
            switch (state)
            {
                case FoodState.OnTheAssemblyLine:
                    ResetScaleAndRotation();
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