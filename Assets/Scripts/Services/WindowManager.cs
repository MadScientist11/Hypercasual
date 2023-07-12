using System;
using System.Collections.Generic;
using Hypercasual.UI;

namespace Hypercasual.Services
{
    public interface IWindowManager : IService
    {
        T OpenScreen<T>() where T : BaseScreen;
        void RefreshCurrentWindow();
    }

    public class WindowManager : IWindowManager
    {
        private readonly Dictionary<Type, BaseScreen> _screensCache = new();
        private readonly IGameFactory _gameFactory;

        private BaseScreen _currentScreen;

        public WindowManager(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
        }

        public T OpenScreen<T>() where T : BaseScreen
        {
            if (_currentScreen != null) 
                _currentScreen.Hide();
            
            if (_screensCache.TryGetValue(typeof(T), out var screen))
            {
                screen.Show();
                _currentScreen = screen;
                return (T)screen;
            }

            T newScreen = _gameFactory.CreateScreen<T>();
            _screensCache.Add(typeof(T), newScreen);
            newScreen.Initialize();
            _currentScreen = newScreen;
            return newScreen;
        }

        public void RefreshCurrentWindow()
        {
            _currentScreen.Refresh();
        }
    }
}