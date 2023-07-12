using Hypercasual;
using Hypercasual.AssemblyLine;
using Hypercasual.GameFSM;
using Hypercasual.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerAnimator _player;
    [SerializeField] private AssemblyLine _assemblyLine;
    [SerializeField] private CameraAnimator _cameraAnimator;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_player);
        builder.RegisterComponent(_assemblyLine);
        builder.RegisterComponent(_cameraAnimator);
        builder.Register<GameFSM>(Lifetime.Singleton);
    }
}