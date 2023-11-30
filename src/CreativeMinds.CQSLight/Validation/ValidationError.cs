using System;

namespace CreativeMinds.CQSLight.Validation {

	public record ValidationError {

		public ValidationError(String label, Int32 errorType) {
			this.Label = label;
			this.ErrorType = errorType;
		}

		public String Label { get; private set; }
		public Int32 ErrorType { get; private set; }
	}
}
