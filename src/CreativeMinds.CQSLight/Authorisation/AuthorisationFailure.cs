using System;

namespace CreativeMinds.CQSLight.Authorisation {

	public class AuthorisationFailure {

		public AuthorisationFailure(String label, Int32 failureType) {
			this.Label = label;
			this.FailureType = failureType;
		}

		public String Label { get; private set; }
		public Int32 FailureType { get; private set; }
	}
}
