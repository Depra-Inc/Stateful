// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transitional.Transition
{
    public interface ITransitions
    {
        IEnumerable<IStateTransition> Any { get; }

        IReadOnlyList<IStateTransition> this[IState state] { get; }

        void AddAny(IStateTransition transition);

        void Add(IState from, IStateTransition transition);
    }
}