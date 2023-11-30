using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public class CommandDispatcher : DispatcherBase, ICommandDispatcher {

		public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger) : base(serviceProvider, logger) { }

		public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand {
			CommandHandlerAttribute? handlerAttribute = command.GetType().GetCustomAttribute<CommandHandlerAttribute>(true);
			if (handlerAttribute != null) {

				await this.CheckAuthorisationAsync(command, cancellationToken);

				await this.CheckValidationAsync(command, cancellationToken);

				ICommandHandler<TCommand>? commandHandlerInstance = this.serviceProvider.GetService(handlerAttribute.Handler) as ICommandHandler<TCommand>;
				if (commandHandlerInstance != null) {
					await commandHandlerInstance.HandleAsync(command, cancellationToken);
				}
				else {
					this.logger.LogError($"The command handler for the type '{command.GetType()}' failed to be cast to an ICommandHandler<TCommand>");
					throw new CommandHandlerHasWrongTypeException();
				}
			}
			else {
				this.logger.LogError($"Failed to locate a command handler for the type '{command.GetType()}'");
				throw new NoCommandHandlerFoundException();
			}
		}
	}
}
