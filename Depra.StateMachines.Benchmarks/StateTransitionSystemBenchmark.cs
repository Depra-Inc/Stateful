// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Finite;
using Depra.StateMachines.Transitional.Commands;
using Depra.StateMachines.Transitional.Strategies;
using Depra.StateMachines.Transitional.Transition;

namespace Depra.StateMachines.Benchmarks;

public class StateTransitionSystemBenchmark
{
	private IStateCommand _system = null!;

	[GlobalSetup]
	public void Setup()
	{
		var firstState = new FirstState();
		var secondState = new SecondState();
		_system = new StatefulTransitionSystem(new StateMachine(), new Transitions()
			.At(firstState, secondState, () => true)
			.At(secondState, firstState, () => true));
	}

	[Benchmark]
	public void Tick() => _system.Execute();

	private sealed class FirstState : IState
	{
		void IState.Enter() { }

		void IState.Exit() { }
	}

	private sealed class SecondState : IState
	{
		void IState.Enter() { }

		void IState.Exit() { }
	}
}