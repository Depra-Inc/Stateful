namespace Depra.StateMachines.Domain
{
    public interface IState
    {
        void Enter();

        void Exit();
    }
}