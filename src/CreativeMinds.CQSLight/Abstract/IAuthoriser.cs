using CreativeMinds.CQSLight.Authorisation;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight.Abstract {

	public interface IAuthoriser<TMessage> where TMessage : IMessage {
		Task<AuthorisationResult> AuthoriseAsync(TMessage message, CancellationToken cancellationToken);
	}
}
