using System.Collections.Generic;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hypercasual.EntryPoints
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private LifetimeScope _gameLifeScope;
        
        private IReadOnlyList<IService> _allServices;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(IReadOnlyList<IService> allServices, ISceneLoader sceneLoader)
        {
            _allServices = allServices;
            _sceneLoader = sceneLoader;
        }

        private void Awake() => 
            DontDestroyOnLoad(_gameLifeScope);

        private void Start()
        {
            InitializeServices(_allServices);
            _sceneLoader.LoadScene(GameConstants.Scenes.GamePath);
        }
    
        private void InitializeServices(IReadOnlyList<IService> services)
        {
            foreach (IService service in services)
            {
                service.Initialize();
            }
        }
    }
}