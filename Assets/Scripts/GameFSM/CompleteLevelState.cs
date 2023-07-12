namespace Hypercasual.Services
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
        }

        public void Exit()
        {
        }
    }
}