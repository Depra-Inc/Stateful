// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Background;


namespace Depra.Stateful.UnitTests.Background;

public sealed class BackgroundStateMachineTests
{
	[Fact]
	public async Task ChangeState_SetsCurrentState()
	{
		// Arrange:
		var nextState = Substitute.For<IBackgroundState>();
		var stateMachine = new BackgroundStateMachine();

		// Act:
		await stateMachine.SwitchState(to: nextState);

		// Assert:
		stateMachine.CurrentState.Should().BeEquivalentTo(nextState);
	}

	[Fact]
	public async Task ChangeState_InvokesStateChangedEvent()
	{
		// Arrange:
		var nextState = Substitute.For<IBackgroundState>();
		var stateChangedEventInvoked = false;
		var stateMachine = new BackgroundStateMachine();
		stateMachine.StateChanged += _ => { stateChangedEventInvoked = true; };

		// Act:
		await stateMachine.SwitchState(to: nextState);

		// Assert:
		stateChangedEventInvoked.Should().BeTrue();
	}
}