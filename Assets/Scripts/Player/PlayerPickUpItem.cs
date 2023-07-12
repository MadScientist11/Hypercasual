using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

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
        public event Action OnItemGrabbed;
        private Item _currentItem;

        private void GrabItem(Item item)
        {
            if (item.IsProcessed) return;
            SetIK(item);
            Debug.Log("GRab Item");
            item.IsProcessed = true;
            item.transform.SetParent(_hand);
            item.transform.localPosition = Vector3.zero;
            _currentItem = item;
            _playerAnimator.PlayGrabItemAnimation();
        }

        private void AnimationEventCallback_OnItemPlacedInBasket()
        {
            Debug.Log("EndANim");
            PlaceItemInBasket();
        }

        private void PlaceItemInBasket()
        {
            Transform itemTransform = _currentItem.transform;
            itemTransform.SetParent(null);
            itemTransform.position = _fallInBasketPoint.position;
            itemTransform.localScale = Vector3.one * 0.1f;
            itemTransform.GetComponent<Rigidbody>().isKinematic = false;
            _currentItem = null;
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