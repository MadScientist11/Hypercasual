using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerAnimator _player;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_player);
        builder.Register<Game>(Lifetime.Singleton);
    }

    
}
