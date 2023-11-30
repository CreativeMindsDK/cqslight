using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight.Abstract {

	public interface ICommandHandler<TCommand> where TCommand : ICommand {
		Task HandleAsync(TCommand command, CancellationToken cancellationToken);
	}
}
