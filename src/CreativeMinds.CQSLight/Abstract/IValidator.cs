using CreativeMinds.CQSLight.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight.Abstract {

	public interface IValidator<TMessage> where TMessage : IMessage {
		Task<ValidationResult> ValidateAsync(TMessage message, CancellationToken cancellationToken);
	}
}
