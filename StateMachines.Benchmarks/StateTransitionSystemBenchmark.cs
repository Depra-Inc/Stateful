// // Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// // SPDX-License-Identifier: Apache-2.0
//
// using BenchmarkDotNet.Attributes;
// using Depra.StateMachines.Abstract;
// using Depra.StateMachines.Finite;
// using Depra.StateMachines.Transition;
//
// namespace Depra.StateMachines.Benchmarks
// {
// 	public class StateTransitionSystemBenchmark
// 	{
// 		private IStatefulTransitionSystem _system = null!;
//
// 		[GlobalSetup]
// 		public void Setup()
// 		{
// 			var firstState = new FirstState();
// 			var secondState = new SecondState();
// 			var finiteStateMachine = new StateMachine();
// 			IStateTransitionCoordination transitionCoordination = new StateTransitionCoordination();
// 			transitionCoordination.AddTransition(firstState, new StateTransition(secondState, () => true));
// 			transitionCoordination.AddTransition(secondState, new StateTransition(firstState, () => true));
// 			_system = new StatefulTransitionSystem(finiteStateMachine, transitionCoordination);
// 		}
//
// 		[Benchmark]
// 		public void Tick() => _system.Tick();
//
// 		private sealed class FirstState : IState
// 		{
// 			public void Enter() { }
//
// 			public void Exit() { }
// 		}
//
// 		private sealed class SecondState : IState
// 		{
// 			public void Enter() { }
//
// 			public void Exit() { }
// 		}
// 	}
// }