// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transition;

namespace Depra.StateMachines.UnitTests.Transition;

public sealed class StatefulTransitionSystemTests
{
    [Fact]
    public void ChangeState_SetsCurrentState()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateMachine = Substitute.For<IStateMachine>();
        stateMachine.CurrentState.Returns(newState);
        IStatefulTransitionSystem statefulTransitionSystem = new StatefulTransitionSystem(stateMachine);

        // Act.
        statefulTransitionSystem.ChangeState(newState);

        // Assert.
        statefulTransitionSystem.CurrentState.Should().BeEquivalentTo(newState);
    }

    [Fact]
    public void Tick_NoTransitionNeeded_DoesNotChangeState()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var currentState = Substitute.For<IState>();
        stateMachine.CurrentState.Returns(currentState);
        IStatefulTransitionSystem statefulTransitionSystem = new StatefulTransitionSystem(stateMachine);

        // Act.
        statefulTransitionSystem.Tick();

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
        IStatefulTransitionSystem statefulTransitionSystem = new StatefulTransitionSystem(stateMachine);
        statefulTransitionSystem.AddAnyTransition(transition);

        // Act.
        var result = statefulTransitionSystem.NeedTransition(out var actualNextState);

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
        IStatefulTransitionSystem statefulTransitionSystem = new StatefulTransitionSystem(stateMachine);
        statefulTransitionSystem.AddTransition(currentState, transition);
        stateMachine.When(x => x.ChangeState(Arg.Any<IState>()))
            .Do(info =>
            {
                var newState = info.Arg<IState>();
                stateMachine.StateChanged += Raise.Event<Action<IState>>(newState);
            });

        // Act.
        statefulTransitionSystem.ChangeState(nextState);
        var result = statefulTransitionSystem.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_NoTransitionNeeded_ReturnsFalse()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var transitionStateMachine = new StatefulTransitionSystem(stateMachine);

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
        var act = () => new StatefulTransitionSystem(stateMachine);

        // Assert.
        act.Should().Throw<ArgumentNullException>();
    }
}