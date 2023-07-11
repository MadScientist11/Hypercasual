using System.Collections;
using UnityEngine;

namespace Hypercasual
{
    public class Conveyor : MonoBehaviour
    {
        public Item _itemPrefab;
        public Transform _spawnPoint;
        private void Start()
        {
            StartCoroutine(StartAssemblyLine());
        }


        private IEnumerator StartAssemblyLine()
        {
            float yExtent = _itemPrefab.GetComponent<Collider>().bounds.extents.y;

            while (true)
            {
                Item item = Instantiate(_itemPrefab, _spawnPoint.position + new Vector3(0,yExtent,0), Quaternion.identity);
                yield return new WaitForSeconds(8);
            }
        }
        
    }
}