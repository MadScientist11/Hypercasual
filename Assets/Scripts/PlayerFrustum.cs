using System;
using UnityEngine;

namespace Hypercasual
{
    public class PlayerFrustum : MonoBehaviour
    {
        public event Action<Item> OnItemIsInsidePlayerFrustum;
        public event Action<Item> OnItemEnterPlayerFrustum;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPickable _))
            {
                OnItemEnterPlayerFrustum?.Invoke(other.GetComponent<Item>());
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IPickable _))
            {
                OnItemIsInsidePlayerFrustum?.Invoke(other.GetComponent<Item>());
            }
        }
    }
}