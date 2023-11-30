using CreativeMinds.CQSLight.Validation;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQSLight.Exceptions {

	public class ValidationException : ApplicationException {
		protected readonly IEnumerable<ValidationResult> results;

		public ValidationException(ValidationResult result) {
			if (result != null) {
				this.results = new ValidationResult[] { result };
			}
			else {
				throw new ArgumentNullException(nameof(result));
			}
		}

		public ValidationException(IEnumerable<ValidationResult> results) {
			this.results = results ?? throw new ArgumentNullException(nameof(results));
		}
	}
}
