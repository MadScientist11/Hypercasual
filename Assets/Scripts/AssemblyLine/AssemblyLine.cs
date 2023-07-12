using System;
using System.Collections;
using System.Collections.Generic;
using Freya;
using Hypercasual;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;
using VContainer;

namespace Hypercasual.AssemblyLine
{
    public class AssemblyLine : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private AssemblyLineConfig _assemblyLineConfig;
        [SerializeField] private AssemblyLineEnd _assemblyLineEnd;

        private IGameFactory _gameFactory;
        private HashSet<Item> _foodOnTheLine;
        private HashSet<Item> _foodToPool;

        private Coroutine _assemblyLineCoroutine;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _foodOnTheLine = new HashSet<Item>();
            _foodToPool = new HashSet<Item>();
            RestartAssemblyLine();
        }

        private void Update()
        {
            _foodToPool.Clear();
            foreach (Item food in _foodOnTheLine)
            {
                if (_assemblyLineEnd.Contains(food.CachedTransform.position))
                    _foodToPool.Add(food);

                MoveThroughAssemblyLine(food);
            }

            foreach (Item food in _foodToPool)
            {
                ReturnToPool(food);
                _foodOnTheLine.Remove(food);
            }
        }

        public void RestartAssemblyLine()
        {
            foreach (Item food in _foodOnTheLine)
            {
                ReturnToPool(food);
            }

            _foodToPool.Clear();
            _foodOnTheLine.Clear();

            if (_assemblyLineCoroutine != null)
                StopCoroutine(_assemblyLineCoroutine);
            
            _assemblyLineCoroutine = StartCoroutine(StartAssemblyLine());
        }

        private void ReturnToPool(Item food) =>
            food.Hide();

        private void MoveThroughAssemblyLine(Item food)
        {
            if (!food.IsProcessed)
                food.CachedTransform.Translate(Time.deltaTime * _assemblyLineConfig.AssemblyLineSpeed * Vector3.right);
        }

        private IEnumerator StartAssemblyLine()
        {
            WaitForSeconds assemblySpawnWait = new WaitForSeconds(_assemblyLineConfig.FoodSpawnFrequency);
            while (true)
            {
                Item food = SpawnFood();
                _foodOnTheLine.Add(food);
                yield return assemblySpawnWait;
            }
        }

        private Item SpawnFood()
        {
            Item food = _gameFactory.GetOrCreateFood(FoodType.Apple, _spawnPoint.position);
            float yExtent = food.GetComponent<Collider>().bounds.extents.y;
            food.transform.position += new Vector3(0, yExtent, 0);
            return food;
        }
    }
}