using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transition;

namespace Depra.StateMachines.UnitTests.Transition;

public sealed class StateTransitionCoordinationTests
{
    private readonly IStateTransitionCoordination _coordination;

    public StateTransitionCoordinationTests() => 
        _coordination = new StateTransitionCoordination();
    
    [Fact]
    public void NeedTransition_Should_ReturnTrue_WhenAnyTransitionExists()
    {
        // Arrange.
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();

        _coordination.AddAnyTransition(transition);
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        // Act.
        var result = _coordination.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_Should_ReturnTrue_WhenCurrentTransitionExists()
    {
        // Arrange.
        var state = Substitute.For<IState>();
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();

        _coordination.AddTransition(state, transition);
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        // Act.
        _coordination.Update(state);
        var result = _coordination.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_Should_ReturnFalse_WhenNoTransitionExists()
    {
        // Arrange - not needed.

        // Act.
        var result = _coordination.NeedTransition(out var nextState);

        // Assert.
        result.Should().BeFalse();
        nextState.Should().BeNull();
    }
}