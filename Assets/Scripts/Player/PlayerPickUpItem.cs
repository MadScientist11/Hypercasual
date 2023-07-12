using System;
using System.Collections;
using Hypercasual.Services;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using VContainer;

namespace Hypercasual.Player
{
    public interface IPickable
    {
    }

    public class PlayerPickUpItem : MonoBehaviour
    {
        [SerializeField] private PlayerReachZone playerReachZone;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform _handIKTarget;
        [SerializeField] private Transform _hand;
        [SerializeField] private LayerMask _itemMask;
        [SerializeField] private Transform _fallInBasketPoint;

        private Item _currentFood;
        private ILevelService _levelService;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory, ILevelService levelService)
        {
            _gameFactory = gameFactory;
            _levelService = levelService;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100, _itemMask))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.TryGetComponent<Item>(out var item))
                    {
                        if (playerReachZone.Contains(item.transform.position))
                            GrabItem(item);
                    }
                }
            }
        }

        private void GrabItem(Item food)
        {
            if (food.IsProcessed) return;
            food.SwitchState(FoodState.InThePlayerHand);

            SetIK(food);
            Debug.Log("GRab Item");
            food.IsProcessed = true;
            food.transform.SetParent(_hand);
            food.transform.localPosition = Vector3.zero;
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


        private IEnumerator TryReachItem(Item item)
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

        private void SetIK(Item item)
        {
            _handIKTarget.position = item.transform.position;
        }
    }
}