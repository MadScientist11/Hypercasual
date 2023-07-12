using Hypercasual;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hypercasual.Scopes
{
    public class MainLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            BindServices(builder);
            builder.Register<Game>(Lifetime.Singleton);
        }

        private void BindServices(IContainerBuilder builder)
        {
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DataProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
        }
    }
}