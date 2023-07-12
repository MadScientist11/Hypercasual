using System.Collections;
using Hypercasual;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;
using VContainer;

namespace Hypercasual
{
    public class Conveyor : MonoBehaviour
    {
        public Transform _spawnPoint;

        private IGameFactory _gameFactory;


        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
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