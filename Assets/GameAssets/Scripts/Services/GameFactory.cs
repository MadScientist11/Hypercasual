using System;
using System.Collections.Generic;
using Hypercasual.Food;
using Hypercasual.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Hypercasual.Services
{
    public interface IGameFactory : IService
    {
        GameObject FoodParent { get; }
        FoodView GetOrCreateFood(FoodType foodType, Vector3 position);
        T CreateScreen<T>() where T : BaseScreen;
        UiRoot GetOrCreateUIRoot();
    }

    public class ObjectPoolM<T> where T : class
    {
        private List<T> _objects;
        private Func<T> _create;
        private Action<T> _onRelease;
        private Action<T> _onGet;

        public ObjectPoolM(Func<T> create, Action<T> onRelease, Action<T> onGet)
        {
            _onGet = onGet;
            _onRelease = onRelease;
            _create = create;
            _objects = new List<T>();
        }

        public T Get(Func<T, bool> predicate)
        {
            T obj;
            if (_objects.Count == 0)
            {
                obj = _create.Invoke();
            }
            else
            {
                T searchFor = SearchFor(predicate);
                if (searchFor != null)
                {
                    obj = searchFor;
                    _objects.Remove(searchFor);
                }
                else
                {
                    obj = _create.Invoke();
                }
            }

            _onGet?.Invoke(obj);
            return obj;
        }

        private T SearchFor(Func<T, bool> predicate)
        {
            foreach (T obj in _objects)
            {
                if (predicate.Invoke(obj))
                    return obj;
            }

            return null;
        }

        public void Release(T item)
        {
            if (!_objects.Contains(item))
                _objects.Add(item);
            _onRelease?.Invoke(item);
        }
    }

    public class GameFactory : IGameFactory
    {
        public GameObject FoodParent { get; private set; }

        private readonly IObjectResolver _instantiator;
        private readonly IAssetProvider _assetProvider;
        private readonly IDataProvider _dataProvider;

        private const string UiRootPath = "MainCanvas";
        private const string MainScreenPath = "MainScreen";
        private const string LevelUIPath = "LevelUI";
        private const string WinScreenPath = "WinScreen";

        private readonly Dictionary<Type, string> _screenPaths = new()
        {
            { typeof(MainScreen), MainScreenPath },
            { typeof(LevelUI), LevelUIPath },
            { typeof(WinScreen), WinScreenPath },
        };

        private readonly ObjectPoolM<FoodView> _foodPool;

        private FoodType _currentFoodType;
        private UiRoot _uiRoot;

        public GameFactory(IObjectResolver instantiator, IAssetProvider assetProvider, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _foodPool = new ObjectPoolM<FoodView>(CreateFood, food => food.gameObject.SetActive(false),
                food => food.gameObject.SetActive(true));
        }

        public void Initialize()
        {
        }


        private FoodView CreateFood()
        {
            FoodParent ??= new GameObject("Foods");
            GameObject itemPrefab = _dataProvider.GetData<AllFoods>().GetFood(_currentFoodType).Prefab;
            FoodView instance = InstancePrefab<FoodView>(itemPrefab, FoodParent.transform);
            return instance;
        }


        public FoodView GetOrCreateFood(FoodType foodType, Vector3 position)
        {
            _currentFoodType = foodType;
            FoodView pooledFood = _foodPool.Get(food => food.FoodType == foodType);
            pooledFood.transform.position = position;
            pooledFood.Initialize(_foodPool, this);
            return pooledFood;
        }


        public UiRoot GetOrCreateUIRoot()
        {
            if (_uiRoot == null)
                _uiRoot = InstancePrefab<UiRoot>(UiRootPath);

            return _uiRoot;
        }

        public T CreateScreen<T>() where T : BaseScreen
        {
            return InstancePrefabInjected<T>(_screenPaths[typeof(T)], _uiRoot.transform);
        }

        private T InstancePrefab<T>(string path) where T : MonoBehaviour
        {
            T asset = _assetProvider.LoadAsset<T>(path);
            return Object.Instantiate(asset);
        }

        private T InstancePrefab<T>(GameObject asset, Transform parent) where T : MonoBehaviour
        {
            return Object.Instantiate(asset).GetComponent<T>();
        }

        private T InstancePrefabInjected<T>(string path) where T : MonoBehaviour
        {
            T asset = _assetProvider.LoadAsset<T>(path);
            asset.gameObject.SetActive(false);
            T instance = _instantiator.Instantiate(asset);
            instance.gameObject.SetActive(true);
            return instance;
        }

        private T InstancePrefabInjected<T>(string path, Transform parent) where T : MonoBehaviour
        {
            T asset = _assetProvider.LoadAsset<T>(path);
            asset.gameObject.SetActive(false);
            T instance = _instantiator.Instantiate(asset, parent);
            instance.gameObject.SetActive(true);
            return instance;
        }
    }
}