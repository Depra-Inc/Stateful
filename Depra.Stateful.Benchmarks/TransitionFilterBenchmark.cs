﻿// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Finite;
using Depra.Stateful.Transitional;

namespace Depra.Stateful.Benchmarks;

public class TransitionFilterBenchmark
{
	private const int ANY_COUNT = 10;
	private const int REPEAT_COUNT = 100;

	private IState _initialState;
	private IStateTransitions _transitions;

	[GlobalSetup]
	public void Setup()
	{
		_initialState = new EmptyState();
		_transitions = new StateTransitions()
			.RepeatAtAny(new EmptyState(), ANY_COUNT, () => false)
			.RepeatAt(_initialState, new EmptyState(), REPEAT_COUNT, () => false);
	}

	[Benchmark(Baseline = true)]
	public bool NeedTransition() => _transitions.NeedTransition(_initialState, out _);

	private readonly record struct EmptyState : IFiniteState
	{
		void IFiniteState.Enter() { }
		void IFiniteState.Exit() { }
	}
}