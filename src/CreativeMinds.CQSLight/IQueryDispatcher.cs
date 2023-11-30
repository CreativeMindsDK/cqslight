using CreativeMinds.CQSLight.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public interface IQueryDispatcher {
		Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult>;
	}
}
