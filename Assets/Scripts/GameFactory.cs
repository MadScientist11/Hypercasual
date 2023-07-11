using UnityEngine;
using UnityEngine.Pool;

namespace Hypercasual
{
    public class GameFactory
    {
        private const string BananaPath = "Banana";
        private const string ApplePath = "Apple";
        private const string OrangePath = "Apple";


        private readonly ObjectPool<Item> _foodPool;

        private readonly AssetProvider _assetProvider;
        private FoodType _currentFoodType;

        public GameFactory(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _foodPool = new ObjectPool<Item>(CreateFood, food => food.gameObject.SetActive(true),
                food => food.gameObject.SetActive(false)
                , food => GameObject.Destroy(food.gameObject), 
                true, 
                10, 
                15);
        }

        private GameObject _foodParent;
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
    }
}