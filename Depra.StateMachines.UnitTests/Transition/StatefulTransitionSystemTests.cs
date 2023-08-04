// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Transitional.Strategies;
using Depra.StateMachines.Transitional.Transition;

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
		var transitions = Substitute.For<ITransitions>();
		IStateMachine transitionSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act.
		transitionSystem.SwitchState(to: newState);

		// Assert.
		transitionSystem.CurrentState.Should().BeEquivalentTo(newState);
	}

	[Fact]
	public void Tick_WhenNeedTransition_Should_ChangeState()
	{
		// Arrange.
		IState capturedNextState = null!;
		var nextState = Substitute.For<IState>();
		var stateMachine = Substitute.For<IStateMachine>();
		stateMachine.SwitchState(Arg.Do<IState>(state => capturedNextState = state));

		var transitions = Substitute.For<ITransitions>();
		transitions.Any.Returns(new List<IStateTransition>());
		transitions[stateMachine.CurrentState].Returns(new List<IStateTransition>());
		transitions[stateMachine.CurrentState].ReturnsForAnyArgs(_ => new List<IStateTransition>
		{
			new StateTransition(nextState, () => true)
		});

		var transitionSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act.
		transitionSystem.Execute();

		// Assert.
		capturedNextState.Should().BeEquivalentTo(nextState);
	}

	[Fact]
	public void Tick_WhenNoTransitionNeeded_Should_NotChangeState()
	{
		// Arrange.
		var stateMachine = Substitute.For<IStateMachine>();
		var transitions = Substitute.For<ITransitions>();
		var tickSystem = new StatefulTransitionSystem(stateMachine, transitions);

		// Act.
		tickSystem.Execute();

		// Assert.
		stateMachine.DidNotReceive().SwitchState(to: Arg.Any<IState>());
	}

	[Fact]
	public void Constructor_NullStateMachine_ThrowsArgumentNullException()
	{
		// Arrange.
		IStateMachine stateMachine = null!;
		var transitions = Substitute.For<ITransitions>();

		// Act.
		var act = () => new StatefulTransitionSystem(stateMachine, transitions);

		// Assert.
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void Constructor_NullTransitionCoordinator_ThrowsArgumentNullException()
	{
		// Arrange.
		var stateMachine = Substitute.For<IStateMachine>();
		ITransitions transitions = null!;

		// Act.
		var act = () => new StatefulTransitionSystem(stateMachine, transitions);

		// Assert.
		act.Should().Throw<ArgumentNullException>();
	}
}