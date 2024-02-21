// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Finite;

namespace Depra.Stateful.UnitTests.Finite;

public sealed class AsyncStateMachineTests
{
	[Fact]
	public async Task ChangeState_SetsCurrentState()
	{
		// Arrange:
		var newState = Substitute.For<IState>();
		var stateMachine = new AsyncStateMachine();

		// Act:
		await stateMachine.SwitchState(to: newState);

		// Assert:
		stateMachine.CurrentState.Should().BeEquivalentTo(newState);
	}

	[Fact]
	public async Task ChangeState_InvokesStateChangedEvent()
	{
		// Arrange:
		var newState = Substitute.For<IState>();
		var stateChangedEventInvoked = false;
		var stateMachine = new AsyncStateMachine();
		stateMachine.StateChanged += _ => { stateChangedEventInvoked = true; };

		// Act:
		await stateMachine.SwitchState(to: newState);

		// Assert:
		stateChangedEventInvoked.Should().BeTrue();
	}
}