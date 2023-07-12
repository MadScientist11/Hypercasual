using System;
using System.Collections.Generic;
using Hypercasual.UI;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Hypercasual.Services
{
    public interface  IGameFactory : IService
    {
        Item GetOrCreateFood(FoodType foodType, Vector3 position);
        T CreateScreen<T>() where T : BaseScreen;
        UiRoot GetOrCreateUIRoot();
    }

    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _instantiator;
        private readonly IAssetProvider _assetProvider;
        
        private const string BananaPath = "Banana";
        private const string ApplePath = "Apple";
        private const string OrangePath = "Apple";

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
        private readonly ObjectPool<Item> _foodPool;

        private FoodType _currentFoodType;
        private GameObject _foodParent;
        private UiRoot _uiRoot;

        public GameFactory(IObjectResolver instantiator, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _foodPool = new ObjectPool<Item>(CreateFood, food => food.gameObject.SetActive(true),
                food => food.gameObject.SetActive(false)
                , food => GameObject.Destroy(food.gameObject), 
                true, 
                10, 
                15);
        }

        public void Initialize()
        {
        }


        private Item CreateFood()
        {
            _foodParent ??= new GameObject("Foods");
            string prefabPath = _currentFoodType switch
            {
                FoodType.Apple => ApplePath,
                FoodType.Banana => BananaPath,
                FoodType.Orange => OrangePath,
                _ => null
            };

            Item itemAsset = _assetProvider.LoadAsset<Item>(prefabPath);
            Item instance = GameObject.Instantiate(itemAsset, _foodParent.transform);
            return instance;
        }


        public Item GetOrCreateFood(FoodType foodType, Vector3 position)
        {
            _currentFoodType = foodType;
            _foodPool.Get(out Item pooledFood);
            pooledFood.transform.position = position;
            pooledFood.Initialize(_foodPool);
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