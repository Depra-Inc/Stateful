// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Finite;
using Depra.Stateful.Transitional;

namespace Depra.Stateful.UnitTests.Transition;

public sealed class StatefulTransitionSystemTests
{
	[Fact]
	public void ChangeState_Should_UpdateCurrentState()
	{
		// Arrange:
		var newState = Substitute.For<IFiniteState>();
		var stateMachine = Substitute.For<IFiniteStateMachine>();
		stateMachine.CurrentState.Returns(newState);
		var transitions = Substitute.For<IStateTransitions>();
		IFiniteStateMachine transitionSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act:
		transitionSystem.SwitchState(to: newState);

		// Assert:
		transitionSystem.CurrentState.Should().BeEquivalentTo(newState);
	}

	[Fact]
	public void Tick_WhenNeedTransition_Should_ChangeState()
	{
		// Arrange:
		IState capturedNextState = null!;
		var stateMachine = Substitute.For<IFiniteStateMachine>();
		var transitions = Substitute.For<IStateTransitions>();
		stateMachine.SwitchState(Arg.Do<IFiniteState>(state => capturedNextState = state));
		transitions.NeedTransition(stateMachine.CurrentState, out var nextState).ReturnsForAnyArgs(_ => true);
		var transitionSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act:
		transitionSystem.Tick();

		// Assert:
		capturedNextState.Should().BeEquivalentTo(nextState);
	}

	[Fact]
	public void Tick_WhenNoTransitionNeeded_Should_NotChangeState()
	{
		// Arrange:
		var stateMachine = Substitute.For<IFiniteStateMachine>();
		var transitions = Substitute.For<IStateTransitions>();
		var tickSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act:
		tickSystem.Tick();

		// Assert:
		stateMachine.DidNotReceive().SwitchState(to: Arg.Any<IFiniteState>());
	}

	[Fact]
	public void Constructor_NullStateMachine_ThrowsArgumentNullException()
	{
		// Arrange:
		IFiniteStateMachine stateMachine = null!;
		var transitions = Substitute.For<IStateTransitions>();

		// Act:
		var act = () => new StatefulTransitionSystem(stateMachine, transitions);

		// Assert:
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void Constructor_NullTransitionCoordinator_ThrowsArgumentNullException()
	{
		// Arrange:
		var stateMachine = Substitute.For<IFiniteStateMachine>();
		IStateTransitions stateTransitions = null!;

		// Act:
		var act = () => new StatefulTransitionSystem(stateMachine, stateTransitions);

		// Assert:
		act.Should().Throw<ArgumentNullException>();
	}
}