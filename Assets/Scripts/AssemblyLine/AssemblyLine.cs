using System;
using System.Collections;
using System.Collections.Generic;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Hypercasual.AssemblyLine
{
    public enum AssemblyLineState
    {
        MainMenu = 0,
        GameStart = 1,
        LevelCompleted = 2,
    }

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

        private void Awake()
        {
            _foodOnTheLine = new HashSet<Item>();
            _foodToPool = new HashSet<Item>();
        }

        private void Update()
        {
            foreach (Item food in _foodOnTheLine)
            {
                MoveThroughAssemblyLine(food);
            }

            ReturnToPool(food => _assemblyLineEnd.Contains(food.CachedTransform.position));
        }

        private void ReturnToPool(Func<Item, bool> predicate)
        {
            foreach (Item food in _foodOnTheLine)
            {
                if (predicate.Invoke(food))
                    _foodToPool.Add(food);
            }

            foreach (Item food in _foodToPool)
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
                Item food = SpawnFood();
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

        private void MoveThroughAssemblyLine(Item food)
        {
            if (!food.IsProcessed)
                food.CachedTransform.Translate(Time.deltaTime * _assemblyLineConfig.AssemblyLineSpeed * Vector3.right);
        }

        private Item SpawnFood()
        {
            Item food = _gameFactory.GetOrCreateFood(EnumExtensions<FoodType>.Random, _spawnPoint.position);
            float yExtent = food.GetComponent<Collider>().bounds.extents.y;
            food.transform.position += new Vector3(0, yExtent, 0);
            return food;
        }
    }
}