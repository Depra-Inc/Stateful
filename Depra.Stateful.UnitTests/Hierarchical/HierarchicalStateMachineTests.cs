// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Hierarchical;


namespace Depra.Stateful.UnitTests.Hierarchical;

public sealed class HierarchicalStateMachineTests
{
	[Fact]
	public async Task ChangeState_SetsCurrentState()
	{
		// Arrange:
		var nextState = Substitute.For<IStateNode>();
		var stateMachine = new HierarchicalStateMachine();

		// Act:
		await stateMachine.SwitchState(to: nextState);

		// Assert:
		stateMachine.CurrentState.Should().BeEquivalentTo(nextState);
	}

	[Fact]
	public async Task ChangeState_InvokesStateChangedEvent()
	{
		// Arrange:
		var nextState = Substitute.For<IStateNode>();
		var stateChangedEventInvoked = false;
		var stateMachine = new HierarchicalStateMachine();
		stateMachine.StateChanged += _ => { stateChangedEventInvoked = true; };

		// Act:
		await stateMachine.SwitchState(to: nextState);

		// Assert:
		stateChangedEventInvoked.Should().BeTrue();
	}
}