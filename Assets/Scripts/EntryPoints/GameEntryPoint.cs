using Hypercasual.GameFSM;
using Hypercasual.Services;
using UnityEngine;
using VContainer;

namespace Hypercasual.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        private GameFSM.GameFSM _gameFsm;

        [Inject]
        public void Construct(GameFSM.GameFSM gameFsm)
        {
            _gameFsm = gameFsm;
        }
        
        private void Awake()
        {
            _gameFsm.SwitchState(GameFlow.InitGame);
        }

        public void Restart()
        {
        
        }
    }
}
