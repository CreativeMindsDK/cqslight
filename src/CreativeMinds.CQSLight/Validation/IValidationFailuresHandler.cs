using CreativeMinds.CQSLight.Abstract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight.Validation {

	public interface IQueryValidationFailuresHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
		Task<TResult> HandleAsync(IEnumerable<ValidationResult> errors, CancellationToken cancellationToken);
	}
}
