// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Application
{
    public sealed class TransitionStateMachine : ITransitionStateMachine
    {
        private static readonly List<IStateTransition> EMPTY_TRANSITIONS = new List<IStateTransition>();
        
        private readonly IStateMachine _stateMachine;
        private readonly List<IStateTransition> _anyTransitions;
        private readonly Dictionary<Type, List<IStateTransition>> _transitions;
        
        private List<IStateTransition> _currentTransitions;

        public event Action<IState> StateChanged;

        public TransitionStateMachine(IStateMachine stateMachine)
        {
            _anyTransitions = new List<IStateTransition>();
            _currentTransitions = new List<IStateTransition>();
            _transitions = new Dictionary<Type, List<IStateTransition>>();
            
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _stateMachine.StateChanged += OnStateChanged;
        }

        public IState CurrentState => _stateMachine.CurrentState;

        public void ChangeState(IState state) => _stateMachine.ChangeState(state);

        public void Tick()
        {
            if (NeedTransition(out var nextState))
            {
                ChangeState(nextState);
            }
        }

        public bool NeedTransition(out IState nextState)
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.ShouldTransition())
                {
                    nextState = transition.NextState;
                    return true;
                }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.ShouldTransition())
                {
                    nextState = transition.NextState;
                    return true;
                }
            }

            nextState = null;
            return false;
        }

        public void AddAnyTransition(IStateTransition transition) => 
            _anyTransitions.Add(transition);

        public void AddTransition(IState from, IStateTransition transition)
        {
            var sourceStateType = from.GetType();

            if (_transitions.TryGetValue(sourceStateType, out var transitions) == false)
            {
                transitions = new List<IStateTransition>();
                _transitions[sourceStateType] = transitions;
            }

            transitions.Add(transition);
        }

        private void OnStateChanged(IState state)
        {
            FetchCurrentTransitions();
            StateChanged?.Invoke(state);
        }

        private void FetchCurrentTransitions()
        {
            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EMPTY_TRANSITIONS;
        }
    }
}