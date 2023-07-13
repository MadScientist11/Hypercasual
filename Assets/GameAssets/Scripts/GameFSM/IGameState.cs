namespace Hypercasual.GameFSM
{
    public interface IGameState
    {
        void Enter();
        void Exit();
    }
}