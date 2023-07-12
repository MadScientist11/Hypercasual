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
            //Analytics
            _context.Player.ResetAnimatorState();
            _context.SwitchState(GameFlow.MainScreenState);
        }

        public void Exit()
        {
        }
    }
}