using CreativeMinds.CQSLight.Authorisation;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQSLight.Exceptions {

	public class AuthorisationException : ApplicationException {
		protected readonly IEnumerable<AuthorisationResult> results;

		public AuthorisationException(AuthorisationResult result) {
			if (result != null) {
				this.results = new List<AuthorisationResult> {  result };
			}
			else {
				throw new ArgumentNullException(nameof(result));
			}
		}

		public AuthorisationException(IEnumerable<AuthorisationResult> results) {
			this.results = results ?? throw new ArgumentNullException(nameof(results));
		}
	}
}
