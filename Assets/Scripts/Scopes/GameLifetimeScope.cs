using Hypercasual.AssemblyLine;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerAnimator _player;
    [SerializeField] private AssemblyLine _assemblyLine;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_player);
        builder.RegisterComponent(_assemblyLine);
        builder.Register<GameFSM>(Lifetime.Singleton);
    }

    
}
