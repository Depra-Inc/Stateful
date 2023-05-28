// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Depra.StateMachines.Domain
{
    public interface IStateMachine
    {
        event Action<IState> StateChanged;
        
        IState CurrentState { get; }

        void ChangeState(IState state);
    }
}