using Hypercasual.Services;
using UnityEngine;
using VContainer;

namespace Hypercasual.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        private Game _game;

        [Inject]
        public void Construct(Game game)
        {
            _game = game;
        }
        
        private void Awake()
        {
            _game.SwitchState(GameFlow.InitGame);
        }

        public void Restart()
        {
        
        }
    }
}
