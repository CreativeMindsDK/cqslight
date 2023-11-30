using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public class QueryDispatcher : DispatcherBase, IQueryDispatcher {

		public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger) : base(serviceProvider, logger) { }

		public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult> {
			QueryHandlerAttribute? handlerAttribute = query.GetType().GetCustomAttribute<QueryHandlerAttribute>(true);
			if (handlerAttribute != null) {

				await this.CheckAuthorisationAsync(query, cancellationToken);

				await this.CheckValidationAsync(query, cancellationToken);

				IQueryHandler<TQuery, TResult>? queryHandlerInstance = this.serviceProvider.GetService(handlerAttribute.Handler) as IQueryHandler<TQuery, TResult>;
				if (queryHandlerInstance != null) {
					return await queryHandlerInstance.HandleAsync(query, cancellationToken);
				}
				else {
					this.logger.LogError($"The query handler for the type '{query.GetType()}' failed to be cast to an IQueryHandler<TQuery, TResult>");
					throw new CommandHandlerHasWrongTypeException();
				}
			}
			else {
				this.logger.LogError($"Failed to locate a query handler for the type '{query.GetType()}'");
				throw new NoCommandHandlerFoundException();
			}
		}
	}
}
