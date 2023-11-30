using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQSLight.Authorisation {

	public class AuthorisationResult {
		protected readonly List<AuthorisationFailure> failures = new List<AuthorisationFailure>();

		public void AddFailure(String label, Int32 failureType) {
			this.failures.Add(new AuthorisationFailure(label, failureType));
		}

		public IEnumerable<AuthorisationFailure> Failures { get { return this.failures; } }
		public Boolean Success { get { return this.failures.Any() == false; } }
	}
}
