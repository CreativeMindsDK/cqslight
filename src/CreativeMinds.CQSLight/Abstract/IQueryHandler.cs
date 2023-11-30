using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight.Abstract {

	public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
		Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
	}
}
