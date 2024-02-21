// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Threading;
using System.Threading.Tasks;

namespace Depra.Stateful.Abstract
{
	public interface IState
	{
		void Enter();

		void Exit();

		Task EnterAsync(CancellationToken cancellationToken);

		Task ExitAsync(CancellationToken cancellationToken);
	}
}