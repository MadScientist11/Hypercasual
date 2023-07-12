using Hypercasual;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hypercasual.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            BindConfigs(builder);
            BindServices(builder);
        }

        private static void BindServices(IContainerBuilder builder)
        {
            builder.Register<GameFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
        }
    }
}