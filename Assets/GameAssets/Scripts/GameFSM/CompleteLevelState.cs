using Hypercasual.AssemblyLine;
using Hypercasual.UI;

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
            _context.AssemblyLine.SwitchState(AssemblyLineState.LevelCompleted);
            WinScreen winScreen = _context.WindowManager.OpenScreen<WinScreen>();
            winScreen.Initialize(LoadNextLevel, ToMenu);
            _context.Player.PlayWinAnimation();
            _context.Confetti.gameObject.SetActive(true);
        }

        public void Exit()
        {
        }

        private void LoadNextLevel()
        {
            _context.HeadToNextLevel = true;
            _context.SwitchState(GameFlow.InitGame);
        }

        private void ToMenu()
        {
            _context.HeadToNextLevel = false;
            _context.SwitchState(GameFlow.InitGame);
        }
    }
}