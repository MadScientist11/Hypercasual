using System.Collections;
using Hypercasual;
using UnityEngine;

namespace Hypercasual
{
    public class Conveyor : MonoBehaviour
    {
        public Transform _spawnPoint;

        private GameFactory _gameFactory;


        private void Start()
        {
            _gameFactory = new GameFactory(new AssetProvider());
            StartCoroutine(StartAssemblyLine());
        }


        private IEnumerator StartAssemblyLine()
        {
            while (true)
            {
                Item food = _gameFactory.GetOrCreateFood(FoodType.Apple, _spawnPoint.position);
                float yExtent = food.GetComponent<Collider>().bounds.extents.y;
                food.transform.position += new Vector3(0, yExtent, 0);
                yield return new WaitForSeconds(3);
            }
        }
    }

}


