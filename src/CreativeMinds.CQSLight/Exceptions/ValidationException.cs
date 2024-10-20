using CreativeMinds.CQSLight.Validation;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQSLight.Exceptions {

	public class ValidationException : ApplicationException {
		public readonly IEnumerable<ValidationResult> Results;

		public ValidationException(ValidationResult result) {
			if (result != null) {
				this.Results = new ValidationResult[] { result };
			}
			else {
				throw new ArgumentNullException(nameof(result));
			}
		}

		public ValidationException(IEnumerable<ValidationError> results) : this(new ValidationResult(results)) { }

		public ValidationException(IEnumerable<ValidationResult> results) {
			this.Results = results ?? throw new ArgumentNullException(nameof(results));
		}
	}
}
