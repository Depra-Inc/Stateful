// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public sealed class StatefulTransitionSystem : IStatefulTransitionSystem, IDisposable
    {
        private readonly IStateMachine _stateMachine;
        private readonly IStateTransitionCoordination _coordination;

        public event Action<IState> StateChanged;

        public StatefulTransitionSystem(IStateMachine stateMachine, IStateTransitionCoordination coordination)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _coordination = coordination ?? throw new ArgumentNullException(nameof(coordination));

            _stateMachine.StateChanged += OnStateChanged;
        }

        public void Dispose() =>
            _stateMachine.StateChanged -= OnStateChanged;

        public IState CurrentState => 
            _stateMachine.CurrentState;

        public void ChangeState(IState state) =>
            _stateMachine.ChangeState(state);

        public void Tick()
        {
            if (NeedTransition(out var nextState))
            {
                ChangeState(nextState);
            }
        }

        public bool NeedTransition(out IState nextState) =>
            _coordination.NeedTransition(out nextState);

        public void AddAnyTransition(IStateTransition transition) =>
            _coordination.AddAnyTransition(transition);

        public void AddTransition(IState from, IStateTransition transition) =>
            _coordination.AddTransition(from, transition);

        private void OnStateChanged(IState state)
        {
            _coordination.Update(state);
            StateChanged?.Invoke(state);
        }
    }
}