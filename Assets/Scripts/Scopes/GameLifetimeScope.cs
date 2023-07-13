using Hypercasual.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Hypercasual.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerAnimator _player;
        [SerializeField] private AssemblyLine.AssemblyLine _assemblyLine;
        [SerializeField] private CameraAnimator _cameraAnimator;
        [SerializeField] private Confetti _confetti;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_player);
            builder.RegisterComponent(_assemblyLine);
            builder.RegisterComponent(_cameraAnimator);
            builder.RegisterComponent(_confetti);
            builder.Register<GameFSM.GameFSM>(Lifetime.Singleton);
        }
    }
}