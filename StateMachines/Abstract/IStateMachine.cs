// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Depra.StateMachines.Abstract
{
    public interface IStateMachine : IStateMachine<IState> { }

    public interface IStateMachine<TState>
    {
        event Action<IState> StateChanged;

        TState CurrentState { get; }

        void SwitchState(TState to);
    }
}