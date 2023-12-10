﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQSLight.Validation {

	public class ValidationResult {
		protected readonly List<ValidationError> errors = new List<ValidationError>();

		internal ValidationResult(IEnumerable<ValidationError> errors) {
			this.errors = errors.ToList() ?? throw new ArgumentNullException(nameof(errors));
		}

		public void AddError(String label, Int32 errorType) {
			this.errors.Add(new ValidationError(label, errorType));
		}

		public IEnumerable<ValidationError> Errors { get { return this.errors; } }
		public Boolean Success { get { return this.errors.Any() == false; } }
	}
}
