using System;

namespace CreativeMinds.CQSLight.Validation {

	public record ValidationError {

		public ValidationError(String message, Int32 errorCode) {
			this.Message = message;
			this.ErrorCode = errorCode;
		}

		public String Message { get; private set; }
		public Int32 ErrorCode { get; private set; }
	}
}
