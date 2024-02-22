using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Hierarchical
{
	public interface IStateNode : IState
	{
		Task Enter(CancellationToken cancellationToken);

		Task Exit(CancellationToken cancellationToken);
	}
}