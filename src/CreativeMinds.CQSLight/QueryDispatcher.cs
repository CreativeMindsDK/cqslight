using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using CreativeMinds.CQSLight.Instrumentation;
using CreativeMinds.CQSLight.Validation;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public class QueryDispatcher : DispatcherBase, IQueryDispatcher {

		public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger, CQSLightInstrumentation instrumentation) : base(serviceProvider, logger, instrumentation) { }

		public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult> {
			this.activity = this.instrumentation.Tracer.StartActiveSpan(InstrumentationConstants.QueryDispatchActivityName);

			this.activity?.SetAttribute(InstrumentationConstants.Type, query.GetType().Name);

			QueryHandlerAttribute? handlerAttribute = query.GetType().GetCustomAttribute<QueryHandlerAttribute>(true);
			if (handlerAttribute != null) {

				await this.CheckAuthorisationAsync(query, cancellationToken);

				var errors = await this.GetValidationStatusAsync(query, cancellationToken);

				if (errors.Any() == true) {
					IEnumerable<ValidationFailuresHandlerAttribute> validationFailuresHandlerAttributes = query.GetType().GetCustomAttributes<CreativeMinds.CQSLight.Decoraters.ValidationFailuresHandlerAttribute>(true);
					if (validationFailuresHandlerAttributes.Any() == false) {
						(this.serviceProvider.GetService(typeof(DefaultQueryValidationFailuresHandler)) as DefaultQueryValidationFailuresHandler).Handle(errors, cancellationToken);
					}
					else {
						foreach (ValidationFailuresHandlerAttribute validationFailuresHandlerAttribute in validationFailuresHandlerAttributes) {
							IQueryValidationFailuresHandler<TQuery, TResult> validatorInstance = this.serviceProvider.GetService(validationFailuresHandlerAttribute.ValidationFailuresHandler) as IQueryValidationFailuresHandler<TQuery, TResult>;
							if (validatorInstance != null) {
								return await validatorInstance.HandleAsync(errors, cancellationToken);
							}
							else {
								this.logger.LogWarning($"Trying to get an instance of type '{validationFailuresHandlerAttribute.ValidationFailuresHandler}' failed, or it wasn't an IValidator<TMessage>");
							}
						}
					}
				}

				IQueryHandler<TQuery, TResult>? queryHandlerInstance = this.serviceProvider.GetService(handlerAttribute.Handler) as IQueryHandler<TQuery, TResult>;
				if (queryHandlerInstance != null) {
					var output = await queryHandlerInstance.HandleAsync(query, cancellationToken);
					this.activity?.SetStatus(Status.Ok);
					return output;
				}
				else {
					this.activity?.SetStatus(Status.Error);
					this.logger.LogError($"The query handler for the type '{query.GetType()}' failed to be cast to an IQueryHandler<TQuery, TResult>");
					throw new CommandHandlerHasWrongTypeException();
				}
			}
			else {
				this.activity?.SetStatus(Status.Error);
				this.logger.LogError($"Failed to locate a query handler for the type '{query.GetType()}'");
				throw new NoCommandHandlerFoundException();
			}
		}
	}
}
