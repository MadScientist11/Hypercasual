using System.Collections.Generic;
using Hypercasual.Player;
using Hypercasual.Services;

namespace Hypercasual.GameFSM
{
    public class GameFSM
    {
        public ILevelService LevelService { get; }
        public IGameFactory GameFactory { get; }
        public IWindowManager WindowManager { get; }
        public PlayerAnimator Player { get; }
        public AssemblyLine.AssemblyLine AssemblyLine { get; }
        public CameraAnimator CameraAnimator { get; }


        private readonly Dictionary<GameFlow, IGameState> _states;
        private IGameState _currentState;


        public GameFSM(IGameFactory gameFactory, IWindowManager windowManager, ILevelService levelService,
            PlayerAnimator player,
            AssemblyLine.AssemblyLine assemblyLine,
            CameraAnimator cameraAnimator)
        {
            CameraAnimator = cameraAnimator;
            LevelService = levelService;
            AssemblyLine = assemblyLine;
            Player = player;
            _states = new Dictionary<GameFlow, IGameState>
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
            _currentState = _states[gameFlow];
            _currentState.Enter();
        }
    }
}