// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Transitional.Strategies;
using Depra.StateMachines.Transitional.Transition;

namespace Depra.StateMachines.UnitTests.Transition;

public sealed class StateTransitionCoordinationTests
{
    private readonly ITransitions _transitions;
    private readonly ITransitionStrategy _strategy;

    public StateTransitionCoordinationTests()
    {
        _transitions = new Transitions();
        _strategy = new TransitionStrategy(_transitions);
    }

    [Fact]
    public void NeedTransition_Should_ReturnTrue_WhenAnyTransitionExists()
    {
        // Arrange.
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();
        _transitions.AddAny(transition);
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        // Act.
        var result = _strategy.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_Should_ReturnTrue_WhenCurrentTransitionExists()
    {
        // Arrange.
        var state = Substitute.For<IState>();
        var nextState = Substitute.For<IState>();
        var transition = Substitute.For<IStateTransition>();
        _transitions.Add(state, transition);
        transition.ShouldTransition().Returns(true);
        transition.NextState.Returns(nextState);

        // Act.
        _strategy.Execute(state);
        var result = _strategy.NeedTransition(out var actualNextState);

        // Assert.
        result.Should().BeTrue();
        actualNextState.Should().BeEquivalentTo(nextState);
    }

    [Fact]
    public void NeedTransition_Should_ReturnFalse_WhenNoTransitionExists()
    {
        // Arrange - not needed.

        // Act.
        var result = _strategy.NeedTransition(out var nextState);

        // Assert.
        result.Should().BeFalse();
        nextState.Should().BeNull();
    }
}