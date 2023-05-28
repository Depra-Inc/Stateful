namespace Depra.StateMachines.UnitTests;

public sealed class TransitionStateMachineTests
{
    private readonly ITransitionStateMachine _stateMachine;

    public TransitionStateMachineTests()
    {
        var baseStateMachine = new StateMachine();
        _stateMachine = new TransitionStateMachine(baseStateMachine);
    }
}