// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using Depra.StateMachines.Application;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Benchmarks
{
    public class TransitionStateMachineBenchmark
    {
        private ITransitionStateMachine _stateMachine = null!;

        [GlobalSetup]
        public void Setup()
        {
            var firstState = new FirstState();
            var secondState = new SecondState();
            _stateMachine = new TransitionStateMachine(new StateMachine());
            _stateMachine.AddTransition(firstState, new StateTransition(secondState, () => true));
            _stateMachine.AddTransition(secondState, new StateTransition(firstState, () => true));
        }

        [Benchmark]
        public void Tick() => _stateMachine.Tick();

        private sealed class FirstState : IState
        {
            public void Enter() { }

            public void Exit() { }
        }

        private sealed class SecondState : IState
        {
            public void Enter() { }

            public void Exit() { }
        }
    }
}