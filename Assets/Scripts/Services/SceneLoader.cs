using UnityEngine.SceneManagement;

namespace Hypercasual.Services
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string sceneName, System.Action onLoaded = null);
    }
    public class SceneLoader : ISceneLoader
    {
        public void Initialize()
        {
        }

        public void LoadScene(string nextScene, System.Action onLoaded = null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(nextScene);

            void OnSceneLoaded(Scene s, LoadSceneMode m)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                onLoaded?.Invoke();
            }
        }
    }
}
