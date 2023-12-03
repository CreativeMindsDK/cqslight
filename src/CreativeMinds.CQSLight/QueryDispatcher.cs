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

	public class QueryDispatcher : DispatcherBase, IQueryDispatcher {

		public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger, CQSLightInstrumentation instrumentation) : base(serviceProvider, logger, instrumentation) { }

		public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult> {
			this.activity = this.instrumentation.ActivitySource.StartActivity(InstrumentationConstants.QueryDispatchActivityName);

			if (this.activity != null) {
				if (this.activity.IsAllDataRequested) {
					this.activity.SetTag(InstrumentationConstants.Type, query.GetType().Name);
				}
			}

			QueryHandlerAttribute? handlerAttribute = query.GetType().GetCustomAttribute<QueryHandlerAttribute>(true);
			if (handlerAttribute != null) {

				await this.CheckAuthorisationAsync(query, cancellationToken);

				await this.CheckValidationAsync(query, cancellationToken);

				IQueryHandler<TQuery, TResult>? queryHandlerInstance = this.serviceProvider.GetService(handlerAttribute.Handler) as IQueryHandler<TQuery, TResult>;
				if (queryHandlerInstance != null) {
					var output = await queryHandlerInstance.HandleAsync(query, cancellationToken);
					this.activity?.SetStatus(ActivityStatusCode.Ok, "Handler has handled the query");
					return output;
				}
				else {
					this.activity?.SetStatus(ActivityStatusCode.Error, "Found handler, but not correct type");
					this.logger.LogError($"The query handler for the type '{query.GetType()}' failed to be cast to an IQueryHandler<TQuery, TResult>");
					throw new CommandHandlerHasWrongTypeException();
				}
			}
			else {
				this.activity?.SetStatus(ActivityStatusCode.Error, "No handler found");
				this.logger.LogError($"Failed to locate a query handler for the type '{query.GetType()}'");
				throw new NoCommandHandlerFoundException();
			}
		}
	}
}
