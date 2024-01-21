// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Transitional;

namespace Depra.Stateful.UnitTests.Transition;

public sealed class StateTransitionsTests
{
	[Fact]
	public void NeedTransition_ShouldReturnFalse_WhenNoTransitionsAdded()
	{
		// Arrange:
		var stateTransitions = new StateTransitions();
		var initialState = Substitute.For<IState>();

		// Act:
		var result = stateTransitions.NeedTransition(initialState, out var nextState);

		// Assert:
		result.Should().BeFalse();
		nextState.Should().BeNull();
	}

	[Fact]
	public void NeedTransition_ShouldReturnTrueAndSetNextState_WhenTransitionFound()
	{
		// Arrange:
		var initialState = Substitute.For<IState>();
		var expectedNextState = Substitute.For<IState>();
		var transition = Substitute.For<IStateTransition>();
		transition.ShouldTransition().Returns(true);
		transition.NextState.Returns(expectedNextState);

		var stateTransitions = new StateTransitions();
		stateTransitions.Add(initialState, transition);

		// Act:
		var result = stateTransitions.NeedTransition(initialState, out var actualNextState);

		// Assert:
		result.Should().BeTrue();
		actualNextState.Should().Be(expectedNextState);
	}

	[Fact]
	public void NeedTransition_ShouldReturnFalse_WhenTransitionNotFound()
	{
		// Arrange:
		var stateTransitions = new StateTransitions();
		var initialState = Substitute.For<IState>();
		var nextState = Substitute.For<IState>();
		var transition = Substitute.For<IStateTransition>();
		transition.ShouldTransition().Returns(false);
		transition.NextState.Returns(nextState);
		stateTransitions.Add(initialState, transition);

		// Act:
		var result = stateTransitions.NeedTransition(initialState, out var toState);

		// Assert:
		result.Should().BeFalse();
		toState.Should().BeNull();
	}

	[Fact]
	public void AddAny_ShouldAddTransition_ToAnyList()
	{
		// Arrange:
		var stateTransitions = new StateTransitions();
		var transition = Substitute.For<IStateTransition>();

		// Act:
		stateTransitions.AddAny(transition);

		// Assert:
		stateTransitions.NeedTransition(Substitute.For<IState>(), out _);
		transition.Received(1).ShouldTransition();
	}

	[Fact]
	public void Add_ShouldAddTransition_ToSpecifiedStateList()
	{
		// Arrange:
		var stateTransitions = new StateTransitions();
		var initialState = Substitute.For<IState>();
		var transition = Substitute.For<IStateTransition>();

		// Act:
		stateTransitions.Add(initialState, transition);

		// Assert:
		stateTransitions.NeedTransition(initialState, out _);
		transition.Received(1).ShouldTransition();
	}
}