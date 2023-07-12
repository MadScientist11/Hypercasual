using Hypercasual.UI;
using UnityEngine;

namespace Hypercasual.GameFSM
{
    public class CompleteLevelState : IGameState
    {
        private GameFSM _context;

        public CompleteLevelState(GameFSM context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.CameraAnimator.SwitchToWinCamera();
            _context.AssemblyLine.gameObject.SetActive(false);
            _context.WindowManager.OpenScreen<WinScreen>();
            _context.Player.PlayWinAnimation();
        }

        public void Exit()
        {
        }
    }
}