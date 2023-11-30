using CreativeMinds.CQSLight.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public interface ICommandDispatcher {
		Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
	}
}
