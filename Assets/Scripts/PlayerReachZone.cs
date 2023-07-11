using System;
using Freya;
using UnityEngine;

namespace Hypercasual
{
    public class PlayerReachZone : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Vector3 _scale;
        private Box3D _reachZone;

        private void Start()
        {
            _reachZone = new Box3D()
            {
                center = _center.position,
                extents = _scale * 0.5f
            };
        }

        public bool Contains(Vector3 point)
        {
            return _reachZone.Contains(point);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(_center.position, _scale);
        }
    }
}