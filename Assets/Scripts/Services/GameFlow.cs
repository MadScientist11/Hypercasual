using System.Collections.Generic;
using Hypercasual.UI;
using Hypercasual.Player;

namespace Hypercasual.Services
{
    public enum GameFlow
    {
        InitGame = 0,
        MainScreenState = 1,
        StartLevel = 2,
        CompleteLevel = 3,
    }

    public class Game
    {
        public Dictionary<GameFlow, IGameState> States { get; }
        private IGameState _currentState;
        public IGameFactory GameFactory { get; }
        public IWindowManager WindowManager { get; }
        public PlayerAnimator Player { get; }


        public Game(IGameFactory gameFactory, IWindowManager windowManager, PlayerAnimator player)
        {
            Player = player;
            States = new Dictionary<GameFlow, IGameState>
            {
                { GameFlow.InitGame, new InitGameState(this) },
                { GameFlow.MainScreenState, new MainScreenState(this) },
                { GameFlow.StartLevel, new StartLevelState(this) },
                { GameFlow.CompleteLevel, new CompleteLevelState(this) },
            };
            GameFactory = gameFactory;
            WindowManager = windowManager;
        }

        public void SwitchState(GameFlow gameFlow)
        {
            _currentState?.Exit();
            _currentState = States[gameFlow];
            _currentState.Enter();
        }
    }

    public class CompleteLevelState : IGameState
    {
        public CompleteLevelState(Game game)
        {
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }

    public class StartLevelState : IGameState
    {
        private Game _context;

        public StartLevelState(Game context)
        {
            _context = context;
        }

        public void Enter()
        {
            EnablePlayerLogic();
        }

        private void EnablePlayerLogic() => 
            _context.Player.GetComponent<PlayerPickUpItem>().enabled = true;

        public void Exit()
        {
        }
    }

    public class InitGameState : IGameState
    {
        private readonly Game _context;

        public InitGameState(Game context)
        {
            _context = context;
        }

        public void Enter()
        {
            //Analytics
            _context.SwitchState(GameFlow.MainScreenState);
        }

        public void Exit()
        {
        }
    }

    public class MainScreenState : IGameState
    {
        private readonly Game _context;
        private MainScreen _mainScreen;

        public MainScreenState(Game context)
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
            _context.Player.GetComponent<PlayerPickUpItem>().enabled = true;

        public void Exit()
        {
            _mainScreen.Hide();
        }
    }

    public interface IGameState
    {
        void Enter();
        void Exit();
    }
}