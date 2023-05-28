// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Depra.StateMachines.Domain
{
    public interface IStateTransition
    {
        IState NextState { get; }

        bool ShouldTransition();
    }
}