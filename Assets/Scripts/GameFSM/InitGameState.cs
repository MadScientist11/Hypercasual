namespace Hypercasual.GameFSM
{
    public class InitGameState : IGameState
    {
        private readonly GameFSM _context;

        public InitGameState(GameFSM context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.Player.ResetAnimatorState();
            _context.CameraAnimator.SwitchToDefaultCamera();
            _context.AssemblyLine.Activate();
            
            if (_context.HeadToNextLevel)
            {
                _context.SwitchState(GameFlow.StartLevel);
                _context.HeadToNextLevel = false;
                return;
            }

            _context.SwitchState(GameFlow.MainScreenState);
        }

        public void Exit()
        {
        }
    }
}