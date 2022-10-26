namespace Depra.StateMachines.Domain
{
    public interface IStateTransitionCondition
    {
        bool IsMet();
    }
}