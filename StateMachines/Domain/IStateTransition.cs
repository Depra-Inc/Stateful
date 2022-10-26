namespace Depra.StateMachines.Domain
{
    public interface IStateTransition
    {
        IState NextState { get; }

        bool ShouldTransition();
    }
}