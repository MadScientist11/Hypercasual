using Freya;
using UnityEngine;

namespace Hypercasual.AssemblyLine
{
    public class AssemblyLineEnd : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Vector3 _scale;
        private Box3D _end;

        private void Start()
        {
            _end = new Box3D()
            {
                center = _center.position,
                extents = _scale * 0.5f
            };
        }

        public bool Contains(Vector3 point)
        {
            return _end.Contains(point);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(_center.position, _scale);
        }
    }
}