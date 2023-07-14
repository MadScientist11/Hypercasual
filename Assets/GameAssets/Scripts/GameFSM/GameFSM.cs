using System.Collections.Generic;
using Hypercasual.Player;
using Hypercasual.Services;
using UnityEngine;

namespace Hypercasual.GameFSM
{
    public class GameFSM
    {
        public ILevelManager LevelManager { get; }
        public IGameFactory GameFactory { get; }
        public IWindowManager WindowManager { get; }
        public PlayerAnimator Player { get; }
        public AssemblyLine.AssemblyLine AssemblyLine { get; }
        public CameraAnimator CameraAnimator { get; }
        public Confetti Confetti { get; }

        public bool HeadToNextLevel { get;  set; }


        private readonly Dictionary<GameFlow, IGameState> _states;
        private IGameState _currentState;


        public GameFSM(IGameFactory gameFactory, IWindowManager windowManager, ILevelManager levelManager,
            PlayerAnimator player,
            AssemblyLine.AssemblyLine assemblyLine,
            CameraAnimator cameraAnimator,
            Confetti confetti)
        {
            Confetti = confetti;
            CameraAnimator = cameraAnimator;
            LevelManager = levelManager;
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

        public void SwitchState(GameFlow nextState)
        {
            if (_states[nextState] == _currentState)
            {
                Debug.LogWarning($"State {nextState} is already active");
                return;
            }
            
            _currentState?.Exit();
            _currentState = _states[nextState];
            _currentState.Enter();
        }
    }
}