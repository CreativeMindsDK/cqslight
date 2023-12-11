using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using CreativeMinds.CQSLight.Instrumentation;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
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

				await this.CheckValidationAsync(query, cancellationToken);

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
