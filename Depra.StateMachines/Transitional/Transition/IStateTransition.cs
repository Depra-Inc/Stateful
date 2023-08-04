// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transitional.Transition
{
    public interface IStateTransition
    {
        IState NextState { get; }

        bool ShouldTransition();
    }
}