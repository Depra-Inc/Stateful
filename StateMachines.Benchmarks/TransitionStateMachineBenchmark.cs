// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using Depra.StateMachines.Application;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Benchmarks
{
    public class TransitionStateMachineBenchmark
    {
        private IStatefulTransitionMachine _machine = null!;

        [GlobalSetup]
        public void Setup()
        {
            var firstState = new FirstState();
            var secondState = new SecondState();
            _machine = new StatefulTransitionMachine(new StateMachine());
            _machine.AddTransition(firstState, new StateTransition(secondState, () => true));
            _machine.AddTransition(secondState, new StateTransition(firstState, () => true));
        }

        [Benchmark]
        public void Tick() => _machine.Tick();

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