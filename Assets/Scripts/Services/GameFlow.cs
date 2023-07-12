using System.Collections.Generic;
using Hypercasual.UI;

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
        public IWindowManager WindowManager { get; set; }


        public Game(IGameFactory gameFactory, IWindowManager windowManager)
        {
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
        public StartLevelState(Game game)
        {
        }

        public void Enter()
        {
        }

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
            _context.GameFactory.GetOrCreateUIRoot();
            _mainScreen = _context.WindowManager.OpenScreen<MainScreen>();
            _mainScreen.Initialize(() => _context.SwitchState(GameFlow.StartLevel));
        }

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