// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transition;

namespace Depra.StateMachines.UnitTests.Transition;

public sealed class StatefulTransitionSystemTests
{
    [Fact]
    public void ChangeState_Should_UpdateCurrentState()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateMachine = Substitute.For<IStateMachine>();
        stateMachine.CurrentState.Returns(newState);
        var coordination = Substitute.For<IStateTransitionCoordination>();
        IStatefulTransitionSystem transitionSystem = new StatefulTransitionSystem(stateMachine, coordination);

        // Act.
        transitionSystem.ChangeState(newState);

        // Assert.
        transitionSystem.CurrentState.Should().BeEquivalentTo(newState);
    }

    [Fact]
    public void Tick_WhenNeedTransition_Should_ChangeState()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var coordination = Substitute.For<IStateTransitionCoordination>();
        coordination.NeedTransition(out var nextState).Returns(true);
        var transitionSystem = new StatefulTransitionSystem(stateMachine, coordination);

        // Act.
        transitionSystem.Tick();

        // Assert.
        stateMachine.Received(1).ChangeState(nextState);
    }

    [Fact]
    public void Tick_WhenNoTransitionNeeded_Should_NotChangeState()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var coordination = Substitute.For<IStateTransitionCoordination>();
        IStatefulTransitionSystem statefulTransitionSystem = new StatefulTransitionSystem(stateMachine, coordination);
        coordination.NeedTransition(out _).Returns(false);

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
        var coordination = Substitute.For<IStateTransitionCoordination>();
        IStatefulTransitionSystem transitionSystem = new StatefulTransitionSystem(stateMachine, coordination);
        transitionSystem.AddAnyTransition(transition);
        coordination.NeedTransition(out Arg.Any<IState>())
            .Returns(x =>
            {
                x[0] = nextState;
                return true;
            });

        // Act.
        var result = transitionSystem.NeedTransition(out var actualNextState);

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
        var coordination = Substitute.For<IStateTransitionCoordination>();
        IStatefulTransitionSystem transitionSystem = new StatefulTransitionSystem(stateMachine, coordination);
        transitionSystem.AddTransition(currentState, transition);
        coordination.NeedTransition(out Arg.Any<IState>())
            .Returns(x =>
            {
                x[0] = nextState;
                return true;
            });

        // Act.
        var result = transitionSystem.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_NoTransitionNeeded_ReturnsFalse()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        var coordination = Substitute.For<IStateTransitionCoordination>();
        var transitionSystem = new StatefulTransitionSystem(stateMachine, coordination);

        // Act.
        var result = transitionSystem.NeedTransition(out var nextState);

        // Assert.
        result.Should().BeFalse();
        nextState.Should().BeNull();
    }

    [Fact]
    public void Constructor_NullStateMachine_ThrowsArgumentNullException()
    {
        // Arrange.
        IStateMachine stateMachine = null!;
        var coordination = Substitute.For<IStateTransitionCoordination>();

        // Act.
        var act = () => new StatefulTransitionSystem(stateMachine, coordination);

        // Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_NullTransitionCoordinator_ThrowsArgumentNullException()
    {
        // Arrange.
        var stateMachine = Substitute.For<IStateMachine>();
        IStateTransitionCoordination coordination = null!;

        // Act.
        var act = () => new StatefulTransitionSystem(stateMachine, coordination);

        // Assert.
        act.Should().Throw<ArgumentNullException>();
    }
}