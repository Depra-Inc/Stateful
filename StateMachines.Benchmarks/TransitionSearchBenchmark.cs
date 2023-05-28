// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Depra.StateMachines.Application;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Benchmarks
{
    public class TransitionSearchBenchmark
    {
        private const int ANY_TRANSITIONS_COUNT = 100;
        private const int CURRENT_TRANSITIONS_COUNT = 100;

        private List<IStateTransition> _anyTransitions = null!;
        private List<IStateTransition> _currentTransitions = null!;

        [GlobalSetup]
        public void Setup()
        {
            _anyTransitions = new List<IStateTransition>();
            for (var i = 0; i < ANY_TRANSITIONS_COUNT; i++)
            {
                _anyTransitions.Add(new StateTransition(new EmptyState(), () => false));
            }

            _currentTransitions = new List<IStateTransition>();
            for (var i = 0; i < CURRENT_TRANSITIONS_COUNT; i++)
            {
                _currentTransitions.Add(new StateTransition(new EmptyState(), () => false));
            }
        }

        [Benchmark(Baseline = true)]
        public IState FindNextState()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.ShouldTransition())
                {
                    return transition.NextState;
                }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.ShouldTransition())
                {
                    return transition.NextState;
                }
            }

            return null!;
        }

        [Benchmark]
        public IState FindNextState_Concat()
        {
            var transitions = _anyTransitions.Concat(_currentTransitions);
            foreach (var transition in transitions)
            {
                if (transition.ShouldTransition())
                {
                    return transition.NextState;
                }
            }

            return null!;
        }

        [Benchmark]
        public IState FindNextState_Concat_And_Where()
        {
            var transitions = _anyTransitions.Concat(_currentTransitions);
            foreach (var transition in transitions.Where(t => t.ShouldTransition()))
            {
                return transition.NextState;
            }

            return null!;
        }

        [Benchmark]
        public IState FindNextState_Concat_And_FirstOrDefault()
        {
            var transitions = _anyTransitions.Concat(_currentTransitions);
            return transitions.FirstOrDefault(t => t.ShouldTransition())?.NextState!;
        }

        private sealed class EmptyState : IState
        {
            public void Enter() { }

            public void Exit() { }
        }
    }
}