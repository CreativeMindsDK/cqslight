﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQSLight.Validation {

	public class ValidationResult {
		protected readonly List<ValidationError> errors = new List<ValidationError>();

		public ValidationResult() { }

		internal ValidationResult(IEnumerable<ValidationError> errors) {
			if (errors == null) {
				throw new ArgumentNullException(nameof(errors));
			}
			this.errors = errors.ToList();
		}

		public void AddError(String label, Int32 errorType) {
			this.errors.Add(new ValidationError(label, errorType));
		}

		public IEnumerable<ValidationError> Errors { get { return this.errors; } }
		public Boolean Success { get { return this.errors.Any() == false; } }
	}
}
