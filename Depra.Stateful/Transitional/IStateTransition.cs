// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
    public interface IStateTransition
    {
        IState NextState { get; }

        bool ShouldTransition();
    }
}