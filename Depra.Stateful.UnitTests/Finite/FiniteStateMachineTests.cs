// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Finite;

namespace Depra.Stateful.UnitTests.Finite;

public sealed class FiniteStateMachineTests
{
    [Fact]
    public void AutoTransitionOnStart_SetsCurrentState()
    {
        // Arrange:
        var startState = Substitute.For<IFiniteState>();
        var stateMachine = new FiniteStateMachine(startState);

        // Act - No action needed.

        // Assert:
        stateMachine.CurrentState.Should().BeEquivalentTo(startState);
    }

    [Fact]
    public void ChangeState_SetsCurrentState()
    {
        // Arrange:
        var nextState = Substitute.For<IFiniteState>();
        var stateMachine = new FiniteStateMachine();

        // Act:
        stateMachine.SwitchState(to: nextState);

        // Assert:
        stateMachine.CurrentState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void ChangeState_InvokesStateChangedEvent()
    {
        // Arrange:
        var nextState = Substitute.For<IFiniteState>();
        var stateChangedEventInvoked = false;
        var stateMachine = new FiniteStateMachine();
        stateMachine.StateChanged += _ => { stateChangedEventInvoked = true; };

        // Act:
        stateMachine.SwitchState(to: nextState);

        // Assert:
        stateChangedEventInvoked.Should().BeTrue();
    }
}