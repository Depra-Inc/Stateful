// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transition;

namespace Depra.StateMachines.Benchmarks
{
    public class StateTransitionCoordinationBenchmark
    {
        private const int ANY_TRANSITIONS_COUNT = 100;
        private const int CURRENT_TRANSITIONS_COUNT = 100;

        private StateTransitionCoordination _standardCoordination = null!;
        private StateTransitionCoordinationZeroAlloc _zeroAllocCoordination = null!;

        [GlobalSetup]
        public void Setup()
        {
            _standardCoordination = new StateTransitionCoordination();
            for (var i = 0; i < ANY_TRANSITIONS_COUNT; i++)
            {
                _standardCoordination.AddAnyTransition(
                    new StateTransition(new EmptyState(), () => false));
            }

            for (var i = 0; i < CURRENT_TRANSITIONS_COUNT; i++)
            {
                _standardCoordination.AddTransition(new EmptyState(),
                    new StateTransition(new EmptyState(), () => false));
            }

            _zeroAllocCoordination = new StateTransitionCoordinationZeroAlloc();
            for (var i = 0; i < ANY_TRANSITIONS_COUNT; i++)
            {
                _zeroAllocCoordination.AddAnyTransition(
                    new StateTransitionZeroAlloc(new EmptyState(), () => false));
            }

            for (var i = 0; i < CURRENT_TRANSITIONS_COUNT; i++)
            {
                _zeroAllocCoordination.AddTransition(new EmptyState(),
                    new StateTransitionZeroAlloc(new EmptyState(), () => false));
            }
        }

        [Benchmark(Baseline = true)]
        public bool NeedTransition_Standard() => _standardCoordination.NeedTransition(out _);

        [Benchmark]
        public bool NeedTransition_ZeroAlloc() => _zeroAllocCoordination.NeedTransition(out _);

        private sealed class EmptyState : IState
        {
            public void Enter() { }

            public void Exit() { }
        }
    }
}