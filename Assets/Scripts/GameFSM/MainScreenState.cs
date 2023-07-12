using Hypercasual.Player;
using Hypercasual.UI;

namespace Hypercasual.GameFSM
{
    public class MainScreenState : IGameState
    {
        private readonly GameFSM _context;
        private MainScreen _mainScreen;

        public MainScreenState(GameFSM context)
        {
            _context = context;
        }

        public void Enter()
        {
            DisablePlayerLogic();
            OpenMainMenuScreen();
        }

        private void OpenMainMenuScreen()
        {
            _context.GameFactory.GetOrCreateUIRoot();
            _mainScreen = _context.WindowManager.OpenScreen<MainScreen>();
            _mainScreen.Initialize(() => _context.SwitchState(GameFlow.StartLevel));
        }

        private void DisablePlayerLogic() =>
            _context.Player.GetComponent<PlayerPickUpItem>().enabled = false;

        public void Exit()
        {
            _mainScreen.Hide();
        }
    }
}