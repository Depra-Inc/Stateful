// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Depra.StateMachines.UnitTests;

public sealed class StatefulTransitionMachineTests
{
    [Fact]
    public void ChangeState_SetsCurrentState()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateMachine = Substitute.For<IStateMachine>();
        stateMachine.CurrentState.Returns(newState);
        IStatefulTransitionMachine statefulTransitionMachine = new StatefulTransitionMachine(stateMachine);

        // Act.
        statefulTransitionMachine.ChangeState(newState);

        // Assert.
        statefulTransitionMachine.CurrentState.Should().BeEquivalentTo(newState);
    }

    [Fact]
    public void Tick_NoTransitionNeeded_DoesNotChangeState()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var currentState = Substitute.For<IState>();
        stateMachine.CurrentState.Returns(currentState);
        IStatefulTransitionMachine statefulTransitionMachine = new StatefulTransitionMachine(stateMachine);

        // Act.
        statefulTransitionMachine.Tick();

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
        IStatefulTransitionMachine statefulTransitionMachine = new StatefulTransitionMachine(stateMachine);
        statefulTransitionMachine.AddAnyTransition(transition);

        // Act.
        var result = statefulTransitionMachine.NeedTransition(out var actualNextState);

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
        IStatefulTransitionMachine statefulTransitionMachine = new StatefulTransitionMachine(stateMachine);
        statefulTransitionMachine.AddTransition(currentState, transition);
        stateMachine.When(x => x.ChangeState(Arg.Any<IState>()))
            .Do(info =>
            {
                var newState = info.Arg<IState>();
                stateMachine.StateChanged += Raise.Event<Action<IState>>(newState);
            });

        // Act.
        statefulTransitionMachine.ChangeState(nextState);
        var result = statefulTransitionMachine.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_NoTransitionNeeded_ReturnsFalse()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var transitionStateMachine = new StatefulTransitionMachine(stateMachine);

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
        var act = () => new StatefulTransitionMachine(stateMachine);

        // Assert.
        act.Should().Throw<ArgumentNullException>();
    }
}