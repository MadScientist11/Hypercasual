﻿using System;
using System.Collections.Generic;
using Hypercasual.Food;
using Hypercasual.Level;

namespace Hypercasual.Services
{
    public interface IDataProvider : IService
    {
        T GetData<T>();
    }

    public class DataProvider : IDataProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Type, IData> _data;

        private const string AllFoodsPath = "AllFoods";
        private const string AllLevelsPath = "AllLevels";


        public DataProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _data = new Dictionary<Type, IData>();
        }

        public void Initialize()
        {
            AllFoods allFoods = _assetProvider.LoadAsset<AllFoods>(AllFoodsPath);
            AllLevels allLevels = _assetProvider.LoadAsset<AllLevels>(AllLevelsPath);

            _data.Add(typeof(AllFoods), allFoods);
            _data.Add(typeof(AllLevels), allLevels);
        }

        public T GetData<T>()
        {
            return (T)_data[typeof(T)];
        }
    }
}