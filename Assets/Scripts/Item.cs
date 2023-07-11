using System;
using UnityEngine;

namespace Hypercasual
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour, IPickable
    {
        public bool IsProcessed { get; set; } = false;

        private void Update()
        {
            transform.Translate( Time.deltaTime * 3 * Vector3.right);
        }
    }
}