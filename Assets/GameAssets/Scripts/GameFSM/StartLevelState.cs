using Hypercasual.AssemblyLine;
using Hypercasual.Player;
using Hypercasual.UI;

namespace Hypercasual.GameFSM
{
    public class StartLevelState : IGameState
    {
        private readonly GameFSM _context;

        public StartLevelState(GameFSM context)
        {
            _context = context;
        }

        public void Enter()
        {
            InitializeLevel();
            _context.WindowManager.OpenScreen<LevelUI>();
            EnablePlayerLogic();
            _context.AssemblyLine.SwitchState(AssemblyLineState.MainMenu);
        }

        public void Exit()
        {
            _context.LevelManager.OnLevelCompleted -= OnLevelCompleted;
        }

        private void InitializeLevel()
        {
            _context.LevelManager.LoadNextLevel();
            _context.LevelManager.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnLevelCompleted()
        {
            _context.SwitchState(GameFlow.CompleteLevel);
        }

        private void EnablePlayerLogic() =>
            _context.Player.GetComponent<PlayerGrabFood>().enabled = true;
    }
}