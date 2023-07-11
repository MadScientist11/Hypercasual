using System;
using UnityEngine;

namespace Hypercasual
{
    public interface IPickable
    {
        
    }
    public class PlayerPickUpItem : MonoBehaviour
    {
        [SerializeField] private PlayerFrustum _playerFrustum;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform _handIKTarget;
        [SerializeField] private Transform _hand;
        public event Action OnItemGrabbed;

        private void OnEnable()
        {
            _playerFrustum.OnItemEnterPlayerFrustum += _ => _playerAnimator.PlayGrabItemAnimation();
            _playerFrustum.OnItemIsInsidePlayerFrustum += TryReachItem;
        }

        private void OnDisable()
        {
            _playerFrustum.OnItemIsInsidePlayerFrustum -= TryReachItem;
        }

        private void TryReachItem(Item item)
        {
            if(item.IsProcessed) return;
           
            SetIK(item);
            float distance = Vector3.Distance(_hand.position, item.transform.position);
            if (distance < 0.6f)
            {
                
                _playerAnimator.PlayGrabItemBackAnimation();
                item.IsProcessed = true;
            }
        }

        private void SetIK(Item item)
        {
            _handIKTarget.position = item.transform.position;
        }
    }
}
