// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Transitional.Strategies;
using Depra.StateMachines.Transitional.Transition;

namespace Depra.StateMachines.Benchmarks;

public class StateTransitionCoordinationBenchmark
{
	private ITransitionStrategy _standardStrategy;

	[GlobalSetup]
	public void Setup() => _standardStrategy = new TransitionStrategy(new Transitions()
		.AnyAt(new EmptyState(), () => false)
		.AnyAt(new EmptyState(), () => false)
		.At(new EmptyState(), new EmptyState(), () => false)
		.At(new EmptyState(), new EmptyState(), () => false)
		.At(new EmptyState(), new EmptyState(), () => false)
		.At(new EmptyState(), new EmptyState(), () => false));

	[Benchmark(Baseline = true)]
	public bool NeedTransition_Standard() => _standardStrategy.NeedTransition(out _);

	private sealed class EmptyState : IState
	{
		void IState.Enter() { }

		void IState.Exit() { }
	}
}