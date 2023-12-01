using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using CreativeMinds.CQSLight.Instrumentation;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public class CommandDispatcher : DispatcherBase, ICommandDispatcher {

		public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger) : base(serviceProvider, logger) {		}

		public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand {
			this.activity = CQSLightInstrumentation.ActivitySource.StartActivity(InstrumentationConstants.CommandDispatchActivityName, ActivityKind.Internal);

			if (this.activity != null) {
				if (this.activity.IsAllDataRequested) {
					this.activity.SetTag(InstrumentationConstants.Type, command.GetType().Name);
				}
			}

			CommandHandlerAttribute? handlerAttribute = command.GetType().GetCustomAttribute<CommandHandlerAttribute>(true);
			if (handlerAttribute != null) {

				await this.CheckAuthorisationAsync(command, cancellationToken);

				await this.CheckValidationAsync(command, cancellationToken);

				ICommandHandler<TCommand>? commandHandlerInstance = this.serviceProvider.GetService(handlerAttribute.Handler) as ICommandHandler<TCommand>;
				if (commandHandlerInstance != null) {
					await commandHandlerInstance.HandleAsync(command, cancellationToken);
					this.activity?.SetStatus(ActivityStatusCode.Ok, "Handler has handled the command");
				}
				else {
					this.activity?.SetStatus(ActivityStatusCode.Error, "Found handler, but not correct type");
					this.logger.LogError($"The command handler for the type '{command.GetType()}' failed to be cast to an ICommandHandler<TCommand>");
					throw new CommandHandlerHasWrongTypeException();
				}
			}
			else {
				this.activity?.SetStatus(ActivityStatusCode.Error, "No handler found");
				this.logger.LogError($"Failed to locate a command handler for the type '{command.GetType()}'");
				throw new NoCommandHandlerFoundException();
			}
		}
	}
}
