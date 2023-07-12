using Hypercasual.UI;
using UnityEngine;

namespace Hypercasual.GameFSM
{
    public class CompleteLevelState : IGameState
    {
        private readonly GameFSM _context;

        public CompleteLevelState(GameFSM context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.CameraAnimator.SwitchToWinCamera();
            _context.AssemblyLine.Deactivate();
            WinScreen winScreen = _context.WindowManager.OpenScreen<WinScreen>();
            winScreen.Initialize(LoadNextLevel);
            _context.Player.PlayWinAnimation();
        }

        private void LoadNextLevel()
        {
            _context.HeadToNextLevel = true;
            _context.SwitchState(GameFlow.InitGame);
        }

        public void Exit()
        {
        }
    }
}