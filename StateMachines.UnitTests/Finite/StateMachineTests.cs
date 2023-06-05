// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Abstract;
using Depra.StateMachines.Finite;

namespace Depra.StateMachines.UnitTests.Finite;

public sealed class StateMachineTests
{
    [Fact]
    public void AutoTransitionOnStart_SetsCurrentState()
    {
        // Arrange.
        var startState = Substitute.For<IState>();
        var stateMachine = new StateMachine(startState);

        // Act - No action needed.

        // Assert.
        stateMachine.CurrentState.Should().BeEquivalentTo(startState);
    }

    [Fact]
    public void ChangeState_SetsCurrentState()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateMachine = new StateMachine();

        // Act.
        stateMachine.SwitchState(to: newState);

        // Assert.
        stateMachine.CurrentState.Should().BeEquivalentTo(newState);
    }

    [Fact]
    public void ChangeState_InvokesStateChangedEvent()
    {
        // Arrange.
        var newState = Substitute.For<IState>();
        var stateChangedEventInvoked = false;
        var stateMachine = new StateMachine();
        stateMachine.StateChanged += _ => { stateChangedEventInvoked = true; };

        // Act.
        stateMachine.SwitchState(to: newState);

        // Assert.
        stateChangedEventInvoked.Should().BeTrue();
    }
}