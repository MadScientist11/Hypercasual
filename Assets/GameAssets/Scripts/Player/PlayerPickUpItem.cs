using System;
using System.Collections;
using DG.Tweening;
using Hypercasual.Food;
using Hypercasual.Services;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Hypercasual.Player
{
    public class PlayerPickUpItem : MonoBehaviour
    {
        [SerializeField] private PlayerReachZone playerReachZone;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform[] _handIKTargets;
        [SerializeField] private Transform _hand;
        [SerializeField] private Transform _fallInBasketPoint;

        private FoodView _currentFood;
        private ILevelService _levelService;
        private IGameFactory _gameFactory;

        private Quaternion _initialSpineRotation;

        [Inject]
        public void Construct(IGameFactory gameFactory, ILevelService levelService)
        {
            _gameFactory = gameFactory;
            _levelService = levelService;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.TryGetComponent<FoodView>(out var food))
                    {
                        if (playerReachZone.Contains(food.transform.position))
                            GrabItem(food);
                    }
                }
            }
        }

        private void GrabItem(FoodView food)
        {
            if (food.IsProcessed) return;
            food.SwitchState(FoodState.InThePlayerHand);

            SetIK(food);

            //Debug.Log("GRab Item");
//
            //food.transform.SetParent(_hand);
            //food.transform.localPosition = Vector3.zero;
//
            _currentFood = food;
            _playerAnimator.PlayGrabItemAnimation();
        }

        [UsedImplicitly]
        private void AnimationEventCallback_OnItemPlacedInBasket()
        {
            Debug.Log("EndANim");
            PlaceItemInBasket();
        }

        private void PlaceItemInBasket()
        {
            _currentFood.CachedTransform.position = _fallInBasketPoint.position;
            _currentFood.SwitchState(FoodState.FallInBasket);
            _levelService.CheckFood(_currentFood);
            _currentFood = null;
        }


        private IEnumerator TryReachItem(FoodView item)
        {
            if (item.IsProcessed) yield break;

            float distance = Vector3.Distance(_hand.position, item.transform.position);
            while (distance > 0.6f)
            {
                SetIK(item);
                yield return null;
            }

            item.IsProcessed = true;
            item.transform.SetParent(_hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            _playerAnimator.PlayGrabItemBackAnimation();
        }

        private void SetIK(FoodView item)
        {
            foreach (Transform handIKTarget in _handIKTargets)
            {
                handIKTarget.position = item.transform.position;
            }
        }
    }
}