// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Depra.StateMachines.Domain
{
    public interface ITransitionStateMachine : IStateMachine
    {
        void Tick();
        
        bool NeedTransition(out IState nextState);

        void AddAnyTransition(IStateTransition transition);
        
        void AddTransition(IState from, IStateTransition transition);
    }
}