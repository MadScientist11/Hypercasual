using System;
using System.Collections;
using System.Collections.Generic;
using Hypercasual.Food;
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
        private HashSet<FoodView> _foodOnTheLine;
        private HashSet<FoodView> _foodToPool;

        private Coroutine _assemblyLineCoroutine;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Awake()
        {
            _foodOnTheLine = new HashSet<FoodView>();
            _foodToPool = new HashSet<FoodView>();
        }

        private void Update()
        {
            foreach (FoodView food in _foodOnTheLine)
            {
                MoveThroughAssemblyLine(food);
            }

            ReturnToPool(food => _assemblyLineEnd.Contains(food.CachedTransform.position));
        }

        private void ReturnToPool(Func<FoodView, bool> predicate)
        {
            foreach (FoodView food in _foodOnTheLine)
            {
                if (predicate.Invoke(food))
                    _foodToPool.Add(food);
            }

            foreach (FoodView food in _foodToPool)
            {
                food.Hide();
                _foodOnTheLine.Remove(food);
            }

            _foodToPool.Clear();
        }

        public void SwitchState(AssemblyLineState state)
        {
            switch (state)
            {
                case AssemblyLineState.MainMenu:
                    Activate();
                    RestartAssemblyLine();
                    break;
                case AssemblyLineState.GameStart:
                    RestartAssemblyLine();
                    break;
                case AssemblyLineState.LevelCompleted:
                    ReturnToPool(food => !food.IsProcessed);
                    Deactivate();
                    break;
            }
        }

        private void Activate() => gameObject.SetActive(true);

        private void Deactivate() => gameObject.SetActive(false);

        private void RestartAssemblyLine()
        {
            ClearAssemblyLine();

            if (_assemblyLineCoroutine != null)
                StopCoroutine(_assemblyLineCoroutine);

            _assemblyLineCoroutine = StartCoroutine(StartAssemblyLine());
        }

        private IEnumerator StartAssemblyLine()
        {
            WaitForSeconds assemblySpawnWait = new WaitForSeconds(_assemblyLineConfig.FoodSpawnFrequency);
            while (true)
            {
                FoodView food = SpawnFood();
                food.SwitchState(FoodState.OnTheAssemblyLine);
                _foodOnTheLine.Add(food);
                yield return assemblySpawnWait;
            }
        }

        private void ClearAssemblyLine()
        {
            ReturnToPool(_ => true);
            _foodToPool.Clear();
            _foodOnTheLine.Clear();
        }

        private void MoveThroughAssemblyLine(FoodView food)
        {
            if (!food.IsProcessed)
                food.CachedTransform.Translate(Time.deltaTime * _assemblyLineConfig.AssemblyLineSpeed * Vector3.right);
        }

        private FoodView SpawnFood()
        {
            FoodView food = _gameFactory.GetOrCreateFood(EnumExtensions<FoodType>.Random, _spawnPoint.position);
            float yExtent = food.GetComponent<Collider>().bounds.extents.y;
            food.transform.position += new Vector3(0, yExtent, 0);
            return food;
        }
    }
}