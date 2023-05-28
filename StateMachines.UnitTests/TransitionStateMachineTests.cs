// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Depra.StateMachines.UnitTests;

public sealed class TransitionStateMachineTests
{
    [Fact]
    public void ChangeState_SetsCurrentState()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateMachine = Substitute.For<IStateMachine>();
        stateMachine.CurrentState.Returns(newState);
        ITransitionStateMachine transitionStateMachine = new TransitionStateMachine(stateMachine);

        // Act.
        transitionStateMachine.ChangeState(newState);

        // Assert.
        transitionStateMachine.CurrentState.Should().BeEquivalentTo(newState);
    }

    [Fact]
    public void Tick_NoTransitionNeeded_DoesNotChangeState()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var currentState = Substitute.For<IState>();
        stateMachine.CurrentState.Returns(currentState);
        ITransitionStateMachine transitionStateMachine = new TransitionStateMachine(stateMachine);

        // Act.
        transitionStateMachine.Tick();

        // Assert.
        stateMachine.DidNotReceive().ChangeState(Arg.Any<IState>());
    }

    [Fact]
    public void NeedTransition_AnyTransitionTrue_ReturnsTrue()
    {
        // Arrange.
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        var stateMachine = Substitute.For<IStateMachine>();
        ITransitionStateMachine transitionStateMachine = new TransitionStateMachine(stateMachine);
        transitionStateMachine.AddAnyTransition(transition);

        // Act.
        var result = transitionStateMachine.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_CurrentTransitionTrue_ReturnsTrue()
    {
        // Arrange.
        var currentState = Substitute.For<IState>();
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        var stateMachine = Substitute.For<IStateMachine>();
        ITransitionStateMachine transitionStateMachine = new TransitionStateMachine(stateMachine);
        transitionStateMachine.AddTransition(currentState, transition);
        stateMachine.When(x => x.ChangeState(Arg.Any<IState>()))
            .Do(info =>
            {
                var newState = info.Arg<IState>();
                stateMachine.StateChanged += Raise.Event<Action<IState>>(newState);
            });

        // Act.
        transitionStateMachine.ChangeState(nextState);
        var result = transitionStateMachine.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_NoTransitionNeeded_ReturnsFalse()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var transitionStateMachine = new TransitionStateMachine(stateMachine);

        // Act.
        var result = transitionStateMachine.NeedTransition(out var nextState);

        // Assert.
        result.Should().BeFalse();
        nextState.Should().BeNull();
    }

    [Fact]
    public void Constructor_NullStateMachine_ThrowsArgumentNullException()
    {
        // Arrange.
        IStateMachine stateMachine = null!;

        // Act.
        var act = () => new TransitionStateMachine(stateMachine);

        // Assert.
        act.Should().Throw<ArgumentNullException>();
    }
}