using UnityEngine;

namespace Hypercasual
{
    public interface IAssetProvider : IService
    {
        T LoadAsset<T>(string path) where T : Object;
    }
 
    public class AssetProvider : IAssetProvider
    {
        public void Initialize()
        {
        }

        public T LoadAsset<T>(string path) where T : Object
        {
            T asset = Resources.Load<T>(path);

            if (asset == null)
            {
                Debug.LogError($"The asset at path \"{path}\" doesn't exist");
            }

            return asset;
        }
    }
}