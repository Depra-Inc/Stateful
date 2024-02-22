// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Abstract;

namespace Depra.Stateful.Finite
{
    public interface IFiniteStateMachine : IStateMachine<IFiniteState>
    {
        void SwitchState(IFiniteState to);
    }
}